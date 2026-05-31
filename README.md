# XinhaoOS 桌面环境 - 完整功能文档

> 注：至于文件夹为什么取名 windows7，只是因为我自己对 win7 陪伴我中学的怀念。

## 项目概述

这是一个基于 WPF (Windows Presentation Foundation) 开发的微型桌面操作系统，提供了完整的现代操作系统用户体验。项目采用 C# 和 XAML 技术栈，模拟了 Windows+Linux+MacOS+ChromeOS 的界面风格、应用程序生态系统和系统管理功能。

![开机引导](https://cloudflareimg.cdn.sn/i/6a1bb5944b022_1780200852.webp)
![登陆界面](https://cloudflareimg.cdn.sn/i/6a1bb59507292_1780200853.webp)
![用户操作](https://cloudflareimg.cdn.sn/i/6a1bb59cd989d_1780200860.webp)

### 核心优势

- ✅ **Windows 原生运行**: 无需虚拟机，直接在 Windows 上运行
- 📦 **轻量高效**: 打包后不到 100MB，占用空间小
- 🚀 **快速启动**: 秒级启动，响应迅速
- 🎨 **精美界面**: 融合多系统设计风格
- 🔧 **无需安装**: 绿色软件，解压即用

### 技术特点

- **开发框架**: .NET 10.0 + WPF
- **编程语言**: C# 12.0
- **UI 设计**: XAML 声明式布局
- **架构模式**: MVVM 风格设计
- **Web 浏览**: Microsoft.Web.WebView2
- **部署方式**: 自包含单文件部署

---

## 目录
1. [启动和登录系统](#1-启动和登录系统)
2. [桌面环境](#2-桌面环境)
3. [Shelf 任务栏](#3-shelf-任务栏)
4. [启动器](#4-启动器)
5. [应用程序系统](#5-应用程序系统)
6. [系统服务](#6-系统服务)
7. [关机系统](#7-关机系统)
8. [文件格式和资源](#8-文件格式和资源)

---

## 1. 启动和登录系统

### 1.1 启动界面 (BootScreen)
- **功能**: 系统启动时显示的欢迎界面
- **视觉效果**: 
  - 现代渐变背景设计
  - 动态加载动画
  - 品牌 Logo 展示
- **交互流程**: 自动完成加载后进入登录界面

### 1.2 登录界面 (LoginScreen)
- **功能模块**:
  - 用户头像选择
  - 密码输入框
  - 登录按钮
  - 访客模式选项
  - 语言切换功能
- **安全特性**:
  - 密码隐藏显示
  - 输入验证
  - 错误提示反馈
- **多语言支持**: 通过 LanguageManager 服务支持中英文切换

**核心文件**:
- [LoginScreen.xaml](file:///d:/C%23app/windows7/windows7/Controls/LoginScreen.xaml)
- [LoginScreen.xaml.cs](file:///d:/C%23app/windows7/windows7/Controls/LoginScreen.xaml.cs)

---

## 2. 桌面环境

### 2.1 桌面区域 (MainWindow)
- **桌面功能**:
  - 自定义壁纸背景
  - 桌面图标管理
  - 右键菜单系统
  - 拖放操作支持
- **桌面图标特性**:
  - 文件夹和文件图标
  - 图标重命名
  - 图标删除
  - 属性查看
  - 选中状态高亮

### 2.2 右键菜单
- **新建文件夹**
- **新建文件**
- **剪切/复制/粘贴**
- **更改壁纸**
- **显示设置**
- **个性化设置**
- **打开终端**
- **打开设置**
- **刷新桌面**

### 2.3 布局管理
- **画布布局**: 使用 Canvas 实现自由定位
- **Z-Index 管理**: 控制窗口堆叠顺序
- **窗口状态**: 支持最小化、最大化、关闭

**核心文件**:
- [MainWindow.xaml](file:///d:/C%23app/windows7/windows7/MainWindow.xaml)
- [MainWindow.xaml.cs](file:///d:/C%23app/windows7/windows7/MainWindow.xaml.cs)

---

## 3. Shelf 任务栏

### 3.1 Shelf 组件结构
```
┌─────────────────────────────────────────────────────────────────────────┐
│  [Launcher] │ [Chrome] │ [Files] │ [Settings] │ [Terminal] │ [Calculator] │ ... │ [System Tray] │
└─────────────────────────────────────────────────────────────────────────┘
```

### 3.2 功能特性

#### 左侧应用区域
- **启动器按钮**: 彩色圆形图标，点击打开启动器
- **固定应用**: 常用应用的快捷访问
- **运行指示器**: 应用运行时的绿色小圆点
- **应用切换**: 点击图标可激活或打开应用

#### 右侧系统托盘
- **快速设置面板**: 包含亮度、音量、蓝牙等设置
- **网络状态**: Wi-Fi 和以太网连接状态
- **音量控制**: 音量滑块和静音按钮
- **电池状态**: 电池电量显示和充电状态
- **时间显示**: 实时时钟和日期
- **通知中心**: 系统通知查看
- **关机菜单**: 关机、重启、睡眠、登出选项

### 3.3 视觉设计
- **MacOS 风格**: 半透明磨砂玻璃效果
- **圆角设计**: 18px 圆角边框
- **阴影效果**: 模糊阴影增强层次感
- **居中布局**: 任务栏居中显示

**核心文件**:
- [Shelf.xaml](file:///d:/C%23app/windows7/windows7/Controls/Shelf.xaml)
- [Shelf.xaml.cs](file:///d:/C%23app/windows7/windows7/Controls/Shelf.xaml.cs)

---

## 4. 启动器

### 4.1 启动器界面
- **搜索栏**: 支持实时搜索应用和文件
  - 占位符文本: "Search apps, files..."
  - 聚焦时自动清空
  - 模糊匹配算法
- **应用网格**: WrapPanel 布局，响应式排列
- **背景遮罩**: 半透明黑色背景，点击关闭

### 4.2 应用列表 (18个预置应用)

| 图标 | 应用名称 | AppType | 功能描述 |
|------|---------|---------|---------|
| 🌐 | Chrome | Browser | 网页浏览器 |
| 📁 | Files | Files | 文件管理器 |
| ⚙️ | Settings | Settings | 系统设置 |
| 💻 | Terminal | Terminal | 命令行终端 |
| 📝 | Text Editor | TextEditor | 文本编辑器 |
| 🔢 | Calculator | Calculator | 计算器 |
| 📷 | Camera | Camera | 相机应用 |
| 🖼️ | Photos | Photos | 图片查看器 |
| 🏪 | Play Store | PlayStore | 应用商店 |
| ⬇️ | Downloads | Downloads | 下载管理器 |
| 📧 | Gmail | Gmail | 邮件客户端 |
| 🎬 | YouTube | YouTube | 视频网站 |
| 🗺️ | Maps | Maps | 地图应用 |
| ☁️ | Drive | Drive | 云存储 |
| ⏰ | Clock | Clock | 时钟应用 |
| 📅 | Calendar | Calendar | 日历应用 |
| 🌤️ | Weather | Weather | 天气应用 |
| 📰 | News | News | 新闻应用 |
| 🎵 | Music | Music | 音乐播放器 |
| 👥 | Contacts | Contacts | 联系人 |

### 4.3 搜索功能
- **实时过滤**: 输入时即时筛选应用
- **不区分大小写**: 支持大小写混合搜索
- **名称匹配**: 基于应用名称的模糊搜索

**核心文件**:
- [LauncherControl.xaml](file:///d:/C%23app/windows7/windows7/Controls/LauncherControl.xaml)
- [LauncherControl.xaml.cs](file:///d:/C%23app/windows7/windows7/Controls/LauncherControl.xaml.cs)

---

## 5. 应用程序系统

### 5.1 Chrome 浏览器 (BrowserApp)

#### 功能特性
- **WebView2 引擎**: 基于 Microsoft Edge 的现代浏览器内核
- **地址栏**: 
  - URL 输入
  - 回车键导航
  - 自动添加 https:// 前缀
- **导航按钮**:
  - 后退 (Go Back)
  - 前进 (Go Forward)
  - 刷新 (Reload)
- **快捷按钮**:
  - Google
  - YouTube
  - GitHub

#### 界面布局
```
┌───────────────────────────────────────────────────────────────┐
│  [←][→][⟳]  https://www.google.com  [🔍]                     │
├───────────────────────────────────────────────────────────────┤
│                                                               │
│                    WebView 内容区域                           │
│                                                               │
└───────────────────────────────────────────────────────────────┘
```

**核心文件**:
- [BrowserApp.xaml](file:///d:/C%23app/windows7/windows7/Apps/BrowserApp.xaml)
- [BrowserApp.xaml.cs](file:///d:/C%23app/windows7/windows7/Apps/BrowserApp.xaml.cs)

---

### 5.2 文件管理器 (FilesApp)

#### 功能特性

##### 5.2.1 文件系统结构
预置虚拟文件系统，包含以下目录：
```
My files/
├── Documents/
│   ├── Work/
│   │   ├── project-plan.docx
│   │   ├── meeting-notes.txt
│   │   └── budget.xlsx
│   ├── Personal/
│   │   ├── diary.txt
│   │   └── resume.docx
│   ├── report.docx
│   ├── spreadsheet.xlsx
│   └── presentation.pptx
├── Downloads/
│   ├── installer.exe
│   ├── document.pdf
│   ├── photo.jpg
│   ├── archive.zip
│   └── song.mp3
├── Images/
│   ├── Vacation/
│   │   ├── beach.jpg
│   │   ├── mountain.jpg
│   │   └── sunset.jpg
│   ├── Screenshots/
│   │   ├── screenshot1.png
│   │   └── screenshot2.png
│   ├── wallpaper.png
│   └── avatar.jpg
├── Music/
│   ├── playlist1.mp3
│   ├── song1.mp3
│   └── song2.flac
├── Videos/
│   ├── tutorial.mp4
│   └── vlog.avi
├── readme.txt
└── notes.md

Computer/
├── Local Disk (C:)/
│   ├── Users/
│   ├── Program Files/
│   ├── Windows/
│   └── ProgramData/
├── Local Disk (D:)/
│   ├── Projects/
│   ├── Games/
│   └── Backup/
├── Network/
└── Removable Storage/

Play Files/
└── Android/
    ├── data/
    └── obb/
```

##### 5.2.2 文件类型支持

| 扩展名 | 图标颜色 | 描述 | 处理方式 |
|--------|---------|------|---------|
| .txt, .md | 蓝色 | 文本文档 | 文本预览 |
| .jpg, .png, .gif, .bmp | 黄色 | 图片文件 | 图片查看器 |
| .mp3, .wav, .flac | 红色 | 音频文件 | 音乐播放器 |
| .mp4, .avi, .mkv | 红色 | 视频文件 | 视频播放器 |
| .pdf | 红色 | PDF 文档 | 文档预览 |
| .zip, .rar, .7z | 黄色 | 压缩文件 | 提示信息 |
| .exe, .msi | 绿色 | 可执行文件 | 提示信息 |

##### 5.2.3 导航栏功能
- **后退按钮**: 返回上一目录
- **上级按钮**: 返回父目录
- **刷新按钮**: 重新加载当前目录
- **新建文件夹**: 创建新文件夹
- **新建文件**: 创建新文件
- **视图切换**: 网格视图/列表视图

##### 5.2.4 侧边栏快速访问
- My files
- Downloads
- Play Files
- Images
- Documents
- Music

##### 5.2.5 搜索功能
- **全局搜索**: 在整个文件系统中搜索
- **实时更新**: 输入时即时显示结果
- **结果提示**: 无结果时显示提示信息

##### 5.2.6 文件操作
- **打开**: 双击或右键选择打开
- **重命名**: 修改文件/文件夹名称
- **删除**: 删除文件/文件夹（带确认）
- **属性**: 查看文件详细信息

##### 5.2.7 视图模式

**网格视图** (GridView):
- 110x110px 卡片
- 大图标 (48x48px)
- 名称文字换行显示
- 适合快速浏览

**列表视图** (ListView):
- 详细信息展示
- 显示文件大小
- 显示修改日期
- 悬停高亮效果

**核心文件**:
- [FilesApp.xaml](file:///d:/C%23app/windows7/windows7/Apps/FilesApp.xaml)
- [FilesApp.xaml.cs](file:///d:/C%23app/windows7/windows7/Apps/FilesApp.xaml.cs)

---

### 5.3 其他应用程序

#### 设置 (SettingsApp)
- 系统设置界面
- 各项设置分类
- 语言切换选项
- 显示设置

#### 终端 (TerminalApp)
- 命令行界面模拟
- 支持基本命令
- 键盘交互

#### 文本编辑器 (TextEditorApp)
- 文本编辑功能
- 打开/保存文件
- 基本编辑操作

#### 计算器 (CalculatorApp)
- 基础数学运算
- 界面设计精美

#### 时钟 (ClockApp)
- 数字时钟显示
- 日期展示
- 可能的闹钟功能

#### 日历 (CalendarApp)
- 月视图显示
- 日期导航
- 事件标记

#### 天气 (WeatherApp)
- 天气信息展示
- 温度显示
- 天气图标

**核心文件**:
- 各应用位于: [Apps 文件夹](file:///d:/C%23app/windows7/windows7/Apps/)

---

## 6. 系统服务

### 6.1 声音服务 (SoundService)

#### 功能模块
- **MediaPlayer 管理**: 静态 MediaPlayer 实例
- **音量控制**: 1.0 最大音量
- **启动音效**: PlayStartupSound()
  - 播放 start.mp3
  - 完整播放（0-6秒）

- **关机音效**: PlayShutdownSound()
  - 播放 end.mp3
  - 从第 9 秒开始到第 14 秒结束
  - 总时长 5 秒

#### 播放机制
```csharp
1. 查找音频文件（支持多路径）
   - AppDomain.CurrentDomain.BaseDirectory
   - 程序集所在目录
   - Resources 子目录
   - 项目根目录
2. 停止当前播放
3. 打开音频文件
4. 设置起始位置（如需要）
5. 开始播放
6. 等待指定时长
7. 停止播放
```

#### 搜索路径优先级
1. 应用程序基础目录
2. 程序集所在目录
3. 程序集目录/Resources
4. 项目根目录
5. 解决方案根目录

**核心文件**:
- [SoundService.cs](file:///d:/C%23app/windows7/windows7/Services/SoundService.cs)

---

### 6.2 持久化服务 (PersistenceService)

#### 功能
- **桌面图标保存**: 保存桌面图标的位置和数据
- **桌面图标加载**: 启动时恢复桌面图标
- **数据序列化**: 可能使用 JSON 或 XML 格式

**核心文件**:
- [PersistenceService.cs](file:///d:/C%23app/windows7/windows7/Services/PersistenceService.cs)

---

### 6.3 语言管理 (LanguageManager)

#### 功能
- **多语言支持**: 中文/英文双语
- **语言切换**: 动态切换界面语言
- **资源管理**: 语言资源文件管理

#### 资源文件
- [Strings.en.xaml](file:///d:/C%23app/windows7/windows7/Resources/Strings.en.xaml): 英文资源
- [Strings.zh.xaml](file:///d:/C%23app/windows7/windows7/Resources/Strings.zh.xaml): 中文资源

**核心文件**:
- [LanguageManager.cs](file:///d:/C%23app/windows7/windows7/Services/LanguageManager.cs)

---

## 7. 关机系统

### 7.1 关机确认对话框 (ShutdownConfirmationDialog)

#### 界面优化
- **尺寸调整**: 560×420px（确保按钮完全显示）
- **布局**:
  - 图标区域: 关机图标 + 旋转动画
  - 标题: "确认关机"
  - 提示信息
  - 按钮区域: "取消" + "关机" 按钮

#### 视觉效果
- **图标动画**: 外圈旋转动画
- **渐变背景**: 现代深色主题
- **圆角设计**: 24px 圆角边框
- **阴影效果**: 32px 模糊阴影

#### 按钮样式
- **取消按钮**:
  - 灰色背景 (#3A3A3C)
  - 悬停: #454548
  - 按下: #4E4E51

- **关机按钮**:
  - 红色背景 (#EA4335)
  - 悬停: #F15858
  - 按下: #D63939

**核心文件**:
- [ShutdownConfirmationDialog.xaml](file:///d:/C%23app/windows7/windows7/Controls/ShutdownConfirmationDialog.xaml)
- [ShutdownConfirmationDialog.xaml.cs](file:///d:/C%23app/windows7/windows7/Controls/ShutdownConfirmationDialog.xaml.cs)

---

### 7.2 关机屏幕 (ShutdownScreen)

#### 界面设计
```
┌─────────────────────────────────────────┐
│                                         │
│              [⊙]                        │
│            (动画效果)                    │
│                                         │
│         正在关机...                     │
│         感谢您的使用！                   │
│                                         │
│    [═══════════]                        │
│       请稍候...                         │
│                                         │
└─────────────────────────────────────────┘
```

#### 动画效果
1. **脉冲动画**: 中心圆圈缩放动画
2. **外圈旋转**: 外环持续旋转
3. **内圈旋转**: 内环反向旋转
4. **装饰球动画**: 周围球体缩放

#### 关机流程
```
1. 用户点击关机
   ↓
2. 显示确认对话框
   ↓
3. 用户确认
   ↓
4. 隐藏桌面和所有窗口
   ↓
5. 显示关机屏幕
   ↓
6. 播放关机音效（9秒-14秒，共5秒）
   ↓
7. 等待 5 秒（与音效同步）
   ↓
8. 调用 Application.Current.Shutdown()
   ↓
9. 程序完全退出
```

**核心文件**:
- [ShutdownScreen.xaml](file:///d:/C%23app/windows7/windows7/Controls/ShutdownScreen.xaml)
- [ShutdownScreen.xaml.cs](file:///d:/C%23app/windows7/windows7/Controls/ShutdownScreen.xaml.cs)

---

## 8. 文件格式和资源

### 8.1 资源文件

#### 音频资源
| 文件 | 用途 | 说明 |
|------|------|------|
| [start.mp3](file:///d:/C%23app/windows7/windows7/Resources/start.mp3) | 启动音效 | 系统启动时播放 |
| [end.mp3](file:///d:/C%23app/windows7/windows7/Resources/end.mp3) | 关机音效 | 从第 9 秒播放到 14 秒 |

#### 图像资源
| 文件 | 用途 |
|------|------|
| [logo.png](file:///d:/C%23app/windows7/windows7/Resources/logo.png) | 系统 Logo |

#### 样式资源
- [XinhaoOSResources.xaml](file:///d:/C%23app/windows7/windows7/Resources/XinhaoOSResources.xaml): 全局样式定义
  - 按钮样式
  - 颜色资源
  - 字体样式
  - 布局模板

### 8.2 数据模型

#### AppInfo 模型
```csharp
public class AppInfo
{
    public string Id { get; set; }          // 应用唯一标识
    public string Name { get; set; }        // 应用名称
    public AppType AppType { get; set; }    // 应用类型枚举
    public object Data { get; set; }        // 附加数据
}
```

#### AppType 枚举
```csharp
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
    Gmail,
    YouTube,
    Maps,
    Drive,
    Clock,
    Calendar,
    Weather,
    News,
    Music,
    Contacts
}
```

**核心文件**:
- [AppInfo.cs](file:///d:/C%23app/windows7/windows7/Models/AppInfo.cs)

---

## 9. 系统控制面板

### 9.1 快速设置面板 (QuickSettingsPanel)
- 亮度调节滑块
- 音量控制
- 蓝牙开关
- Wi-Fi 开关
- 飞行模式
- 夜间模式
- 屏幕截图
- 系统设置入口

### 9.2 网络面板 (NetworkPanel)
- Wi-Fi 网络列表
- 已连接网络信息
- 网络状态指示
- 网络设置入口

### 9.3 音量面板 (VolumePanel)
- 音量滑块 (0-100%)
- 静音开关
- 输出设备选择
- 音量调节动画

### 9.4 电池面板 (BatteryPanel)
- 电量百分比显示
- 充电状态指示
- 剩余时间估算
- 省电模式开关

### 9.5 通知中心 (NotificationCenter)
- 通知列表
- 通知清除
- 勿扰模式
- 通知设置

---

## 10. 窗口管理系统

### 10.1 XinhaoOSWindow 组件

#### 窗口特性
- **自定义标题栏**: 关闭、最小化、最大化按钮
- **可拖动**: 标题栏拖动移动窗口
- **可调整大小**: 窗口边缘调整尺寸
- **Z-Index 管理**: 点击窗口置顶
- **状态管理**: 最小化/正常/最大化状态

#### 窗口结构
```
┌─────────────────────────────────────────┐
│ [App Title]              [─][□][✕]      │ ← 标题栏
├─────────────────────────────────────────┤
│                                         │
│           应用内容区域                   │ ← WindowContent
│                                         │
└─────────────────────────────────────────┘
```

#### 窗口状态管理
- **激活状态**: 窗口在最上层显示
- **最小化状态**: 隐藏到任务栏
- **打开窗口列表**: 跟踪所有打开的窗口
- **运行状态指示**: 任务栏上的绿色圆点

**核心文件**:
- [XinhaoOSWindow.xaml](file:///d:/C%23app/windows7/windows7/Controls/XinhaoOSWindow.xaml)
- [XinhaoOSWindow.xaml.cs](file:///d:/C%23app/windows7/windows7/Controls/XinhaoOSWindow.xaml.cs)
- [SystemControls.cs](file:///d:/C%23app/windows7/windows7/Controls/SystemControls.cs)

---

## 11. 用户体验优化

### 11.1 动画效果
- **启动动画**: BootScreen 加载动画
- **登录过渡**: 平滑过渡到桌面
- **窗口动画**: 打开/关闭/最小化动画
- **按钮效果**: 悬停/按下状态变化
- **关机动画**: ShutdownScreen 动画效果

### 11.2 主题设计
- **深色模式**: 默认深色主题
- **配色方案**:
  - 主色调: Google 四色 (#EA4335, #FBBC04, #34A853, #4285F4)
  - 背景色: #0D0D0F, #1E1E20, #2C2C2E
  - 文字色: #FFFFFF, #E8EAED, #9AA0A6
  - 按钮色: #3A3A3C, #35363A, #3C3D40
- **圆角设计**: 统一的圆角风格 (8px-24px)
- **阴影效果**: 多层次阴影设计

### 11.3 响应式设计
- **启动器网格**: 自适应应用排列
- **任务栏**: 响应式布局
- **窗口尺寸**: 多种默认尺寸
- **高 DPI 支持**: 兼容高分辨率屏幕

---

## 12. 技术架构

### 12.1 项目结构
```
windows7/
├── Apps/                    # 应用程序模块
│   ├── BrowserApp.xaml/.cs
│   ├── CalculatorApp.xaml/.cs
│   ├── CalendarApp.xaml/.cs
│   ├── ClockApp.xaml/.cs
│   ├── ContactsApp.xaml/.cs
│   ├── DriveApp.xaml/.cs
│   ├── DownloadsApp.xaml/.cs
│   ├── FilesApp.xaml/.cs
│   ├── GmailApp.xaml/.cs
│   ├── MapsApp.xaml/.cs
│   ├── MusicApp.xaml/.cs
│   ├── NewsApp.xaml/.cs
│   ├── PhotosApp.xaml/.cs
│   ├── PlayStoreApp.xaml/.cs
│   ├── SettingsApp.xaml/.cs
│   ├── TerminalApp.xaml/.cs
│   ├── TextEditorApp.xaml/.cs
│   └── WeatherApp.xaml/.cs
├── Controls/                # UI 控件
│   ├── BootScreen.xaml/.cs
│   ├── XinhaoOSWindow.xaml/.cs
│   ├── InputDialog.cs
│   ├── LauncherControl.xaml/.cs
│   ├── LoginScreen.xaml/.cs
│   ├── NotificationCenter.xaml/.cs
│   ├── QuickSettingsPanel.xaml/.cs
│   ├── Shelf.xaml/.cs
│   ├── ShutdownConfirmationDialog.xaml/.cs
│   ├── ShutdownScreen.xaml/.cs
│   ├── SystemControls.cs
│   ├── SystemTray.xaml/.cs
│   ├── BatteryPanel.xaml/.cs
│   ├── NetworkPanel.xaml/.cs
│   └── VolumePanel.xaml/.cs
├── Models/                  # 数据模型
│   └── AppInfo.cs
├── Resources/               # 资源文件
│   ├── XinhaoOSResources.xaml
│   ├── Strings.en.xaml
│   ├── Strings.zh.xaml
│   ├── end.mp3
│   ├── logo.png
│   └── start.mp3
├── Services/                # 业务服务
│   ├── LanguageManager.cs
│   ├── PersistenceService.cs
│   └── SoundService.cs
├── App.xaml/.cs            # 应用程序入口
├── AssemblyInfo.cs         # 程序集信息
├── MainWindow.xaml/.cs     # 主窗口
└── windows7.csproj         # 项目文件
```

### 12.2 核心依赖
- **.NET 10.0**: 基础框架
- **WPF**: UI 框架
- **Microsoft.Web.WebView2**: Web 浏览引擎

---

## 13. 快速开始

### 13.1 构建项目
```powershell
# 克隆项目
cd d:\C#app\windows7

# 还原依赖
dotnet restore

# 构建项目
dotnet build

# 运行项目
dotnet run
```

### 13.2 开发环境
- **IDE**: Visual Studio 2022 或更高
- **SDK**: .NET 10.0 SDK
- **运行时**: .NET 10.0 Desktop Runtime
- **WebView2**: Microsoft Edge WebView2 Runtime

---

## 14. 未来扩展方向

- [ ] 多用户账户管理
- [ ] 壁纸自定义
- [ ] 主题切换（浅色/深色）
- [ ] 窗口分屏功能
- [ ] 虚拟桌面
- [ ] 更多原生应用
- [ ] 插件系统
- [ ] Android 应用支持
- [ ] 云端同步功能
- [ ] 系统更新功能

---

## 结语

本文档详细介绍了 XinhaoOS 桌面环境的所有功能模块。这个项目展示了如何使用 WPF 创建一个功能完整、设计精美的现代操作系统用户界面，包含了从启动到关机的完整用户旅程。

**项目贡献者**: 炘灏墨麒麟
**最后更新**: 2026-05-31
**版本**: 1.0.0
