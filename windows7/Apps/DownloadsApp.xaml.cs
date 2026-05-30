using System.Windows;
using System.Windows.Controls;

namespace ChromeOS.Apps
{
    public partial class DownloadsApp : UserControl
    {
        public DownloadsApp()
        {
            InitializeComponent();
        }

        private void OnClearClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Are you sure you want to clear all downloads?", "Downloads", MessageBoxButton.YesNo, MessageBoxImage.Question);
        }
    }
}