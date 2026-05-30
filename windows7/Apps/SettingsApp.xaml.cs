using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChromeOS.Services;
using ChromeOS.Controls;

namespace ChromeOS.Apps
{
    public partial class SettingsApp : UserControl
    {
        private bool _wifiEnabled = true;
        private bool _bluetoothEnabled = false;
        private bool _nightLight = false;
        private bool _doNotDisturb = false;
        private double _volume = 80;
        private double _brightness = 80;
        private string _currentLanguage = "en";
        
        private string _userName = "user";
        private string _userEmail = "user@gmail.com";
        private string _userAvatar = "default";
        private string _userPassword = "password";

        public SettingsApp()
        {
            InitializeComponent();
            _currentLanguage = LanguageManager.CurrentLanguage;
            LanguageManager.LanguageChanged += OnLanguageChanged;
            
            LoadUserSettings();
            
            OnNetworkClick(this, new RoutedEventArgs());
        }

        private void LoadUserSettings()
        {
            var settings = PersistenceService.LoadUserSettings();
            _userName = settings.UserName;
            _userEmail = settings.UserEmail;
            _userAvatar = settings.UserAvatar;
            _userPassword = settings.UserPassword;
        }

        private void SaveUserSettings()
        {
            PersistenceService.SaveUserSettings(new UserSettings
            {
                UserName = _userName,
                UserEmail = _userEmail,
                UserAvatar = _userAvatar,
                UserPassword = _userPassword
            });
        }

        private void OnLanguageChanged(object? sender, EventArgs e)
        {
            _currentLanguage = LanguageManager.CurrentLanguage;
            Dispatcher.Invoke(() =>
            {
                OnLanguageClick(this, new RoutedEventArgs());
            });
        }

        private void ShowContent(StackPanel panel)
        {
            SettingsContent.Content = new ScrollViewer
            {
                Content = panel,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };
        }

        private Border CreateCard(string title, FrameworkElement content)
        {
            var card = new Border
            {
                Background = (Brush)FindResource("ChromeOSSurface"),
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(16),
                Margin = new Thickness(0, 0, 0, 12)
            };
            var stack = new StackPanel();
            stack.Children.Add(new TextBlock { Text = title, Foreground = Brushes.White, FontSize = 16, Margin = new Thickness(0, 0, 0, 12) });
            stack.Children.Add(content);
            card.Child = stack;
            return card;
        }

        private StackPanel CreateToggleRow(string label, bool isChecked, RoutedEventHandler toggleHandler)
        {
            var row = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 4, 0, 4) };
            var text = new TextBlock { Text = label, Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8EAED")), FontSize = 14, VerticalAlignment = VerticalAlignment.Center, Width = 200 };
            var toggle = new ToggleButton
            {
                IsChecked = isChecked,
                Content = isChecked ? "On" : "Off",
                Width = 60, Height = 28,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(10, 0, 0, 0)
            };
            toggle.Click += toggleHandler;
            row.Children.Add(text);
            row.Children.Add(toggle);
            return row;
        }

