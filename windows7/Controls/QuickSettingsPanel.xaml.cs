using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Management;

namespace ChromeOS.Controls
{
    public partial class QuickSettingsPanel : UserControl
    {
        private bool _wifiEnabled = true;
        private bool _bluetoothEnabled = true;
        private bool _airplaneMode = false;
        private bool _doNotDisturb = false;
        private bool _nightLight = false;
        private bool _locationEnabled = true;
        private bool _isUpdating = false;
        private DispatcherTimer? _updateTimer;
        
        public event EventHandler<string>? SettingsRequested;

        public QuickSettingsPanel()
        {
            InitializeComponent();
            Loaded += QuickSettingsPanel_Loaded;
            Unloaded += QuickSettingsPanel_Unloaded;
        }

        private void QuickSettingsPanel_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeControls();
            StartUpdateTimer();
        }

        private void QuickSettingsPanel_Unloaded(object sender, RoutedEventArgs e)
        {
            StopUpdateTimer();
        }

        private void StartUpdateTimer()
        {
            _updateTimer = new DispatcherTimer();
            _updateTimer.Interval = TimeSpan.FromSeconds(5);
            _updateTimer.Tick += UpdateTimer_Tick;
            _updateTimer.Start();
        }

        private void StopUpdateTimer()
        {
            if (_updateTimer != null)
            {
                _updateTimer.Stop();
                _updateTimer.Tick -= UpdateTimer_Tick;
            }
        }

        private void UpdateTimer_Tick(object? sender, EventArgs e)
        {
            try
            {
                InitializeBattery();
            }
            catch
            {
                // 忽略更新错误
            }
        }

        private void InitializeControls()
        {
            _isUpdating = true;
            try
            {
                // 初始化音量
                float volume = SystemControls.GetMasterVolume();
                VolumeSlider.Value = volume * 100;
                UpdateVolumeIcon((int)(volume * 100));
                
                // 初始化亮度
                int brightness = SystemControls.GetBrightness();
                BrightnessSlider.Value = brightness;
                
                // 初始化电池
                InitializeBattery();
            }
            catch
            {
                // 出错时使用默认值
            }
            _isUpdating = false;
        }

        private void InitializeBattery()
        {
            try
            {
                int batteryPercent = GetBatteryPercentage();
                BatteryText.Text = $"Battery - {batteryPercent}%";
            }
            catch
            {
                // 如果失败，使用默认值
            }
        }

        private int GetBatteryPercentage()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT EstimatedChargeRemaining FROM Win32_Battery"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        var charge = obj["EstimatedChargeRemaining"];
                        if (charge != null)
                        {
                            return Convert.ToInt32(charge);
                        }
                    }
                }
                return 85; // 默认值
            }
            catch
            {
                return 85; // 出错时使用默认值
            }
        }

        private void OnWifiToggleClick(object sender, RoutedEventArgs e)
        {
            _wifiEnabled = !_wifiEnabled;
            
            if (_wifiEnabled)
            {
                WifiToggleBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8AB4F8"));
                WifiText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8EAED"));
                WifiDetails.Visibility = Visibility.Visible;
            }
            else
            {
                WifiToggleBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#35363A"));
                ((TextBlock)((StackPanel)WifiToggleBtn.Content).Children[1]).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6"));
                WifiText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6"));
                WifiDetails.Visibility = Visibility.Collapsed;
            }
        }

        private void OnBtToggleClick(object sender, RoutedEventArgs e)
        {
            _bluetoothEnabled = !_bluetoothEnabled;
            
            if (_bluetoothEnabled)
            {
                BtToggleBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8AB4F8"));
                BtText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8EAED"));
                BtDetails.Visibility = Visibility.Visible;
            }
            else
            {
                BtToggleBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#35363A"));
                ((TextBlock)((StackPanel)BtToggleBtn.Content).Children[1]).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6"));
                BtText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6"));
                BtDetails.Visibility = Visibility.Collapsed;
            }
        }

        private void OnAirplaneModeClick(object sender, RoutedEventArgs e)
        {
            _airplaneMode = !_airplaneMode;
            var btn = (Button)sender;
            
            if (_airplaneMode)
            {
                btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8AB4F8"));
                ((TextBlock)((StackPanel)btn.Content).Children[1]).Foreground = new SolidColorBrush(Colors.White);
            }
            else
            {
                btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#35363A"));
                ((TextBlock)((StackPanel)btn.Content).Children[1]).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6"));
            }
        }

        private void OnDoNotDisturbClick(object sender, RoutedEventArgs e)
        {
            _doNotDisturb = !_doNotDisturb;
            var btn = (Button)sender;
            
            if (_doNotDisturb)
            {
                btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8AB4F8"));
                ((TextBlock)((StackPanel)btn.Content).Children[1]).Foreground = new SolidColorBrush(Colors.White);
            }
            else
            {
                btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#35363A"));
                ((TextBlock)((StackPanel)btn.Content).Children[1]).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6"));
            }
        }

        private void OnNightLightClick(object sender, RoutedEventArgs e)
        {
            _nightLight = !_nightLight;
            var btn = (Button)sender;
            
            if (_nightLight)
            {
                btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8AB4F8"));
                ((TextBlock)((StackPanel)btn.Content).Children[1]).Foreground = new SolidColorBrush(Colors.White);
            }
            else
            {
                btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#35363A"));
                ((TextBlock)((StackPanel)btn.Content).Children[1]).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6"));
            }
        }

        private void OnLocationClick(object sender, RoutedEventArgs e)
        {
            _locationEnabled = !_locationEnabled;
            var btn = (Button)sender;
            
            if (_locationEnabled)
            {
                btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8AB4F8"));
                ((TextBlock)((StackPanel)btn.Content).Children[1]).Foreground = new SolidColorBrush(Colors.White);
            }
            else
            {
                btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#35363A"));
                ((TextBlock)((StackPanel)btn.Content).Children[1]).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6"));
            }
        }

        private void OnBrightnessChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_isUpdating) return;
            
            try
            {
                SystemControls.SetBrightness((int)e.NewValue);
            }
            catch
            {
                // 如果失败，至少保持 UI 更新
            }
        }

        private void OnVolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_isUpdating) return;
            
            var value = (int)e.NewValue;
            UpdateVolumeIcon(value);
            
            try
            {
                SystemControls.SetMasterVolume(value / 100.0f);
            }
            catch
            {
                // 如果 API 调用失败，至少保持 UI 更新
            }
        }

        private void UpdateVolumeIcon(int value)
        {
            if (value == 0)
            {
                VolumeIcon.Text = "🔇";
            }
            else if (value < 50)
            {
                VolumeIcon.Text = "🔉";
            }
            else
            {
                VolumeIcon.Text = "🔊";
            }
        }

        private void OnSettingsClick(object sender, RoutedEventArgs e)
        {
            SettingsRequested?.Invoke(this, "settings");
        }

        private void OnLockScreenClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Lock screen?", "Lock Screen", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.OK)
            {
                SettingsRequested?.Invoke(this, "lock");
            }
        }

        private void OnLogOutClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to log out?\n\nYou will be returned to the login screen.", "Log Out", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                SettingsRequested?.Invoke(this, "logout");
            }
        }

        private void OnSleepClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Put computer to sleep?", "Sleep", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.OK)
            {
                SettingsRequested?.Invoke(this, "sleep");
            }
        }

        private void OnPowerOffClick(object sender, RoutedEventArgs e)
        {
            SettingsRequested?.Invoke(this, "shutdown");
        }
    }
}

