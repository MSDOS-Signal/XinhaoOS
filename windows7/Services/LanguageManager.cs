using System;
using System.Linq;
using System.Windows;
using System.Windows.Markup;

namespace ChromeOS.Services
{
    public static class LanguageManager
    {
        public static event EventHandler? LanguageChanged;

        public static string CurrentLanguage { get; private set; } = "en";

        public static void SetLanguage(string languageCode)
        {
            if (CurrentLanguage == languageCode) return;
            
            CurrentLanguage = languageCode;
            
            // 清除旧的语言资源
            var existingDicts = Application.Current.Resources.MergedDictionaries
                .Where(d => d.Source?.OriginalString.Contains("Strings.") ?? false)
                .ToList();
            
            foreach (var dict in existingDicts)
            {
                Application.Current.Resources.MergedDictionaries.Remove(dict);
            }

            // 添加新的语言资源
            var resourceDictionary = new ResourceDictionary();
            
            switch (languageCode)
            {
                case "zh":
                    resourceDictionary.Source = new Uri("Resources/Strings.zh.xaml", UriKind.Relative);
                    break;
                case "en":
                default:
                    resourceDictionary.Source = new Uri("Resources/Strings.en.xaml", UriKind.Relative);
                    break;
            }

            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);

            // 通知语言已更改
            LanguageChanged?.Invoke(null, EventArgs.Empty);
        }

        public static string GetString(string key)
        {
            if (Application.Current.Resources.Contains(key))
            {
                return Application.Current.Resources[key]?.ToString() ?? key;
            }
            return key;
        }
    }
}