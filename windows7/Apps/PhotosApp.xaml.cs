using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChromeOS.Apps
{
    public partial class PhotosApp : UserControl
    {
        public PhotosApp()
        {
            InitializeComponent();
        }

        private void OnPhotoClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Photo viewer:\n- Zoom with mouse wheel\n- Swipe to navigate\n- Edit, Share, Delete options available", 
                "Photos", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnImportClick(object sender, System.Windows.RoutedEventArgs e)
        {
            MessageBox.Show("Import photos from:\n- USB Drive\n- SD Card\n- Google Photos\n- Files", 
                "Photos", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
