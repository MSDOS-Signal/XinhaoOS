using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Media;
using System.Management;

namespace ChromeOS.Controls
{
    public partial class SystemTray : UserControl
    {
        private DispatcherTimer? _clockTimer;
        private DispatcherTimer? _statusTimer;

        public event EventHandler? QuickSettingsPanelRequested;
        public event EventHandler? NetworkPanelRequested;
        public event EventHandler? VolumePanelRequested;
        public event EventHandler? BatteryPanelRequested;
        public event EventHandler? ClockPanelRequested;

        public SystemTray()
        {
            InitializeComponent();
            UpdateClock();
            UpdateBatteryStatus();
            
            _clockTimer = new DispatcherTimer();
            _clockTimer.Interval = TimeSpan.FromSeconds(1);
            _clockTimer.Tick += (s, e) => UpdateClock();
            _clockTimer.Start();

            _statusTimer = new DispatcherTimer();
            _statusTimer.Interval = TimeSpan.FromSeconds(10);
            _statusTimer.Tick += (s, e) => UpdateBatteryStatus();
            _statusTimer.Start();
        }

        private void UpdateClock()
        {
            var now = DateTime.Now;
            TimeText.Text = now.ToString("HH:mm");
            DateText.Text = now.ToString("M/d");
        }

        private void UpdateBatteryStatus()
        {
            try
            {
                int batteryPercent = GetBatteryPercentage();
                
                // 更新电池填充宽度
                double fillWidth = (batteryPercent / 100.0) * 10;
                BatteryFill.Width = Math.Max(1, fillWidth);
                
                // 更新电池颜色
                if (batteryPercent < 20)
                {
                    BatteryFill.Fill = new SolidColorBrush(Color.FromRgb(255, 87, 87)); // 红色
                }
                else if (batteryPercent < 50)
                {
                    BatteryFill.Fill = new SolidColorBrush(Color.FromRgb(255, 193, 7)); // 黄色
                }
                else
                {
                    BatteryFill.Fill = new SolidColorBrush(Color.FromRgb(138, 180, 248)); // 蓝色
                }

                BatteryButton.ToolTip = $"Battery: {batteryPercent}%";
            }
            catch
            {
                // 如果获取电池信息失败，使用默认值
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

        private void OnWifiClick(object sender, RoutedEventArgs e)
        {
            NetworkPanelRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnVolumeClick(object sender, RoutedEventArgs e)
        {
            VolumePanelRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnBatteryClick(object sender, RoutedEventArgs e)
        {
            BatteryPanelRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnClockClick(object sender, RoutedEventArgs e)
        {
            ClockPanelRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnQuickSettingsClick(object sender, RoutedEventArgs e)
        {
            QuickSettingsPanelRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
