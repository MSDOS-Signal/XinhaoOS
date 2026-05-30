namespace ChromeOS.Models
{
    public class AppInfo
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public AppType AppType { get; set; }
        public string? Data { get; set; }
    }

    public enum AppType
    {
        Browser,
        Files,
        Settings,
        Terminal,
        TextEditor,
        Calculator,
        Camera,
        Photos,
        PlayStore,
        Downloads,
        Music,
        Maps,
        Gmail,
        YouTube,
        Clock,
        Contacts,
        Calendar,
        Weather,
        News,
        Drive
    }
}
