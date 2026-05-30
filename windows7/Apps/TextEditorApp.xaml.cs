using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using ChromeOS.Controls;
using ChromeOS.Services;

namespace ChromeOS.Apps
{
    public partial class TextEditorApp : UserControl
    {
        private string _currentFile = "";
        private bool _isWordWrap = false;

        public TextEditorApp(string? fileName = null)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(fileName))
            {
                _currentFile = fileName;
                var parent = this.Parent as FrameworkElement;
                while (parent != null && !(parent is ChromeOSWindow))
                {
                    parent = parent.Parent as FrameworkElement;
                }
                if (parent is ChromeOSWindow window)
                {
                    window.SetTitle(fileName);
                }
                var content = PersistenceService.LoadTextFile(fileName);
                if (!string.IsNullOrEmpty(content))
                {
                    Editor.Text = content;
                }
            }
            Editor.Focus();
            UpdateWordCounts();
        }

        private void OnEditorTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateWordCounts();
        }

        private void OnEditorSelectionChanged(object sender, RoutedEventArgs e)
        {
            var idx = Editor.SelectionStart;
            var text = Editor.Text;
            var line = text.Substring(0, idx).Split('\n').Length;
            var col = idx - text.LastIndexOf('\n', Math.Max(0, idx - 1));
            LineColText.Text = $"Ln {line}, Col {col}";
        }

        private void UpdateWordCounts()
        {
            var text = Editor.Text.Trim();
            var words = string.IsNullOrEmpty(text) ? 0 : text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
            var chars = text.Length;
            WordCountText.Text = $"{words} words";
            CharCountText.Text = $"{chars} chars";
        }

        private void OnNewClick(object sender, RoutedEventArgs e)
        {
            Editor.Text = "";
            _currentFile = "";
            FilePathText.Text = "Untitled";
            Editor.Focus();
        }

        private void OnOpenClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt;*.md;*.cs;*.json;*.xml;*.html)|*.txt;*.md;*.cs;*.json;*.xml;*.html|All files (*.*)|*.*"
            };
            if (dialog.ShowDialog() == true)
            {
                Editor.Text = File.ReadAllText(dialog.FileName);
                _currentFile = dialog.FileName;
                FilePathText.Text = Path.GetFileName(_currentFile);
                Editor.Focus();
            }
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_currentFile))
            {
                OnSaveAsClick(sender, e);
            }
            else
            {
                PersistenceService.SaveTextFile(_currentFile, Editor.Text);
                ShowStatusMessage("File saved successfully.");
            }
        }

        private void OnSaveAsClick(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|Markdown (*.md)|*.md|All files (*.*)|*.*"
            };
            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, Editor.Text);
                _currentFile = dialog.FileName;
                FilePathText.Text = Path.GetFileName(_currentFile);
                ShowStatusMessage("File saved successfully.");
            }
        }

        private void OnUndoClick(object sender, RoutedEventArgs e)
        {
            if (Editor.CanUndo)
            {
                Editor.Undo();
            }
        }

        private void OnRedoClick(object sender, RoutedEventArgs e)
        {
            if (Editor.CanRedo)
            {
                Editor.Redo();
            }
        }

        private void OnWordWrapClick(object sender, RoutedEventArgs e)
        {
            _isWordWrap = !_isWordWrap;
            Editor.TextWrapping = _isWordWrap ? TextWrapping.Wrap : TextWrapping.NoWrap;
            Editor.HorizontalScrollBarVisibility = _isWordWrap ? ScrollBarVisibility.Disabled : ScrollBarVisibility.Auto;
            WordWrapText.Text = _isWordWrap ? "Word Wrap: On" : "Word Wrap: Off";
            WordWrapText.Foreground = _isWordWrap ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8AB4F8")) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6"));
        }

        private void ShowStatusMessage(string message)
        {
            // Could be extended to show toast notifications
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                switch (e.Key)
                {
                    case Key.S:
                        e.Handled = true;
                        OnSaveClick(sender, e);
                        break;
                    case Key.N:
                        e.Handled = true;
                        OnNewClick(sender, e);
                        break;
                    case Key.O:
                        e.Handled = true;
                        OnOpenClick(sender, e);
                        break;
                    case Key.Z:
                        e.Handled = true;
                        OnUndoClick(sender, e);
                        break;
                    case Key.Y:
                        e.Handled = true;
                        OnRedoClick(sender, e);
                        break;
                }
            }
        }

        private string GetDocumentsPath()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var docsPath = Path.Combine(appData, "ChromeOS", "Documents");
            Directory.CreateDirectory(docsPath);
            return docsPath;
        }
    }
}
