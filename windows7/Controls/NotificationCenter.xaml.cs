using System.Windows;
using System.Windows.Controls;

namespace ChromeOS.Controls
{
    public partial class NotificationCenter : UserControl
    {
        public event EventHandler<string>? SettingsRequested;

        public NotificationCenter()
        {
            InitializeComponent();
        }

        private void OnClearAllClick(object sender, RoutedEventArgs e)
        {
            NotificationList.Children.Clear();
            var emptyMsg = new TextBlock
            {
                Text = "No notifications",
                Foreground = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#9AA0A6")),
                FontSize = 14,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 50, 0, 0)
            };
            NotificationList.Children.Add(emptyMsg);
        }

        private void OnUpdateClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Checking for updates...\n\nChromeOS is already up to date.", "System Update", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnSettingsClick(object sender, RoutedEventArgs e)
        {
            SettingsRequested?.Invoke(this, "notifications");
        }
    }
}
