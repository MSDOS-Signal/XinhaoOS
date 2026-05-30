using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ChromeOS.Models;

namespace ChromeOS.Controls
{
    public partial class LauncherControl : UserControl
    {
        public event EventHandler<AppInfo>? AppLaunched;

        public LauncherControl()
        {
            InitializeComponent();
        }

        private void OnBackgroundClick(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void OnSearchGotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text == "Search apps, files...")
            {
                SearchBox.Text = "";
                SearchBox.Foreground = System.Windows.Media.Brushes.White;
            }
        }

        private void OnSearchLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchBox.Text))
            {
                SearchBox.Text = "Search apps, files...";
                SearchBox.Foreground = (System.Windows.Media.Brush)FindResource("ChromeOSTextSecondary");
            }
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (AppGrid == null) return;
            var query = SearchBox.Text.ToLower();
            foreach (UIElement child in AppGrid.Children)
            {
                if (child is Button btn && btn.Tag is string tag)
                {
                    if (string.IsNullOrEmpty(query) || query == "search apps, files...")
                    {
                        btn.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        btn.Visibility = tag.ToLower().Contains(query) ? Visibility.Visible : Visibility.Collapsed;
                    }
                }
            }
        }

        private void LaunchApp(AppType type, string name, string id)
        {
            AppLaunched?.Invoke(this, new AppInfo { Id = id, Name = name, AppType = type });
            this.Visibility = Visibility.Collapsed;
        }

        private void OnChromeClick(object sender, RoutedEventArgs e) => LaunchApp(AppType.Browser, "Chrome", "chrome");
        private void OnFilesClick(object sender, RoutedEventArgs e) => LaunchApp(AppType.Files, "Files", "files");
        private void OnSettingsClick(object sender, RoutedEventArgs e) => LaunchApp(AppType.Settings, "Settings", "settings");
        private void OnTerminalClick(object sender, RoutedEventArgs e) => LaunchApp(AppType.Terminal, "Terminal", "terminal");
        private void OnTextEditorClick(object sender, RoutedEventArgs e) => LaunchApp(AppType.TextEditor, "Text Editor", "texteditor");
        private void OnCalculatorClick(object sender, RoutedEventArgs e) => LaunchApp(AppType.Calculator, "Calculator", "calculator");
        private void OnCameraClick(object sender, RoutedEventArgs e) => LaunchApp(AppType.Camera, "Camera", "camera");
        private void OnPhotosClick(object sender, RoutedEventArgs e) => LaunchApp(AppType.Photos, "Photos", "photos");
        private void OnPlayStoreClick(object sender, RoutedEventArgs e) => LaunchApp(AppType.PlayStore, "Play Store", "playstore");
        private void OnDownloadsClick(object sender, RoutedEventArgs e) => LaunchApp(AppType.Downloads, "Downloads", "downloads");
        private void OnGmailClick(object sender, RoutedEventArgs e) => LaunchApp(AppType.Gmail, "Gmail", "gmail");
        private void OnYouTubeClick(object sender, RoutedEventArgs e) => LaunchApp(AppType.YouTube, "YouTube", "youtube");
        private void OnMapsClick(object sender, RoutedEventArgs e) => LaunchApp(AppType.Maps, "Maps", "maps");
        private void OnDriveClick(object sender, RoutedEventArgs e) => LaunchApp(AppType.Drive, "Drive", "drive");
        private void OnClockClick(object sender, RoutedEventArgs e) => LaunchApp(AppType.Clock, "Clock", "clock");
        private void OnCalendarClick(object sender, RoutedEventArgs e) => LaunchApp(AppType.Calendar, "Calendar", "calendar");
        private void OnWeatherClick(object sender, RoutedEventArgs e) => LaunchApp(AppType.Weather, "Weather", "weather");
        private void OnNewsClick(object sender, RoutedEventArgs e) => LaunchApp(AppType.News, "News", "news");
        private void OnMusicClick(object sender, RoutedEventArgs e) => LaunchApp(AppType.Music, "Music", "music");
        private void OnContactsClick(object sender, RoutedEventArgs e) => LaunchApp(AppType.Contacts, "Contacts", "contacts");
    }
}
