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

### 3. 还原记忆
通过 GitHub API 下载 memories/ 目录下的 MEMORY.md 和 USER.md，写入：
```
C:\Users\Administrator\AppData\Local\hermes\memories\
C:\Users\Administrator\AppData\Local\hermes\profiles\<profile>\memories\
```

### 4. 提示用户创建 .env
告知用户复制 `env-template.txt` 到 `C:\Users\Administrator\AppData\Local\hermes\.env`，填入 API Key。

### 5. 还原早报脚本
```bash
copy scripts\daily_briefing.py %APPDATA%\Local\hermes\scripts\
```

### 6. 创建 cron 任务
创建每日早报任务，脚本: daily_briefing.py，时间: 0 10 * * *，投递: all

### 7. 编译 ghproxy-cli
```bash
cd E:\13_WorkSpaceForHermes\Hermes-agent-backup\ghproxy-cli
dotnet build -c Release
```

### 8. 设置 ghproxy-cli 开机自启
复制 ghproxy-cli-startup.bat 到启动文件夹

### 9. 设置系统环境变量
```bash
setx HTTPS_PROXY "http://127.0.0.1:16888"
setx NO_PROXY "localhost,127.0.0.1,.dashscope.aliyuncs.com,.deepseek.com,.bing.com,.baidu.com,.qq.com,.weixin.qq.com,.openstd.samr.gov.cn,pypi.org,files.pythonhosted.org"
setx PIP_INDEX_URL "https://pypi.tuna.tsinghua.edu.cn/simple"
```

### 10. 告知用户手动操作
- 微信扫码绑定：hermes gateway install → 选 WeChat
- 复制知识库 PDF：从旧电脑 E 盘复制
- 如需完整会话历史：从旧电脑复制整个 hermes 目录
- 启动 ghproxy-cli：ghproxy-cli.exe start

### 11. 完成确认
列出所有已完成和需要手动操作的步骤。
