using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ChromeOS.Controls
{
    public partial class ShutdownConfirmationDialog : UserControl
    {
        public event EventHandler? Confirmed;
        public event EventHandler? Cancelled;

        public ShutdownConfirmationDialog()
        {
            InitializeComponent();
            SetupButtons();
            StartIconAnimation();
        }

        private void SetupButtons()
        {
            ShutdownButton.Click += (s, e) => Confirmed?.Invoke(this, EventArgs.Empty);
            CancelButton.Click += (s, e) => Cancelled?.Invoke(this, EventArgs.Empty);
        }

        private void StartIconAnimation()
        {
            var rotateAnim = new DoubleAnimation
            {
                From = 0,
                To = 360,
                Duration = TimeSpan.FromSeconds(4),
                RepeatBehavior = RepeatBehavior.Forever
            };
            IconRotate.BeginAnimation(RotateTransform.AngleProperty, rotateAnim);
        }
    }
}