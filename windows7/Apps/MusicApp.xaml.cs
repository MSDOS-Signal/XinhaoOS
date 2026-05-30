using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace ChromeOS.Apps
{
    public partial class MusicApp : UserControl
    {
        private class Song
        {
            public string Title { get; set; }
            public string Artist { get; set; }
            public string Album { get; set; }
            public string Duration { get; set; }
            public string Icon { get; set; }
            public bool IsFavorite { get; set; }
        }

        private readonly List<Song> _allSongs = new()
        {
            new Song { Title = "Bohemian Rhapsody", Artist = "Queen", Album = "A Night at the Opera", Duration = "5:55", Icon = "🎸", IsFavorite = true },
            new Song { Title = "Stairway to Heaven", Artist = "Led Zeppelin", Album = "Led Zeppelin IV", Duration = "8:02", Icon = "🎸" },
            new Song { Title = "Imagine", Artist = "John Lennon", Album = "Imagine", Duration = "3:07", Icon = "🎹", IsFavorite = true },
            new Song { Title = "Hotel California", Artist = "Eagles", Album = "Hotel California", Duration = "6:30", Icon = "🎸" },
            new Song { Title = "Smells Like Teen Spirit", Artist = "Nirvana", Album = "Nevermind", Duration = "5:01", Icon = "🎸", IsFavorite = true },
            new Song { Title = "Yesterday", Artist = "The Beatles", Album = "Help!", Duration = "2:05", Icon = "🎹" },
            new Song { Title = "Billie Jean", Artist = "Michael Jackson", Album = "Thriller", Duration = "4:54", Icon = "🎤" },
            new Song { Title = "Sweet Child O' Mine", Artist = "Guns N' Roses", Album = "Appetite for Destruction", Duration = "5:56", Icon = "🎸" },
            new Song { Title = "Wonderwall", Artist = "Oasis", Album = "Morning Glory", Duration = "4:18", Icon = "🎸", IsFavorite = true },
            new Song { Title = "Creep", Artist = "Radiohead", Album = "Pablo Honey", Duration = "3:56", Icon = "🎸" },
            new Song { Title = "Lose Yourself", Artist = "Eminem", Album = "8 Mile", Duration = "5:20", Icon = "🎤" },
            new Song { Title = "Shape of You", Artist = "Ed Sheeran", Album = "Divide", Duration = "3:53", Icon = "🎤", IsFavorite = true },
            new Song { Title = "Blinding Lights", Artist = "The Weeknd", Album = "After Hours", Duration = "3:20", Icon = "🎹" },
            new Song { Title = "Someone Like You", Artist = "Adele", Album = "21", Duration = "4:45", Icon = "🎤" },
            new Song { Title = "Despacito", Artist = "Luis Fonsi", Album = "Vida", Duration = "4:42", Icon = "🎵" }
        };

        private readonly List<Song> _favorites = new();
        private readonly List<Song> _recentlyPlayed = new();
        private int _currentSongIndex = -1;
        private bool _isPlaying = false;
        private bool _isShuffle = false;
        private int _repeatMode = 0;
        private List<Song> _currentPlaylist;
        private DispatcherTimer? _progressTimer;
        private double _currentProgress;

        public MusicApp()
        {
            InitializeComponent();
            _currentPlaylist = _allSongs;
            LoadSongs(_allSongs);
            InitializeFavorites();

            _progressTimer = new DispatcherTimer();
            _progressTimer.Interval = TimeSpan.FromSeconds(1);
            _progressTimer.Tick += OnProgressTick;
        }

        private void InitializeFavorites()
        {
            _favorites.Clear();
            foreach (var song in _allSongs)
            {
                if (song.IsFavorite)
                    _favorites.Add(song);
            }
        }

        private void LoadSongs(List<Song> songs)
        {
            SongList.Children.Clear();
            for (int i = 0; i < songs.Count; i++)
            {
                SongList.Children.Add(CreateSongRow(songs[i], i));
            }
        }

        private Border CreateSongRow(Song song, int index)
        {
            var border = new Border
            {
                Background = new SolidColorBrush(Colors.Transparent),
                CornerRadius = new CornerRadius(8),
                Margin = new Thickness(0, 2, 0, 2),
                Padding = new Thickness(12, 8, 12, 8),
                Cursor = System.Windows.Input.Cursors.Hand
            };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60) });

            var indexText = new TextBlock
            {
                Text = $"{index + 1}",
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")),
                FontSize = 13,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var titlePanel = new StackPanel { Orientation = Orientation.Horizontal, VerticalAlignment = VerticalAlignment.Center };
            var icon = new TextBlock { Text = song.Icon, FontSize = 20, Margin = new Thickness(0, 0, 12, 0) };
            var titleStack = new StackPanel { VerticalAlignment = VerticalAlignment.Center };
            var title = new TextBlock { Text = song.Title, Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8EAED")), FontSize = 14, FontWeight = FontWeight.FromOpenTypeWeight(500) };
            var artist = new TextBlock { Text = song.Artist, Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")), FontSize = 12 };
            titleStack.Children.Add(title);
            titleStack.Children.Add(artist);
            titlePanel.Children.Add(icon);
            titlePanel.Children.Add(titleStack);

            var albumText = new TextBlock
            {
                Text = song.Album,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")),
                FontSize = 12,
                VerticalAlignment = VerticalAlignment.Center
            };

            var durationText = new TextBlock
            {
                Text = song.Duration,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")),
                FontSize = 12,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            Grid.SetColumn(indexText, 0);
            Grid.SetColumn(titlePanel, 1);
            Grid.SetColumn(albumText, 2);
            Grid.SetColumn(durationText, 3);
            grid.Children.Add(indexText);
            grid.Children.Add(titlePanel);
            grid.Children.Add(albumText);
            grid.Children.Add(durationText);
            border.Child = grid;

            border.MouseLeftButtonDown += (s, e) =>
            {
                if (e.ClickCount == 2)
                {
                    PlaySong(song, index);
                }
            };
            border.MouseEnter += (s, e) => border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3C3D40"));
            border.MouseLeave += (s, e) => border.Background = new SolidColorBrush(Colors.Transparent);

            return border;
        }

        private void PlaySong(Song song, int index)
        {
            _currentSongIndex = index;
            _isPlaying = true;
            PlayPauseIcon.Text = "⏸️";
            CurrentSongTitle.Text = song.Title;
            CurrentSongArtist.Text = song.Artist;
            AlbumArt.Text = song.Icon;
            TotalTimeText.Text = song.Duration;
            ProgressSlider.Value = 0;
            _currentProgress = 0;

            if (!_recentlyPlayed.Contains(song))
            {
                _recentlyPlayed.Insert(0, song);
                if (_recentlyPlayed.Count > 20)
                    _recentlyPlayed.RemoveAt(_recentlyPlayed.Count - 1);
            }

            _progressTimer?.Start();
        }

        private void OnProgressTick(object? sender, EventArgs e)
        {
            if (_currentSongIndex >= 0 && _currentSongIndex < _currentPlaylist.Count)
            {
                var song = _currentPlaylist[_currentSongIndex];
                var parts = song.Duration.Split(':');
                var totalSeconds = int.Parse(parts[0]) * 60 + int.Parse(parts[1]);
                _currentProgress += 1;

                if (_currentProgress >= totalSeconds)
                {
                    if (_repeatMode == 2)
                    {
                        _currentProgress = 0;
                    }
                    else
                    {
                        OnNextClick(this, new RoutedEventArgs());
                        return;
                    }
                }

                var currentMin = (int)_currentProgress / 60;
                var currentSec = (int)_currentProgress % 60;
                CurrentTimeText.Text = $"{currentMin}:{currentSec:D2}";
                ProgressSlider.Value = (_currentProgress / totalSeconds) * 100;
            }
        }

        private void OnPlayPauseClick(object sender, RoutedEventArgs e)
        {
            _isPlaying = !_isPlaying;
            PlayPauseIcon.Text = _isPlaying ? "⏸️" : "▶️";
            
            if (_isPlaying && _currentSongIndex < 0 && _currentPlaylist.Count > 0)
            {
                PlaySong(_currentPlaylist[0], 0);
            }

            if (_isPlaying)
                _progressTimer?.Start();
            else
                _progressTimer?.Stop();
        }

        private void OnPreviousClick(object sender, RoutedEventArgs e)
        {
            if (_currentPlaylist.Count == 0) return;
            
            if (_currentSongIndex > 0)
            {
                PlaySong(_currentPlaylist[_currentSongIndex - 1], _currentSongIndex - 1);
            }
            else
            {
                PlaySong(_currentPlaylist[_currentPlaylist.Count - 1], _currentPlaylist.Count - 1);
            }
        }

        private void OnNextClick(object sender, RoutedEventArgs e)
        {
            if (_currentPlaylist.Count == 0) return;

            int nextIndex;
            if (_isShuffle)
            {
                var rand = new Random();
                nextIndex = rand.Next(0, _currentPlaylist.Count);
            }
            else
            {
                nextIndex = (_currentSongIndex + 1) % _currentPlaylist.Count;
            }

            PlaySong(_currentPlaylist[nextIndex], nextIndex);
        }

        private void OnShuffleClick(object sender, RoutedEventArgs e)
        {
            _isShuffle = !_isShuffle;
            MessageBox.Show(_isShuffle ? "Shuffle mode enabled" : "Shuffle mode disabled", "Shuffle", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnRepeatClick(object sender, RoutedEventArgs e)
        {
            _repeatMode = (_repeatMode + 1) % 3;
            RepeatIcon.Text = _repeatMode switch
            {
                0 => "🔁",
                1 => "🔁",
                2 => "🔂",
                _ => "🔁"
            };
            var msg = _repeatMode switch
            {
                0 => "Repeat off",
                1 => "Repeat all",
                2 => "Repeat one",
                _ => ""
            };
            MessageBox.Show(msg, "Repeat", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnProgressChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_currentSongIndex >= 0 && _currentPlaylist.Count > 0)
            {
                var song = _currentPlaylist[_currentSongIndex];
                var parts = song.Duration.Split(':');
                var totalSeconds = int.Parse(parts[0]) * 60 + int.Parse(parts[1]);
                _currentProgress = (e.NewValue / 100) * totalSeconds;
                var currentMin = (int)_currentProgress / 60;
                var currentSec = (int)_currentProgress % 60;
                CurrentTimeText.Text = $"{currentMin}:{currentSec:D2}";
            }
        }

        private void OnMuteClick(object sender, RoutedEventArgs e)
        {
            var currentVol = VolumeSlider.Value;
            VolumeSlider.Value = currentVol > 0 ? 0 : 80;
            VolumeIcon.Text = VolumeSlider.Value == 0 ? "🔇" : "🔊";
        }

        private void OnAllSongsClick(object sender, RoutedEventArgs e)
        {
            _currentPlaylist = _allSongs;
            LoadSongs(_allSongs);
        }

        private void OnFavoritesClick(object sender, RoutedEventArgs e)
        {
            InitializeFavorites();
            _currentPlaylist = _favorites;
            LoadSongs(_favorites);
        }

        private void OnRecentClick(object sender, RoutedEventArgs e)
        {
            _currentPlaylist = _recentlyPlayed;
            LoadSongs(_recentlyPlayed);
        }

        private void OnPlaylistClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Playlist Manager\n\nYou can create, edit, and manage your playlists here.\n\nFeatures:\n- Create new playlists\n- Add/remove songs\n- Reorder songs\n- Share playlists", "Playlists", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnCreatePlaylistClick(object sender, RoutedEventArgs e)
        {
            var dialog = new Controls.InputDialog("Enter playlist name:", "New Playlist", "My Playlist");
            if (dialog.ShowDialog() == true && !string.IsNullOrEmpty(dialog.Result))
            {
                MessageBox.Show($"Playlist '{dialog.Result}' created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
