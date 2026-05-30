using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChromeOS.Apps
{
    public partial class CalendarApp : UserControl
    {
        private DateTime _currentDate;
        private readonly List<CalendarEvent> _events = new List<CalendarEvent>();

        private class CalendarEvent
        {
            public DateTime Date { get; set; }
            public string Title { get; set; } = "";
            public string Description { get; set; } = "";
            public string Color { get; set; } = "#8AB4F8";
            public TimeSpan? StartTime { get; set; }
        }

        public CalendarApp()
        {
            InitializeComponent();
            _currentDate = DateTime.Today;
            LoadSampleEvents();
            RenderCalendar();
            RenderWeekdayHeaders();
        }

        private void LoadSampleEvents()
        {
            var today = DateTime.Today;

            _events.Add(new CalendarEvent
            {
                Date = today,
                Title = "团队站会",
                Description = "每日站会，讨论项目进度和阻塞问题",
                StartTime = new TimeSpan(9, 30, 0)
            });
            _events.Add(new CalendarEvent
            {
                Date = today,
                Title = "代码审查",
                Description = "审查新功能 Pull Request",
                StartTime = new TimeSpan(14, 0, 0),
                Color = "#F28B82"
            });
            _events.Add(new CalendarEvent
            {
                Date = today.AddDays(1),
                Title = "产品评审会议",
                Description = "评审Q2产品路线图",
                StartTime = new TimeSpan(10, 0, 0),
                Color = "#FDD663"
            });
            _events.Add(new CalendarEvent
            {
                Date = today.AddDays(2),
                Title = "项目截止日期",
                Description = "ChromeOS 模拟器 v1.0 发布",
                Color = "#F28B82"
            });
            _events.Add(new CalendarEvent
            {
                Date = today.AddDays(3),
                Title = "技术分享",
                Description = "分享 WPF 最佳实践",
                StartTime = new TimeSpan(15, 0, 0),
                Color = "#81C995"
            });
            _events.Add(new CalendarEvent
            {
                Date = today.AddDays(-1),
                Title = "需求讨论",
                Description = "讨论下一版本功能需求",
                StartTime = new TimeSpan(11, 0, 0)
            });
            _events.Add(new CalendarEvent
            {
                Date = today.AddDays(5),
                Title = "团队建设",
                Description = "团队聚餐活动",
                StartTime = new TimeSpan(18, 0, 0),
                Color = "#81C995"
            });
            _events.Add(new CalendarEvent
            {
                Date = today.AddDays(7),
                Title = "月度总结",
                Description = "6月工作总结与7月计划",
                StartTime = new TimeSpan(16, 0, 0)
            });
        }

        private void RenderWeekdayHeaders()
        {
            WeekdayGrid.Children.Clear();
            var weekdays = new[] { "日", "一", "二", "三", "四", "五", "六" };
            for (int i = 0; i < 7; i++)
            {
                WeekdayGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                var text = new TextBlock
                {
                    Text = weekdays[i],
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")),
                    FontSize = 12,
                    FontWeight = FontWeights.SemiBold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetColumn(text, i);
                WeekdayGrid.Children.Add(text);
            }
        }

        private void RenderCalendar()
        {
            MonthTitle.Text = _currentDate.ToString("yyyy年 MM月");

            var firstDayOfMonth = new DateTime(_currentDate.Year, _currentDate.Month, 1);
            int startDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
            int daysInMonth = DateTime.DaysInMonth(_currentDate.Year, _currentDate.Month);
            int daysInPrevMonth = DateTime.DaysInMonth(_currentDate.Year, _currentDate.Month - 1 > 0 ? _currentDate.Month - 1 : 12);

            CalendarGrid.Children.Clear();

            for (int row = 0; row < 6; row++)
            {
                CalendarGrid.RowDefinitions[row] = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
            }

            var today = DateTime.Today;
            var prevMonthBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5F6368"));
            var todayBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8AB4F8"));
            var textBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8EAED"));
            var hoverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3C4043"));

            int dayCounter = 0;

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    int dayIndex = row * 7 + col;
                    int dayNum;
                    bool isCurrentMonth = true;
                    DateTime cellDate;

                    if (dayIndex < startDayOfWeek)
                    {
                        dayNum = daysInPrevMonth - startDayOfWeek + dayIndex + 1;
                        isCurrentMonth = false;
                        var prevMonth = _currentDate.Month - 1;
                        var prevYear = _currentDate.Year;
                        if (prevMonth < 1)
                        {
                            prevMonth = 12;
                            prevYear--;
                        }
                        cellDate = new DateTime(prevYear, prevMonth, dayNum);
                    }
                    else if (dayIndex - startDayOfWeek >= daysInMonth)
                    {
                        dayNum = dayIndex - startDayOfWeek - daysInMonth + 1;
                        isCurrentMonth = false;
                        var nextMonth = _currentDate.Month + 1;
                        var nextYear = _currentDate.Year;
                        if (nextMonth > 12)
                        {
                            nextMonth = 1;
                            nextYear++;
                        }
                        cellDate = new DateTime(nextYear, nextMonth, dayNum);
                    }
                    else
                    {
                        dayNum = dayIndex - startDayOfWeek + 1;
                        cellDate = new DateTime(_currentDate.Year, _currentDate.Month, dayNum);
                    }

                    if (row == 5 && dayNum > daysInMonth && dayNum <= daysInMonth + 7)
                    {
                        if (dayIndex - startDayOfWeek >= daysInMonth)
                        {
                        }
                    }

                    var cellPanel = new StackPanel
                    {
                        Margin = new Thickness(2)
                    };

                    var border = new Border
                    {
                        Background = cellDate == today
                            ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3C4043"))
                            : Brushes.Transparent,
                        CornerRadius = new CornerRadius(8),
                        Padding = new Thickness(4),
                        Cursor = System.Windows.Input.Cursors.Hand
                    };
                    border.MouseEnter += (s, e) => border.Background = hoverBrush;
                    border.MouseLeave += (s, e) => border.Background = cellDate == today
                        ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3C4043"))
                        : Brushes.Transparent;

                    var dayPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(4, 2, 0, 2) };
                    var dayText = new TextBlock
                    {
                        Text = dayNum.ToString(),
                        FontSize = 14,
                        FontWeight = cellDate == today ? FontWeights.SemiBold : FontWeights.Normal,
                        Foreground = !isCurrentMonth ? prevMonthBrush : (cellDate == today ? todayBrush : textBrush)
                    };
                    dayPanel.Children.Add(dayText);
                    cellPanel.Children.Add(dayPanel);

                    var dayEvents = _events.FindAll(e => e.Date.Date == cellDate.Date);
                    foreach (var evt in dayEvents)
                    {
                        var eventBorder = new Border
                        {
                            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(evt.Color)),
                            CornerRadius = new CornerRadius(4),
                            Padding = new Thickness(4, 2, 4, 2),
                            Margin = new Thickness(4, 1, 4, 1),
                            Cursor = System.Windows.Input.Cursors.Hand
                        };
                        var eventText = new TextBlock
                        {
                            Text = evt.Title,
                            FontSize = 11,
                            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202124")),
                            FontWeight = FontWeights.Medium,
                            TextTrimming = TextTrimming.CharacterEllipsis
                        };
                        eventBorder.Child = eventText;
                        eventBorder.MouseDown += (s, e) => ShowEventDetail(evt);
                        cellPanel.Children.Add(eventBorder);
                    }

                    border.Child = cellPanel;
                    Grid.SetRow(border, row);
                    Grid.SetColumn(border, col);
                    CalendarGrid.Children.Add(border);

                    dayCounter++;
                }
            }
        }

        private void ShowEventDetail(CalendarEvent evt)
        {
            var timeStr = evt.StartTime.HasValue ? evt.StartTime.Value.ToString(@"hh\:mm") : "全天";
            MessageBox.Show(
                $"事件：{evt.Title}\n日期：{evt.Date:yyyy年MM月dd日}\n时间：{timeStr}\n\n描述：{evt.Description}",
                evt.Title,
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }

        private void OnPrevMonthClick(object sender, RoutedEventArgs e)
        {
            _currentDate = _currentDate.AddMonths(-1);
            RenderCalendar();
        }

        private void OnNextMonthClick(object sender, RoutedEventArgs e)
        {
            _currentDate = _currentDate.AddMonths(1);
            RenderCalendar();
        }

        private void OnTodayClick(object sender, RoutedEventArgs e)
        {
            _currentDate = DateTime.Today;
            RenderCalendar();
        }

        private void OnNewEventClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "新建事件功能\n\n请选择日历上的日期来添加事件。",
                "新建事件",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }
    }
}
