# Hermes Agent 配置备份 & 恢复指南

## 概述
本仓库用于备份 Hermes Agent 的配置文件、板换知识库索引和开发工具，
方便换电脑时一键恢复工作环境。

---

## 🔄 快速恢复（推荐）

**新电脑上安装 Hermes 后，对我说：**

> `恢复我的配置`

我就会自动执行 `hermes-restore` skill，按以下步骤恢复：

1. 创建 `config.yaml` 基础配置
2. 设置 pywin32 / comtypes 等依赖
3. 编译 ghproxy-cli 并配置开机自启
4. 创建每日早报 cron 任务
5. 设置 Windows 环境变量（HTTPS_PROXY / NO_PROXY / PIP_INDEX_URL）
6. 提示你手动操作的部分

---

## 🛠 手动恢复步骤

### 1. 安装 Hermes
下载并安装 Hermes 桌面版，启动一次确保正常。

### 2. 克隆本仓库
```bash
cd E:\13_WorkSpaceForHermes
git clone https://github.com/xz1996618/Hermes-agent-backup.git
```

### 3. 还原配置文件
将 `config.yaml` 覆盖到：
```
C:\Users\<你的用户名>\AppData\Local\hermes\
```

### 4. 配置 API Key
复制 `env-template.txt` 为 `.env`，填入你的 API Key：
```
C:\Users\<你的用户名>\AppData\Local\hermes\.env
```

需要填入的 Key：
| Key | 来源 | 说明 |
|-----|------|------|
| `DASHSCOPE_API_KEY` | dashscope.aliyuncs.com | 千问模型（识图+生图） |
| `DEEPSEEK_API_KEY` | platform.deepseek.com | DeepSeek 主模型 |
| `WEIXIN_HOME_CHANNEL` | 微信扫码绑定后自动生成 | 微信网关 |

### 5. 还原早报脚本
```bash
mkdir -p %APPDATA%\Local\hermes\scripts
copy scripts\daily_briefing.py %APPDATA%\Local\hermes\scripts\
```

然后创建 cron 任务：
```bash
hermes cron add --name "每日行业资讯早报" --schedule "0 10 * * *" --script daily_briefing.py --deliver all
```

### 6. 编译并启动 ghproxy-cli
```bash
cd ghproxy-cli
dotnet build -c Release
.\bin\Release\net8.0\win-x64\ghproxy-cli.exe start
```

### 7. 设置 ghproxy-cli 开机自启
```bash
copy scripts\ghproxy-cli-startup.bat "%APPDATA%\Microsoft\Windows\Start Menu\Programs\Startup\"
```

### 8. 设置 Windows 环境变量
```cmd
setx HTTPS_PROXY "http://127.0.0.1:16888"
setx NO_PROXY "localhost,127.0.0.1,.dashscope.aliyuncs.com,.deepseek.com,.bing.com,.baidu.com,.qq.com,.weixin.qq.com,.openstd.samr.gov.cn,pypi.org,files.pythonhosted.org"
```

### 9. 配置微信网关
```cmd
hermes gateway install
# 按提示选择 WeChat → 扫码绑定
```

### 10. 还原知识库
知识库 PDF 文件较大（62篇），需从旧电脑手动复制：
```
旧电脑: E:\13_WorkSpaceForHermes\01_bphe_knowledge_base\文献阅读\
新电脑: 同样路径
```

### 11. 恢复 SW 技能
```cmd
hermes skills install solidworks-automation-skill
```

---

## ⚠️ 注意事项

| 注意点 | 说明 |
|-------|------|
| 🔑 **API Key** | 已从配置中移除，`env-template.txt` 是模板，请勿直接上传真实 Key |
| 📄 **知识库 PDF** | 仅备份了索引（INDEX.md），原始 PDF 需手动复制 |
| 📱 **微信网关** | 换电脑后需重新扫码绑定 |
| 🪟 **快捷方式** | Hermes 更新后快捷方式会失效，需重建 |
| 📦 **ghproxy-cli** | 需要 .NET 8 SDK 才能编译 |
| 🐍 **PyPI 镜像** | 已配清华镜像，解决 pypi.org 连不上的问题 |

---

## 📁 目录结构

```
Hermes-agent-backup/
├── README.md                  ← 本文件（恢复指南）
├── AGENTS.md                  ← Hermes 项目规范
├── env-template.txt           ← API Key 模板（需填入自己的 Key）
├── scripts/
│   ├── daily_briefing.py      ← 每日行业早报脚本
│   └── ghproxy-cli-startup.bat  ← 开机自启脚本
├── ghproxy-cli/
│   ├── Program.cs             ← 主程序
│   ├── ProxyServer.cs         ← TCP 代理核心
│   ├── IpSelector.cs          ← IP 测速优选
│   ├── GitConfig.cs           ← Git 代理配置
│   └── ghproxy-cli.csproj     ← 项目文件
└── knowledge_base/
    └── INDEX.md               ← 板换知识库索引
```

---

## 🔄 更新备份

如需更新本仓库的备份内容，直接修改文件后对我或群里说：
> `更新 Github 备份`

---

*备份日期: 2026-07-02*
*下次换电脑时，别忘了先对我说 "恢复我的配置"*


## 💾 记忆与会话恢复

记忆文件和会话历史存储在 `C:\Users\<用户名>\AppData\Local\hermes\` 下：

| 内容 | 大小 | 备份方式 | 恢复方式 |
|------|------|---------|---------|
| **记忆**（MEMORY.md / USER.md） | ~12KB | ✅ 已上传 GitHub | 说"恢复我的配置"自动还原 |
| **各profile记忆**（6个profile） | ~60KB | ✅ 已上传 GitHub | 说"恢复我的配置"自动还原 |
| **会话历史** | ~77MB | ⚠️ 太大，手动复制 | 从旧电脑复制整个 hermes 目录 |

### 完整还原（推荐，包含会话历史）
从旧电脑复制整个目录到新电脑：
```
旧电脑: C:\Users\Administrator\AppData\Local\hermes\
新电脑: 同样路径覆盖
```
所有记忆、会话、配置、Skill **全部保留**。

### 仅还原记忆（自动）
安装 Hermes 后对我说 **"恢复我的配置"**，GitHub 上的记忆文件会自动下载还原。
