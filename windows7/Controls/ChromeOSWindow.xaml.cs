using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ChromeOS.Models;

namespace ChromeOS.Controls
{
    public partial class ChromeOSWindow : UserControl
    {
        public AppInfo AppInfo { get; set; } = new();
        public Action<ChromeOSWindow>? CloseRequested { get; set; }
        public Action<ChromeOSWindow>? MinimizeRequested { get; set; }
        
        private bool _isDragging;
        private Point _dragStartScreen;
        private Point _windowStart;
        private bool _isMaximized;
        private Rect _restoreBounds;
        private ResizeDirection? _resizeDirection;
        private Point _resizeStart;
        private Size _resizeStartSize;

        private enum ResizeDirection
        {
            Left, Right, Top, Bottom,
            TopLeft, TopRight, BottomLeft, BottomRight
        }

        private static double GetCanvasLeft(UIElement element)
        {
            var val = Canvas.GetLeft(element);
            return double.IsNaN(val) ? 0 : val;
        }

        private static double GetCanvasTop(UIElement element)
        {
            var val = Canvas.GetTop(element);
            return double.IsNaN(val) ? 0 : val;
        }

        public ChromeOSWindow()
        {
            InitializeComponent();
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Top;
            this.PreviewMouseMove += OnPreviewMouseMoveForDrag;
            this.PreviewMouseUp += OnPreviewMouseUpForDrag;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        public void SetTitle(string title)
        {
            TitleText.Text = title;
        }

        private void OnTitleBarMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_isMaximized) return;
            _isDragging = true;
            _dragStartScreen = e.GetPosition(null);
            _windowStart = new Point(GetCanvasLeft(this), GetCanvasTop(this));
            CaptureMouse();
        }

        private void OnTitleBarMouseUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
            ReleaseMouseCapture();
        }

        private void OnTitleBarMouseMove(object sender, MouseEventArgs e)
        {
        }

        private void OnPreviewMouseMoveForDrag(object sender, MouseEventArgs e)
        {
            if (!_isDragging) return;
            
            var parent = this.Parent as Canvas;
            if (parent == null) return;
            
            var currentPosScreen = e.GetPosition(null);
            var offsetX = currentPosScreen.X - _dragStartScreen.X;
            var offsetY = currentPosScreen.Y - _dragStartScreen.Y;
            
            var left = Math.Max(0, Math.Min(parent.ActualWidth - this.ActualWidth, _windowStart.X + offsetX));
            var top = Math.Max(0, Math.Min(parent.ActualHeight - this.ActualHeight - 48, _windowStart.Y + offsetY));
            
            Canvas.SetLeft(this, left);
            Canvas.SetTop(this, top);
        }

        private void OnPreviewMouseUpForDrag(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
            ReleaseMouseCapture();
        }

        private void OnMinimizeClick(object sender, RoutedEventArgs e)
        {
            MinimizeRequested?.Invoke(this);
        }

        private void OnMaximizeClick(object sender, RoutedEventArgs e)
        {
            ToggleMaximize();
        }

        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            CloseRequested?.Invoke(this);
        }

        private void ToggleMaximize()
        {
            var parent = this.Parent as Canvas;
            if (parent == null) return;

            if (!_isMaximized)
            {
                _restoreBounds = new Rect(GetCanvasLeft(this), GetCanvasTop(this), this.Width, this.Height);

                Canvas.SetLeft(this, 0);
                Canvas.SetTop(this, 0);
                this.Width = parent.ActualWidth;
                this.Height = parent.ActualHeight - 48;
                WindowBorder.CornerRadius = new CornerRadius(0);
                _isMaximized = true;
            }
            else
            {
                Canvas.SetLeft(this, _restoreBounds.X);
                Canvas.SetTop(this, _restoreBounds.Y);
                this.Width = _restoreBounds.Width;
                this.Height = _restoreBounds.Height;
                WindowBorder.CornerRadius = new CornerRadius(12);
                _isMaximized = false;
            }
        }

        private void StartResize(MouseButtonEventArgs e, ResizeDirection direction)
        {
            if (_isMaximized) return;
            _resizeDirection = direction;
            _resizeStart = e.GetPosition(null);
            _resizeStartSize = new Size(this.Width, this.Height);
            CaptureMouse();
            Mouse.AddPreviewMouseMoveHandler(this, OnPreviewMouseMove);
            Mouse.AddPreviewMouseUpHandler(this, OnPreviewMouseUp);
        }

        private void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!_resizeDirection.HasValue) return;

            var currentPos = e.GetPosition(null);
            var offset = currentPos - _resizeStart;
            var dir = _resizeDirection.Value;

            double newWidth = _resizeStartSize.Width;
            double newHeight = _resizeStartSize.Height;
            double newLeft = GetCanvasLeft(this);
            double newTop = GetCanvasTop(this);

            if (dir == ResizeDirection.Right || dir == ResizeDirection.TopRight || dir == ResizeDirection.BottomRight)
                newWidth = Math.Max(300, _resizeStartSize.Width + offset.X);
            
            if (dir == ResizeDirection.Left || dir == ResizeDirection.TopLeft || dir == ResizeDirection.BottomLeft)
            {
                newWidth = Math.Max(300, _resizeStartSize.Width - offset.X);
                if (newWidth > 300)
                    newLeft = (_resizeStartSize.Width - newWidth) + GetCanvasLeft(this);
            }

            if (dir == ResizeDirection.Bottom || dir == ResizeDirection.BottomLeft || dir == ResizeDirection.BottomRight)
                newHeight = Math.Max(200, _resizeStartSize.Height + offset.Y);

            if (dir == ResizeDirection.Top || dir == ResizeDirection.TopLeft || dir == ResizeDirection.TopRight)
            {
                newHeight = Math.Max(200, _resizeStartSize.Height - offset.Y);
                if (newHeight > 200)
                    newTop = (_resizeStartSize.Height - newHeight) + GetCanvasTop(this);
            }

            this.Width = newWidth;
            this.Height = newHeight;
            Canvas.SetLeft(this, newLeft);
            Canvas.SetTop(this, newTop);
        }

        private void OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            _resizeDirection = null;
            ReleaseMouseCapture();
            Mouse.RemovePreviewMouseMoveHandler(this, OnPreviewMouseMove);
            Mouse.RemovePreviewMouseUpHandler(this, OnPreviewMouseUp);
        }

        private void OnResizeLeft(object sender, MouseButtonEventArgs e) => StartResize(e, ResizeDirection.Left);
        private void OnResizeRight(object sender, MouseButtonEventArgs e) => StartResize(e, ResizeDirection.Right);
        private void OnResizeTop(object sender, MouseButtonEventArgs e) => StartResize(e, ResizeDirection.Top);
        private void OnResizeBottom(object sender, MouseButtonEventArgs e) => StartResize(e, ResizeDirection.Bottom);
        private void OnResizeTopLeft(object sender, MouseButtonEventArgs e) => StartResize(e, ResizeDirection.TopLeft);
        private void OnResizeTopRight(object sender, MouseButtonEventArgs e) => StartResize(e, ResizeDirection.TopRight);
        private void OnResizeBottomLeft(object sender, MouseButtonEventArgs e) => StartResize(e, ResizeDirection.BottomLeft);
        private void OnResizeBottomRight(object sender, MouseButtonEventArgs e) => StartResize(e, ResizeDirection.BottomRight);

        public void Restore()
        {
            this.Visibility = Visibility.Visible;
        }
    }
}
