using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ChromeOS.Apps
{
    public partial class GmailApp : UserControl
    {
        private class Email
        {
            public string Sender { get; set; } = "";
            public string Subject { get; set; } = "";
            public string Preview { get; set; } = "";
            public string Date { get; set; } = "";
            public bool IsUnread { get; set; }
            public bool IsStarred { get; set; }
            public string Body { get; set; } = "";
            public string Folder { get; set; } = "inbox";
        }

        private readonly List<Email> _emails = new List<Email>();
        private string _currentFolder = "inbox";

        public GmailApp()
        {
            InitializeComponent();
            LoadSampleEmails();
            RefreshEmailList();
        }

        private void LoadSampleEmails()
        {
            _emails.Add(new Email
            {
                Sender = "Google Security",
                Subject = "安全提醒：新设备登录",
                Preview = "我们检测到您的 Google 账户在新设备上登录。如果这是您本人操作，请忽略此邮件...",
                Date = "上午10:32",
                IsUnread = true,
                IsStarred = true,
                Body = "我们检测到您的 Google 账户在新设备上登录。\n\n设备：ChromeOS Desktop\n位置：中国 北京\n时间：2026年5月30日 上午10:30\n\n如果这是您本人操作，请忽略此邮件。如果您没有进行此操作，请立即更改您的密码。\n\nGoogle 安全团队",
                Folder = "inbox"
            });
            _emails.Add(new Email
            {
                Sender = "张三",
                Subject = "项目进度更新",
                Preview = "你好，以下是本周的项目进度报告：1. 前端开发已完成80% 2. 后端API...",
                Date = "上午9:15",
                IsUnread = true,
                IsStarred = false,
                Body = "你好，\n\n以下是本周的项目进度报告：\n\n1. 前端开发已完成80%\n2. 后端API接口已全部完成\n3. 数据库优化进行中\n4. 测试用例编写完成50%\n\n请查阅附件获取详细信息。\n\n此致\n张三",
                Folder = "inbox"
            });
            _emails.Add(new Email
            {
                Sender = "GitHub",
                Subject = "[windows7] Pull Request #42: Fix build issues",
                Preview = "zhangsan has opened a pull request on windows7 repository. Fix build issues for .NET 10...",
                Date = "昨天",
                IsUnread = true,
                IsStarred = true,
                Body = "zhangsan has opened a pull request on windows7 repository.\n\nFix build issues for .NET 10\n\nChanges:\n- Update package references\n- Fix compilation warnings\n- Add missing dependencies\n\nView the pull request on GitHub.",
                Folder = "inbox"
            });
            _emails.Add(new Email
            {
                Sender = "李四",
                Subject = "周末聚餐安排",
                Preview = "这个周末大家有空一起吃个饭吗？我找到了一个不错的餐厅...",
                Date = "昨天",
                IsUnread = false,
                IsStarred = false,
                Body = "大家好！\n\n这个周末大家有空一起吃个饭吗？\n\n我找到了一个不错的餐厅，在朝阳区，评价很好。\n\n时间：周六晚上6点\n地点：XX餐厅\n\n能来的回复一下～\n\n李四",
                Folder = "inbox"
            });
            _emails.Add(new Email
            {
                Sender = "王五",
                Subject = "Re: 会议记录 - 5月29日",
                Preview = "收到，我会跟进这些任务的。关于第三点，我有一个建议...",
                Date = "5月29日",
                IsUnread = false,
                IsStarred = false,
                Body = "收到，我会跟进这些任务的。\n\n关于第三点，我有一个建议：我们可以考虑使用新的框架来提高效率。\n\n另外，下次会议时间可以提前到周三吗？\n\n王五",
                Folder = "inbox"
            });
            _emails.Add(new Email
            {
                Sender = "我",
                Subject = "项目提案 - ChromeOS 模拟器",
                Preview = "以下是我关于ChromeOS模拟器项目的初步提案...",
                Date = "5月28日",
                IsUnread = false,
                IsStarred = false,
                Body = "各位好，\n\n以下是我关于ChromeOS模拟器项目的初步提案：\n\n1. 项目背景\n2. 技术方案\n3. 开发计划\n4. 资源需求\n\n请查阅完整文档并反馈意见。\n\n谢谢！",
                Folder = "sent"
            });
            _emails.Add(new Email
            {
                Sender = "我",
                Subject = "Re: 代码审查反馈",
                Preview = "感谢反馈，我已经按照建议修改了代码...",
                Date = "5月27日",
                IsUnread = false,
                IsStarred = false,
                Body = "感谢反馈，我已经按照建议修改了代码：\n\n1. 重构了数据访问层\n2. 添加了单元测试\n3. 修复了性能问题\n\n请再次审查。",
                Folder = "sent"
            });
            _emails.Add(new Email
            {
                Sender = "我",
                Subject = "草稿：年度总结报告",
                Preview = "2026年度工作总结...\n\n本年度主要完成了以下工作...",
                Date = "5月26日",
                IsUnread = false,
                IsStarred = false,
                Body = "2026年度工作总结\n\n本年度主要完成了以下工作：\n\n1. 完成ChromeOS模拟器项目\n2. 优化系统性能\n3. 编写技术文档\n4. 参与团队建设",
                Folder = "drafts"
            });
            _emails.Add(new Email
            {
                Sender = "我",
                Subject = "草稿：产品需求文档 v2",
                Preview = "产品需求文档第二版...\n\n功能列表：\n1. 邮件管理\n2. 日历同步...",
                Date = "5月25日",
                IsUnread = false,
                IsStarred = false,
                Body = "产品需求文档 v2\n\n功能列表：\n1. 邮件管理\n2. 日历同步\n3. 文件存储\n4. 应用商店\n5. 系统设置",
                Folder = "drafts"
            });
            _emails.Add(new Email
            {
                Sender = "我",
                Subject = "草稿：团队建设方案",
                Preview = "关于2026年下半年团队建设活动的方案...",
                Date = "5月24日",
                IsUnread = false,
                IsStarred = false,
                Body = "关于2026年下半年团队建设活动的方案\n\n目标：增强团队凝聚力\n预算：50000元\n活动时间：9月份",
                Folder = "drafts"
            });
            _emails.Add(new Email
            {
                Sender = "赵六",
                Subject = "⭐ 重要：系统架构评审",
                Preview = "请参加下周一的系统架构评审会议，会议将讨论...",
                Date = "5月23日",
                IsUnread = false,
                IsStarred = true,
                Body = "请参加下周一的系统架构评审会议。\n\n时间：6月2日 下午2:00\n地点：会议室A\n议题：\n1. 微服务架构评估\n2. 数据库分片方案\n3. 缓存策略优化\n\n请提前准备相关材料。",
                Folder = "inbox"
            });
            _emails.Add(new Email
            {
                Sender = "促销",
                Subject = "🎉 限时优惠：ChromeOS Pro 版",
                Preview = "立即升级 ChromeOS Pro 版，享受更多高级功能...",
                Date = "5月22日",
                IsUnread = false,
                IsStarred = false,
                Body = "尊敬的用户：\n\n限时优惠！ChromeOS Pro 版现仅需 ¥99/年。\n\nPro 版功能：\n- 无限云存储\n- 高级安全保护\n- 优先技术支持\n- 专属主题\n\n立即升级！",
                Folder = "spam"
            });
        }

        private void RefreshEmailList()
        {
            EmailList.Children.Clear();

            var filtered = _currentFolder switch
            {
                "inbox" => _emails.FindAll(e => e.Folder == "inbox"),
                "starred" => _emails.FindAll(e => e.IsStarred),
                "sent" => _emails.FindAll(e => e.Folder == "sent"),
                "drafts" => _emails.FindAll(e => e.Folder == "drafts"),
                "spam" => _emails.FindAll(e => e.Folder == "spam"),
                "trash" => new List<Email>(),
                _ => _emails.FindAll(e => e.Folder == "inbox")
            };

            foreach (var email in filtered)
            {
                EmailList.Children.Add(CreateEmailItem(email));
            }

            if (filtered.Count == 0)
            {
                var emptyText = new TextBlock
                {
                    Text = "此文件夹为空",
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")),
                    FontSize = 14,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 40, 0, 0)
                };
                EmailList.Children.Add(emptyText);
            }
        }

        private Border CreateEmailItem(Email email)
        {
            var border = new Border
            {
                Background = email.IsUnread
                    ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#35363A"))
                    : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#292A2D")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5F6368")),
                BorderThickness = new Thickness(0, 0, 0, 1),
                Padding = new Thickness(16, 12, 16, 12),
                Cursor = Cursors.Hand
            };

            border.MouseEnter += (s, e) => border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3C4043"));
            border.MouseLeave += (s, e) => border.Background = email.IsUnread
                ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#35363A"))
                : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#292A2D"));

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });

            var starText = new TextBlock
            {
                Text = email.IsStarred ? "⭐" : "☆",
                FontSize = 16,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Cursor = Cursors.Hand
            };
            starText.MouseDown += (s, e) =>
            {
                email.IsStarred = !email.IsStarred;
                RefreshEmailList();
            };
            Grid.SetColumn(starText, 0);
            grid.Children.Add(starText);

            var senderText = new TextBlock
            {
                Text = email.Sender,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8EAED")),
                FontSize = 14,
                FontWeight = email.IsUnread ? FontWeights.SemiBold : FontWeights.Normal,
                VerticalAlignment = VerticalAlignment.Center,
                TextTrimming = TextTrimming.CharacterEllipsis
            };
            Grid.SetColumn(senderText, 1);
            grid.Children.Add(senderText);

            var contentPanel = new StackPanel { Orientation = Orientation.Vertical };
            var subjectText = new TextBlock
            {
                Text = email.Subject,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8EAED")),
                FontSize = 14,
                FontWeight = email.IsUnread ? FontWeights.SemiBold : FontWeights.Normal,
                TextTrimming = TextTrimming.CharacterEllipsis
            };
            var previewText = new TextBlock
            {
                Text = email.Preview,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")),
                FontSize = 12,
                TextTrimming = TextTrimming.CharacterEllipsis,
                Margin = new Thickness(0, 2, 0, 0)
            };
            contentPanel.Children.Add(subjectText);
            contentPanel.Children.Add(previewText);
            Grid.SetColumn(contentPanel, 2);
            grid.Children.Add(contentPanel);

            var dateText = new TextBlock
            {
                Text = email.Date,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")),
                FontSize = 12,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            Grid.SetColumn(dateText, 3);
            grid.Children.Add(dateText);

            border.Child = grid;
            border.MouseDown += (s, e) => ShowEmailDetail(email);

            return border;
        }

        private void ShowEmailDetail(Email email)
        {
            email.IsUnread = false;
            RefreshEmailList();

            MessageBox.Show(
                $"发件人：{email.Sender}\n主题：{email.Subject}\n日期：{email.Date}\n\n{email.Body}",
                email.Subject,
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }

        private void OnFolderClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string folder)
            {
                _currentFolder = folder;
                RefreshEmailList();
            }
        }

        private void OnComposeClick(object sender, RoutedEventArgs e)
        {
            var composeWindow = new Window
            {
                Title = "撰写邮件",
                Width = 600,
                Height = 500,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Window.GetWindow(this),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#292A2D"))
            };

            var panel = new StackPanel { Margin = new Thickness(16) };

            panel.Children.Add(new TextBlock
            {
                Text = "收件人",
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")),
                FontSize = 12,
                Margin = new Thickness(0, 0, 0, 4)
            });
            var toBox = new TextBox
            {
                Style = (Style)FindResource("ChromeOSTextBox"),
                Margin = new Thickness(0, 0, 0, 12)
            };
            panel.Children.Add(toBox);

            panel.Children.Add(new TextBlock
            {
                Text = "主题",
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")),
                FontSize = 12,
                Margin = new Thickness(0, 0, 0, 4)
            });
            var subjectBox = new TextBox
            {
                Style = (Style)FindResource("ChromeOSTextBox"),
                Margin = new Thickness(0, 0, 0, 12)
            };
            panel.Children.Add(subjectBox);

            panel.Children.Add(new TextBlock
            {
                Text = "正文",
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")),
                FontSize = 12,
                Margin = new Thickness(0, 0, 0, 4)
            });
            var bodyBox = new TextBox
            {
                Style = (Style)FindResource("ChromeOSTextBox"),
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
                Height = 200,
                Margin = new Thickness(0, 0, 0, 16)
            };
            panel.Children.Add(bodyBox);

            var btnPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
            var sendBtn = new Button
            {
                Content = "发送",
                Style = (Style)FindResource("ChromeOSButton"),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8AB4F8")),
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202124")),
                FontWeight = FontWeights.SemiBold,
                Padding = new Thickness(24, 10, 24, 10),
                Margin = new Thickness(0, 0, 8, 0)
            };
            sendBtn.Click += (s, e2) =>
            {
                MessageBox.Show("邮件已发送！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                composeWindow.Close();
            };
            var cancelBtn = new Button
            {
                Content = "取消",
                Style = (Style)FindResource("ChromeOSButton"),
                Padding = new Thickness(24, 10, 24, 10)
            };
            cancelBtn.Click += (s, e2) => composeWindow.Close();

            btnPanel.Children.Add(sendBtn);
            btnPanel.Children.Add(cancelBtn);
            panel.Children.Add(btnPanel);

            composeWindow.Content = panel;
            composeWindow.ShowDialog();
        }

        private void OnSearchGotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && tb.Text == "搜索邮件")
            {
                tb.Text = "";
                tb.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8EAED"));
            }
        }

        private void OnSearchLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && string.IsNullOrEmpty(tb.Text))
            {
                tb.Text = "搜索邮件";
                tb.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6"));
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
