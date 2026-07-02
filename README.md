# Hermes Agent 配置备份 & 恢复指南

## 概述
本仓库用于备份、恢复 Hermes Agent 完整工作环境。
换电脑时只需**两步**即可恢复全部配置、记忆和会话。

---

## 🚀 换电脑恢复流程（只需两步）

### 第一步：克隆本仓库（新电脑上操作）
```cmd
git clone https://github.com/xz1996618/Hermes-agent-backup.git E:\13_WorkSpaceForHermes\Hermes-agent-backup
```

### 第二步：安装恢复技能
把仓库里的恢复 skill 复制到 Hermes 技能目录：
```cmd
mkdir "%APPDATA%\Local\hermes\skills\hermes-restore"
copy "E:\13_WorkSpaceForHermes\Hermes-agent-backup\skills\hermes-restore\SKILL.md" "%APPDATA%\Local\hermes\skills\hermes-restore\SKILL.md"
```

然后打开 Hermes，对我说：

> **恢复我的配置**

我就会自动执行所有恢复步骤。

---

## 🔄 `hermes-restore` skill 将自动执行以下操作

### ✅ 自动完成的步骤

| # | 操作 | 说明 |
|---|------|------|
| 1 | 还原 AGENTS.md | 复制到 E:\13_WorkSpaceForHermes\ |
| 2 | 还原记忆文件 | 从 GitHub 下载 MEMORY.md / USER.md 到所有 profile |
| 3 | 还原早报脚本 | 复制 daily_briefing.py 到 scripts 目录 |
| 4 | 创建 cron 任务 | 每日 10:00 准时推送行业早报 |
| 5 | 编译 ghproxy-cli | dotnet build，配置开机自启 |
| 6 | 设置环境变量 | HTTPS_PROXY / NO_PROXY / PIP_INDEX_URL |
| 7 | 安装 pywin32 / comtypes | SolidWorks COM 自动化依赖 |

### ⚠️ 需要你手动完成的步骤

| # | 操作 | 命令 |
|---|------|------|
| 1 | 配置 API Key | 复制 `env-template.txt` 为 `.env`，填入自己的 Key |
| 2 | 扫码绑定微信 | `hermes gateway install` → 选 WeChat |
| 3 | 复制知识库 PDF | 从旧电脑 E 盘复制 → 新电脑同样路径 |
| 4 | 复制会话历史（可选） | 从旧电脑复制整个 `C:\...\hermes\` 目录 |
| 5 | 重启电脑 | 使环境变量生效 |
| 6 | 启动 ghproxy-cli | 运行 ghproxy-cli.exe start |

---

## 🔧 手动恢复（如果自动化失败）

### 1. 安装 Hermes
从官网下载并安装 Hermes 桌面版，启动一次确认正常。

### 2. 克隆仓库
```cmd
cd E:\13_WorkSpaceForHermes
git clone https://github.com/xz1996618/Hermes-agent-backup.git
```

### 3. 还原配置文件
复制仓库中的文件到 Hermes 目录：
```cmd
copy AGENTS.md E:\13_WorkSpaceForHermes\AGENTS.md
```

### 4. 配置 API Key
```cmd
copy env-template.txt %APPDATA%\Local\hermes\.env
```
然后用记事本打开 `.env`，填入你的真实 API Key：
| Key | 从哪获取 |
|-----|---------|
| DASHSCOPE_API_KEY | dashscope.aliyuncs.com |
| DEEPSEEK_API_KEY | platform.deepseek.com |

### 5. 还原记忆
```cmd
mkdir %APPDATA%\Local\hermes\memories\
copy memories\* %APPDATA%\Local\hermes\memories\
```
每个 profile 下的记忆也要还原（目录名对照旧电脑的 profile 名）。

### 6. 还原早报
```cmd
mkdir %APPDATA%\Local\hermes\scripts\
copy scripts\daily_briefing.py %APPDATA%\Local\hermes\scripts\
```

### 7. 还原会话历史（可选，推荐）
关闭 Hermes，复制整个 hermes 目录：
```cmd
# 旧电脑上打包
# 新电脑上解压覆盖 %APPDATA%\Local\hermes\
```

### 8. 编译并启动 ghproxy-cli
```cmd
cd ghproxy-cli
dotnet build -c Release
.\bin\Release\net8.0\win-x64\ghproxy-cli.exe start
```

### 9. 设置 ghproxy-cli 开机自启
```cmd
copy scripts\ghproxy-cli-startup.bat "%APPDATA%\Microsoft\Windows\Start Menu\Programs\Startup\"
```

### 10. 设置系统环境变量
```cmd
setx HTTPS_PROXY "http://127.0.0.1:16888"
setx NO_PROXY "localhost,127.0.0.1,.dashscope.aliyuncs.com,.deepseek.com,.bing.com,.baidu.com,.qq.com,.weixin.qq.com,.openstd.samr.gov.cn,pypi.org,files.pythonhosted.org"
setx PIP_INDEX_URL "https://pypi.tuna.tsinghua.edu.cn/simple"
```

### 11. 配置微信网关
```cmd
hermes gateway install
```

### 12. 安装 SW 技能
```cmd
hermes skills install solidworks-automation-skill
```

---

## 📁 收录内容清单

```
Hermes-agent-backup/
├── README.md                               ← 恢复指南（本文件）
├── AGENTS.md                               ← Hermes 项目规范
├── env-template.txt                        ← API Key 模板
├── skills/
│   └── hermes-restore/
│       └── SKILL.md                        ← 恢复 skill（关键！）
├── scripts/
│   ├── daily_briefing.py                   ← 每日早报脚本
│   └── ghproxy-cli-startup.bat              ← ghproxy-cli 开机自启
├── ghproxy-cli/                            ← GitHub 加速工具源码
│   ├── Program.cs
│   ├── ProxyServer.cs
│   ├── IpSelector.cs
│   ├── GitConfig.cs
│   └── ghproxy-cli.csproj
├── knowledge_base/
│   └── INDEX.md                            ← 板换知识库索引
├── memories/
│   ├── MEMORY.md                           ← 默认 profile 记忆
│   └── USER.md                             ← 用户信息
└── profiles/
    └── <profile名>/
        └── memories/
            ├── MEMORY.md
            └── USER.md
```

---

## ⚠️ 注意事项

- **环境变量**：`setx` 设置后需要重启电脑或重新打开 Hermes 才能生效
- **API Key**：已从配置中移除，`env-template.txt` 是模板，请勿上传真实 Key
- **微信**：换电脑后需要重新扫码绑定
- **知识库 PDF**：仅备份了索引，原始 PDF（62篇）需从旧电脑复制
- **快捷方式**：Hermes 更新后快捷方式会失效，需重建
- **ghproxy-cli**：需要 .NET 8 SDK 才能编译
- **会话历史**：存储在 `state.db`（~59MB）和 `sessions/`（~18MB），太大无法上传 GitHub，建议直接复制整个 `hermes` 目录

---

## 📝 补充说明

### 如何更新备份
修改本仓库中的文件后，在 Hermes 里对我说：
> `更新 GitHub 备份`

### 如何查看备份内容
```cmd
git clone https://github.com/xz1996618/Hermes-agent-backup.git
dir Hermes-agent-backup /s
```

---

*备份日期: 2026-07-02*
*换电脑后先克隆本仓库 → 安装 skill → 对我说 "恢复我的配置"*
