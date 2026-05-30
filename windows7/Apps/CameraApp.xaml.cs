using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace ChromeOS.Apps
{
    public partial class CameraApp : UserControl
    {
        private int _photoCount = 0;

        public CameraApp()
        {
            InitializeComponent();
        }

        private void OnShutterClick(object sender, MouseButtonEventArgs e)
        {
            _photoCount++;
            MessageBox.Show($"Photo {_photoCount} saved!", "Camera", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnGalleryClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Gallery feature coming soon!", "Camera", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnSwitchCamera(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Switching camera...", "Camera", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnTimerClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Timer: 3s, 5s, 10s", "Camera", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnSettingsClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Camera settings:\n- Resolution: 1080p\n- Grid: On\n- Flash: Auto", "Camera", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
