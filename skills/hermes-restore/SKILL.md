---
name: hermes-restore
description: 从 GitHub 备份仓库恢复 Hermes Agent 完整工作环境。换电脑后说"恢复我的配置"即触发。
tags: [backup, restore, setup, migration]
---

# Hermes 配置恢复技能

## 触发条件
用户说：恢复我的配置 / 还原环境 / 换电脑恢复 / 备份还原

## 步骤

### 1. 克隆备份仓库
```bash
git clone https://github.com/xz1996618/Hermes-agent-backup.git E:\13_WorkSpaceForHermes\Hermes-agent-backup
```

### 2. 还原 AGENTS.md
```bash
copy E:\13_WorkSpaceForHermes\Hermes-agent-backup\AGENTS.md E:\13_WorkSpaceForHermes\AGENTS.md
```

### 3. 提示用户创建 .env
告知用户复制 `env-template.txt` 到 `C:\Users\Administrator\AppData\Local\hermes\.env`，填入自己的 API Key。

### 4. 还原早报脚本
```bash
mkdir -p C:\Users\Administrator\AppData\Local\hermes\scripts
copy E:\13_WorkSpaceForHermes\Hermes-agent-backup\scripts\daily_briefing.py C:\Users\Administrator\AppData\Local\hermes\scripts\
```

### 5. 创建 cron 任务
使用 cronjob 工具创建每日早报任务：
- 名称: 每日行业资讯早报
- 时间: 0 10 * * *
- 脚本: daily_briefing.py
- 投递: all

### 6. 编译 ghproxy-cli
```bash
cd E:\13_WorkSpaceForHermes\Hermes-agent-backup\ghproxy-cli
dotnet build -c Release
```

### 7. 设置 ghproxy-cli 开机自启
```bash
copy scripts\ghproxy-cli-startup.bat "%APPDATA%\Microsoft\Windows\Start Menu\Programs\Startup\"
```

### 8. 设置系统环境变量
```bash
setx HTTPS_PROXY "http://127.0.0.1:16888"
setx NO_PROXY "localhost,127.0.0.1,.dashscope.aliyuncs.com,.deepseek.com,.bing.com,.baidu.com,.qq.com,.weixin.qq.com,.openstd.samr.gov.cn,pypi.org,files.pythonhosted.org"
setx PIP_INDEX_URL "https://pypi.tuna.tsinghua.edu.cn/simple"
```

### 9. 安装依赖
```bash
pip install pywin32 comtypes
```

### 10. 告知用户手动操作
- 微信扫码绑定：`hermes gateway install` → 选 WeChat
- 复制知识库 PDF：从旧电脑 E 盘复制
- 安装 SW skill：`hermes skills install solidworks-automation-skill`
- 启动 ghproxy-cli：运行 ghproxy-cli.exe start

### 11. 完成确认
告知用户所有可自动完成的操作已完成，列出还需手动操作的步骤。
