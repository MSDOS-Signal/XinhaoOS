using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace ChromeOS.Controls
{
    public partial class LoginScreen : UserControl
    {
        public event EventHandler? LoginSuccessful;
        private DispatcherTimer? _clockTimer;

        public LoginScreen()
        {
            InitializeComponent();
            UpdateClock();
            _clockTimer = new DispatcherTimer();
            _clockTimer.Interval = TimeSpan.FromSeconds(1);
            _clockTimer.Tick += (s, e) => UpdateClock();
            _clockTimer.Start();
            PasswordInput.Focus();
        }

        private void UpdateClock()
        {
            var now = DateTime.Now;
            LoginTimeText.Text = now.ToString("HH:mm");
            LoginDateText.Text = now.ToString("dddd, MMMM d");
        }

        private void OnPasswordKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TryLogin();
            }
        }

        private void OnLoginClick(object sender, RoutedEventArgs e)
        {
            TryLogin();
        }

        private void TryLogin()
        {
            var password = PasswordInput.Password;
            if (string.IsNullOrEmpty(password))
            {
                ErrorText.Text = "Please enter your password";
                return;
            }

            ErrorText.Text = "";
            LoginSuccessful?.Invoke(this, EventArgs.Empty);
        }

        public void ResetPassword()
        {
            PasswordInput.Password = "";
            ErrorText.Text = "";
            PasswordInput.Focus();
        }
    }
}