using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ChromeOS.Services
{
    public static class SoundService
    {
        private static MediaPlayer? _mediaPlayer;

        static SoundService()
        {
            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.Volume = 1.0;
        }

        public static async Task PlayStartupSound()
        {
            try
            {
                await PlaySound("start.mp3", 0, 6000);
            }
            catch { }
        }

        public static async Task PlayShutdownSound()
        {
            try
            {
                await PlaySound("end.mp3", 9000, 14000);
            }
            catch { }
        }

        public static void StopSound()
        {
            try
            {
                _mediaPlayer?.Stop();
            }
            catch { }
        }

        private static async Task PlaySound(string fileName, int startMs, int endMs)
        {
            try
            {
                var soundPath = GetSoundPath(fileName);
                if (!string.IsNullOrEmpty(soundPath) && File.Exists(soundPath))
                {
                    _mediaPlayer?.Stop();
                    _mediaPlayer?.Open(new Uri(soundPath, UriKind.Absolute));
                    
                    if (startMs > 0)
                    {
                        _mediaPlayer.Position = TimeSpan.FromMilliseconds(startMs);
                    }
                    
                    _mediaPlayer?.Play();
                    
                    int durationMs = endMs - startMs;
                    await Task.Delay(durationMs);
                    
                    try
                    {
                        _mediaPlayer?.Stop();
                    }
                    catch { }
                }
            }
            catch { }
        }

        private static string? GetSoundPath(string fileName)
        {
            try
            {
                // 单文件发布时，资源会被解压到临时目录
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                var soundPath = Path.Combine(baseDir, fileName);
                if (File.Exists(soundPath))
                {
                    return soundPath;
                }

                // 检查是否在 Resources 子目录
                soundPath = Path.Combine(baseDir, "Resources", fileName);
                if (File.Exists(soundPath))
                {
                    return soundPath;
                }

                // 尝试获取程序集位置（可能是临时目录）
                var assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
                baseDir = Path.GetDirectoryName(assemblyLocation) ?? AppDomain.CurrentDomain.BaseDirectory;
                soundPath = Path.Combine(baseDir, fileName);
                if (File.Exists(soundPath))
                {
                    return soundPath;
                }

                soundPath = Path.Combine(baseDir, "Resources", fileName);
                if (File.Exists(soundPath))
                {
                    return soundPath;
                }

                // 开发环境路径
                var projectDir = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\"));
                soundPath = Path.Combine(projectDir, "Resources", fileName);
                if (File.Exists(soundPath))
                {
                    return soundPath;
                }

                // 尝试找到exe所在目录
                var processModule = System.Diagnostics.Process.GetCurrentProcess().MainModule;
                if (processModule != null)
                {
                    var exeDir = Path.GetDirectoryName(processModule.FileName);
                    if (!string.IsNullOrEmpty(exeDir))
                    {
                        soundPath = Path.Combine(exeDir, fileName);
                        if (File.Exists(soundPath))
                        {
                            return soundPath;
                        }

                        soundPath = Path.Combine(exeDir, "Resources", fileName);
                        if (File.Exists(soundPath))
                        {
                            return soundPath;
                        }
                    }
                }
            }
            catch { }
            return null;
        }
    }
}