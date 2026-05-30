using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Management;

namespace ChromeOS.Controls
{
    public partial class BatteryPanel : UserControl
    {
        private DispatcherTimer? _updateTimer;
        private bool _isInitialized;

        public BatteryPanel()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _isInitialized = true;
            UpdateBatteryStatus();
            
            // 设置定时更新
            _updateTimer = new DispatcherTimer();
            _updateTimer.Interval = TimeSpan.FromSeconds(10);
            _updateTimer.Tick += (s, args) => UpdateBatteryStatus();
            _updateTimer.Start();
        }

        private void UpdateBatteryStatus()
        {
            if (!_isInitialized) return;
            
            try
            {
                int batteryPercent = GetBatteryPercentage();
                bool isCharging = IsCharging();
                
                if (BatteryPercentText != null)
                    BatteryPercentText.Text = $"{batteryPercent}%";
                
                // 更新电池填充宽度
                if (BatteryFill != null)
                {
                    double fillWidth = (batteryPercent / 100.0) * 48;
                    BatteryFill.Width = Math.Max(2, fillWidth);
                
                    // 更新电池颜色
                    if (batteryPercent < 20)
                    {
                        BatteryFill.Background = new SolidColorBrush(Color.FromRgb(234, 67, 53)); // 红色
                    }
                    else if (batteryPercent < 50)
                    {
                        BatteryFill.Background = new SolidColorBrush(Color.FromRgb(251, 188, 5)); // 黄色
                    }
                    else
                    {
                        BatteryFill.Background = new SolidColorBrush(Color.FromRgb(66, 133, 244)); // 蓝色
                    }
                }
                
                // 更新状态
                if (BatteryStatusText != null)
                {
                    if (isCharging)
                    {
                        BatteryStatusText.Text = "Battery Charging";
                    }
                    else
                    {
                        BatteryStatusText.Text = "Battery Discharging";
                    }
                }
                
                if (PowerSource != null)
                {
                    if (isCharging)
                    {
                        PowerSource.Text = "AC Adapter";
                    }
                    else
                    {
                        PowerSource.Text = "Battery";
                    }
                }
                
                // 更新剩余时间（模拟）
                if (TimeRemaining != null)
                {
                    TimeRemaining.Text = isCharging ? "Charging..." : $"Approx. {6 + (batteryPercent / 20)} hours";
                }
            }
            catch
            {
                // 出错时使用默认值
                if (BatteryPercentText != null)
                    BatteryPercentText.Text = "85%";
                if (BatteryFill != null)
                {
                    BatteryFill.Width = 40;
                    BatteryFill.Background = new SolidColorBrush(Color.FromRgb(66, 133, 244));
                }
                if (BatteryStatusText != null)
                    BatteryStatusText.Text = "Battery Charging";
                if (PowerSource != null)
                    PowerSource.Text = "AC Adapter";
                if (TimeRemaining != null)
                    TimeRemaining.Text = "Approx. 6 hours";
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
                return 85;
            }
            catch
            {
                return 85;
            }
        }

        private bool IsCharging()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT BatteryStatus FROM Win32_Battery"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        var status = obj["BatteryStatus"];
                        if (status != null)
                        {
                            // 2 表示充电中，1 表示电量高，6 表示充电中
                            int statusValue = Convert.ToInt32(status);
                            return statusValue == 2 || statusValue == 6;
                        }
                    }
                }
                return true;
            }
            catch
            {
                return true;
            }
        }

        private void OnOpenBatterySettings(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Battery settings panel would open here", 
                            "Settings", 
                            MessageBoxButton.OK, 
                            MessageBoxImage.Information);
        }
    }
}
