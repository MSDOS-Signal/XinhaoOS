using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Net.NetworkInformation;

namespace ChromeOS.Controls
{
    public partial class NetworkPanel : UserControl
    {
        private DispatcherTimer? _scanTimer;
        private bool _isWifiEnabled = true;
        private bool _isBluetoothEnabled = true;
        private bool _isInitialized;

        public NetworkPanel()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // 初始化状态
            _isInitialized = true;
            
            if (WifiToggle != null)
                WifiToggle.IsChecked = true;
            if (BluetoothToggle != null)
                BluetoothToggle.IsChecked = true;
            
            // 扫描可用网络
            ScanForNetworks();
            
            // 设置定时扫描
            _scanTimer = new DispatcherTimer();
            _scanTimer.Interval = TimeSpan.FromSeconds(10);
            _scanTimer.Tick += (s, args) => ScanForNetworks();
            _scanTimer.Start();
        }

        private void ScanForNetworks()
        {
            if (!_isInitialized || NetworkList == null) return;
            
            NetworkList.Children.Clear();
            
            try
            {
                // 获取可用网络接口
                var interfaces = NetworkInterface.GetAllNetworkInterfaces();
                bool foundWifi = false;
                
                foreach (var iface in interfaces)
                {
                    // 检查是否是 WiFi 或以太网接口
                    if (iface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                        iface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        if (iface.OperationalStatus == OperationalStatus.Up)
                        {
                            foundWifi = true;
                            AddNetworkToList(iface.Name, true, GetSignalStrength(iface));
                        }
                    }
                }
                
                // 如果没有找到真实网络，添加一些示例网络
                if (!foundWifi)
                {
                    AddNetworkToList("XinhaoOS-WiFi", true, 90);
                    AddNetworkToList("Guest Network", false, 75);
                    AddNetworkToList("Office Network", false, 60);
                    AddNetworkToList("Home Network", false, 45);
                }
            }
            catch
            {
                // 出错时添加示例网络
                AddNetworkToList("XinhaoOS-WiFi", true, 90);
                AddNetworkToList("Guest Network", false, 75);
                AddNetworkToList("Office Network", false, 60);
            }
        }

        private int GetSignalStrength(NetworkInterface iface)
        {
            // 简单模拟信号强度
            return 85 + new Random().Next(-15, 15);
        }

        private void AddNetworkToList(string name, bool isConnected, int signalStrength)
        {
            if (NetworkList == null) return;
            
            var border = new Border
            {
                Background = isConnected 
                    ? new SolidColorBrush(Color.FromRgb(66, 133, 244)) 
                    : new SolidColorBrush(Color.FromRgb(53, 54, 58)),
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(12),
                Margin = new Thickness(0, 0, 0, 8),
                Cursor = System.Windows.Input.Cursors.Hand
            };

            var dockPanel = new DockPanel();

            // 信号强度图标
            var signalIcon = new TextBlock
            {
                Text = GetSignalIcon(signalStrength),
                FontSize = 20,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 12, 0)
            };
            DockPanel.SetDock(signalIcon, Dock.Left);

            // 网络名称
            var nameBlock = new TextBlock
            {
                Text = name,
                Foreground = isConnected 
                    ? new SolidColorBrush(Colors.White) 
                    : new SolidColorBrush(Color.FromRgb(232, 234, 237)),
                FontSize = 14,
                VerticalAlignment = VerticalAlignment.Center
            };
            DockPanel.SetDock(nameBlock, Dock.Left);

            // 连接状态图标
            if (isConnected)
            {
                var connectedIcon = new TextBlock
                {
                    Text = "✓",
                    FontSize = 14,
                    Foreground = new SolidColorBrush(Colors.White),
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(8, 0, 0, 0)
                };
                DockPanel.SetDock(connectedIcon, Dock.Right);
                dockPanel.Children.Add(connectedIcon);
            }

            dockPanel.Children.Add(signalIcon);
            dockPanel.Children.Add(nameBlock);
            border.Child = dockPanel;

            border.MouseLeftButtonDown += (s, args) => ConnectToNetwork(name);
            NetworkList.Children.Add(border);
        }

        private string GetSignalIcon(int strength)
        {
            if (strength >= 80) return "📶";
            if (strength >= 60) return "📶";
            if (strength >= 40) return "📶";
            return "📶";
        }

        private void ConnectToNetwork(string networkName)
        {
            MessageBox.Show($"Connecting to {networkName}...\n\nConnection successful!", 
                            "Network Connection", 
                            MessageBoxButton.OK, 
                            MessageBoxImage.Information);
            
            // 重新扫描以更新连接状态
            ScanForNetworks();
        }

        private void OnWifiToggle(object sender, RoutedEventArgs e)
        {
            if (!_isInitialized) return;
            
            _isWifiEnabled = WifiToggle?.IsChecked ?? false;
            
            if (_isWifiEnabled)
            {
                ScanForNetworks();
            }
            else
            {
                if (NetworkList != null)
                    NetworkList.Children.Clear();
            }
        }

        private void OnBluetoothToggle(object sender, RoutedEventArgs e)
        {
            if (!_isInitialized) return;
            
            _isBluetoothEnabled = BluetoothToggle?.IsChecked ?? false;
        }

        private void OnOpenNetworkSettings(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Network settings panel would open here", 
                            "Settings", 
                            MessageBoxButton.OK, 
                            MessageBoxImage.Information);
        }
    }
}
