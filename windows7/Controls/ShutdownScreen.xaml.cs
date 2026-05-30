using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Threading.Tasks;

namespace ChromeOS.Controls
{
    public partial class ShutdownScreen : UserControl
    {
        public event EventHandler? ShutdownComplete;

        public ShutdownScreen()
        {
            InitializeComponent();
            StartShutdownAnimation();
        }

        private async void StartShutdownAnimation()
        {
            await Task.Delay(100);
            
            var pulseAnim = new DoubleAnimation
            {
                From = 1,
                To = 1.5,
                Duration = TimeSpan.FromSeconds(2),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };
            PulseScale.BeginAnimation(ScaleTransform.ScaleXProperty, pulseAnim);
            PulseScale.BeginAnimation(ScaleTransform.ScaleYProperty, pulseAnim);

            var outerRotateAnim = new DoubleAnimation
            {
                From = 0,
                To = 360,
                Duration = TimeSpan.FromSeconds(4),
                RepeatBehavior = RepeatBehavior.Forever
            };
            OuterRotate.BeginAnimation(RotateTransform.AngleProperty, outerRotateAnim);

            var innerRotateAnim = new DoubleAnimation
            {
                From = 360,
                To = 0,
                Duration = TimeSpan.FromSeconds(3.5),
                RepeatBehavior = RepeatBehavior.Forever
            };
            InnerRotate.BeginAnimation(RotateTransform.AngleProperty, innerRotateAnim);

            var orb1Anim = new DoubleAnimation
            {
                From = 1,
                To = 1.2,
                Duration = TimeSpan.FromSeconds(3),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };
            Orb1Scale.BeginAnimation(ScaleTransform.ScaleXProperty, orb1Anim);
            Orb1Scale.BeginAnimation(ScaleTransform.ScaleYProperty, orb1Anim);

            var orb2Anim = new DoubleAnimation
            {
                From = 1,
                To = 1.15,
                Duration = TimeSpan.FromSeconds(2.5),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                BeginTime = TimeSpan.FromSeconds(0.5)
            };
            Orb2Scale.BeginAnimation(ScaleTransform.ScaleXProperty, orb2Anim);
            Orb2Scale.BeginAnimation(ScaleTransform.ScaleYProperty, orb2Anim);

            await Task.Delay(3000);
            
            ShutdownComplete?.Invoke(this, EventArgs.Empty);
        }
    }
}
