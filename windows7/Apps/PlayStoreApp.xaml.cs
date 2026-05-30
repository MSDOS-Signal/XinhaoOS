using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;

namespace ChromeOS.Apps
{
    public class PlayStoreItem
    {
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public string Rating { get; set; } = "";
        public string Icon { get; set; } = "";
        public string IconColor { get; set; } = "#4285F4";
        public string Description { get; set; } = "";
        public string Size { get; set; } = "";
    }

    public partial class PlayStoreApp : UserControl
    {
        private ObservableCollection<PlayStoreItem> _apps = new();

        public PlayStoreApp()
        {
            InitializeComponent();
            LoadApps();
            FeaturedApps.ItemsSource = _apps;
        }

        private void LoadApps()
        {
            _apps.Add(new PlayStoreItem { Name = "Instagram", Category = "Social", Rating = "4.5 ★", Icon = "📷", IconColor = "#E1306C", Description = "Share photos and videos with friends" });
            _apps.Add(new PlayStoreItem { Name = "WhatsApp", Category = "Communication", Rating = "4.3 ★", Icon = "💬", IconColor = "#25D366", Description = "Simple, reliable messaging" });
            _apps.Add(new PlayStoreItem { Name = "Spotify", Category = "Music", Rating = "4.4 ★", Icon = "🎵", IconColor = "#1DB954", Description = "Music and podcasts" });
            _apps.Add(new PlayStoreItem { Name = "Netflix", Category = "Entertainment", Rating = "4.2 ★", Icon = "🎬", IconColor = "#E50914", Description = "Movies and TV shows" });
            _apps.Add(new PlayStoreItem { Name = "TikTok", Category = "Social", Rating = "4.5 ★", Icon = "🎭", IconColor = "#FF0050", Description = "Short videos" });
            _apps.Add(new PlayStoreItem { Name = "Zoom", Category = "Communication", Rating = "4.1 ★", Icon = "📹", IconColor = "#2D8CFF", Description = "Video conferencing" });
            _apps.Add(new PlayStoreItem { Name = "Discord", Category = "Social", Rating = "4.4 ★", Icon = "🎮", IconColor = "#5865F2", Description = "Chat for communities" });
            _apps.Add(new PlayStoreItem { Name = "Slack", Category = "Business", Rating = "4.3 ★", Icon = "💼", IconColor = "#4A154B", Description = "Team communication" });
            _apps.Add(new PlayStoreItem { Name = "Twitter", Category = "Social", Rating = "4.0 ★", Icon = "🐦", IconColor = "#1DA1F2", Description = "What's happening" });
            _apps.Add(new PlayStoreItem { Name = "Telegram", Category = "Communication", Rating = "4.5 ★", Icon = "✈️", IconColor = "#0088CC", Description = "Secure messaging" });
            _apps.Add(new PlayStoreItem { Name = "Minecraft", Category = "Games", Rating = "4.6 ★", Icon = "⛏️", IconColor = "#5D8C3E", Description = "Build and explore" });
            _apps.Add(new PlayStoreItem { Name = "Duolingo", Category = "Education", Rating = "4.7 ★", Icon = "🦉", IconColor = "#58CC02", Description = "Learn languages for free" });
        }

        private void OnAppClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as Border)?.DataContext is PlayStoreItem app)
            {
                ShowAppDetail(app);
            }
        }

        private void ShowAppDetail(PlayStoreItem app)
        {
            var window = new Window
            {
                Title = app.Name,
                Width = 400,
                Height = 500,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#292A2D")),
                WindowStyle = WindowStyle.ToolWindow,
                ResizeMode = ResizeMode.NoResize
            };

            var stack = new StackPanel { Margin = new Thickness(20) };
            
            // App icon
            var iconBorder = new Border
            {
                Width = 80,
                Height = 80,
                CornerRadius = new CornerRadius(16),
                Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(app.IconColor)),
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 16)
            };
            iconBorder.Child = new TextBlock { Text = app.Icon, FontSize = 40, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
            stack.Children.Add(iconBorder);

            stack.Children.Add(new TextBlock { Text = app.Name, Foreground = System.Windows.Media.Brushes.White, FontSize = 24, HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness(0, 0, 0, 8) });
            stack.Children.Add(new TextBlock { Text = app.Category, Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#9AA0A6")), FontSize = 14, HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness(0, 0, 0, 4) });
            stack.Children.Add(new TextBlock { Text = app.Rating, Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#8AB4F8")), FontSize = 14, HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness(0, 0, 0, 16) });

            stack.Children.Add(new TextBlock { Text = app.Description, Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#E8EAED")), FontSize = 14, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 16) });

            // Install button
            var installBtn = new Button
            {
                Content = "Install",
                Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#8AB4F8")),
                Foreground = System.Windows.Media.Brushes.White,
                Padding = new Thickness(40, 10, 40, 10),
                FontSize = 16,
                HorizontalAlignment = HorizontalAlignment.Center,
                Cursor = System.Windows.Input.Cursors.Hand
            };
            installBtn.Click += (s, e) =>
            {
                installBtn.Content = "Installing...";
                installBtn.IsEnabled = false;
                var timer = new System.Windows.Threading.DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
                timer.Tick += (s2, e2) =>
                {
                    timer.Stop();
                    installBtn.Content = "Open";
                    installBtn.Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#81C995"));
                    MessageBox.Show($"{app.Name} has been installed!", "Play Store", MessageBoxButton.OK, MessageBoxImage.Information);
                };
                timer.Start();
            };
            stack.Children.Add(installBtn);

            window.Content = new ScrollViewer { Content = stack, VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
            window.ShowDialog();
        }

        private void OnSearchGotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is TextBox tb && tb.Text == "Search apps, games, movies...")
            {
                tb.Text = "";
                tb.Foreground = System.Windows.Media.Brushes.White;
            }
        }

        private void OnSearchLostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is TextBox tb && string.IsNullOrEmpty(tb.Text))
            {
                tb.Text = "Search apps, games, movies...";
                tb.Foreground = (System.Windows.Media.Brush)FindResource("ChromeOSTextSecondary");
            }
        }
    }
}
