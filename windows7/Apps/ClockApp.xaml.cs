using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ChromeOS.Apps
{
    public partial class ClockApp : UserControl
    {
        private readonly DispatcherTimer _clockTimer;
        private readonly DispatcherTimer _stopwatchTimer;
        private readonly DispatcherTimer _timerCountdown;
        private TimeSpan _stopwatchElapsed;
        private bool _stopwatchRunning;
        private int _lapCount;
        private TimeSpan _timerRemaining;
        private bool _timerRunning;
        private class Alarm
        {
            public TimeSpan Time { get; set; }
            public bool IsEnabled { get; set; } = true;
            public string Label { get; set; } = "";
        }
        private readonly List<Alarm> _alarms = new List<Alarm>();

        public ClockApp()
        {
            InitializeComponent();

            _clockTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _clockTimer.Tick += OnClockTick;
            _clockTimer.Start();

            _stopwatchTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(10)
            };
            _stopwatchTimer.Tick += OnStopwatchTick;

            _timerCountdown = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timerCountdown.Tick += OnTimerTick;

            UpdateClock();
        }

        private void OnClockTick(object sender, EventArgs e)
        {
            UpdateClock();
            CheckAlarms();
        }

        private void UpdateClock()
        {
            var now = DateTime.Now;
            DigitalTime.Text = now.ToString("HH:mm:ss");
            DigitalDate.Text = now.ToString("yyyy年MM月dd日 dddd", new System.Globalization.CultureInfo("zh-CN"));
            DrawAnalogClock(now);
        }

        private void DrawAnalogClock(DateTime now)
        {
            ClockCanvas.Children.Clear();

            double centerX = 125;
            double centerY = 125;
            double radius = 100;
            var accentBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8AB4F8"));
            var textBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8EAED"));
            var minorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5F6368"));

            var faceCircle = new Ellipse
            {
                Width = radius * 2,
                Height = radius * 2,
                Stroke = minorBrush,
                StrokeThickness = 2,
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#35363A"))
            };
            Canvas.SetLeft(faceCircle, centerX - radius);
            Canvas.SetTop(faceCircle, centerY - radius);
            ClockCanvas.Children.Add(faceCircle);

            for (int i = 0; i < 60; i++)
            {
                double angle = i * 6 * Math.PI / 180;
                bool isHour = i % 5 == 0;
                double len = isHour ? 15 : 5;
                double thickness = isHour ? 3 : 1;

                double x1 = centerX + (radius - len) * Math.Sin(angle);
                double y1 = centerY - (radius - len) * Math.Cos(angle);
                double x2 = centerX + radius * Math.Sin(angle);
                double y2 = centerY - radius * Math.Cos(angle);

                var line = new Line
                {
                    X1 = x1,
                    Y1 = y1,
                    X2 = x2,
                    Y2 = y2,
                    Stroke = isHour ? textBrush : minorBrush,
                    StrokeThickness = thickness
                };
                ClockCanvas.Children.Add(line);

                if (isHour)
                {
                    int hour = i / 5;
                    if (hour == 0) hour = 12;
                    var text = new TextBlock
                    {
                        Text = hour.ToString(),
                        Foreground = textBrush,
                        FontSize = 16,
                        FontWeight = FontWeights.SemiBold
                    };
                    double textRadius = radius - 30;
                    double tx = centerX + textRadius * Math.Sin(angle);
                    double ty = centerY - textRadius * Math.Cos(angle);
                    text.Measure(new Size(double.MaxValue, double.MaxValue));
                    Canvas.SetLeft(text, tx - text.DesiredSize.Width / 2);
                    Canvas.SetTop(text, ty - text.DesiredSize.Height / 2);
                    ClockCanvas.Children.Add(text);
                }
            }

            double hourAngle = (now.Hour % 12 + now.Minute / 60.0) * 30 * Math.PI / 180;
            double minuteAngle = (now.Minute + now.Second / 60.0) * 6 * Math.PI / 180;
            double secondAngle = now.Second * 6 * Math.PI / 180;

            var hourLine = new Line
            {
                X1 = centerX,
                Y1 = centerY,
                X2 = centerX + (radius - 45) * Math.Sin(hourAngle),
                Y2 = centerY - (radius - 45) * Math.Cos(hourAngle),
                Stroke = textBrush,
                StrokeThickness = 4,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round
            };
            ClockCanvas.Children.Add(hourLine);

            var minuteLine = new Line
            {
                X1 = centerX,
                Y1 = centerY,
                X2 = centerX + (radius - 25) * Math.Sin(minuteAngle),
                Y2 = centerY - (radius - 25) * Math.Cos(minuteAngle),
                Stroke = textBrush,
                StrokeThickness = 3,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round
            };
            ClockCanvas.Children.Add(minuteLine);

            var secondLine = new Line
            {
                X1 = centerX,
                Y1 = centerY,
                X2 = centerX + (radius - 15) * Math.Sin(secondAngle),
                Y2 = centerY - (radius - 15) * Math.Cos(secondAngle),
                Stroke = accentBrush,
                StrokeThickness = 1.5
            };
            ClockCanvas.Children.Add(secondLine);

            var centerDot = new Ellipse
            {
                Width = 8,
                Height = 8,
                Fill = accentBrush
            };
            Canvas.SetLeft(centerDot, centerX - 4);
            Canvas.SetTop(centerDot, centerY - 4);
            ClockCanvas.Children.Add(centerDot);
        }

        private void OnTabClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string tab)
            {
                ClockPanel.Visibility = Visibility.Collapsed;
                StopwatchPanel.Visibility = Visibility.Collapsed;
                TimerPanel.Visibility = Visibility.Collapsed;
                AlarmPanel.Visibility = Visibility.Collapsed;

                TabClock.Background = Brushes.Transparent;
                TabStopwatch.Background = Brushes.Transparent;
                TabTimer.Background = Brushes.Transparent;
                TabAlarm.Background = Brushes.Transparent;

                switch (tab)
                {
                    case "clock":
                        ClockPanel.Visibility = Visibility.Visible;
                        TabClock.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3C4043"));
                        break;
                    case "stopwatch":
                        StopwatchPanel.Visibility = Visibility.Visible;
                        TabStopwatch.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3C4043"));
                        break;
                    case "timer":
                        TimerPanel.Visibility = Visibility.Visible;
                        TabTimer.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3C4043"));
                        break;
                    case "alarm":
                        AlarmPanel.Visibility = Visibility.Visible;
                        TabAlarm.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3C4043"));
                        break;
                }
            }
        }

        private void OnStopwatchStartClick(object sender, RoutedEventArgs e)
        {
            if (!_stopwatchRunning)
            {
                _stopwatchRunning = true;
                StopwatchStartBtn.Content = "暂停";
                _stopwatchTimer.Start();
            }
            else
            {
                _stopwatchRunning = false;
                StopwatchStartBtn.Content = "继续";
                _stopwatchTimer.Stop();
            }
        }

        private void OnStopwatchTick(object sender, EventArgs e)
        {
            _stopwatchElapsed = _stopwatchElapsed.Add(TimeSpan.FromMilliseconds(10));
            UpdateStopwatchDisplay();
        }

        private void UpdateStopwatchDisplay()
        {
            StopwatchDisplay.Text = _stopwatchElapsed.ToString(@"mm\:ss");
            StopwatchMs.Text = "." + (_stopwatchElapsed.Milliseconds / 10).ToString("D2");
        }

        private void OnStopwatchLapClick(object sender, RoutedEventArgs e)
        {
            if (_stopwatchElapsed.TotalSeconds > 0)
            {
                _lapCount++;
                LapList.Items.Add($"计次 {_lapCount}: {_stopwatchElapsed:mm\\:ss\\.ff}");
            }
        }

        private void OnStopwatchResetClick(object sender, RoutedEventArgs e)
        {
            _stopwatchRunning = false;
            _stopwatchTimer.Stop();
            _stopwatchElapsed = TimeSpan.Zero;
            _lapCount = 0;
            StopwatchStartBtn.Content = "开始";
            UpdateStopwatchDisplay();
            LapList.Items.Clear();
        }

        private void OnTimerStartClick(object sender, RoutedEventArgs e)
        {
            if (!_timerRunning)
            {
                if (_timerRemaining.TotalSeconds <= 0)
                {
                    int h = int.TryParse(TimerHours.Text, out h) ? h : 0;
                    int m = int.TryParse(TimerMinutes.Text, out m) ? m : 0;
                    int s = int.TryParse(TimerSeconds.Text, out s) ? s : 0;
                    _timerRemaining = new TimeSpan(h, m, s);
                }

                if (_timerRemaining.TotalSeconds > 0)
                {
                    _timerRunning = true;
                    TimerStartBtn.Content = "暂停";
                    _timerCountdown.Start();
                }
            }
            else
            {
                _timerRunning = false;
                TimerStartBtn.Content = "继续";
                _timerCountdown.Stop();
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            _timerRemaining = _timerRemaining.Subtract(TimeSpan.FromSeconds(1));
            TimerDisplay.Text = _timerRemaining.ToString(@"hh\:mm\:ss");

            if (_timerRemaining.TotalSeconds <= 0)
            {
                _timerCountdown.Stop();
                _timerRunning = false;
                TimerStartBtn.Content = "开始";
                MessageBox.Show("计时器时间到！", "计时器", MessageBoxButton.OK, MessageBoxImage.Information);
                _timerRemaining = TimeSpan.Zero;
                TimerDisplay.Text = "00:00:00";
            }
        }

        private void OnTimerResetClick(object sender, RoutedEventArgs e)
        {
            _timerCountdown.Stop();
            _timerRunning = false;
            TimerStartBtn.Content = "开始";
            int h = int.TryParse(TimerHours.Text, out h) ? h : 0;
            int m = int.TryParse(TimerMinutes.Text, out m) ? m : 0;
            int s = int.TryParse(TimerSeconds.Text, out s) ? s : 0;
            _timerRemaining = new TimeSpan(h, m, s);
            TimerDisplay.Text = _timerRemaining.ToString(@"hh\:mm\:ss");
        }

        private void OnAddAlarmClick(object sender, RoutedEventArgs e)
        {
            int h = int.TryParse(AlarmHour.Text, out h) ? h : 0;
            int m = int.TryParse(AlarmMinute.Text, out m) ? m : 0;
            h = Math.Clamp(h, 0, 23);
            m = Math.Clamp(m, 0, 59);

            _alarms.Add(new Alarm
            {
                Time = new TimeSpan(h, m, 0),
                Label = $"闹钟 {h:D2}:{m:D2}"
            });

            AlarmList.Items.Add($"⏰ {h:D2}:{m:D2} - 已启用");
            MessageBox.Show($"闹钟已添加：{h:D2}:{m:D2}", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CheckAlarms()
        {
            var now = DateTime.Now.TimeOfDay;
            foreach (var alarm in _alarms)
            {
                if (alarm.IsEnabled && alarm.Time.Hours == now.Hours && alarm.Time.Minutes == now.Minutes && now.Seconds == 0)
                {
                    MessageBox.Show($"闹钟时间到：{alarm.Label}", "闹钟", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
    }
}
