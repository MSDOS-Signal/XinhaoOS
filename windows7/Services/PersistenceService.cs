using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ChromeOS.Services
{
    public static class PersistenceService
    {
        private static string AppDataPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ChromeOS");
        
        public static void SaveDesktopItems(List<DesktopItem> items)
        {
            try
            {
                Directory.CreateDirectory(AppDataPath);
                var path = Path.Combine(AppDataPath, "desktop.json");
                var json = JsonSerializer.Serialize(items);
                File.WriteAllText(path, json);
            }
            catch { }
        }

        public static List<DesktopItem> LoadDesktopItems()
        {
            try
            {
                var path = Path.Combine(AppDataPath, "desktop.json");
                if (File.Exists(path))
                {
                    var json = File.ReadAllText(path);
                    return JsonSerializer.Deserialize<List<DesktopItem>>(json) ?? new List<DesktopItem>();
                }
            }
            catch { }
            
            return new List<DesktopItem>
            {
                new DesktopItem { Name = "My Files", IsFolder = true, Path = "My files" },
                new DesktopItem { Name = "Downloads", IsFolder = true, Path = "Downloads" },
                new DesktopItem { Name = "Computer", IsFolder = true, Path = "Computer" }
            };
        }

        public static void SaveUserSettings(UserSettings settings)
        {
            try
            {
                Directory.CreateDirectory(AppDataPath);
                var path = Path.Combine(AppDataPath, "user.json");
                var json = JsonSerializer.Serialize(settings);
                File.WriteAllText(path, json);
            }
            catch { }
        }

        public static UserSettings LoadUserSettings()
        {
            try
            {
                var path = Path.Combine(AppDataPath, "user.json");
                if (File.Exists(path))
                {
                    var json = File.ReadAllText(path);
                    return JsonSerializer.Deserialize<UserSettings>(json) ?? new UserSettings();
                }
            }
            catch { }
            
            return new UserSettings();
        }

        public static void SaveTextFile(string fileName, string content)
        {
            try
            {
                var docsPath = Path.Combine(AppDataPath, "Documents");
                Directory.CreateDirectory(docsPath);
                var path = Path.Combine(docsPath, fileName);
                File.WriteAllText(path, content);
            }
            catch { }
        }

        public static string LoadTextFile(string fileName)
        {
            try
            {
                var docsPath = Path.Combine(AppDataPath, "Documents");
                var path = Path.Combine(docsPath, fileName);
                if (File.Exists(path))
                {
                    return File.ReadAllText(path);
                }
            }
            catch { }
            
            return "";
        }

        public static bool TextFileExists(string fileName)
        {
            try
            {
                var docsPath = Path.Combine(AppDataPath, "Documents");
                var path = Path.Combine(docsPath, fileName);
                return File.Exists(path);
            }
            catch { }
            
            return false;
        }

        public static List<string> GetTextFiles()
        {
            try
            {
                var docsPath = Path.Combine(AppDataPath, "Documents");
                if (Directory.Exists(docsPath))
                {
                    return Directory.GetFiles(docsPath, "*.txt").Select(p => Path.GetFileName(p)).ToList();
                }
            }
            catch { }
            
            return new List<string>();
        }

        public static void ShutdownSave(List<DesktopItem> desktopItems, UserSettings userSettings)
        {
            SaveDesktopItems(desktopItems);
            SaveUserSettings(userSettings);
        }
    }

    public class DesktopItem
    {
        public string Name { get; set; } = "";
        public bool IsFolder { get; set; }
        public string Path { get; set; } = "";
    }

    public class UserSettings
    {
        public string UserName { get; set; } = "user";
        public string UserEmail { get; set; } = "user@gmail.com";
        public string UserAvatar { get; set; } = "blue";
        public string UserPassword { get; set; } = "password";
    }
}
