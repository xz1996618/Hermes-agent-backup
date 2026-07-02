---
name: hermes-restore
description: 从 GitHub 备份仓库恢复 Hermes Agent 完整工作环境。换电脑后说"恢复我的配置"即触发。
tags: [backup, restore, setup, migration]
---

# Hermes 配置恢复技能 - 使用手册

## 恢复前准备
确保已完成：
1. 安装 Hermes 桌面版
2. 克隆备份仓库：`git clone https://github.com/xz1996618/Hermes-agent-backup.git E:\13_WorkSpaceForHermes\Hermes-agent-backup`
3. 已在 Hermes 中打开对话

## 触发条件
对我说：恢复我的配置 / 还原环境 / 换电脑恢复 / 备份还原

## 自动恢复步骤

### 1. 还原 AGENTS.md
```cmd
copy /Y "E:\13_WorkSpaceForHermes\Hermes-agent-backup\AGENTS.md" "E:\13_WorkSpaceForHermes\AGENTS.md"
```

### 2. 还原记忆（核心）
从 GitHub API 下载 `memories/MEMORY.md` 和 `memories/USER.md`，保存到：
```
C:\Users\Administrator\AppData\Local\hermes\memories\
```
然后遍历 `profiles/` 下每个 profile 的子目录，下载各自的 MEMORY.md/USER.md 到：
```
C:\Users\Administrator\AppData\Local\hermes\profiles\<profile>\memories\
```
每个 profile 还原后通知用户。

### 3. 提示创建 .env
```
请复制 env-template.txt 到 C:\Users\Administrator\AppData\Local\hermes\.env，
然后填入你的 DASHSCOPE_API_KEY 和 DEEPSEEK_API_KEY。
路径：E:\13_WorkSpaceForHermes\Hermes-agent-backup\env-template.txt
```

### 4. 还原早报脚本
```cmd
mkdir "%APPDATA%\Local\hermes\scripts" 2>nul
copy /Y "E:\13_WorkSpaceForHermes\Hermes-agent-backup\scripts\daily_briefing.py" "%APPDATA%\Local\hermes\scripts\"
```

### 5. 创建 cron 任务
使用 cronjob 工具创建任务：
- 名称：每日行业资讯早报
- 时间：0 10 * * *（每天 10:00）
- 模式：no_agent: true（直跑脚本）
- 脚本：daily_briefing.py
- 投递：all（微信+当前会话）

### 6. 编译 ghproxy-cli
```cmd
cd /d "E:\13_WorkSpaceForHermes\Hermes-agent-backup\ghproxy-cli"
dotnet build -c Release
```
编译成功后，启动一次验证：
```cmd
"E:\13_WorkSpaceForHermes\Hermes-agent-backup\ghproxy-cli\bin\Release\net8.0\win-x64\ghproxy-cli.exe" start
```

### 7. 设置 ghproxy-cli 开机自启
```cmd
copy /Y "E:\13_WorkSpaceForHermes\Hermes-agent-backup\scripts\ghproxy-cli-startup.bat" "%APPDATA%\Microsoft\Windows\Start Menu\Programs\Startup\"
```

### 8. 设置系统环境变量
```cmd
setx HTTPS_PROXY "http://127.0.0.1:16888"
setx NO_PROXY "localhost,127.0.0.1,.dashscope.aliyuncs.com,.deepseek.com,.bing.com,.baidu.com,.qq.com,.weixin.qq.com,.openstd.samr.gov.cn,pypi.org,files.pythonhosted.org"
setx PIP_INDEX_URL "https://pypi.tuna.tsinghua.edu.cn/simple"
```

### 9. 安装 Python 依赖
```cmd
pip install pywin32 comtypes
```

### 10. 完成确认
向用户汇报已完成的操作，并告知以下需手动完成：

```
✅ 已完成（共 9 项）：
  1. AGENTS.md 已还原
  2. 记忆已还原（默认 profile + 所有子 profile）
  3. 早报脚本已安装
  4. cron 已创建（每日 10:00）
  5. ghproxy-cli 已编译
  6. ghproxy-cli 开机自启已配
  7. 系统环境变量已设置（需重启生效）
  8. Python 依赖已安装
  9. .env 模板已就绪

⚠️ 还需手动操作：
  1. 编辑 .env 填入真实的 API Key
  2. 执行 hermes gateway install → 选 WeChat → 扫码绑定
  3. 重启电脑
  4. 复制知识库 PDF（从旧电脑 E 盘）
  5. 如需完整会话历史：复制旧电脑整个 hermes 目录
  6. 安装 SW skill：hermes skills install solidworks-automation-skill
```

---

## 如何手动安装本技能（新电脑上第一步）

如果 Hermes 没有本 skill，先执行：
```cmd
mkdir "%APPDATA%\Local\hermes\skills\hermes-restore"
copy "E:\13_WorkSpaceForHermes\Hermes-agent-backup\skills\hermes-restore\SKILL.md" "%APPDATA%\Local\hermes\skills\hermes-restore\SKILL.md"
```
然后重启 Hermes，再说"恢复我的配置"。
