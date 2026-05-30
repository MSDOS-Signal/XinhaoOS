using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChromeOS.Apps
{
    public partial class NewsApp : UserControl
    {
        private class NewsArticle
        {
            public string Title { get; set; } = "";
            public string Source { get; set; } = "";
            public string Time { get; set; } = "";
            public string Preview { get; set; } = "";
            public string Content { get; set; } = "";
            public string Category { get; set; } = "";
            public string Icon { get; set; } = "📰";
        }

        private readonly List<NewsArticle> _articles = new List<NewsArticle>();

        public NewsApp()
        {
            InitializeComponent();
            LoadArticles();
            RenderNews("recommend");
        }

        private void LoadArticles()
        {
            _articles.Add(new NewsArticle
            {
                Title = "ChromeOS 模拟器迎来重大更新，支持更多应用",
                Source = "科技日报",
                Time = "2小时前",
                Preview = "最新的 ChromeOS 模拟器版本增加了对 Android 应用的更好支持，用户可以在桌面环境中流畅运行移动应用...",
                Content = "最新的 ChromeOS 模拟器版本增加了对 Android 应用的更好支持，用户现在可以在桌面环境中流畅运行移动应用。\n\n更新亮点：\n• 改进了应用兼容性\n• 提升了运行性能\n• 优化了内存管理\n• 支持多窗口模式\n\n开发者可以通过新的 API 接口，更好地适配桌面环境。",
                Category = "tech",
                Icon = "💻"
            });
            _articles.Add(new NewsArticle
            {
                Title = "WPF 10 正式发布，带来全新设计系统",
                Source = "InfoQ",
                Time = "4小时前",
                Preview = "微软发布了 WPF 10 版本，引入了全新的 Fluent Design 设计系统和更好的高性能渲染引擎...",
                Content = "微软今天正式发布了 WPF 10 版本，这是 Windows Presentation Foundation 的重大更新。\n\n新功能包括：\n• 全新 Fluent Design 支持\n• 改进的渲染性能\n• 原生暗色主题\n• 更好的触摸屏支持\n• 现代化控件样式\n\n开发者可以通过 NuGet 包获取最新版本。",
                Category = "tech",
                Icon = "🖥"
            });
            _articles.Add(new NewsArticle
            {
                Title = "中国队在世界编程大赛中夺得金牌",
                Source = "体育新闻",
                Time = "5小时前",
                Preview = "在刚刚结束的国际编程大赛中，中国队凭借出色的算法能力和问题解决技巧，成功夺得团体金牌...",
                Content = "在刚刚结束的国际编程大赛中，中国队以满分成绩夺得团体金牌。\n\n比赛亮点：\n• 中国队解决所有8道题目\n• 用时最短\n• 代码质量最高\n\n这是中国队连续第三年获得该赛事的团体冠军。",
                Category = "sports",
                Icon = "🏆"
            });
            _articles.Add(new NewsArticle
            {
                Title = "AI 辅助编程工具改变开发方式",
                Source = "TechCrunch",
                Time = "6小时前",
                Preview = "随着 AI 技术的快速发展，越来越多的开发者开始使用 AI 辅助编程工具来提高工作效率...",
                Content = "AI 辅助编程工具正在改变软件开发的方式。\n\n主要趋势：\n• 代码自动生成\n• Bug 自动检测\n• 代码审查辅助\n• 文档自动生成\n\n调查显示，使用 AI 工具的开发者平均效率提升了 40%。",
                Category = "tech",
                Icon = "🤖"
            });
            _articles.Add(new NewsArticle
            {
                Title = "2026 年全球开发者大会在北京开幕",
                Source = "新华社",
                Time = "8小时前",
                Preview = "年度全球开发者大会今天在北京国家会议中心开幕，预计将有来自世界各地的超过 5000 名开发者参加...",
                Content = "2026 年全球开发者大会今天在北京国家会议中心隆重开幕。\n\n大会亮点：\n• 5000+ 参会者\n• 100+ 技术演讲\n• 30+ 工作坊\n• 开发者招聘会\n\n大会将持续三天，涵盖云计算、AI、移动开发等多个领域。",
                Category = "tech",
                Icon = "🎪"
            });
            _articles.Add(new NewsArticle
            {
                Title = "CBA 总决赛：北京队 vs 广东队",
                Source = "体育频道",
                Time = "10小时前",
                Preview = "今晚将迎来 CBA 总决赛的关键一战，北京队将在主场迎战广东队，目前双方战成 2:2 平...",
                Content = "今晚将迎来 CBA 总决赛第五场比赛。\n\n比赛信息：\n• 时间：今晚 19:30\n• 地点：北京五棵松体育馆\n• 比分：2:2 平\n\n这场比赛将是决定冠军归属的关键之战。",
                Category = "sports",
                Icon = "🏀"
            });
            _articles.Add(new NewsArticle
            {
                Title = "量子计算机实现 1000 量子比特突破",
                Source = "Nature",
                Time = "12小时前",
                Preview = "研究人员宣布成功构建了包含超过 1000 个量子比特的量子计算机，这是量子计算领域的重要里程碑...",
                Content = "研究团队今天宣布了一项重大突破：成功构建了包含 1024 个量子比特的量子计算机。\n\n突破意义：\n• 量子优势进一步证明\n• 可解决更复杂问题\n• 商业化进程加速\n\n专家表示，这标志着量子计算进入了一个新的发展阶段。",
                Category = "tech",
                Icon = "⚛"
            });
            _articles.Add(new NewsArticle
            {
                Title = "夏季奥运会筹备进展顺利",
                Source = "奥运官网",
                Time = "1天前",
                Preview = "2028 年洛杉矶奥运会筹备工作进展顺利，场馆建设已完成 60%，预计明年将进入最后冲刺阶段...",
                Content = "洛杉矶奥组委今天发布了最新的筹备进展报告。\n\n筹备进展：\n• 场馆建设完成 60%\n• 志愿者报名超过 10 万人\n• 门票销售即将启动\n• 火炬传递路线已确定\n\n奥组委表示，一切工作都在按计划顺利进行。",
                Category = "sports",
                Icon = "🏅"
            });
        }

        private void RenderNews(string category)
        {
            NewsList.Children.Clear();

            var filtered = category == "recommend"
                ? _articles
                : _articles.FindAll(a => a.Category == category);

            foreach (var article in filtered)
            {
                NewsList.Children.Add(CreateNewsCard(article));
            }
        }

        private Border CreateNewsCard(NewsArticle article)
        {
            var border = new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#35363A")),
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(16),
                Margin = new Thickness(0, 0, 0, 12),
                Cursor = System.Windows.Input.Cursors.Hand,
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5F6368")),
                BorderThickness = new Thickness(1)
            };

            border.MouseEnter += (s, e) => border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3C4043"));
            border.MouseLeave += (s, e) => border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#35363A"));

            var panel = new StackPanel();

            var headerPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 8) };
            headerPanel.Children.Add(new TextBlock
            {
                Text = article.Icon,
                FontSize = 20,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 8, 0)
            });
            headerPanel.Children.Add(new TextBlock
            {
                Text = article.Source,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8AB4F8")),
                FontSize = 12,
                FontWeight = FontWeights.SemiBold,
                VerticalAlignment = VerticalAlignment.Center
            });
            headerPanel.Children.Add(new TextBlock
            {
                Text = " · " + article.Time,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")),
                FontSize = 12,
                VerticalAlignment = VerticalAlignment.Center
            });
            panel.Children.Add(headerPanel);

            var titleText = new TextBlock
            {
                Text = article.Title,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8EAED")),
                FontSize = 16,
                FontWeight = FontWeights.SemiBold,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 8)
            };
            panel.Children.Add(titleText);

            var previewText = new TextBlock
            {
                Text = article.Preview,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")),
                FontSize = 13,
                TextWrapping = TextWrapping.Wrap,
                TextTrimming = TextTrimming.CharacterEllipsis,
                MaxHeight = 40
            };
            panel.Children.Add(previewText);

            border.Child = panel;
            border.MouseDown += (s, e) => ShowArticle(article);

            return border;
        }

        private void ShowArticle(NewsArticle article)
        {
            MessageBox.Show(
                $"来源：{article.Source} | {article.Time}\n\n{article.Content}",
                article.Title,
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }

        private void OnCategoryClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string cat)
            {
                RenderNews(cat);
            }
        }

        private void OnSearchGotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && tb.Text == "搜索新闻")
            {
                tb.Text = "";
                tb.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8EAED"));
            }
        }

        private void OnSearchLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && string.IsNullOrEmpty(tb.Text))
            {
                tb.Text = "搜索新闻";
                tb.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6"));
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
