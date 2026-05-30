using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChromeOS.Apps
{
    public partial class WeatherApp : UserControl
    {
        public WeatherApp()
        {
            InitializeComponent();
            CurrentDate.Text = DateTime.Now.ToString("yyyy年MM月dd日 dddd", new System.Globalization.CultureInfo("zh-CN"));
            LoadHourlyForecast();
            LoadDailyForecast();
        }

        private void LoadHourlyForecast()
        {
            HourlyForecast.Children.Clear();
            var hours = new[]
            {
                new { Time = "现在", Icon = "☀️", Temp = "28°" },
                new { Time = "13:00", Icon = "☀️", Temp = "30°" },
                new { Time = "14:00", Icon = "⛅", Temp = "31°" },
                new { Time = "15:00", Icon = "⛅", Temp = "32°" },
                new { Time = "16:00", Icon = "🌤", Temp = "30°" },
                new { Time = "17:00", Icon = "🌤", Temp = "29°" },
                new { Time = "18:00", Icon = "🌥", Temp = "27°" },
                new { Time = "19:00", Icon = "🌙", Temp = "25°" },
                new { Time = "20:00", Icon = "🌙", Temp = "24°" },
                new { Time = "21:00", Icon = "🌙", Temp = "23°" },
                new { Time = "22:00", Icon = "🌙", Temp = "22°" },
                new { Time = "23:00", Icon = "🌙", Temp = "22°" }
            };

            foreach (var h in hours)
            {
                var panel = new StackPanel
                {
                    Width = 70,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(4)
                };

                panel.Children.Add(new TextBlock
                {
                    Text = h.Time,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")),
                    FontSize = 12,
                    HorizontalAlignment = HorizontalAlignment.Center
                });
                panel.Children.Add(new TextBlock
                {
                    Text = h.Icon,
                    FontSize = 28,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 8, 0, 8)
                });
                panel.Children.Add(new TextBlock
                {
                    Text = h.Temp,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8EAED")),
                    FontSize = 14,
                    HorizontalAlignment = HorizontalAlignment.Center
                });

                HourlyForecast.Children.Add(panel);
            }
        }

        private void LoadDailyForecast()
        {
            DailyForecast.Children.Clear();
            var days = new[]
            {
                new { Day = "今天", Icon = "☀️", Low = "22°", High = "32°", Desc = "晴天" },
                new { Day = "明天", Icon = "⛅", Low = "23°", High = "31°", Desc = "多云" },
                new { Day = "后天", Icon = "🌧", Low = "20°", High = "27°", Desc = "小雨" },
                new { Day = DateTime.Now.AddDays(3).ToString("dddd"), Icon = "⛈", Low = "19°", High = "25°", Desc = "雷阵雨" },
                new { Day = DateTime.Now.AddDays(4).ToString("dddd"), Icon = "🌤", Low = "21°", High = "29°", Desc = "晴转多云" },
                new { Day = DateTime.Now.AddDays(5).ToString("dddd"), Icon = "☀️", Low = "22°", High = "33°", Desc = "晴天" },
                new { Day = DateTime.Now.AddDays(6).ToString("dddd"), Icon = "☀️", Low = "23°", High = "34°", Desc = "晴天" }
            };

            foreach (var d in days)
            {
                var row = new Grid
                {
                    Margin = new Thickness(0, 8, 0, 8)
                };
                row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60) });
                row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });
                row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });
                row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });

                var dayText = new TextBlock
                {
                    Text = d.Day,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8EAED")),
                    FontSize = 14,
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetColumn(dayText, 0);
                row.Children.Add(dayText);

                var iconText = new TextBlock
                {
                    Text = d.Icon,
                    FontSize = 24,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                Grid.SetColumn(iconText, 1);
                row.Children.Add(iconText);

                var descText = new TextBlock
                {
                    Text = d.Desc,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")),
                    FontSize = 13,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(12, 0, 0, 0)
                };
                Grid.SetColumn(descText, 2);
                row.Children.Add(descText);

                var lowText = new TextBlock
                {
                    Text = d.Low,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")),
                    FontSize = 14,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Right
                };
                Grid.SetColumn(lowText, 3);
                row.Children.Add(lowText);

                var highText = new TextBlock
                {
                    Text = d.High,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8EAED")),
                    FontSize = 14,
                    FontWeight = FontWeights.SemiBold,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(12, 0, 0, 0)
                };
                Grid.SetColumn(highText, 4);
                row.Children.Add(highText);

                var border = new Border
                {
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5F6368")),
                    BorderThickness = new Thickness(0, 0, 0, 1),
                    Padding = new Thickness(4, 4, 4, 8),
                    Child = row
                };

                DailyForecast.Children.Add(border);
            }
        }
    }
}
