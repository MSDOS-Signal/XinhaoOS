﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ChromeOS.Apps;
using ChromeOS.Controls;
using ChromeOS.Models;
using ChromeOS.Services;

namespace ChromeOS
{
    public partial class MainWindow : Window
    {
        private readonly List<ChromeOSWindow> _openWindows = new();
        private readonly List<ChromeOS.Services.DesktopItem> _desktopItems = new();
        private int _windowCounter;
        private ChromeOS.Services.DesktopItem? _selectedDesktopItem;
        private const int ICON_SIZE = 64;
        private const int ICON_MARGIN = 8;
        private int _currentColumn = 0;
        private int _currentRow = 0;
        private const int MAX_ICONS_PER_COLUMN = 8;

        public MainWindow()
        {
            InitializeComponent();

            BootScreenControl.BootComplete += OnBootComplete;
            LoginScreenControl.LoginSuccessful += OnLoginSuccessful;
            ShelfControl.AppLaunched += OnAppLaunched;
            ShelfControl.LauncherToggled += OnLauncherToggled;
            
            ShelfControl.SystemTrayControl.QuickSettingsPanelRequested += OnQuickSettingsPanelRequested;
            ShelfControl.SystemTrayControl.NetworkPanelRequested += OnNetworkPanelRequested;
            ShelfControl.SystemTrayControl.VolumePanelRequested += OnVolumePanelRequested;
            ShelfControl.SystemTrayControl.BatteryPanelRequested += OnBatteryPanelRequested;
            ShelfControl.SystemTrayControl.ClockPanelRequested += OnClockPanelRequested;
            
            QuickSettingsPanelControl.SettingsRequested += OnSettingsPanelRequested;
            
            LauncherControl.AppLaunched += OnAppLaunched;

            ShutdownConfirmationDialogControl.Confirmed += OnShutdownConfirmed;
            ShutdownConfirmationDialogControl.Cancelled += OnShutdownCancelled;

            LanguageManager.LanguageChanged += OnLanguageChanged;
            
            UpdateMenuTexts();
        }

        private void InitializeDesktop()
        {
            _desktopItems.Clear();
            var savedItems = PersistenceService.LoadDesktopItems();
            _desktopItems.AddRange(savedItems);
            RefreshDesktopIcons();
        }

        private void RefreshDesktopIcons()
        {
            DesktopIconsCanvas.Children.Clear();
            _currentColumn = 0;
            _currentRow = 0;

            foreach (var item in _desktopItems)
            {
                AddDesktopIconToCanvas(item);
            }
        }

        private void AddDesktopIconToCanvas(DesktopItem item)
        {
            var iconWidth = ICON_SIZE;
            var iconHeight = ICON_SIZE + 20;
            var x = 20 + _currentColumn * (iconWidth + ICON_MARGIN);
            var y = 20 + _currentRow * (iconHeight + ICON_MARGIN);

            var border = new Border
            {
                Width = iconWidth,
                Height = iconHeight,
                Background = Brushes.Transparent,
                Tag = item,
                Cursor = Cursors.Hand
            };

            var stackPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var iconPath = new Path
            {
                Data = item.IsFolder
                    ? Geometry.Parse("M10 4H4c-1.1 0-2 .9-2 2v12c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V8c0-1.1-.9-2-2-2h-8l-2-2z")
                    : Geometry.Parse("M14 2H6c-1.1 0-2 .9-2 2v16c0 1.1.9 2 2 2h12c1.1 0 2-.9 2-2V8l-6-6zm-1 7V3.5L18.5 9H13z"),
                Fill = item.IsFolder
                    ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8AB4F8"))
                    : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")),
                Width = 48,
                Height = 48,
                Stretch = Stretch.Uniform,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var nameText = new TextBlock
            {
                Text = item.Name,
                Foreground = Brushes.White,
                FontSize = 11,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                MaxWidth = iconWidth - 10,
                Margin = new Thickness(0, 5, 0, 0)
            };

            stackPanel.Children.Add(iconPath);
            stackPanel.Children.Add(nameText);
            border.Child = stackPanel;

            border.MouseLeftButtonDown += OnDesktopIconClick;
            border.MouseLeftButtonUp += OnDesktopIconMouseUp;
            border.MouseRightButtonDown += OnDesktopIconRightClick;
            border.MouseMove += OnDesktopIconMouseMove;
            border.MouseLeave += OnDesktopIconMouseLeave;

            Canvas.SetLeft(border, x);
            Canvas.SetTop(border, y);
            DesktopIconsCanvas.Children.Add(border);

            _currentRow++;
            if (_currentRow >= MAX_ICONS_PER_COLUMN)
            {
                _currentRow = 0;
                _currentColumn++;
            }
        }

        private bool _isDragging = false;
        private Point _dragStartPoint;
        private Border? _draggedBorder;

        private void OnDesktopIconMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && sender is Border border)
            {
                if (!_isDragging)
                {
                    var position = e.GetPosition(DesktopIconsCanvas);
                    _dragStartPoint = position;
                    _draggedBorder = border;
                    _isDragging = true;
                    border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4A4B4E"));
                }
                else
                {
                    var position = e.GetPosition(DesktopIconsCanvas);
                    var deltaX = position.X - _dragStartPoint.X;
                    var deltaY = position.Y - _dragStartPoint.Y;

                    if (Math.Abs(deltaX) > 5 || Math.Abs(deltaY) > 5)
                    {
                        var currentLeft = Canvas.GetLeft(border);
                        var currentTop = Canvas.GetTop(border);
                        
                        Canvas.SetLeft(border, Math.Max(0, Math.Min(DesktopIconsCanvas.ActualWidth - border.Width, currentLeft + deltaX)));
                        Canvas.SetTop(border, Math.Max(0, Math.Min(DesktopIconsCanvas.ActualHeight - border.Height, currentTop + deltaY)));
                        
                        _dragStartPoint = position;
                    }
                }
            }
        }

