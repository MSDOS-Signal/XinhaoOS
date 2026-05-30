using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChromeOS.Apps
{
    public partial class ContactsApp : UserControl
    {
        private class Contact
        {
            public string Name { get; set; } = "";
            public string Phone { get; set; } = "";
            public string Email { get; set; } = "";
            public string Company { get; set; } = "";
            public string Avatar { get; set; } = "👤";
        }

        private readonly List<Contact> _contacts = new List<Contact>();

        public ContactsApp()
        {
            InitializeComponent();
            LoadSampleContacts();
            RenderContacts(_contacts);
        }

        private void LoadSampleContacts()
        {
            _contacts.Add(new Contact { Name = "张三", Phone = "138-0000-0001", Email = "zhangsan@example.com", Company = "科技有限公司", Avatar = "👨‍💼" });
            _contacts.Add(new Contact { Name = "李四", Phone = "138-0000-0002", Email = "lisi@example.com", Company = "设计工作室", Avatar = "👩‍💻" });
            _contacts.Add(new Contact { Name = "王五", Phone = "138-0000-0003", Email = "wangwu@example.com", Company = "互联网集团", Avatar = "👨‍🔬" });
            _contacts.Add(new Contact { Name = "赵六", Phone = "138-0000-0004", Email = "zhaoliu@example.com", Company = "创业公司", Avatar = "👩‍🎨" });
            _contacts.Add(new Contact { Name = "钱七", Phone = "138-0000-0005", Email = "qianqi@example.com", Company = "教育机构", Avatar = "👨‍🏫" });
            _contacts.Add(new Contact { Name = "孙八", Phone = "138-0000-0006", Email = "sunba@example.com", Company = "金融机构", Avatar = "👩‍⚕️" });
            _contacts.Add(new Contact { Name = "周九", Phone = "138-0000-0007", Email = "zhoujiu@example.com", Company = "咨询公司", Avatar = "👨‍🍳" });
            _contacts.Add(new Contact { Name = "吴十", Phone = "138-0000-0008", Email = "wushi@example.com", Company = "媒体公司", Avatar = "👩‍🎤" });
            _contacts.Add(new Contact { Name = "陈一一", Phone = "138-0000-0009", Email = "chenyiyi@example.com", Company = "物流公司", Avatar = "👨‍✈️" });
            _contacts.Add(new Contact { Name = "刘二二", Phone = "138-0000-0010", Email = "liuerer@example.com", Company = "制造企业", Avatar = "👩‍🔧" });
        }

        private void RenderContacts(List<Contact> contacts)
        {
            ContactList.Children.Clear();

            if (contacts.Count == 0)
            {
                ContactList.Children.Add(new TextBlock
                {
                    Text = "没有找到联系人",
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")),
                    FontSize = 14,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 40, 0, 0)
                });
                return;
            }

            string lastLetter = "";
            var sorted = new List<Contact>(contacts);
            sorted.Sort((a, b) => string.Compare(a.Name, b.Name, ignoreCase: true));

            foreach (var contact in sorted)
            {
                var firstLetter = contact.Name[0].ToString();
                if (firstLetter != lastLetter)
                {
                    lastLetter = firstLetter;
                    ContactList.Children.Add(new TextBlock
                    {
                        Text = firstLetter,
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8AB4F8")),
                        FontSize = 14,
                        FontWeight = FontWeights.SemiBold,
                        Margin = new Thickness(0, 16, 0, 4)
                    });
                }

                ContactList.Children.Add(CreateContactItem(contact));
            }
        }

        private Border CreateContactItem(Contact contact)
        {
            var border = new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#35363A")),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(12, 10, 12, 10),
                Margin = new Thickness(0, 0, 0, 6),
                Cursor = System.Windows.Input.Cursors.Hand,
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5F6368")),
                BorderThickness = new Thickness(1)
            };

            border.MouseEnter += (s, e) => border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3C4043"));
            border.MouseLeave += (s, e) => border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#35363A"));

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60) });

            var avatar = new TextBlock
            {
                Text = contact.Avatar,
                FontSize = 28,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Grid.SetColumn(avatar, 0);
            grid.Children.Add(avatar);

            var infoPanel = new StackPanel { VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(12, 0, 0, 0) };
            infoPanel.Children.Add(new TextBlock
            {
                Text = contact.Name,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8EAED")),
                FontSize = 15,
                FontWeight = FontWeights.SemiBold
            });
            infoPanel.Children.Add(new TextBlock
            {
                Text = contact.Company,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")),
                FontSize = 12,
                Margin = new Thickness(0, 2, 0, 0)
            });
            Grid.SetColumn(infoPanel, 1);
            grid.Children.Add(infoPanel);

            var phoneText = new TextBlock
            {
                Text = contact.Phone,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")),
                FontSize = 12,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                TextTrimming = TextTrimming.CharacterEllipsis
            };
            Grid.SetColumn(phoneText, 2);
            grid.Children.Add(phoneText);

            border.Child = grid;
            border.MouseDown += (s, e) => ShowContactDetail(contact);

            return border;
        }

        private void ShowContactDetail(Contact contact)
        {
            MessageBox.Show(
                $"姓名：{contact.Name}\n电话：{contact.Phone}\n邮箱：{contact.Email}\n公司：{contact.Company}",
                contact.Name,
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }

        private void OnAddContactClick(object sender, RoutedEventArgs e)
        {
            var dialog = new Window
            {
                Title = "添加联系人",
                Width = 400,
                Height = 420,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Window.GetWindow(this),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#292A2D"))
            };

            var panel = new StackPanel { Margin = new Thickness(16) };

            var fields = new[]
            {
                new { Label = "姓名", Name = "name" },
                new { Label = "电话", Name = "phone" },
                new { Label = "邮箱", Name = "email" },
                new { Label = "公司", Name = "company" }
            };

            var textBoxes = new Dictionary<string, TextBox>();

            foreach (var field in fields)
            {
                panel.Children.Add(new TextBlock
                {
                    Text = field.Label,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")),
                    FontSize = 12,
                    Margin = new Thickness(0, 0, 0, 4)
                });
                var tb = new TextBox
                {
                    Style = (Style)FindResource("ChromeOSTextBox"),
                    Margin = new Thickness(0, 0, 0, 12)
                };
                textBoxes[field.Name] = tb;
                panel.Children.Add(tb);
            }

            var btnPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 8, 0, 0) };

            var saveBtn = new Button
            {
                Content = "保存",
                Style = (Style)FindResource("ChromeOSButton"),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8AB4F8")),
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202124")),
                FontWeight = FontWeights.SemiBold,
                Padding = new Thickness(24, 10, 24, 10),
                Margin = new Thickness(0, 0, 8, 0)
            };
            saveBtn.Click += (s, e2) =>
            {
                var newContact = new Contact
                {
                    Name = textBoxes["name"].Text.Trim(),
                    Phone = textBoxes["phone"].Text.Trim(),
                    Email = textBoxes["email"].Text.Trim(),
                    Company = textBoxes["company"].Text.Trim(),
                    Avatar = "👤"
                };

                if (string.IsNullOrEmpty(newContact.Name))
                {
                    MessageBox.Show("请输入联系人姓名", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _contacts.Add(newContact);
                RenderContacts(_contacts);
                MessageBox.Show($"联系人 {newContact.Name} 已添加", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                dialog.Close();
            };

            var cancelBtn = new Button
            {
                Content = "取消",
                Style = (Style)FindResource("ChromeOSButton"),
                Padding = new Thickness(24, 10, 24, 10)
            };
            cancelBtn.Click += (s, e2) => dialog.Close();

            btnPanel.Children.Add(saveBtn);
            btnPanel.Children.Add(cancelBtn);
            panel.Children.Add(btnPanel);

            dialog.Content = panel;
            dialog.ShowDialog();
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchBox.Text == "搜索联系人") return;

            var query = SearchBox.Text.ToLower();
            if (string.IsNullOrEmpty(query))
            {
                RenderContacts(_contacts);
                return;
            }

            var filtered = _contacts.FindAll(c =>
                c.Name.ToLower().Contains(query) ||
                c.Phone.Contains(query) ||
                c.Email.ToLower().Contains(query) ||
                c.Company.ToLower().Contains(query));

            RenderContacts(filtered);
        }

        private void OnSearchGotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && tb.Text == "搜索联系人")
            {
                tb.Text = "";
                tb.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8EAED"));
            }
        }

        private void OnSearchLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && string.IsNullOrEmpty(tb.Text))
            {
                tb.Text = "搜索联系人";
                tb.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6"));
                RenderContacts(_contacts);
            }
        }
    }
}
