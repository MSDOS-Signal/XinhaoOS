using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ChromeOS.Controls;

namespace ChromeOS.Apps
{
    public partial class FilesApp : UserControl
    {
        private class FileSystemItem
        {
            public string Name { get; set; }
            public bool IsFolder { get; set; }
            public string Size { get; set; }
            public string Modified { get; set; }
            public string Path { get; set; }
        }

        private readonly Stack<string> _navigationHistory = new Stack<string>();
        private string _currentPath = "My files";
        private bool _isGridView = true;
        private readonly Dictionary<string, List<FileSystemItem>> _fileSystem = new Dictionary<string, List<FileSystemItem>>
        {
            ["My files"] = new List<FileSystemItem>
            {
                new FileSystemItem { Name = "Documents", IsFolder = true, Size = "", Modified = "2024-01-15", Path = "Documents" },
                new FileSystemItem { Name = "Downloads", IsFolder = true, Size = "", Modified = "2024-01-20", Path = "Downloads" },
                new FileSystemItem { Name = "Images", IsFolder = true, Size = "", Modified = "2024-01-18", Path = "Images" },
                new FileSystemItem { Name = "Music", IsFolder = true, Size = "", Modified = "2024-01-10", Path = "Music" },
                new FileSystemItem { Name = "Videos", IsFolder = true, Size = "", Modified = "2024-01-12", Path = "Videos" },
                new FileSystemItem { Name = "readme.txt", IsFolder = false, Size = "2 KB", Modified = "2024-01-20", Path = "readme.txt" },
                new FileSystemItem { Name = "notes.md", IsFolder = false, Size = "1 KB", Modified = "2024-01-19", Path = "notes.md" }
            },
            ["Computer"] = new List<FileSystemItem>
            {
                new FileSystemItem { Name = "Local Disk (C:)", IsFolder = true, Size = "", Modified = "2024-01-20", Path = "Computer/C:" },
                new FileSystemItem { Name = "Local Disk (D:)", IsFolder = true, Size = "", Modified = "2024-01-20", Path = "Computer/D:" },
                new FileSystemItem { Name = "Network", IsFolder = true, Size = "", Modified = "2024-01-20", Path = "Computer/Network" },
                new FileSystemItem { Name = "Removable Storage", IsFolder = true, Size = "", Modified = "2024-01-18", Path = "Computer/Removable" }
            },
            ["Computer/C:"] = new List<FileSystemItem>
            {
                new FileSystemItem { Name = "Users", IsFolder = true, Size = "", Modified = "2024-01-20", Path = "Computer/C:/Users" },
                new FileSystemItem { Name = "Program Files", IsFolder = true, Size = "", Modified = "2024-01-20", Path = "Computer/C:/Program Files" },
                new FileSystemItem { Name = "Windows", IsFolder = true, Size = "", Modified = "2024-01-20", Path = "Computer/C:/Windows" },
                new FileSystemItem { Name = "ProgramData", IsFolder = true, Size = "", Modified = "2024-01-20", Path = "Computer/C:/ProgramData" }
            },
            ["Computer/D:"] = new List<FileSystemItem>
            {
                new FileSystemItem { Name = "Projects", IsFolder = true, Size = "", Modified = "2024-01-19", Path = "Computer/D:/Projects" },
                new FileSystemItem { Name = "Games", IsFolder = true, Size = "", Modified = "2024-01-18", Path = "Computer/D:/Games" },
                new FileSystemItem { Name = "Backup", IsFolder = true, Size = "", Modified = "2024-01-15", Path = "Computer/D:/Backup" }
            },
            ["Downloads"] = new List<FileSystemItem>
            {
                new FileSystemItem { Name = "installer.exe", IsFolder = false, Size = "45 MB", Modified = "2024-01-20", Path = "Downloads/installer.exe" },
                new FileSystemItem { Name = "document.pdf", IsFolder = false, Size = "2.5 MB", Modified = "2024-01-19", Path = "Downloads/document.pdf" },
                new FileSystemItem { Name = "photo.jpg", IsFolder = false, Size = "3.2 MB", Modified = "2024-01-18", Path = "Downloads/photo.jpg" },
                new FileSystemItem { Name = "archive.zip", IsFolder = false, Size = "128 MB", Modified = "2024-01-17", Path = "Downloads/archive.zip" },
                new FileSystemItem { Name = "song.mp3", IsFolder = false, Size = "5 MB", Modified = "2024-01-16", Path = "Downloads/song.mp3" }
            },
            ["Documents"] = new List<FileSystemItem>
            {
                new FileSystemItem { Name = "Work", IsFolder = true, Size = "", Modified = "2024-01-15", Path = "Documents/Work" },
                new FileSystemItem { Name = "Personal", IsFolder = true, Size = "", Modified = "2024-01-14", Path = "Documents/Personal" },
                new FileSystemItem { Name = "report.docx", IsFolder = false, Size = "1.2 MB", Modified = "2024-01-15", Path = "Documents/report.docx" },
                new FileSystemItem { Name = "spreadsheet.xlsx", IsFolder = false, Size = "856 KB", Modified = "2024-01-14", Path = "Documents/spreadsheet.xlsx" },
                new FileSystemItem { Name = "presentation.pptx", IsFolder = false, Size = "5.6 MB", Modified = "2024-01-13", Path = "Documents/presentation.pptx" }
            },
            ["Documents/Work"] = new List<FileSystemItem>
            {
                new FileSystemItem { Name = "project-plan.docx", IsFolder = false, Size = "2.1 MB", Modified = "2024-01-15", Path = "Documents/Work/project-plan.docx" },
                new FileSystemItem { Name = "meeting-notes.txt", IsFolder = false, Size = "45 KB", Modified = "2024-01-14", Path = "Documents/Work/meeting-notes.txt" },
                new FileSystemItem { Name = "budget.xlsx", IsFolder = false, Size = "1.5 MB", Modified = "2024-01-13", Path = "Documents/Work/budget.xlsx" }
            },
            ["Documents/Personal"] = new List<FileSystemItem>
            {
                new FileSystemItem { Name = "diary.txt", IsFolder = false, Size = "12 KB", Modified = "2024-01-14", Path = "Documents/Personal/diary.txt" },
                new FileSystemItem { Name = "resume.docx", IsFolder = false, Size = "256 KB", Modified = "2024-01-10", Path = "Documents/Personal/resume.docx" }
            },
            ["Images"] = new List<FileSystemItem>
            {
                new FileSystemItem { Name = "Vacation", IsFolder = true, Size = "", Modified = "2024-01-12", Path = "Images/Vacation" },
                new FileSystemItem { Name = "Screenshots", IsFolder = true, Size = "", Modified = "2024-01-18", Path = "Images/Screenshots" },
                new FileSystemItem { Name = "wallpaper.png", IsFolder = false, Size = "4.2 MB", Modified = "2024-01-18", Path = "Images/wallpaper.png" },
                new FileSystemItem { Name = "avatar.jpg", IsFolder = false, Size = "156 KB", Modified = "2024-01-15", Path = "Images/avatar.jpg" }
            },
            ["Images/Vacation"] = new List<FileSystemItem>
            {
                new FileSystemItem { Name = "beach.jpg", IsFolder = false, Size = "3.5 MB", Modified = "2024-01-12", Path = "Images/Vacation/beach.jpg" },
                new FileSystemItem { Name = "mountain.jpg", IsFolder = false, Size = "4.1 MB", Modified = "2024-01-12", Path = "Images/Vacation/mountain.jpg" },
                new FileSystemItem { Name = "sunset.jpg", IsFolder = false, Size = "3.8 MB", Modified = "2024-01-12", Path = "Images/Vacation/sunset.jpg" }
            },
            ["Images/Screenshots"] = new List<FileSystemItem>
            {
                new FileSystemItem { Name = "screenshot1.png", IsFolder = false, Size = "1.2 MB", Modified = "2024-01-18", Path = "Images/Screenshots/screenshot1.png" },
                new FileSystemItem { Name = "screenshot2.png", IsFolder = false, Size = "980 KB", Modified = "2024-01-17", Path = "Images/Screenshots/screenshot2.png" }
            },
            ["Music"] = new List<FileSystemItem>
            {
                new FileSystemItem { Name = "playlist1.mp3", IsFolder = false, Size = "5.2 MB", Modified = "2024-01-10", Path = "Music/playlist1.mp3" },
                new FileSystemItem { Name = "song1.mp3", IsFolder = false, Size = "4.8 MB", Modified = "2024-01-09", Path = "Music/song1.mp3" },
                new FileSystemItem { Name = "song2.flac", IsFolder = false, Size = "32 MB", Modified = "2024-01-08", Path = "Music/song2.flac" }
            },
            ["Videos"] = new List<FileSystemItem>
            {
                new FileSystemItem { Name = "tutorial.mp4", IsFolder = false, Size = "256 MB", Modified = "2024-01-12", Path = "Videos/tutorial.mp4" },
                new FileSystemItem { Name = "vlog.avi", IsFolder = false, Size = "512 MB", Modified = "2024-01-11", Path = "Videos/vlog.avi" }
            },
            ["Play Files"] = new List<FileSystemItem>
            {
                new FileSystemItem { Name = "Android", IsFolder = true, Size = "", Modified = "2024-01-20", Path = "Play Files/Android" },
                new FileSystemItem { Name = "app-backup.apk", IsFolder = false, Size = "45 MB", Modified = "2024-01-19", Path = "Play Files/app-backup.apk" }
            },
            ["Play Files/Android"] = new List<FileSystemItem>
            {
                new FileSystemItem { Name = "data", IsFolder = true, Size = "", Modified = "2024-01-20", Path = "Play Files/Android/data" },
                new FileSystemItem { Name = "obb", IsFolder = true, Size = "", Modified = "2024-01-20", Path = "Play Files/Android/obb" }
            },
            ["Play Files/Android/data"] = new List<FileSystemItem>(),
            ["Play Files/Android/obb"] = new List<FileSystemItem>()
        };

