using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChromeOS.Apps
{
    public partial class BrowserApp : UserControl
    {
        public BrowserApp()
        {
            InitializeComponent();
        }

        public async void NavigateTo(string url)
        {
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                url = "https://" + url;
            UrlBar.Text = url;
            try
            {
                await WebView.EnsureCoreWebView2Async();
                WebView.Source = new Uri(url);
            }
            catch { }
        }

        private void OnUrlBarKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                NavigateTo(UrlBar.Text);
            }
        }

        private async void OnBackClick(object sender, RoutedEventArgs e)
        {
            try
            {
                await WebView.EnsureCoreWebView2Async();
                if (WebView.CanGoBack)
                    WebView.GoBack();
            }
            catch { }
        }

        private async void OnForwardClick(object sender, RoutedEventArgs e)
        {
            try
            {
                await WebView.EnsureCoreWebView2Async();
                if (WebView.CanGoForward)
                    WebView.GoForward();
            }
            catch { }
        }

        private async void OnRefreshClick(object sender, RoutedEventArgs e)
        {
            try
            {
                await WebView.EnsureCoreWebView2Async();
                WebView.Reload();
            }
            catch { }
        }

        private void OnGoogleClick(object sender, RoutedEventArgs e) => NavigateTo("https://www.google.com");
        private void OnYouTubeClick(object sender, RoutedEventArgs e) => NavigateTo("https://www.youtube.com");
        private void OnGitHubClick(object sender, RoutedEventArgs e) => NavigateTo("https://github.com");

        private void UrlBar_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}