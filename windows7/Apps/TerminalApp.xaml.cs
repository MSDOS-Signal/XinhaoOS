using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace ChromeOS.Apps
{
    public partial class TerminalApp : UserControl
    {
        private readonly List<string> _commandHistory = new();
        private int _historyIndex = -1;
        private string _currentDir = "/home/user";
        private readonly Dictionary<string, string> _envVars = new()
        {
            ["USER"] = "user",
            ["HOME"] = "/home/user",
            ["SHELL"] = "/bin/bash",
            ["TERM"] = "xterm-256color",
            ["PATH"] = "/usr/local/bin:/usr/bin:/bin:/usr/sbin:/sbin",
            ["LANG"] = "en_US.UTF-8",
            ["EDITOR"] = "vim",
            ["PWD"] = "/home/user"
        };
        private readonly Dictionary<string, List<string>> _fileSystem = new()
        {
            ["/home/user"] = new() { "Documents", "Downloads", "Images", "Music", ".config", ".local", "readme.txt", "notes.md" },
            ["/home/user/Documents"] = new() { "report.docx", "spreadsheet.xlsx", "presentation.pptx" },
            ["/home/user/Downloads"] = new() { "installer.exe", "document.pdf", "photo.jpg" },
            ["/home/user/Images"] = new() { "wallpaper.png", "avatar.jpg" },
            ["/home/user/Music"] = new() { "playlist1.mp3", "song1.mp3" }
        };

        public TerminalApp()
        {
            InitializeComponent();
            TerminalInput.Focus();
        }

        private void OnTerminalInputKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var command = TerminalInput.Text.Trim();
                if (!string.IsNullOrEmpty(command))
                {
                    _commandHistory.Add(command);
                    _historyIndex = _commandHistory.Count;
                }
                ProcessCommand(command);
                TerminalInput.Text = "";
            }
            else if (e.Key == Key.Up)
            {
                e.Handled = true;
                if (_historyIndex > 0)
                {
                    _historyIndex--;
                    TerminalInput.Text = _commandHistory[_historyIndex];
                    TerminalInput.CaretIndex = TerminalInput.Text.Length;
                }
            }
            else if (e.Key == Key.Down)
            {
                e.Handled = true;
                if (_historyIndex < _commandHistory.Count - 1)
                {
                    _historyIndex++;
                    TerminalInput.Text = _commandHistory[_historyIndex];
                    TerminalInput.CaretIndex = TerminalInput.Text.Length;
                }
                else
                {
                    _historyIndex = _commandHistory.Count;
                    TerminalInput.Text = "";
                }
            }
            else if (e.Key == Key.L && Keyboard.Modifiers == ModifierKeys.Control)
            {
                e.Handled = true;
                TerminalOutput.Text = "";
            }
            else if (e.Key == Key.C && Keyboard.Modifiers == ModifierKeys.Control)
            {
                e.Handled = true;
                AppendOutput($"^C\n");
            }
        }

        private void OnTerminalInputTextChanged(object sender, TextChangedEventArgs e)
        {
            // Auto-resize input box
        }

        private void ProcessCommand(string command)
        {
            AppendOutput($"{_currentDir}$ {command}\n", "#8AB4F8");

            var parts = command.Split(' ');
            var lowerCommand = parts[0].ToLower();

            switch (lowerCommand)
            {
                case "":
                    break;
                case "help":
                    AppendOutput("Available Linux commands:\n");
                    AppendOutput("  help          - Show this help message\n", "#9AA0A6");
                    AppendOutput("  clear         - Clear terminal\n", "#9AA0A6");
                    AppendOutput("  date          - Show current date and time\n", "#9AA0A6");
                    AppendOutput("  whoami        - Show current user\n", "#9AA0A6");
                    AppendOutput("  hostname      - Show hostname\n", "#9AA0A6");
                    AppendOutput("  uname [-a]    - Show system information\n", "#9AA0A6");
                    AppendOutput("  pwd           - Print working directory\n", "#9AA0A6");
                    AppendOutput("  ls [-la]      - List directory contents\n", "#9AA0A6");
                    AppendOutput("  cd <dir>      - Change directory\n", "#9AA0A6");
                    AppendOutput("  cat <file>    - Display file contents\n", "#9AA0A6");
                    AppendOutput("  echo <text>   - Echo text\n", "#9AA0A6");
                    AppendOutput("  env           - Show environment variables\n", "#9AA0A6");
                    AppendOutput("  export K=V    - Set environment variable\n", "#9AA0A6");
                    AppendOutput("  history       - Show command history\n", "#9AA0A6");
                    AppendOutput("  neofetch      - Show system info\n", "#9AA0A6");
                    AppendOutput("  ping <host>   - Ping a host\n", "#9AA0A6");
                    AppendOutput("  df            - Show disk usage\n", "#9AA0A6");
                    AppendOutput("  free          - Show memory usage\n", "#9AA0A6");
                    AppendOutput("  top           - Show processes\n", "#9AA0A6");
                    AppendOutput("  mkdir <dir>   - Create directory\n", "#9AA0A6");
                    AppendOutput("  touch <file>  - Create empty file\n", "#9AA0A6");
                    AppendOutput("  rm <file>     - Remove file\n", "#9AA0A6");
                    AppendOutput("  rmdir <dir>   - Remove empty directory\n", "#9AA0A6");
                    AppendOutput("  cp <src> <dst>- Copy file\n", "#9AA0A6");
                    AppendOutput("  mv <src> <dst>- Move file\n", "#9AA0A6");
                    AppendOutput("  which <cmd>   - Locate command\n", "#9AA0A6");
                    AppendOutput("  id            - Show user ID info\n", "#9AA0A6");
                    AppendOutput("  uptime        - Show uptime\n", "#9AA0A6");
                    AppendOutput("  cal           - Show calendar\n", "#9AA0A6");
                    AppendOutput("  factor <num>  - Show factors\n", "#9AA0A6");
                    AppendOutput("  cowsay <msg>  - Cow says message\n", "#9AA0A6");
                    AppendOutput("  grep <pat> <file> - Search pattern\n", "#9AA0A6");
                    AppendOutput("  wc <file>     - Word count\n", "#9AA0A6");
                    AppendOutput("  head <file>   - Show first lines\n", "#9AA0A6");
                    AppendOutput("  tail <file>   - Show last lines\n", "#9AA0A6");
                    AppendOutput("  sort <file>   - Sort file\n", "#9AA0A6");
                    AppendOutput("  chmod <perm> <file> - Change permissions\n", "#9AA0A6");
                    AppendOutput("  chown <user> <file> - Change owner\n", "#9AA0A6");
                    AppendOutput("  ps            - Show processes\n", "#9AA0A6");
                    AppendOutput("  kill <pid>    - Kill process\n", "#9AA0A6");
                    AppendOutput("  sudo <cmd>    - Run as root\n", "#9AA0A6");
                    AppendOutput("  apt-get <cmd> - Package management\n", "#9AA0A6");
                    AppendOutput("  man <cmd>     - Show manual\n", "#9AA0A6");
                    AppendOutput("  git           - Git commands\n", "#9AA0A6");
                    AppendOutput("  vi/vim        - Text editor\n", "#9AA0A6");
                    AppendOutput("  nano          - Text editor\n", "#9AA0A6");
                    AppendOutput("  exit          - Close terminal\n", "#9AA0A6");
                    break;
                case "clear":
                    TerminalOutput.Text = "";
                    break;
                case "date":
                    AppendOutput(DateTime.Now.ToString("ddd MMM dd HH:mm:ss yyyy\n"));
                    break;
                case "whoami":
                    AppendOutput("user\n");
                    break;
                case "hostname":
                    AppendOutput("xinhaoos\n");
                    break;
                case "uname":
                    if (parts.Length > 1 && parts[1] == "-a")
                        AppendOutput("XinhaoOS 1.0.0 x86_64 GNU/Linux\n");
                    else
                        AppendOutput("XinhaoOS\n");
                    break;
                case "pwd":
                    AppendOutput($"{_currentDir}\n");
                    break;
                case "ls":
                    HandleLs(parts);
                    break;
                case "cd":
                    HandleCd(parts);
                    break;
                case "cat":
                    HandleCat(parts);
                    break;
                case "echo":
                    AppendOutput(command.Substring(5) + "\n");
                    break;
                case "env":
                    foreach (var kv in _envVars)
                        AppendOutput($"{kv.Key}={kv.Value}\n");
                    break;
                case "export":
                    if (parts.Length > 1)
                    {
                        var eqIdx = command.IndexOf('=');
                        if (eqIdx > 0)
                        {
                            var key = command.Substring(7, eqIdx - 7).Trim();
                            var val = command.Substring(eqIdx + 1).Trim();
                            _envVars[key] = val;
                        }
                    }
                    break;
                case "history":
                    for (int i = 0; i < _commandHistory.Count; i++)
                        AppendOutput($"  {i + 1}  {_commandHistory[i]}\n");
                    break;
                case "neofetch":
                    HandleNeofetch();
                    break;
                case "ping":
                    HandlePing(parts);
                    break;
                case "df":
                    AppendOutput("Filesystem     Size   Used  Avail Use% Mounted on\n");
                    AppendOutput("/dev/sda1      128G   45G    83G  35% /\n");
                    AppendOutput("tmpfs          3.9G   1.2M   3.9G   1% /tmp\n");
                    break;
                case "free":
                    AppendOutput("              total    used    free   shared  buff/cache  available\n");
                    AppendOutput("Mem:        8192000  3245612  2145284   524288     2801104    4521088\n");
                    AppendOutput("Swap:       2097152   51200   1585152\n");
                    break;
                case "top":
                    AppendOutput("  PID USER      PR  NI    VIRT    RES    SHR S  %CPU  %MEM     TIME+ COMMAND\n");
                    AppendOutput("  1234 user      20   0  524288  123456  87654 S   2.3   1.5   0:12.34 chrome\n");
                    AppendOutput("  1235 user      20   0  262144   65432  43210 S   1.0   0.8   0:05.67 terminal\n");
                    AppendOutput("  1236 user      20   0  131072   32768  21504 S   0.3   0.4   0:02.34 files\n");
                    break;
                case "mkdir":
                    if (parts.Length > 1)
                        AppendOutput($"Created directory: {parts[1]}\n");
                    else
                        AppendOutput("mkdir: missing operand\n");
                    break;
                case "touch":
                    if (parts.Length > 1)
                        AppendOutput($"Created file: {parts[1]}\n");
                    else
                        AppendOutput("touch: missing operand\n");
                    break;
                case "rm":
                    if (parts.Length > 1)
                        AppendOutput($"Removed: {parts[1]}\n");
                    else
                        AppendOutput("rm: missing operand\n");
                    break;
                case "rmdir":
                    if (parts.Length > 1)
                        AppendOutput($"Removed directory: {parts[1]}\n");
                    else
                        AppendOutput("rmdir: missing operand\n");
                    break;
                case "cp":
                    if (parts.Length > 2)
                        AppendOutput($"Copied {parts[1]} to {parts[2]}\n");
                    else
                        AppendOutput("cp: missing operands\n");
                    break;
                case "mv":
                    if (parts.Length > 2)
                        AppendOutput($"Moved {parts[1]} to {parts[2]}\n");
                    else
                        AppendOutput("mv: missing operands\n");
                    break;
                case "grep":
                    if (parts.Length > 2)
                        AppendOutput($"Searching for '{parts[1]}' in {parts[2]}...\n");
                    else
                        AppendOutput("grep: missing operands\n");
                    break;
                case "wc":
                    if (parts.Length > 1)
                        AppendOutput($"  10  50  300 {parts[1]}\n");
                    else
                        AppendOutput("wc: missing operand\n");
                    break;
                case "head":
                    if (parts.Length > 1)
                        AppendOutput($"==> {parts[1]} <==\nLine 1\nLine 2\nLine 3\nLine 4\nLine 5\n");
                    else
                        AppendOutput("head: missing operand\n");
                    break;
                case "tail":
                    if (parts.Length > 1)
                        AppendOutput($"==> {parts[1]} <==\nLine 6\nLine 7\nLine 8\nLine 9\nLine 10\n");
                    else
                        AppendOutput("tail: missing operand\n");
                    break;
                case "sort":
                    if (parts.Length > 1)
                        AppendOutput($"Sorted {parts[1]}\n");
                    else
                        AppendOutput("sort: missing operand\n");
                    break;
                case "chmod":
                    if (parts.Length > 2)
                        AppendOutput($"Changed permissions of {parts[2]} to {parts[1]}\n");
                    else
                        AppendOutput("chmod: missing operands\n");
                    break;
                case "chown":
                    if (parts.Length > 2)
                        AppendOutput($"Changed owner of {parts[2]} to {parts[1]}\n");
                    else
                        AppendOutput("chown: missing operands\n");
                    break;
                case "ps":
                    AppendOutput("  PID TTY          TIME CMD\n");
                    AppendOutput(" 1234 pts/0    00:00:01 bash\n");
                    AppendOutput(" 1235 pts/0    00:00:00 terminal\n");
                    AppendOutput(" 1236 pts/0    00:00:00 ps\n");
                    break;
                case "kill":
                    if (parts.Length > 1)
                        AppendOutput($"Killed process {parts[1]}\n");
                    else
                        AppendOutput("kill: missing operand\n");
                    break;
                case "sudo":
                    if (parts.Length > 1)
                        AppendOutput($"[sudo] password for user: ******\nExecuting: {string.Join(" ", parts, 1, parts.Length - 1)}\n");
                    else
                        AppendOutput("sudo: missing command\n");
                    break;
                case "apt-get":
                    if (parts.Length > 1)
                    {
                        switch (parts[1])
                        {
                            case "update":
                                AppendOutput("Hit:1 http://archive.ubuntu.com/ubuntu focal InRelease\n");
                                AppendOutput("Reading package lists... Done\n");
                                break;
                            case "upgrade":
                                AppendOutput("Reading package lists... Done\n");
                                AppendOutput("Building dependency tree... Done\n");
                                AppendOutput("Calculating upgrade... Done\n");
                                AppendOutput("0 upgraded, 0 newly installed, 0 to remove and 0 not upgraded.\n");
                                break;
                            case "install":
                                if (parts.Length > 2)
                                    AppendOutput($"Reading package lists... Done\nBuilding dependency tree... Done\n\nThe following NEW packages will be installed:\n  {parts[2]}\n0 upgraded, 1 newly installed, 0 to remove and 0 not upgraded.\n");
                                else
                                    AppendOutput("apt-get install: missing package name\n");
                                break;
                            default:
                                AppendOutput($"apt-get: invalid operation '{parts[1]}'\n");
                                break;
                        }
                    }
                    else
                        AppendOutput("apt-get: missing operation\n");
                    break;
                case "man":
                    if (parts.Length > 1)
                        AppendOutput($"{parts[1]}(1)                  General Commands Manual                 {parts[1]}(1)\n\nNAME\n       {parts[1]} - help\n\nSYNOPSIS\n       {parts[1]} [OPTION]...\n\nDESCRIPTION\n       Help command.\n\nSEE ALSO\n       help(1)\n\nXinhaoOS 1.0.0              January 2024                 {parts[1]}(1)\n");
                    else
                        AppendOutput("man: missing operand\n");
                    break;
                case "git":
                    if (parts.Length > 1)
                    {
                        switch (parts[1])
                        {
                            case "status":
                                AppendOutput("On branch main\nYour branch is up to date with 'origin/main'.\n\nnothing to commit, working tree clean\n");
                                break;
                            case "log":
                                AppendOutput("commit abc1234567890abc1234567890abc1234567890\nAuthor: User <user@xinhaoos.local>\nDate:   Wed May 29 12:00:00 2024 +0800\n\n    Initial commit\n");
                                break;
                            case "init":
                                AppendOutput("Initialized empty Git repository in /home/user/.git/\n");
                                break;
                            default:
                                AppendOutput($"git: '{parts[1]}' is not a git command. See 'git --help'.\n");
                                break;
                        }
                    }
                    else
                    {
                        AppendOutput("usage: git [--version] [--help] [-C <path>] [-c <name>=<value>]\n           [--exec-path[=<path>]] [--html-path] [--man-path] [--info-path]\n           [-p|--paginate|--no-pager] [--no-replace-objects] [--bare]\n           [--git-dir=<path>] [--work-tree=<path>] [--namespace=<name>]\n           <command> [<args>]\n");
                    }
                    break;
                case "vi":
                case "vim":
                    AppendOutput("~                          VIM - Vi IMproved\n\n~                          version 9.0\n~              by Bram Moolenaar et al.\n~          Modified by pkg-vim-maintainers@lists.debian.org\n~              Vim is open source and freely distributable\n~                        Sponsor Vim development!\n~            type  :help sponsor<Enter>    for information\n~            type  :q<Enter>               to exit\n~            type  :help<Enter>  or  <F1>  for on-line help\n~            type  :help version9<Enter>   for version info\n~            type  :checkhealth<Enter>     for health check\n~            Press ENTER or type command to continue\n");
                    break;
                case "nano":
                    AppendOutput(@"GNU nano 6.2

^G Get Help  ^O WriteOut  ^W Where Is  ^K Cut Text  ^J Justify
^X Exit      ^R Read File ^\ Replace   ^U UnCut Text ^T To Spell

~                                                                               
~                                                                               
~                                                                               
~                                                                               
~                                                                               
~                                                                               
~                                                                               

              [ New File ]
");
                    break;
                case "which":
                    if (parts.Length > 1)
                        AppendOutput($"/usr/bin/{parts[1]}\n");
                    else
                        AppendOutput("which: missing operand\n");
                    break;
                case "id":
                    AppendOutput("uid=1000(user) gid=1000(user) groups=1000(user),27(sudo)\n");
                    break;
                case "uptime":
                    AppendOutput($" {DateTime.Now:HH:mm:ss} up 2:34, 1 user, load average: 0.52, 0.58, 0.59\n");
                    break;
                case "cal":
                    HandleCal();
                    break;
                case "factor":
                    if (parts.Length > 1 && int.TryParse(parts[1], out int num))
                    {
                        var factors = new List<int>();
                        for (int i = 1; i <= num; i++)
                            if (num % i == 0) factors.Add(i);
                        AppendOutput($"{num}: {string.Join(" ", factors)}\n");
                    }
                    else
                        AppendOutput("factor: missing number operand\n");
                    break;
                case "cowsay":
                    HandleCowsay(parts);
                    break;
                case "exit":
                    AppendOutput("logout\n");
                    break;
                default:
                    AppendOutput($"bash: {parts[0]}: command not found\n", "#F28B82");
                    break;
            }

            ScrollToEnd();
        }

        private void HandleLs(string[] parts)
        {
            bool showAll = false;
            string targetDir = _currentDir;

            for (int i = 1; i < parts.Length; i++)
            {
                if (parts[i].StartsWith("-"))
                {
                    if (parts[i].Contains("a")) showAll = true;
                }
                else
                {
                    targetDir = parts[i].StartsWith("/") ? parts[i] : $"{_currentDir}/{parts[i]}";
                }
            }

            if (_fileSystem.ContainsKey(targetDir))
            {
                var files = _fileSystem[targetDir];
                if (showAll)
                    files = new List<string> { ".", ".." }.Concat(files).ToList();

                bool hasLong = false;
                for (int i = 1; i < parts.Length; i++)
                    if (parts[i].Contains("l")) hasLong = true;

                if (hasLong)
                {
                    foreach (var f in files)
                    {
                        var isDir = f.StartsWith(".") || !f.Contains(".");
                        AppendOutput($"{(isDir ? "d" : "-")}rw-r--r-- 1 user user  4096 Jan 20 12:00 {f}\n", isDir ? "#8AB4F8" : "#E8EAED");
                    }
                }
                else
                {
                    foreach (var f in files)
                    {
                        var isDir = f.StartsWith(".") || !f.Contains(".");
                        AppendOutput($"{f}  ", isDir ? "#8AB4F8" : "#E8EAED");
                    }
                    AppendOutput("\n");
                }
            }
            else
            {
                AppendOutput($"ls: cannot access '{targetDir}': No such file or directory\n", "#F28B82");
            }
        }

        private void HandleCd(string[] parts)
        {
            if (parts.Length == 1 || parts[1] == "~")
            {
                _currentDir = "/home/user";
            }
            else if (parts[1] == "..")
            {
                var idx = _currentDir.LastIndexOf('/');
                _currentDir = idx > 0 ? _currentDir.Substring(0, idx) : "/";
            }
            else
            {
                var newDir = parts[1].StartsWith("/") ? parts[1] : $"{_currentDir}/{parts[1]}";
                if (_fileSystem.ContainsKey(newDir))
                    _currentDir = newDir;
                else
                    AppendOutput($"cd: no such file or directory: {parts[1]}\n", "#F28B82");
            }
        }

        private void HandleCat(string[] parts)
        {
            if (parts.Length > 1)
            {
                var fileContent = parts[1] switch
                {
                    "readme.txt" => "Welcome to ChromeOS!\n\nThis is a simulated terminal environment.\nUse 'help' to see available commands.\n",
                    "notes.md" => "# Notes\n\n- Setup development environment\n- Configure ChromeOS settings\n- Install apps\n",
                    _ => $"cat: {parts[1]}: No such file or directory"
                };
                AppendOutput(fileContent + "\n");
            }
            else
            {
                AppendOutput("cat: missing operand\n", "#F28B82");
            }
        }

        private void HandleNeofetch()
        {
            AppendOutput("       .--.        ", "#0A84FF");
            AppendOutput("user@xinhaoos\n");
            AppendOutput("      |o_o |       ", "#0A84FF");
            AppendOutput("----------------\n");
            AppendOutput("      |:_/ |       ", "#0A84FF");
            AppendOutput("OS: XinhaoOS 1.0.0 x86_64\n");
            AppendOutput("     //   \\ \\      ", "#0A84FF");
            AppendOutput("Host: XinhaoOS Virtual Machine\n");
            AppendOutput("    (|     | )     ", "#0A84FF");
            AppendOutput("Kernel: 5.15.0-xinhaoos\n");
            AppendOutput("   /'\\_   _/`\\    ", "#0A84FF");
            AppendOutput("Uptime: 2 hours, 34 mins\n");
            AppendOutput("   \\___)=(___/     ", "#0A84FF");
            AppendOutput("Shell: bash\n");
            AppendOutput("                   Resolution: 1920x1080\n");
            AppendOutput("                   DE: XinhaoOS Desktop\n");
            AppendOutput("                   Theme: XinhaoOS Dark\n");
            AppendOutput("                   CPU: Intel Core i7-1165G7 @ 2.80GHz\n");
            AppendOutput("                   GPU: Intel Iris Xe Graphics\n");
            AppendOutput("                   Memory: 3245MiB / 8192MiB\n");
            AppendOutput("\n");
        }

        private void HandlePing(string[] parts)
        {
            if (parts.Length > 1)
            {
                var host = parts[1];
                AppendOutput($"PING {host} (93.184.216.34) 56(84) bytes of data.\n");
                for (int i = 0; i < 4; i++)
                {
                    var time = (new Random()).Next(10, 50);
                    AppendOutput($"64 bytes from {host}: icmp_seq={i + 1} ttl=56 time={time}.{(new Random()).Next(0, 999)} ms\n");
                }
                AppendOutput($"\n--- {host} ping statistics ---\n");
                AppendOutput("4 packets transmitted, 4 received, 0% packet loss\n");
            }
            else
            {
                AppendOutput("ping: missing host operand\n", "#F28B82");
            }
        }

        private void HandleCal()
        {
            var now = DateTime.Now;
            AppendOutput($"     {now:MMMM yyyy}\n");
            AppendOutput("Su Mo Tu We Th Fr Sa\n");
            var firstDay = new DateTime(now.Year, now.Month, 1);
            int startDay = (int)firstDay.DayOfWeek;
            int daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);
            AppendOutput(new string(' ', startDay * 3));
            for (int d = 1; d <= daysInMonth; d++)
            {
                AppendOutput($"{d,3}");
                if ((startDay + d) % 7 == 0)
                    AppendOutput("\n");
            }
            AppendOutput("\n\n");
        }

        private void HandleCowsay(string[] parts)
        {
            var msg = parts.Length > 1 ? string.Join(" ", parts, 1, parts.Length - 1) : "Moo!";
            var line = new string('-', msg.Length + 2);
            AppendOutput($" {line}\n");
            AppendOutput($"< {msg} >\n");
            AppendOutput($" {line}\n");
            AppendOutput("        \\   ^__^\n");
            AppendOutput("         \\  (oo)\\_______\n");
            AppendOutput("            (__)\\       )\\/\\\n");
            AppendOutput("                ||----w |\n");
            AppendOutput("                ||     ||\n");
        }

        private void AppendOutput(string text, string? color = null)
        {
            var run = new Run(text);
            if (color != null)
            {
                run.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
            }
            TerminalOutput.Inlines.Add(run);
        }

        private void ScrollToEnd()
        {
            OutputScroll.ScrollToEnd();
            TerminalInput.Focus();
        }
    }
}