        public FilesApp(string? initialPath = null)
        {
            InitializeComponent();
            var path = !string.IsNullOrEmpty(initialPath) && _fileSystem.ContainsKey(initialPath) 
                ? initialPath 
                : "My files";
            LoadFiles(path);
        }

        private List<FileSystemItem> _allFiles = new List<FileSystemItem>();

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = SearchBox.Text;
            if (string.IsNullOrWhiteSpace(searchText))
            {
                LoadFiles(_currentPath);
            }
            else
            {
                SearchFiles(searchText);
            }
        }

        private void SearchFiles(string searchText)
        {
            FileList.Children.Clear();
            if (PathText.Content is TextBlock tb)
            {
                tb.Text = $"Search results for: {searchText}";
            }
            else
            {
                PathText.Content = $"Search results for: {searchText}";
            }

            var allItems = new List<FileSystemItem>();
            foreach (var path in _fileSystem.Keys)
            {
                foreach (var item in _fileSystem[path])
                {
                    if (item.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    {
                        allItems.Add(item);
                    }
                }
            }

            foreach (var item in allItems)
            {
                if (_isGridView)
                {
                    FileList.Children.Add(CreateGridItem(item));
                }
                else
                {
                    FileList.Children.Add(CreateListItem(item));
                }
            }

            if (allItems.Count == 0)
            {
                var noResults = new TextBlock
                {
                    Text = $"No files found matching '{searchText}'",
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")),
                    FontSize = 14,
                    Margin = new Thickness(16)
                };
                FileList.Children.Add(noResults);
            }
        }

        private void OnPathTextClick(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = "";
            LoadFiles(_currentPath);
        }

        private void LoadFiles(string path)
        {
            _currentPath = path;
            if (PathText.Content is TextBlock tb)
            {
                tb.Text = path;
            }
            else
            {
                PathText.Content = path;
            }
            FileList.Children.Clear();

            if (_fileSystem.ContainsKey(path))
            {
                foreach (var item in _fileSystem[path])
                {
                    if (_isGridView)
                    {
                        FileList.Children.Add(CreateGridItem(item));
                    }
                    else
                    {
                        FileList.Children.Add(CreateListItem(item));
                    }
                }
            }

            UpdateNavButtons();
        }

        private Button CreateGridItem(FileSystemItem item)
        {
            var btn = new Button
            {
                Style = (Style)FindResource("ChromeOSButton"),
                Width = 110,
                Height = 110,
                Margin = new Thickness(8),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Tag = item
            };

            var panel = new StackPanel { HorizontalAlignment = HorizontalAlignment.Center };
            var icon = new Path
            {
                Data = item.IsFolder 
                    ? Geometry.Parse("M10 4H4c-1.1 0-2 .9-2 2v12c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V8c0-1.1-.9-2-2-2h-8l-2-2z")
                    : GetFileIcon(item.Name),
                Fill = item.IsFolder ? (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8AB4F8")) : GetFileColor(item.Name),
                Width = 48,
                Height = 48,
                Stretch = Stretch.Uniform
            };
            var text = new TextBlock
            {
                Text = item.Name,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8EAED")),
                FontSize = 11,
                Margin = new Thickness(0, 6, 0, 0),
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                MaxWidth = 100
            };

            panel.Children.Add(icon);
            panel.Children.Add(text);
            btn.Content = panel;

            btn.Click += (s, e) => OnFileClick(item);
            btn.MouseRightButtonDown += (s, e) => ShowFileContextMenu(item, e);

            return btn;
        }

        private Border CreateListItem(FileSystemItem item)
        {
            var border = new Border
            {
                Width = 600,
                Height = 32,
                Margin = new Thickness(0, 1, 0, 1),
                Background = new SolidColorBrush(Colors.Transparent),
                Tag = item
            };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });

            var namePanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(8, 0, 0, 0) };
            var icon = new Path
            {
                Data = item.IsFolder
                    ? Geometry.Parse("M10 4H4c-1.1 0-2 .9-2 2v12c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V8c0-1.1-.9-2-2-2h-8l-2-2z")
                    : GetFileIcon(item.Name),
                Fill = item.IsFolder ? (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8AB4F8")) : GetFileColor(item.Name),
                Width = 18,
                Height = 18,
                Stretch = Stretch.Uniform,
                Margin = new Thickness(0, 0, 8, 0),
                VerticalAlignment = VerticalAlignment.Center
            };
            var nameText = new TextBlock
            {
                Text = item.Name,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8EAED")),
                FontSize = 13,
                VerticalAlignment = VerticalAlignment.Center
            };
            namePanel.Children.Add(icon);
            namePanel.Children.Add(nameText);

            var sizeText = new TextBlock
            {
                Text = item.IsFolder ? "" : item.Size,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")),
                FontSize = 12,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var dateText = new TextBlock
            {
                Text = item.Modified,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")),
                FontSize = 12,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            Grid.SetColumn(namePanel, 0);
            Grid.SetColumn(sizeText, 1);
            Grid.SetColumn(dateText, 2);
            grid.Children.Add(namePanel);
            grid.Children.Add(sizeText);
            grid.Children.Add(dateText);
            border.Child = grid;

            border.MouseLeftButtonDown += (s, e) =>
            {
                if (e.ClickCount == 2) OnFileClick(item);
            };
            border.MouseRightButtonDown += (s, e) => ShowFileContextMenu(item, e);
            border.MouseEnter += (s, e) => border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3C3D40"));
            border.MouseLeave += (s, e) => border.Background = new SolidColorBrush(Colors.Transparent);

            return border;
        }

        private Geometry GetFileIcon(string fileName)
        {
            var ext = System.IO.Path.GetExtension(fileName).ToLower();
            return ext switch
            {
                ".txt" or ".md" => Geometry.Parse("M14 2H6c-1.1 0-2 .9-2 2v16c0 1.1.9 2 2 2h12c1.1 0 2-.9 2-2V8l-6-6zm-1 7V3.5L18.5 9H13z"),
                ".jpg" or ".png" or ".gif" or ".bmp" => Geometry.Parse("M21 3H3c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h18c1.1 0 2-.9 2-2V5c0-1.1-.9-2-2-2zm0 16H3V5h18v14z"),
                ".mp3" or ".wav" or ".flac" => Geometry.Parse("M12 3v10.55A4 4 0 1 0 14 17V7h4V3h-6z"),
                ".mp4" or ".avi" or ".mkv" => Geometry.Parse("M18 4l2 4h-3l-2-4h-2l2 4h-3l-2-4H8l2 4H7L5 4H4c-1.1 0-2 .9-2 2v12c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V4h-4z"),
                ".pdf" => Geometry.Parse("M14 2H6c-1.1 0-2 .9-2 2v16c0 1.1.9 2 2 2h12c1.1 0 2-.9 2-2V8l-6-6zM6 20V4h7l5 5v11H6z"),
                ".zip" or ".rar" or ".7z" => Geometry.Parse("M20 6h-8l-2-2H4c-1.1 0-2 .9-2 2v12c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V8c0-1.1-.9-2-2-2zm-6 10H10v-2h4v2z"),
                ".exe" or ".msi" => Geometry.Parse("M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-2 15l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z"),
                _ => Geometry.Parse("M14 2H6c-1.1 0-2 .9-2 2v16c0 1.1.9 2 2 2h12c1.1 0 2-.9 2-2V8l-6-6zm-1 7V3.5L18.5 9H13z")
            };
        }

        private Brush GetFileColor(string fileName)
        {
            var ext = System.IO.Path.GetExtension(fileName).ToLower();
            return ext switch
            {
                ".txt" or ".md" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8AB4F8")),
                ".jpg" or ".png" or ".gif" or ".bmp" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDD663")),
                ".mp3" or ".wav" or ".flac" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F28B82")),
                ".mp4" or ".avi" or ".mkv" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F28B82")),
                ".pdf" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F28B82")),
                ".zip" or ".rar" or ".7z" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDD663")),
                ".exe" or ".msi" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#81C995")),
                _ => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6"))
            };
        }

        private void OnFileClick(FileSystemItem item)
        {
            if (item.IsFolder)
            {
                _navigationHistory.Push(_currentPath);
                LoadFiles(item.Name);
            }
            else
            {
                var ext = System.IO.Path.GetExtension(item.Name).ToLower();
                
                if (ext == ".mp3" || ext == ".wav" || ext == ".flac")
                {
                    var result = MessageBox.Show($"Play '{item.Name}'?", "Music Player", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        var appInfo = new ChromeOS.Models.AppInfo { Id = "music", Name = "Music", AppType = ChromeOS.Models.AppType.Music };
                        var parent = this.Parent as FrameworkElement;
                        while (parent != null && !(parent is ChromeOSWindow))
                        {
                            parent = parent.Parent as FrameworkElement;
                        }
                        if (parent is ChromeOSWindow window)
                        {
                            MessageBox.Show($"Playing: {item.Name}\n\nDuration: 3:45\nArtist: Unknown Artist\nAlbum: Unknown Album", "Now Playing", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                else if (ext == ".pdf" || ext == ".docx" || ext == ".txt" || ext == ".md")
                {
                    MessageBox.Show($"Opening: {item.Name}\n\nFile: {item.Name}\nSize: {item.Size}\nType: {GetFileTypeDescription(ext)}\nModified: {item.Modified}\n\nDocument preview would open here.", "Document Viewer", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (ext == ".jpg" || ext == ".png" || ext == ".gif" || ext == ".bmp")
                {
                    MessageBox.Show($"Opening: {item.Name}\n\nFile: {item.Name}\nSize: {item.Size}\nType: Image\nModified: {item.Modified}\n\nImage preview would open here.", "Photo Viewer", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (ext == ".mp4" || ext == ".avi" || ext == ".mkv")
                {
                    MessageBox.Show($"Opening: {item.Name}\n\nFile: {item.Name}\nSize: {item.Size}\nType: Video\nDuration: 15:30\nModified: {item.Modified}\n\nVideo player would open here.", "Video Player", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"Opening file: {item.Name}\n\nSize: {item.Size}\nModified: {item.Modified}\nPath: {item.Path}", "File Preview", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private string GetFileTypeDescription(string ext)
        {
            return ext switch
            {
                ".txt" or ".md" => "Text Document",
                ".docx" or ".doc" => "Word Document",
                ".pdf" => "PDF Document",
                ".xlsx" or ".xls" => "Spreadsheet",
                ".pptx" or ".ppt" => "Presentation",
                _ => "File"
            };
        }

        private void ShowFileContextMenu(FileSystemItem item, MouseButtonEventArgs e)
        {
            e.Handled = true;
            var menu = new ContextMenu();

            if (item.IsFolder)
            {
                var openItem = new MenuItem { Header = "Open" };
                openItem.Click += (s, args) => OnFileClick(item);
                menu.Items.Add(openItem);
            }
            else
            {
                var openItem = new MenuItem { Header = "Open" };
                openItem.Click += (s, args) => OnFileClick(item);
                menu.Items.Add(openItem);
            }

            menu.Items.Add(new Separator());

            var renameItem = new MenuItem { Header = "Rename" };
            renameItem.Click += (s, args) => RenameItem(item);
            menu.Items.Add(renameItem);

            var deleteItem = new MenuItem { Header = "Delete", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F28B82")) };
            deleteItem.Click += (s, args) => DeleteItem(item);
            menu.Items.Add(deleteItem);

            menu.Items.Add(new Separator());

            var infoItem = new MenuItem { Header = "Properties" };
            infoItem.Click += (s, args) => ShowProperties(item);
            menu.Items.Add(infoItem);

            menu.IsOpen = true;
        }

        private void RenameItem(FileSystemItem item)
        {
            var dialog = new InputDialog($"Enter new name for '{item.Name}':", "Rename", item.Name);
            if (dialog.ShowDialog() == true && !string.IsNullOrEmpty(dialog.Result) && dialog.Result != item.Name)
            {
                if (_fileSystem.ContainsKey(_currentPath))
                {
                    var fileItem = _fileSystem[_currentPath].Find(f => f.Name == item.Name);
                    if (fileItem != null)
                    {
                        fileItem.Name = dialog.Result;
                        LoadFiles(_currentPath);
                        MessageBox.Show($"Renamed to '{dialog.Result}'", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private void DeleteItem(FileSystemItem item)
        {
            var confirm = MessageBox.Show($"Are you sure you want to delete '{item.Name}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (confirm == MessageBoxResult.Yes)
            {
                if (_fileSystem.ContainsKey(_currentPath))
                {
                    _fileSystem[_currentPath].Remove(item);
                    LoadFiles(_currentPath);
                    MessageBox.Show($"Deleted '{item.Name}'", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void ShowProperties(FileSystemItem item)
        {
            var props = $"Name: {item.Name}\n";
            props += item.IsFolder ? "Type: Folder\n" : $"Type: File\n";
            if (!item.IsFolder) props += $"Size: {item.Size}\n";
            props += $"Modified: {item.Modified}\n";
            props += $"Path: {_currentPath}/{item.Name}";
            MessageBox.Show(props, "Properties", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            if (_navigationHistory.Count > 0)
            {
                var previousPath = _navigationHistory.Pop();
                LoadFiles(previousPath);
            }
        }

        private void OnUpClick(object sender, RoutedEventArgs e)
        {
            var parts = _currentPath.Split('/');
            if (parts.Length > 1)
            {
                var parentPath = string.Join("/", parts, 0, parts.Length - 1);
                _navigationHistory.Push(_currentPath);
                LoadFiles(parentPath);
            }
            else
            {
                _navigationHistory.Push(_currentPath);
                LoadFiles("My files");
            }
        }

        private void OnRefreshClick(object sender, RoutedEventArgs e)
        {
            LoadFiles(_currentPath);
        }

        private void OnNewFolderClick(object sender, RoutedEventArgs e)
        {
            var dialog = new InputDialog("Enter folder name:", "New Folder", "New Folder");
            if (dialog.ShowDialog() == true && !string.IsNullOrEmpty(dialog.Result))
            {
                if (!_fileSystem.ContainsKey(_currentPath))
                {
                    _fileSystem[_currentPath] = new List<FileSystemItem>();
                }
                _fileSystem[_currentPath].Add(new FileSystemItem
                {
                    Name = dialog.Result,
                    IsFolder = true,
                    Size = "",
                    Modified = DateTime.Now.ToString("yyyy-MM-dd"),
                    Path = $"{_currentPath}/{dialog.Result}"
                });
                LoadFiles(_currentPath);
                MessageBox.Show($"Created folder '{dialog.Result}'", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OnNewFileClick(object sender, RoutedEventArgs e)
        {
            var dialog = new InputDialog("Enter file name:", "New File", "newfile.txt");
            if (dialog.ShowDialog() == true && !string.IsNullOrEmpty(dialog.Result))
            {
                if (!_fileSystem.ContainsKey(_currentPath))
                {
                    _fileSystem[_currentPath] = new List<FileSystemItem>();
                }
                _fileSystem[_currentPath].Add(new FileSystemItem
                {
                    Name = dialog.Result,
                    IsFolder = false,
                    Size = "0 KB",
                    Modified = DateTime.Now.ToString("yyyy-MM-dd"),
                    Path = $"{_currentPath}/{dialog.Result}"
                });
                LoadFiles(_currentPath);
                MessageBox.Show($"Created file '{dialog.Result}'", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OnViewToggleClick(object sender, RoutedEventArgs e)
        {
            _isGridView = !_isGridView;
            ViewIcon.Text = _isGridView ? "▦" : "☰";
            LoadFiles(_currentPath);
        }

        private void OnNavMyFilesClick(object sender, RoutedEventArgs e)
        {
            _navigationHistory.Push(_currentPath);
            LoadFiles("My files");
            UpdateNavButtons(sender as Button);
        }

        private void OnNavDownloadsClick(object sender, RoutedEventArgs e)
        {
            _navigationHistory.Push(_currentPath);
            LoadFiles("Downloads");
            UpdateNavButtons(sender as Button);
        }

        private void OnNavPlayFilesClick(object sender, RoutedEventArgs e)
        {
            _navigationHistory.Push(_currentPath);
            LoadFiles("Play Files");
            UpdateNavButtons(sender as Button);
        }

        private void OnNavImagesClick(object sender, RoutedEventArgs e)
        {
            _navigationHistory.Push(_currentPath);
            LoadFiles("Images");
            UpdateNavButtons(sender as Button);
        }

        private void OnNavDocumentsClick(object sender, RoutedEventArgs e)
        {
            _navigationHistory.Push(_currentPath);
            LoadFiles("Documents");
            UpdateNavButtons(sender as Button);
        }

        private void OnNavMusicClick(object sender, RoutedEventArgs e)
        {
            _navigationHistory.Push(_currentPath);
            LoadFiles("Music");
            UpdateNavButtons(sender as Button);
        }

        private void UpdateNavButtonStyles(Button selectedButton)
        {
            NavMyFiles.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#35363A"));
            NavDownloads.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#35363A"));
            NavPlayFiles.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#35363A"));
            NavImages.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#35363A"));
            NavDocuments.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#35363A"));
            NavMusic.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#35363A"));

            if (selectedButton != null)
            {
                selectedButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3C3D40"));
            }
        }

        private void UpdateNavButtons(object? sender = null)
        {
            UpdateNavButtonStyles(sender as Button);
        }
    }
}
