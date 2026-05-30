using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ChromeOS.Controls
{
    public partial class VolumePanel : UserControl
    {
        private bool _isInitialized;
        
        public VolumePanel()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // 获取当前系统音量
                float currentVolume = SystemControls.GetMasterVolume();
                VolumeSlider.Value = currentVolume * 100;
            }
            catch
            {
                VolumeSlider.Value = 70;
            }
            
            // 确保在设置初始值之后再更新UI
            _isInitialized = true;
            UpdateVolumeDisplay((int)VolumeSlider.Value);
        }

        private void OnVolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!_isInitialized) return;
            
            int volume = (int)e.NewValue;
            UpdateVolumeDisplay(volume);
            
            try
            {
                SystemControls.SetMasterVolume(volume / 100.0f);
            }
            catch
            {
                // 忽略错误
            }
        }

        private void UpdateVolumeDisplay(int volume)
        {
            if (VolumePercent != null)
                VolumePercent.Text = $"{volume}%";
            
            if (VolumeIcon != null)
            {
                if (volume == 0)
                {
                    VolumeIcon.Text = "🔇";
                }
                else if (volume < 50)
                {
                    VolumeIcon.Text = "🔉";
                }
                else
                {
                    VolumeIcon.Text = "🔊";
                }
            }
        }

        private void OnOpenSoundSettings(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Sound settings panel would open here", 
                            "Settings", 
                            MessageBoxButton.OK, 
                            MessageBoxImage.Information);
        }
    }
}