        private StackPanel CreateSliderRow(string label, double value, double min, double max, RoutedPropertyChangedEventHandler<double> valueChanged)
        {
            var row = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 4, 0, 4) };
            var text = new TextBlock { Text = label, Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8EAED")), FontSize = 14, VerticalAlignment = VerticalAlignment.Center, Width = 200 };
            var slider = new Slider { Value = value, Minimum = min, Maximum = max, Width = 300, Margin = new Thickness(10, 0, 0, 0), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8AB4F8")) };
            var valText = new TextBlock { Text = value.ToString("0"), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")), FontSize = 14, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(10, 0, 0, 0), Width = 40 };
            slider.ValueChanged += (s, e) => { valText.Text = e.NewValue.ToString("0"); valueChanged?.Invoke(s, e); };
            row.Children.Add(text);
            row.Children.Add(slider);
            row.Children.Add(valText);
            return row;
        }

        private Button CreateActionButton(string text, RoutedEventHandler clickHandler)
        {
            return new Button
            {
                Content = text,
                Style = (Style)FindResource("ChromeOSButton"),
                Margin = new Thickness(0, 4, 0, 4),
                HorizontalContentAlignment = HorizontalAlignment.Left
            };
        }

        private void UpdateNavButtons(string activeTag)
        {
            foreach (var child in SettingsNav.Children)
            {
                if (child is Button btn)
                {
                    if (btn.Tag?.ToString() == activeTag)
                    {
                        btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8AB4F8"));
                        foreach (var innerChild in ((StackPanel)btn.Content).Children)
                        {
                            if (innerChild is TextBlock tb)
                            {
                                tb.Foreground = Brushes.White;
                            }
                        }
                    }
                    else
                    {
                        btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#35363A"));
                        foreach (var innerChild in ((StackPanel)btn.Content).Children)
                        {
                            if (innerChild is TextBlock tb && !char.IsSymbol(tb.Text[0]))
                            {
                                tb.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6"));
                            }
                        }
                    }
                }
            }
        }

        private void OnNetworkClick(object? sender, RoutedEventArgs? e)
        {
            UpdateNavButtons("network");
            var panel = new StackPanel();
            panel.Children.Add(CreateCard("WiFi", new StackPanel
            {
                Children =
                {
                    CreateToggleRow("Enable WiFi", _wifiEnabled, (s, args) => { _wifiEnabled = !_wifiEnabled; OnNetworkClick(null, new RoutedEventArgs()); }),
                    new Separator { Margin = new Thickness(0, 8, 0, 8) },
                    new TextBlock { Text = _wifiEnabled ? "Connected: XinhaoOS-WiFi" : "WiFi is disabled", Foreground = _wifiEnabled ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#81C995")) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F28B82")), FontSize = 14, Margin = new Thickness(0, 4, 0, 4) },
                    new TextBlock { Text = "Signal: Excellent | Speed: 100 Mbps", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")), FontSize = 12, Margin = new Thickness(0, 4, 0, 4) },
                    new Separator { Margin = new Thickness(0, 8, 0, 8) },
                    CreateActionButton("View available networks", (s, args) => MessageBox.Show("Available Networks:\n1. XinhaoOS-WiFi (Connected)\n2. Home-Network-5G\n3. Office-WiFi\n4. Guest-Network", "WiFi Networks", MessageBoxButton.OK, MessageBoxImage.Information)),
                    CreateActionButton("Add network", (s, args) => MessageBox.Show("Enter network name and password to connect.", "Add Network", MessageBoxButton.OK, MessageBoxImage.Information))
                }
            }));
            panel.Children.Add(CreateCard("Mobile Network", new StackPanel
            {
                Children =
                {
                    new TextBlock { Text = "Status: Not connected", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")), FontSize = 14 },
                    new TextBlock { Text = "Data usage: 2.3 GB this month", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")), FontSize = 12, Margin = new Thickness(0, 4, 0, 4) },
                    CreateActionButton("Configure mobile network", (s, args) => MessageBox.Show("Mobile network settings", "Mobile Network", MessageBoxButton.OK, MessageBoxImage.Information))
                }
            }));
            panel.Children.Add(CreateCard("Proxy", new StackPanel
            {
                Children =
                {
                    new TextBlock { Text = "Proxy: None", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")), FontSize = 14 },
                    CreateActionButton("Change proxy settings", (s, args) => MessageBox.Show("Proxy configuration", "Proxy", MessageBoxButton.OK, MessageBoxImage.Information))
                }
            }));
            ShowContent(panel);
        }

        private void OnBluetoothClick(object? sender, RoutedEventArgs? e)
        {
            UpdateNavButtons("bluetooth");
            var panel = new StackPanel();
            panel.Children.Add(CreateCard("Bluetooth", new StackPanel
            {
                Children =
                {
                    CreateToggleRow("Enable Bluetooth", _bluetoothEnabled, (s, args) => { _bluetoothEnabled = !_bluetoothEnabled; OnBluetoothClick(null, new RoutedEventArgs()); }),
                    new Separator { Margin = new Thickness(0, 8, 0, 8) },
                    new TextBlock { Text = _bluetoothEnabled ? "Visible to other devices" : "Bluetooth is disabled", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")), FontSize = 14 },
                    new Separator { Margin = new Thickness(0, 8, 0, 8) },
                    new TextBlock { Text = "Paired devices:", Foreground = Brushes.White, FontSize = 14, Margin = new Thickness(0, 8, 0, 8) },
                    new TextBlock { Text = "🎧 Sony WH-1000XM4 - Connected", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#81C995")), FontSize = 12, Margin = new Thickness(0, 4, 0, 4) },
                    new TextBlock { Text = "⌨️ Logitech MX Keys - Paired", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")), FontSize = 12, Margin = new Thickness(0, 4, 0, 4) },
                    new TextBlock { Text = "🖱️ Logitech MX Master 3 - Paired", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")), FontSize = 12, Margin = new Thickness(0, 4, 0, 4) },
                    new Separator { Margin = new Thickness(0, 8, 0, 8) },
                    CreateActionButton("Pair new device", (s, args) => MessageBox.Show("Searching for Bluetooth devices...", "Pair Device", MessageBoxButton.OK, MessageBoxImage.Information)),
                    CreateActionButton("Send file via Bluetooth", (s, args) => MessageBox.Show("Select file to send", "Bluetooth", MessageBoxButton.OK, MessageBoxImage.Information))
                }
            }));
            ShowContent(panel);
        }

        private void OnDisplayClick(object? sender, RoutedEventArgs? e)
        {
            UpdateNavButtons("display");
            var panel = new StackPanel();
            panel.Children.Add(CreateCard("Display Settings", new StackPanel
            {
                Children =
                {
                    CreateSliderRow("Brightness", _brightness, 0, 100, (s, args) => _brightness = args.NewValue),
                    new Separator { Margin = new Thickness(0, 8, 0, 8) },
                    CreateToggleRow("Night Light", _nightLight, (s, args) => { _nightLight = !_nightLight; OnDisplayClick(null, new RoutedEventArgs()); }),
                    new TextBlock { Text = _nightLight ? "Night Light: ON (Warm colors)" : "Night Light: OFF", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")), FontSize = 12, Margin = new Thickness(0, 4, 0, 4) },
                    new Separator { Margin = new Thickness(0, 8, 0, 8) },
                    CreateSliderRow("Font Size", 14, 10, 24, (s, args) => { }),
                    new Separator { Margin = new Thickness(0, 8, 0, 8) },
                    CreateActionButton("Display resolution: 1920 x 1080", (s, args) => MessageBox.Show("Available resolutions:\n- 1920 x 1080 (Recommended)\n- 1600 x 900\n- 1366 x 768\n- 1280 x 720", "Resolution", MessageBoxButton.OK, MessageBoxImage.Information)),
                    CreateActionButton("Orientation: Landscape", (s, args) => MessageBox.Show("Orientation: Landscape", "Orientation", MessageBoxButton.OK, MessageBoxImage.Information)),
                    CreateActionButton("Built-in display", (s, args) => MessageBox.Show("Display: 15.6\" LCD\nResolution: 1920 x 1080\nRefresh rate: 60Hz", "Display Info", MessageBoxButton.OK, MessageBoxImage.Information))
                }
            }));
            ShowContent(panel);
        }

        private void OnSoundClick(object? sender, RoutedEventArgs? e)
        {
            UpdateNavButtons("sound");
            var panel = new StackPanel();
            panel.Children.Add(CreateCard("Audio Output", new StackPanel
            {
                Children =
                {
                    CreateSliderRow("Volume", _volume, 0, 100, (s, args) => _volume = args.NewValue),
                    new Separator { Margin = new Thickness(0, 8, 0, 8) },
                    CreateActionButton("Output device: Built-in speakers", (s, args) => MessageBox.Show("Output devices:\n1. Built-in speakers\n2. Sony WH-1000XM4 (Bluetooth)\n3. HDMI Monitor", "Output Devices", MessageBoxButton.OK, MessageBoxImage.Information)),
                    CreateActionButton("Test audio", (s, args) => MessageBox.Show("Playing test sound...", "Audio Test", MessageBoxButton.OK, MessageBoxImage.Information))
                }
            }));
            panel.Children.Add(CreateCard("Audio Input", new StackPanel
            {
                Children =
                {
                    CreateSliderRow("Microphone", 70, 0, 100, (s, args) => { }),
                    new Separator { Margin = new Thickness(0, 8, 0, 8) },
                    CreateActionButton("Input device: Built-in microphone", (s, args) => MessageBox.Show("Input devices:\n1. Built-in microphone\n2. Sony WH-1000XM4 (Bluetooth)", "Input Devices", MessageBoxButton.OK, MessageBoxImage.Information))
                }
            }));
            ShowContent(panel);
        }

        private void OnBatteryClick(object? sender, RoutedEventArgs? e)
        {
            UpdateNavButtons("battery");
            var panel = new StackPanel();
            panel.Children.Add(CreateCard("Battery Status", new StackPanel
            {
                Children =
                {
                    new TextBlock { Text = "🔋 85% - Plugged in", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#81C995")), FontSize = 24, Margin = new Thickness(0, 0, 0, 8) },
                    new ProgressBar { Value = 85, Height = 8, Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#81C995")), Margin = new Thickness(0, 0, 0, 12) },
                    new TextBlock { Text = "Time remaining: Calculating...", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")), FontSize = 14, Margin = new Thickness(0, 4, 0, 4) },
                    new TextBlock { Text = "Battery health: Good", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")), FontSize = 14, Margin = new Thickness(0, 4, 0, 4) },
                    new TextBlock { Text = "Cycle count: 156", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")), FontSize = 14, Margin = new Thickness(0, 4, 0, 4) },
                }
            }));
            panel.Children.Add(CreateCard("Power Settings", new StackPanel
            {
                Children =
                {
                    CreateActionButton("Screen timeout: 10 minutes", (s, args) => MessageBox.Show("Screen timeout options:\n- 1 minute\n- 5 minutes\n- 10 minutes (Current)\n- 30 minutes\n- Never", "Screen Timeout", MessageBoxButton.OK, MessageBoxImage.Information)),
                    CreateActionButton("Sleep timeout: 30 minutes", (s, args) => MessageBox.Show("Sleep timeout options:\n- 5 minutes\n- 15 minutes\n- 30 minutes (Current)\n- 1 hour\n- Never", "Sleep Timeout", MessageBoxButton.OK, MessageBoxImage.Information)),
                    CreateActionButton("Power saver mode", (s, args) => MessageBox.Show("Power saver: OFF\nWhen enabled, reduces performance and screen brightness to extend battery life.", "Power Saver", MessageBoxButton.OK, MessageBoxImage.Information))
                }
            }));
            ShowContent(panel);
        }

        private void OnAccountsClick(object? sender, RoutedEventArgs? e)
        {
            UpdateNavButtons("accounts");
            var panel = new StackPanel();
            
            var profileCard = new Border
            {
                Background = (Brush)FindResource("ChromeOSSurface"),
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(20),
                Margin = new Thickness(0, 0, 0, 12)
            };
            
            var profileStack = new StackPanel { Orientation = Orientation.Horizontal };
            
            var avatarColor = _userAvatar switch
            {
                "green" => "#34A853",
                "red" => "#EA4335",
                "purple" => "#9C27B0",
                "orange" => "#FBBC05",
                "pink" => "#F48FB1",
                "teal" => "#00ACC1",
                "yellow" => "#FFEB3B",
                _ => "#4285F4"
            };
            
            var avatarBorder = new Border
            {
                Width = 80,
                Height = 80,
                CornerRadius = new CornerRadius(40),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(avatarColor)),
                Margin = new Thickness(0, 0, 20, 0)
            };
            
            var avatarText = new TextBlock
            {
                Text = _userName.Length > 0 ? _userName[0].ToString().ToUpper() : "U",
                FontSize = 32,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            avatarBorder.Child = avatarText;
            
            var infoStack = new StackPanel { VerticalAlignment = VerticalAlignment.Center };
            infoStack.Children.Add(new TextBlock { Text = _userName, Foreground = Brushes.White, FontSize = 20, FontWeight = FontWeights.Bold });
            infoStack.Children.Add(new TextBlock { Text = _userEmail, Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")), FontSize = 14, Margin = new Thickness(0, 4, 0, 0) });
            
            profileStack.Children.Add(avatarBorder);
            profileStack.Children.Add(infoStack);
            profileCard.Child = profileStack;
            panel.Children.Add(profileCard);
            
            panel.Children.Add(CreateCard("Personal Info", new StackPanel
            {
                Children =
                {
                    CreateAccountActionButton($"Name: {_userName}", (s, args) => ChangeUserName()),
                    CreateAccountActionButton($"Email: {_userEmail}", (s, args) => MessageBox.Show("Email cannot be changed in this demo.", "Info", MessageBoxButton.OK, MessageBoxImage.Information))
                }
            }));
            
            panel.Children.Add(CreateCard("Security", new StackPanel
            {
                Children =
                {
                    CreateAccountActionButton("Change password", (s, args) => ChangePassword()),
                    CreateAccountActionButton("Lock screen password", (s, args) => MessageBox.Show("Lock screen password is currently disabled.", "Lock Screen", MessageBoxButton.OK, MessageBoxImage.Information)),
                    CreateToggleRow("Require password to login", true, (s, args) => { MessageBox.Show("Password requirement updated.", "Security", MessageBoxButton.OK, MessageBoxImage.Information); })
                }
            }));
            
            panel.Children.Add(CreateCard("Profile Picture", new StackPanel
            {
                Children =
                {
                    CreateAccountActionButton("Change avatar", (s, args) => ChangeAvatar()),
                    CreateAccountActionButton("Take photo", (s, args) => MessageBox.Show("Camera access is required to take a photo.", "Camera", MessageBoxButton.OK, MessageBoxImage.Information))
                }
            }));
            
            panel.Children.Add(CreateCard("Google Account", new StackPanel
            {
                Children =
                {
                    new TextBlock { Text = _userEmail, Foreground = Brushes.White, FontSize = 16, Margin = new Thickness(0, 0, 0, 4) },
                    new TextBlock { Text = "Sync: On", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#81C995")), FontSize = 14 },
                    new Separator { Margin = new Thickness(0, 8, 0, 8) },
                    CreateActionButton("Manage Google Account", (s, args) => MessageBox.Show("Open Google Account settings in browser", "Google Account", MessageBoxButton.OK, MessageBoxImage.Information)),
                    CreateActionButton("Sync settings", (s, args) => MessageBox.Show("Sync enabled for:\n✅ Chrome\n✅ Gmail\n✅ Drive\n✅ Photos\n✅ Calendar\n✅ Contacts", "Sync Settings", MessageBoxButton.OK, MessageBoxImage.Information)),
                    CreateActionButton("Remove account", (s, args) => 
                    {
                        var result = MessageBox.Show("Are you sure you want to remove this Google account?", "Remove Account", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (result == MessageBoxResult.Yes)
                        {
                            MessageBox.Show("Google account removed.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    })
                }
            }));
            
            panel.Children.Add(CreateCard("Other accounts", new StackPanel
            {
                Children =
                {
                    CreateAccountButton("Add account", (s, args) => MessageBox.Show("Add Google, Microsoft, or other account", "Add Account", MessageBoxButton.OK, MessageBoxImage.Information)),
                    CreateAccountButton("Guest mode", (s, args) => MessageBox.Show("Guest mode: Allows temporary access without signing in", "Guest Mode", MessageBoxButton.OK, MessageBoxImage.Information)),
                    CreateAccountButton("Add family member", (s, args) => MessageBox.Show("Add a family member to share this device with them.", "Family", MessageBoxButton.OK, MessageBoxImage.Information))
                }
            }));
            
            ShowContent(panel);
        }

        private Button CreateAccountButton(string text, RoutedEventHandler clickHandler)
        {
            var btn = new Button
            {
                Content = text,
                Style = (Style)FindResource("ChromeOSButton"),
                Margin = new Thickness(0, 4, 0, 4),
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Padding = new Thickness(12, 8, 12, 8)
            };
            btn.Click += clickHandler;
            return btn;
        }

        private Button CreateAccountActionButton(string text, RoutedEventHandler clickHandler)
        {
            var btn = new Button
            {
                Content = text,
                Style = (Style)FindResource("ChromeOSButton"),
                Margin = new Thickness(0, 4, 0, 4),
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Padding = new Thickness(12, 8, 12, 8)
            };
            btn.Click += clickHandler;
            return btn;
        }

        private void ChangeUserName()
        {
            var dialog = new InputDialog("Enter new username:", "Change Username", _userName);
            if (dialog.ShowDialog() == true && !string.IsNullOrEmpty(dialog.Result))
            {
                if (dialog.Result.Length >= 3)
                {
                    _userName = dialog.Result;
                    SaveUserSettings();
                    OnAccountsClick(this, new RoutedEventArgs());
                }
                else
                {
                    MessageBox.Show("Username must be at least 3 characters.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void ChangePassword()
        {
            var currentPasswordDialog = new InputDialog("Enter current password:", "Change Password", "");
            if (currentPasswordDialog.ShowDialog() == true)
            {
                if (currentPasswordDialog.Result == _userPassword)
                {
                    var newPasswordDialog = new InputDialog("Enter new password:", "Change Password", "");
                    if (newPasswordDialog.ShowDialog() == true && !string.IsNullOrEmpty(newPasswordDialog.Result))
                    {
                        if (newPasswordDialog.Result.Length >= 6)
                        {
                            _userPassword = newPasswordDialog.Result;
                            SaveUserSettings();
                        }
                        else
                        {
                            MessageBox.Show("Password must be at least 6 characters.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Current password is incorrect.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void ChangeAvatar()
        {
            var options = new List<string> { "Blue Circle", "Green Circle", "Red Circle", "Purple Circle", "Orange Circle", "Pink Circle", "Teal Circle", "Yellow Circle" };
            var option = MessageBox.Show(
                "Select avatar color:\n\n" +
                "Click Yes for Blue\n" +
                "Click No for Green\n" +
                "Click Cancel for Red",
                "Change Avatar",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question);

            if (option == MessageBoxResult.Yes)
            {
                _userAvatar = "blue";
            }
            else if (option == MessageBoxResult.No)
            {
                _userAvatar = "green";
            }
            else
            {
                _userAvatar = "red";
            }

            SaveUserSettings();
            OnAccountsClick(this, new RoutedEventArgs());
        }

        private void OnAccessibilityClick(object? sender, RoutedEventArgs? e)
        {
            UpdateNavButtons("accessibility");
            var panel = new StackPanel();
            panel.Children.Add(CreateCard("Accessibility Features", new StackPanel
            {
                Children =
                {
                    CreateToggleRow("Large cursor", false, (s, args) => MessageBox.Show("Large cursor mode toggled", "Accessibility", MessageBoxButton.OK, MessageBoxImage.Information)),
                    CreateToggleRow("High contrast mode", false, (s, args) => MessageBox.Show("High contrast mode toggled", "Accessibility", MessageBoxButton.OK, MessageBoxImage.Information)),
                    CreateToggleRow("Screen magnifier", false, (s, args) => MessageBox.Show("Screen magnifier toggled", "Accessibility", MessageBoxButton.OK, MessageBoxImage.Information)),
                    CreateToggleRow("Select-to-speak", false, (s, args) => MessageBox.Show("Select-to-speak toggled", "Accessibility", MessageBoxButton.OK, MessageBoxImage.Information)),
                    CreateToggleRow("ChromeVox (screen reader)", false, (s, args) => MessageBox.Show("ChromeVox toggled", "Accessibility", MessageBoxButton.OK, MessageBoxImage.Information)),
                    new Separator { Margin = new Thickness(0, 8, 0, 8) },
                    CreateSliderRow("Magnification", 100, 100, 400, (s, args) => { }),
                    CreateActionButton("Manage accessibility features", (s, args) => MessageBox.Show("Full accessibility settings panel", "Accessibility", MessageBoxButton.OK, MessageBoxImage.Information))
                }
            }));
            ShowContent(panel);
        }

        private void OnPrivacyClick(object? sender, RoutedEventArgs? e)
        {
            UpdateNavButtons("privacy");
            var panel = new StackPanel();
            panel.Children.Add(CreateCard("Privacy Settings", new StackPanel
            {
                Children =
                {
                    CreateToggleRow("Do Not Disturb", _doNotDisturb, (s, args) => { _doNotDisturb = !_doNotDisturb; OnPrivacyClick(null, new RoutedEventArgs()); }),
                    new TextBlock { Text = _doNotDisturb ? "Do Not Disturb: ON" : "Do Not Disturb: OFF", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")), FontSize = 12, Margin = new Thickness(0, 4, 0, 4) },
                    new Separator { Margin = new Thickness(0, 8, 0, 8) },
                    CreateActionButton("Location services", (s, args) => MessageBox.Show("Location: ON\nApps using location:\n- Maps\n- Weather\n- Photos", "Location", MessageBoxButton.OK, MessageBoxImage.Information)),
                    CreateActionButton("Camera permissions", (s, args) => MessageBox.Show("Apps with camera access:\n- Camera\n- Chrome\n- Zoom\n- Meet", "Camera", MessageBoxButton.OK, MessageBoxImage.Information)),
                    CreateActionButton("Microphone permissions", (s, args) => MessageBox.Show("Apps with mic access:\n- Camera\n- Chrome\n- Zoom\n- Meet\n- Recorder", "Microphone", MessageBoxButton.OK, MessageBoxImage.Information)),
                    CreateActionButton("Clear browsing data", (s, args) => MessageBox.Show("Clear browsing data from Chrome", "Privacy", MessageBoxButton.OK, MessageBoxImage.Information))
                }
            }));
            ShowContent(panel);
        }

        private void OnAdvancedClick(object? sender, RoutedEventArgs? e)
        {
            UpdateNavButtons("advanced");
            var panel = new StackPanel();
            panel.Children.Add(CreateCard("Date & Time", new StackPanel
            {
                Children =
                {
                    new TextBlock { Text = DateTime.Now.ToString("F"), Foreground = Brushes.White, FontSize = 14, Margin = new Thickness(0, 0, 0, 8) },
                    CreateToggleRow("Automatic time", true, (s, args) => { }),
                    CreateActionButton("Time zone: Asia/Shanghai (UTC+8)", (s, args) => MessageBox.Show("Select time zone", "Time Zone", MessageBoxButton.OK, MessageBoxImage.Information))
                }
            }));
            panel.Children.Add(CreateCard("Language & Input", new StackPanel
            {
                Children =
                {
                    new TextBlock { Text = "Language: English (US)", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")), FontSize = 14 },
                    new TextBlock { Text = "Input method: US Keyboard", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")), FontSize = 14, Margin = new Thickness(0, 4, 0, 4) },
                    CreateActionButton("Manage languages", (s, args) => MessageBox.Show("Installed languages:\n- English (US)\n- 中文 (简体)\n- 日本語", "Languages", MessageBoxButton.OK, MessageBoxImage.Information)),
                    CreateActionButton("Manage input methods", (s, args) => MessageBox.Show("Input methods:\n- US Keyboard\n- Pinyin\n- Japanese", "Input Methods", MessageBoxButton.OK, MessageBoxImage.Information))
                }
            }));
            panel.Children.Add(CreateCard("Storage", new StackPanel
            {
                Children =
                {
                    new TextBlock { Text = "Used: 45.2 GB / 128 GB", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")), FontSize = 14, Margin = new Thickness(0, 0, 0, 8) },
                    new ProgressBar { Value = 35, Height = 8, Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8AB4F8")), Margin = new Thickness(0, 0, 0, 8) },
                    CreateActionButton("Manage storage", (s, args) => MessageBox.Show("Storage breakdown:\n- Apps: 12.3 GB\n- Downloads: 8.5 GB\n- Photos: 15.2 GB\n- Files: 9.2 GB", "Storage", MessageBoxButton.OK, MessageBoxImage.Information))
                }
            }));
            panel.Children.Add(CreateCard("Developers", new StackPanel
            {
                Children =
                {
                    CreateActionButton("Linux development environment", (s, args) => MessageBox.Show("Linux (Beta) is turned off.\nTurn on to install Linux development environment.", "Linux", MessageBoxButton.OK, MessageBoxImage.Information)),
                    CreateActionButton("USB debugging", (s, args) => MessageBox.Show("USB debugging: OFF", "USB Debugging", MessageBoxButton.OK, MessageBoxImage.Information)),
                    CreateActionButton("Crosh terminal", (s, args) => MessageBox.Show("Opening Crosh terminal...", "Crosh", MessageBoxButton.OK, MessageBoxImage.Information))
                }
            }));
            ShowContent(panel);
        }

        private void OnLanguageClick(object? sender, RoutedEventArgs? e)
        {
            UpdateNavButtons("language");
            var panel = new StackPanel();
            
            panel.Children.Add(CreateCard("Language", new StackPanel
            {
                Children =
                {
                    new TextBlock { Text = "Select your preferred language", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")), FontSize = 14, Margin = new Thickness(0, 0, 0, 12) },
                    
                    CreateLanguageOption("English", "en"),
                    CreateLanguageOption("中文 (简体)", "zh"),
                    
                    new Separator { Margin = new Thickness(0, 12, 0, 12) },
                    
                    new TextBlock { Text = "Language changes will apply to:", Foreground = Brushes.White, FontSize = 14, Margin = new Thickness(0, 0, 0, 8) },
                    new TextBlock { Text = "• Desktop interface", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")), FontSize = 12, Margin = new Thickness(0, 2, 0, 2) },
                    new TextBlock { Text = "• Settings menu", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")), FontSize = 12, Margin = new Thickness(0, 2, 0, 2) },
                    new TextBlock { Text = "• System notifications", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")), FontSize = 12, Margin = new Thickness(0, 2, 0, 2) },
                    new TextBlock { Text = "• Quick settings panel", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")), FontSize = 12, Margin = new Thickness(0, 2, 0, 2) }
                }
            }));
            
            panel.Children.Add(CreateCard("Keyboard & Input Methods", new StackPanel
            {
                Children =
                {
                    CreateActionButton("Manage keyboards", (s, args) => MessageBox.Show("Installed keyboards:\n- English (US)\n- Chinese (Simplified)\n\nClick Add to install more keyboards.", "Keyboards", MessageBoxButton.OK, MessageBoxImage.Information)),
                    CreateActionButton("Language switch shortcut", (s, args) => MessageBox.Show("Current shortcut: Super + Space\n\nYou can change this in Keyboard settings.", "Shortcut", MessageBoxButton.OK, MessageBoxImage.Information))
                }
            }));
            
            ShowContent(panel);
        }

        private StackPanel CreateLanguageOption(string languageName, string languageCode)
        {
            var row = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 4, 0, 4), Cursor = System.Windows.Input.Cursors.Hand };
            
            var radioButton = new RadioButton
            {
                Content = languageName,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8EAED")),
                FontSize = 14,
                IsChecked = _currentLanguage == languageCode,
                GroupName = "Language",
                Margin = new Thickness(0, 0, 12, 0),
                VerticalContentAlignment = VerticalAlignment.Center
            };
            
            radioButton.Checked += (s, e) => 
            {
                if (_currentLanguage != languageCode)
                {
                    _currentLanguage = languageCode;
                    LanguageManager.SetLanguage(languageCode);
                    MessageBox.Show($"Language changed to {languageName}\n\nSome changes may require restarting applications.", "Language Changed", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            };
            
            row.Children.Add(radioButton);
            
            if (_currentLanguage == languageCode)
            {
                row.Children.Add(new TextBlock { Text = "✓", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8AB4F8")), FontSize = 14, VerticalAlignment = VerticalAlignment.Center });
            }
            
            return row;
        }

        private void OnAboutClick(object? sender, RoutedEventArgs? e)
        {
            UpdateNavButtons("about");
            var panel = new StackPanel();
            panel.Children.Add(CreateCard("About XinhaoOS", new StackPanel
            {
                Children =
                {
                    new TextBlock { Text = "XinhaoOS", Foreground = Brushes.White, FontSize = 24, FontWeight = FontWeights.Bold, Margin = new Thickness(0,0,0,12) },
                    new TextBlock { Text = "Version 1.0.0 (Official Build)", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#98989D")), FontSize = 14, Margin = new Thickness(0,4,0,4) },
                    new TextBlock { Text = "macOS + Google Hybrid Theme", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#98989D")), FontSize = 14, Margin = new Thickness(0,4,0,4) },
                    new TextBlock { Text = "Linux-style Desktop Environment", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#98989D")), FontSize = 14, Margin = new Thickness(0,4,0,4) },
                    new TextBlock { Text = "Device: Xinhao Pro", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#98989D")), FontSize = 14, Margin = new Thickness(0,4,0,4) },
                    new TextBlock { Text = "CPU: Intel Core i7-1260P", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#98989D")), FontSize = 14, Margin = new Thickness(0,4,0,4) },
                    new TextBlock { Text = "Memory: 16 GB LPDDR5", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#98989D")), FontSize = 14, Margin = new Thickness(0,4,0,4) },
                    new Separator { Margin = new Thickness(0,12,0,12) },
                    CreateActionButton("Check for updates", (s, args) => MessageBox.Show("Checking for updates...\n\nYou're up to date!\nXinhaoOS Version 1.0.0 is the latest version.", "Update", MessageBoxButton.OK, MessageBoxImage.Information)),
                    CreateActionButton("Release notes", (s, args) => MessageBox.Show("Version 1.0.0\n\n- macOS + Google Hybrid Theme\n- Linux-style Desktop\n- Security fixes\n- Performance improvements", "Release Notes", MessageBoxButton.OK, MessageBoxImage.Information))
                }
            }));
            ShowContent(panel);
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            if (SettingsContent != null)
            {
                SettingsContent.ScrollToVerticalOffset(SettingsContent.VerticalOffset - e.Delta);
            }
        }
    }
}
