using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ChromeOS.Controls
{
    public class InputDialog : Window
    {
        private TextBox _textBox;
        public string Result { get; private set; }

        public InputDialog(string prompt, string title, string defaultValue = "")
        {
            Title = title;
            Width = 400;
            Height = 200;
            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#292A2D"));

            var mainPanel = new StackPanel { Margin = new Thickness(20) };

            var promptText = new TextBlock
            {
                Text = prompt,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8EAED")),
                FontSize = 14,
                Margin = new Thickness(0, 0, 0, 12),
                TextWrapping = TextWrapping.Wrap
            };
            mainPanel.Children.Add(promptText);

            _textBox = new TextBox
            {
                Text = defaultValue,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#35363A")),
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8EAED")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5F6368")),
                BorderThickness = new Thickness(1),
                Padding = new Thickness(8),
                FontSize = 14,
                Margin = new Thickness(0, 0, 0, 16)
            };
            _textBox.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    Result = _textBox.Text;
                    DialogResult = true;
                    Close();
                }
            };
            mainPanel.Children.Add(_textBox);

            var buttonPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };

            var cancelBtn = new Button
            {
                Content = "Cancel",
                Style = (Style)FindResource("ChromeOSButton"),
                Width = 80,
                Height = 32,
                Margin = new Thickness(0, 0, 8, 0)
            };
            cancelBtn.Click += (s, e) =>
            {
                Result = "";
                DialogResult = false;
                Close();
            };
            buttonPanel.Children.Add(cancelBtn);

            var okBtn = new Button
            {
                Content = "OK",
                Style = (Style)FindResource("ChromeOSButton"),
                Width = 80,
                Height = 32,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8AB4F8"))
            };
            okBtn.Click += (s, e) =>
            {
                Result = _textBox.Text;
                DialogResult = true;
                Close();
            };
            buttonPanel.Children.Add(okBtn);

            mainPanel.Children.Add(buttonPanel);
            Content = mainPanel;

            Loaded += (s, e) =>
            {
                _textBox.Focus();
                _textBox.SelectAll();
            };
        }
    }
}
