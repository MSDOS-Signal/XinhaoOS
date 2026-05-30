using System.Windows.Controls;
using ChromeOS.Models;

namespace ChromeOS.Controls
{
    public partial class Shelf : UserControl
    {
        public event EventHandler<AppInfo>? AppLaunched;
        public event EventHandler? LauncherToggled;

        public Shelf()
        {
            InitializeComponent();
        }

        private void OnLauncherClick(object sender, System.Windows.RoutedEventArgs e)
        {
            LauncherToggled?.Invoke(this, EventArgs.Empty);
        }

        private void OnChromeClick(object sender, System.Windows.RoutedEventArgs e)
        {
            AppLaunched?.Invoke(this, new AppInfo { Id = "chrome", Name = "Chrome", AppType = AppType.Browser });
        }

        private void OnFilesClick(object sender, System.Windows.RoutedEventArgs e)
        {
            AppLaunched?.Invoke(this, new AppInfo { Id = "files", Name = "Files", AppType = AppType.Files });
        }

        private void OnSettingsClick(object sender, System.Windows.RoutedEventArgs e)
        {
            AppLaunched?.Invoke(this, new AppInfo { Id = "settings", Name = "Settings", AppType = AppType.Settings });
        }

        private void OnTerminalClick(object sender, System.Windows.RoutedEventArgs e)
        {
            AppLaunched?.Invoke(this, new AppInfo { Id = "terminal", Name = "Terminal", AppType = AppType.Terminal });
        }

        private void OnCalculatorClick(object sender, System.Windows.RoutedEventArgs e)
        {
            AppLaunched?.Invoke(this, new AppInfo { Id = "calculator", Name = "Calculator", AppType = AppType.Calculator });
        }

        public void UpdateAppRunningStatus(string appId, bool isRunning)
        {
            var visibility = isRunning ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            
            switch (appId)
            {
                case "chrome":
                    ChromeIndicator.Visibility = visibility;
                    break;
                case "files":
                    FilesIndicator.Visibility = visibility;
                    break;
                case "settings":
                    SettingsIndicator.Visibility = visibility;
                    break;
                case "terminal":
                    TerminalIndicator.Visibility = visibility;
                    break;
                case "calculator":
                    CalculatorIndicator.Visibility = visibility;
                    break;
            }
        }
    }
}