        private void OnDesktopIconMouseLeave(object sender, MouseEventArgs e)
        {
            if (_isDragging && sender is Border border)
            {
                border.Background = Brushes.Transparent;
            }
        }

        private DateTime _lastClickTime = DateTime.MinValue;
        private DesktopItem? _lastClickedItem;

        private void OnDesktopIconClick(object sender, MouseButtonEventArgs e)
        {
            if (_isDragging) return;
            
            if (sender is Border border && border.Tag is DesktopItem item)
            {
                _selectedDesktopItem = item;
                ClearDesktopSelection();
                border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3C3D40"));
            }
        }

        private void OnDesktopIconMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDragging)
            {
                _isDragging = false;
                _draggedBorder = null;
                return;
            }

            if (sender is Border border && border.Tag is DesktopItem item)
            {
                var now = DateTime.Now;
                if (_lastClickedItem == item && (now - _lastClickTime).TotalMilliseconds < 500)
                {
                    OpenDesktopItem(item);
                }
                _lastClickTime = now;
                _lastClickedItem = item;
            }
        }

        private void OpenDesktopItem(DesktopItem item)
        {
            if (item.IsFolder)
            {
                var appInfo = new AppInfo { Id = "files", Name = item.Name, AppType = AppType.Files };
                appInfo.Data = item.Path;
                CreateAppWindow(appInfo);
            }
            else
            {
                var ext = System.IO.Path.GetExtension(item.Name).ToLower();
                if (ext == ".txt" || ext == ".md")
                {
                    var appInfo = new AppInfo { Id = "texteditor", Name = "Text Editor", AppType = AppType.TextEditor };
                    appInfo.Data = item.Name;
                    CreateAppWindow(appInfo);
                }
                else
                {
                    MessageBox.Show($"Opening: {item.Name}\n\nType: {ext.ToUpper()}\nPath: Desktop/{item.Name}", "File Preview", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void OnDesktopIconRightClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (sender is Border border && border.Tag is DesktopItem item)
            {
                _selectedDesktopItem = item;
                ShowDesktopIconContextMenu(item, e);
            }
        }

        private void ShowDesktopIconContextMenu(DesktopItem item, MouseButtonEventArgs e)
        {
            var contextMenu = new ContextMenu();

            var openItem = new MenuItem { Header = "Open" };
            openItem.Click += (s, args) => OpenDesktopItem(item);
            contextMenu.Items.Add(openItem);

            contextMenu.Items.Add(new Separator());

            var renameItem = new MenuItem { Header = "Rename" };
            renameItem.Click += (s, args) => RenameDesktopItem(item);
            contextMenu.Items.Add(renameItem);

            var deleteItem = new MenuItem { Header = "Delete", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F28B82")) };
            deleteItem.Click += (s, args) => DeleteDesktopItem(item);
            contextMenu.Items.Add(deleteItem);

            contextMenu.Items.Add(new Separator());

            var propertiesItem = new MenuItem { Header = "Properties" };
            propertiesItem.Click += (s, args) => ShowDesktopItemProperties(item);
            contextMenu.Items.Add(propertiesItem);

            contextMenu.IsOpen = true;
        }

        private void RenameDesktopItem(DesktopItem item)
        {
            var dialog = new InputDialog($"Enter new name for '{item.Name}':", "Rename", item.Name);
            if (dialog.ShowDialog() == true && !string.IsNullOrEmpty(dialog.Result) && dialog.Result != item.Name)
            {
                item.Name = dialog.Result;
                RefreshDesktopIcons();
                MessageBox.Show($"Renamed to '{dialog.Result}'", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteDesktopItem(DesktopItem item)
        {
            var confirm = MessageBox.Show($"Are you sure you want to delete '{item.Name}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (confirm == MessageBoxResult.Yes)
            {
                _desktopItems.Remove(item);
                RefreshDesktopIcons();
                MessageBox.Show($"Deleted '{item.Name}'", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ShowDesktopItemProperties(DesktopItem item)
        {
            var props = $"Name: {item.Name}\n";
            props += item.IsFolder ? "Type: Folder\n" : "Type: File\n";
            props += $"Path: Desktop/{item.Name}";
            MessageBox.Show(props, "Properties", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ClearDesktopSelection()
        {
            foreach (var child in DesktopIconsCanvas.Children)
            {
                if (child is Border border)
                {
                    border.Background = Brushes.Transparent;
                }
            }
        }

        private void OnBootComplete(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                BootScreenControl.Visibility = Visibility.Collapsed;
                LoginScreenControl.Visibility = Visibility.Visible;
            });
        }

        private void OnLoginSuccessful(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                LoginScreenControl.Visibility = Visibility.Collapsed;
                DesktopGrid.Visibility = Visibility.Visible;
                InitializeDesktop();
            });
        }

        private void OnLauncherToggled(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (LauncherControl.Visibility == Visibility.Visible)
                    LauncherControl.Visibility = Visibility.Collapsed;
                else
                    LauncherControl.Visibility = Visibility.Visible;
            });
        }

        private void OnAppLaunched(object? sender, AppInfo appInfo)
        {
            Dispatcher.Invoke(() =>
            {
                // 检查是否已存在该应用的窗口
                var existingWindow = _openWindows.FirstOrDefault(w => w.AppInfo.Id == appInfo.Id);
                
                if (existingWindow != null)
                {
                    // 如果窗口已存在，激活它
                    ActivateWindow(existingWindow);
                }
                else
                {
                    // 否则创建新窗口
                    CreateAppWindow(appInfo);
                }
            });
        }

        private void ActivateWindow(ChromeOSWindow window)
        {
            // 激活窗口（将其置于顶层）
            Canvas.SetZIndex(window, _openWindows.Count + 1);
            
            // 确保窗口可见
            window.Visibility = Visibility.Visible;
        }

        private void CreateAppWindow(AppInfo appInfo)
        {
            var window = new ChromeOSWindow
            {
                AppInfo = appInfo,
                Width = appInfo.AppType == AppType.Browser ? 1000 : 700,
                Height = appInfo.AppType == AppType.Browser ? 700 : 500,
                CloseRequested = OnWindowClose,
                MinimizeRequested = OnWindowMinimize
            };

            window.SetTitle(appInfo.Name);
            
            ContentControl contentControl = new ContentControl();
            contentControl.Content = appInfo.AppType switch
            {
                AppType.Browser => new BrowserApp(),
                AppType.Files => new FilesApp(appInfo.Data),
                AppType.Settings => new SettingsApp(),
                AppType.Terminal => new TerminalApp(),
                AppType.TextEditor => new TextEditorApp(appInfo.Data),
                AppType.Calculator => new CalculatorApp(),
                AppType.Camera => new CameraApp(),
                AppType.Photos => new PhotosApp(),
                AppType.PlayStore => new PlayStoreApp(),
                AppType.Downloads => new DownloadsApp(),
                AppType.Gmail => new GmailApp(),
                AppType.YouTube => new BrowserApp { DataContext = "https://www.youtube.com" },
                AppType.Maps => new MapsApp(),
                AppType.Drive => new DriveApp(),
                AppType.Clock => new ClockApp(),
                AppType.Calendar => new CalendarApp(),
                AppType.Weather => new WeatherApp(),
                AppType.News => new NewsApp(),
                AppType.Music => new MusicApp(),
                AppType.Contacts => new ContactsApp(),
                _ => new TextBlock { Text = "App not found" }
            };

            if (appInfo.AppType == AppType.YouTube)
            {
                var browser = new BrowserApp();
                Dispatcher.BeginInvoke(new Action(async () =>
                {
                    await System.Threading.Tasks.Task.Delay(100);
                    browser.NavigateTo("https://www.youtube.com");
                }));
                contentControl.Content = browser;
            }

            window.WindowContent.Content = contentControl;

            _windowCounter++;
            double left = 50 + (_windowCounter % 5) * 40;
            double top = 50 + (_windowCounter % 5) * 40;
            
            Canvas.SetLeft(window, left);
            Canvas.SetTop(window, top);
            Canvas.SetZIndex(window, _openWindows.Count + 1);

            _openWindows.Add(window);
            AppContainer.Children.Add(window);
            
            // 更新任务栏状态标记
            UpdateRunningStatus(appInfo.Id);
        }

        private void OnWindowMinimize(ChromeOSWindow window)
        {
            // 最小化窗口（隐藏它，但保持在列表中）
            window.Visibility = Visibility.Collapsed;
        }

        private void OnWindowClose(ChromeOSWindow window)
        {
            _openWindows.Remove(window);
            AppContainer.Children.Remove(window);
            
            // 更新任务栏状态标记
            ShelfControl.UpdateAppRunningStatus(window.AppInfo.Id, false);
        }

        private void UpdateRunningStatus(string appId)
        {
            bool isRunning = _openWindows.Any(w => w.AppInfo.Id == appId);
            ShelfControl.UpdateAppRunningStatus(appId, isRunning);
        }

        private void OnQuickSettingsRequested(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                ToggleAllPanels();
                // 使用网络面板作为默认
                if (NetworkPanelControl.Visibility == Visibility.Visible)
                    NetworkPanelControl.Visibility = Visibility.Collapsed;
                else
                {
                    LauncherControl.Visibility = Visibility.Collapsed;
                    NetworkPanelControl.Visibility = Visibility.Visible;
                }
            });
        }

        private void OnNetworkPanelRequested(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                ToggleAllPanels();
                if (NetworkPanelControl.Visibility == Visibility.Visible)
                    NetworkPanelControl.Visibility = Visibility.Collapsed;
                else
                {
                    LauncherControl.Visibility = Visibility.Collapsed;
                    NetworkPanelControl.Visibility = Visibility.Visible;
                }
            });
        }

        private void OnVolumePanelRequested(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                ToggleAllPanels();
                if (VolumePanelControl.Visibility == Visibility.Visible)
                    VolumePanelControl.Visibility = Visibility.Collapsed;
                else
                {
                    LauncherControl.Visibility = Visibility.Collapsed;
                    VolumePanelControl.Visibility = Visibility.Visible;
                }
            });
        }

        private void OnBatteryPanelRequested(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                ToggleAllPanels();
                if (BatteryPanelControl.Visibility == Visibility.Visible)
                    BatteryPanelControl.Visibility = Visibility.Collapsed;
                else
                {
                    LauncherControl.Visibility = Visibility.Collapsed;
                    BatteryPanelControl.Visibility = Visibility.Visible;
                }
            });
        }

        private void OnClockPanelRequested(object? sender, EventArgs e)
        {
            // 时钟面板可以复用网络面板或创建单独的时钟面板
            Dispatcher.Invoke(() =>
            {
                ToggleAllPanels();
                if (NetworkPanelControl.Visibility == Visibility.Visible)
                    NetworkPanelControl.Visibility = Visibility.Collapsed;
                else
                {
                    LauncherControl.Visibility = Visibility.Collapsed;
                    NetworkPanelControl.Visibility = Visibility.Visible;
                }
            });
        }

        private void ToggleAllPanels()
        {
            QuickSettingsPanelControl.Visibility = Visibility.Collapsed;
            NetworkPanelControl.Visibility = Visibility.Collapsed;
            VolumePanelControl.Visibility = Visibility.Collapsed;
            BatteryPanelControl.Visibility = Visibility.Collapsed;
        }

        private void OnQuickSettingsPanelRequested(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                ToggleAllPanels();
                if (QuickSettingsPanelControl.Visibility == Visibility.Visible)
                    QuickSettingsPanelControl.Visibility = Visibility.Collapsed;
                else
                {
                    LauncherControl.Visibility = Visibility.Collapsed;
                    QuickSettingsPanelControl.Visibility = Visibility.Visible;
                }
            });
        }

        private void OnSettingsPanelRequested(object? sender, string e)
        {
            Dispatcher.Invoke(() =>
            {
                ToggleAllPanels();
                
                switch (e)
                {
                    case "settings":
                        CreateAppWindow(new AppInfo { Id = "settings", Name = "Settings", AppType = AppType.Settings });
                        break;
                    case "lock":
                        LockScreen();
                        break;
                    case "logout":
                        Logout();
                        break;
                    case "sleep":
                        Sleep();
                        break;
                    case "shutdown":
                        Shutdown();
                        break;
                }
            });
        }

        private void OnDesktopClick(object sender, MouseButtonEventArgs e)
        {
            ToggleAllPanels();
            LauncherControl.Visibility = Visibility.Collapsed;
            ClearDesktopSelection();
        }

        private void OnDesktopMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (_openWindows.Count > 0)
            {
                var activeWindow = _openWindows.LastOrDefault();
                if (activeWindow != null)
                {
                    var scrollViewer = FindVisualChild<ScrollViewer>(activeWindow);
                    if (scrollViewer != null)
                    {
                        scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
                    }
                }
            }
        }

        private T? FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T t)
                    return t;
                var result = FindVisualChild<T>(child);
                if (result != null)
                    return result;
            }
            return null;
        }

        private void OnSettingsRequested(object? sender, string e)
        {
            ToggleAllPanels();
            
            switch (e)
            {
                case "settings":
                    CreateAppWindow(new AppInfo { Id = "settings", Name = "Settings", AppType = AppType.Settings });
                    break;
                case "lock":
                    LockScreen();
                    break;
                case "logout":
                    Logout();
                    break;
                case "sleep":
                    Sleep();
                    break;
                case "shutdown":
                    Shutdown();
                    break;
            }
        }

        private void LockScreen()
        {
            foreach (var window in _openWindows)
            {
                window.Visibility = Visibility.Hidden;
            }
            DesktopGrid.Visibility = Visibility.Collapsed;
            LoginScreenControl.Visibility = Visibility.Visible;
            LoginScreenControl.ResetPassword();
        }

        private void Logout()
        {
            foreach (var window in _openWindows.ToList())
            {
                OnWindowClose(window);
            }
            DesktopGrid.Visibility = Visibility.Collapsed;
            LoginScreenControl.Visibility = Visibility.Visible;
            LoginScreenControl.ResetPassword();
        }

        private void Sleep()
        {
            // No message box - just show sleep animation
        }

        private void Shutdown()
        {
            ShutdownConfirmationDialogControl.Visibility = Visibility.Visible;
        }

        private async void OnShutdownConfirmed(object? sender, EventArgs e)
        {
            ShutdownConfirmationDialogControl.Visibility = Visibility.Collapsed;
            foreach (var window in _openWindows)
            {
                window.Visibility = Visibility.Hidden;
            }
            DesktopGrid.Visibility = Visibility.Collapsed;
            ShutdownScreenControl.Visibility = Visibility.Visible;
            
            PersistenceService.SaveDesktopItems(_desktopItems);
            
            _ = SoundService.PlayShutdownSound();
            
            await Task.Delay(5000);
            
            Application.Current.Shutdown();
        }

        private void OnShutdownCancelled(object? sender, EventArgs e)
        {
            ShutdownConfirmationDialogControl.Visibility = Visibility.Collapsed;
        }

        private void OnDesktopRightClick(object sender, MouseButtonEventArgs e)
        {
            ClearDesktopSelection();
        }

        private void OnNewFolderClick(object sender, RoutedEventArgs e)
        {
            var dialog = new InputDialog("Enter folder name:", "New Folder", "New Folder");
            if (dialog.ShowDialog() == true && !string.IsNullOrEmpty(dialog.Result))
            {
                var newFolder = new DesktopItem
                {
                    Name = dialog.Result,
                    IsFolder = true,
                    Path = $"Desktop/{dialog.Result}"
                };
                _desktopItems.Add(newFolder);
                RefreshDesktopIcons();
            }
        }

        private void OnNewFileClick(object sender, RoutedEventArgs e)
        {
            var dialog = new InputDialog("Enter file name:", "New File", "newfile.txt");
            if (dialog.ShowDialog() == true && !string.IsNullOrEmpty(dialog.Result))
            {
                var newFile = new DesktopItem
                {
                    Name = dialog.Result,
                    IsFolder = false,
                    Path = $"Desktop/{dialog.Result}"
                };
                _desktopItems.Add(newFile);
                RefreshDesktopIcons();
            }
        }

        private void OnPasteClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clipboard is empty.", "Paste", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnDisplaySettingsClick(object sender, RoutedEventArgs e)
        {
            CreateAppWindow(new AppInfo { Id = "settings", Name = "Settings", AppType = AppType.Settings });
        }

        private void OnPersonalizeClick(object sender, RoutedEventArgs e)
        {
            CreateAppWindow(new AppInfo { Id = "settings", Name = "Settings", AppType = AppType.Settings });
        }

        private void OnOpenTerminalFromDesktopClick(object sender, RoutedEventArgs e)
        {
            CreateAppWindow(new AppInfo { Id = "terminal", Name = "Terminal", AppType = AppType.Terminal });
        }

        private void OnOpenSettingsFromDesktopClick(object sender, RoutedEventArgs e)
        {
            CreateAppWindow(new AppInfo { Id = "settings", Name = "Settings", AppType = AppType.Settings });
        }

        private void OnRefreshDesktopClick(object sender, RoutedEventArgs e)
        {
            // Refresh desktop (visual feedback)
        }

        private void OnCutClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Cut operation initiated", "Cut", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnCopyClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Copy operation initiated", "Copy", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnChangeWallpaperClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Wallpaper settings opened\nSelect from default wallpapers or upload your own", "Change Wallpaper", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnAppearanceClick(object sender, RoutedEventArgs e)
        {
            // Appearance menu handled by sub-items
        }

        private void OnLightModeClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Light mode enabled - Coming soon!", "Appearance", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnDarkModeClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Dark mode is already enabled", "Appearance", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnSmallIconsClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Small icons enabled", "Appearance", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnMediumIconsClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Medium icons enabled", "Appearance", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnLargeIconsClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Large icons enabled", "Appearance", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnSortByClick(object sender, RoutedEventArgs e)
        {
            // Sort menu handled by sub-items
        }

        private void OnLanguageChanged(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(UpdateMenuTexts);
        }

        private void UpdateMenuTexts()
        {
            MenuItemNewFolder.Header = LanguageManager.GetString("NewFolder");
            MenuItemNewFile.Header = LanguageManager.GetString("NewFile");
            MenuItemCut.Header = LanguageManager.GetString("Cut");
            MenuItemCopy.Header = LanguageManager.GetString("Copy");
            MenuItemPaste.Header = LanguageManager.GetString("Paste");
            MenuItemChangeWallpaper.Header = LanguageManager.GetString("ChangeWallpaper");
            MenuItemDisplaySettings.Header = LanguageManager.GetString("DisplaySettings");
            MenuItemAppearance.Header = LanguageManager.GetString("Appearance");
            MenuItemLightMode.Header = LanguageManager.GetString("LightMode");
            MenuItemDarkMode.Header = LanguageManager.GetString("DarkMode");
            MenuItemSmallIcons.Header = LanguageManager.GetString("SmallIcons");
            MenuItemMediumIcons.Header = LanguageManager.GetString("MediumIcons");
            MenuItemLargeIcons.Header = LanguageManager.GetString("LargeIcons");
            MenuItemPersonalize.Header = LanguageManager.GetString("Personalize");
            MenuItemOpenTerminal.Header = LanguageManager.GetString("OpenInTerminal");
            MenuItemSettings.Header = LanguageManager.GetString("Settings");
            MenuItemSortBy.Header = LanguageManager.GetString("SortBy");
            MenuItemName.Header = LanguageManager.GetString("Name");
            MenuItemDateModified.Header = LanguageManager.GetString("DateModified");
            MenuItemSize.Header = LanguageManager.GetString("Size");
            MenuItemType.Header = LanguageManager.GetString("Type");
            MenuItemRefresh.Header = LanguageManager.GetString("Refresh");
        }
    }
}
