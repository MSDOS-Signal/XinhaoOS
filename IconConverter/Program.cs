using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        var pngPath = Path.Combine(projectRoot, "..", "logo.png");
        var icoPath = Path.Combine(projectRoot, "..", "windows7", "Resources", "app.ico");

        Console.WriteLine($"Project root: {projectRoot}");
        Console.WriteLine($"Looking for PNG at: {pngPath}");
        Console.WriteLine($"Will save ICO to: {icoPath}");

        if (!File.Exists(pngPath))
        {
            Console.WriteLine($"Error: {pngPath} not found!");
            Console.WriteLine("Files in current directory:");
            foreach (var f in Directory.GetFiles(Path.GetDirectoryName(pngPath)))
            {
                Console.WriteLine($"  - {f}");
            }
            return;
        }

        var outputDir = Path.GetDirectoryName(icoPath);
        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
            Console.WriteLine($"Created directory: {outputDir}");
        }

        try
        {
            ConvertToIcon(pngPath, icoPath);
            Console.WriteLine($"Success! Icon created at: {icoPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }

    static void ConvertToIcon(string pngPath, string icoPath)
    {
        using var pngImage = new Bitmap(pngPath);
        Console.WriteLine($"Loaded PNG: {pngImage.Width}x{pngImage.Height}");

        var iconSizes = new[] { 16, 24, 32, 48, 64, 128, 256 };
        var iconImages = new List<Bitmap>();

        Console.WriteLine($"Creating {iconSizes.Length} icon sizes...");
        foreach (var size in iconSizes)
        {
            var resized = new Bitmap(size, size, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(resized))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.Clear(Color.Transparent);

                var ratio = Math.Min((double)size / pngImage.Width, (double)size / pngImage.Height);
                var newWidth = (int)(pngImage.Width * ratio);
                var newHeight = (int)(pngImage.Height * ratio);
                var x = (size - newWidth) / 2;
                var y = (size - newHeight) / 2;

                g.DrawImage(pngImage, x, y, newWidth, newHeight);
            }
            iconImages.Add(resized);
            Console.WriteLine($"  - {size}x{size}");
        }

        using var fs = new FileStream(icoPath, FileMode.Create);
        using var bw = new BinaryWriter(fs);

        // ICO header
        bw.Write((short)0); // reserved
        bw.Write((short)1); // type: 1 = icon
        bw.Write((short)iconImages.Count); // number of images

        var imageData = new List<byte[]>();
        long dataOffset = 6 + (16 * iconImages.Count);

        // Write icon directory
        foreach (var img in iconImages)
        {
            byte width = (byte)(img.Width >= 256 ? 0 : img.Width);
            byte height = (byte)(img.Height >= 256 ? 0 : img.Height);

            bw.Write(width);
            bw.Write(height);
            bw.Write((byte)0); // color palette
            bw.Write((byte)0); // reserved
            bw.Write((short)1); // color planes
            bw.Write((short)32); // bits per pixel

            using var ms = new MemoryStream();
            img.Save(ms, ImageFormat.Png);
            var data = ms.ToArray();
            imageData.Add(data);

            bw.Write(data.Length); // size of data
            bw.Write((int)dataOffset); // offset to data

            dataOffset += data.Length;
        }

        // Write image data
        foreach (var data in imageData)
        {
            bw.Write(data);
        }
    }
}
