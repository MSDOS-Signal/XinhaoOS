#!/usr/bin/env dotnet-script
#r "System.Drawing.Common"

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

var pngPath = Path.Combine(Environment.CurrentDirectory, "logo.png");
var icoPath = Path.Combine(Environment.CurrentDirectory, "windows7", "Resources", "app.ico");

Console.WriteLine($"Converting {pngPath} to {icoPath}...");

if (!File.Exists(pngPath))
{
    Console.WriteLine($"Error: {pngPath} not found!");
    return;
}

Directory.CreateDirectory(Path.GetDirectoryName(icoPath));

try
{
    using var pngImage = new Bitmap(pngPath);
    
    // 创建多尺寸图标
    var iconSizes = new[] { 16, 24, 32, 48, 64, 128, 256 };
    var iconImages = new List<Bitmap>();
    
    foreach (var size in iconSizes)
    {
        var resized = new Bitmap(size, size, PixelFormat.Format32bppArgb);
        using (var g = Graphics.FromImage(resized))
        {
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            
            // 居中缩放图片
            var ratio = Math.Min((double)size / pngImage.Width, (double)size / pngImage.Height);
            var newWidth = (int)(pngImage.Width * ratio);
            var newHeight = (int)(pngImage.Height * ratio);
            var x = (size - newWidth) / 2;
            var y = (size - newHeight) / 2;
            
            g.DrawImage(pngImage, x, y, newWidth, newHeight);
        }
        iconImages.Add(resized);
    }
    
    // 保存为ICO文件
    using var fs = new FileStream(icoPath, FileMode.Create);
    using var bw = new BinaryWriter(fs);
    
    // ICO文件头
    bw.Write((short)0); // reserved
    bw.Write((short)1); // type: 1 = icon
    bw.Write((short)iconImages.Count); // number of images
    
    // 写入图标目录
    long dataOffset = 6 + (16 * iconImages.Count);
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
        
        // 保存为PNG格式在ICO中（支持透明度）
        using var ms = new MemoryStream();
        img.Save(ms, ImageFormat.Png);
        var data = ms.ToArray();
        
        bw.Write(data.Length); // size of data
        bw.Write((int)dataOffset); // offset to data
        
        dataOffset += data.Length;
    }
    
    // 写入图标数据
    foreach (var img in iconImages)
    {
        using var ms = new MemoryStream();
        img.Save(ms, ImageFormat.Png);
        var data = ms.ToArray();
        bw.Write(data);
    }
    
    Console.WriteLine($"Successfully created icon: {icoPath}");
    Console.WriteLine($"Icon includes sizes: {string.Join(", ", iconSizes)}px");
}
catch (Exception ex)
{
    Console.WriteLine($"Error converting icon: {ex.Message}");
    Console.WriteLine(ex.StackTrace);
}
