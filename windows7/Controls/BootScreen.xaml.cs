using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using ChromeOS.Services;

namespace ChromeOS.Controls
{
    public partial class BootScreen : UserControl
    {
        public event EventHandler? BootComplete;

        public BootScreen()
        {
            InitializeComponent();
            LoadLogo();
            StartBootSequence();
        }

        private void LoadLogo()
        {
            try
            {
                var logoPath = GetLogoPath();
                if (!string.IsNullOrEmpty(logoPath) && File.Exists(logoPath))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(logoPath, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze();
                    LogoImage.Source = bitmap;
                }
                else
                {
                    LogoImage.Visibility = Visibility.Collapsed;
                }
            }
            catch
            {
                LogoImage.Visibility = Visibility.Collapsed;
            }
        }

        private string? GetLogoPath()
        {
            try
            {
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                var logoPath = Path.Combine(baseDir, "Resources", "logo.png");

                if (File.Exists(logoPath))
                {
                    return logoPath;
                }

                var assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
                baseDir = Path.GetDirectoryName(assemblyLocation) ?? AppDomain.CurrentDomain.BaseDirectory;
                logoPath = Path.Combine(baseDir, "Resources", "logo.png");

                if (File.Exists(logoPath))
                {
                    return logoPath;
                }

                var projectDir = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\"));
                logoPath = Path.Combine(projectDir, "Resources", "logo.png");

                if (File.Exists(logoPath))
                {
                    return logoPath;
                }

                var solutionDir = Directory.GetParent(baseDir)?.Parent?.Parent?.FullName;
                if (solutionDir != null)
                {
                    logoPath = Path.Combine(solutionDir, "logo.png");
                    if (File.Exists(logoPath))
                    {
                        return logoPath;
                    }
                }
            }
            catch { }
            return null;
        }

        private async void StartBootSequence()
        {
            _ = SoundService.PlayStartupSound();

            for (int i = 0; i <= 100; i += 2)
            {
                await Task.Delay(40);
                LoadingBar.Dispatcher?.Invoke(() => LoadingBar.Value = i);
            }
            BootComplete?.Invoke(this, EventArgs.Empty);
        }
    }
}
