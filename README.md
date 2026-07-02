# Hermes Agent 配置备份

## 概述
本仓库用于备份 Hermes Agent 的配置文件和板式换热器研发工作环境，方便换电脑时快速恢复。

---

## 包含内容

| 目录/文件 | 说明 |
|-----------|------|
| `config/` | Hermes 配置文件（config.yaml 去敏版） |
| `scripts/` | 每日行业早报推送脚本 |
| `knowledge_base/` | 板换知识库索引 INDEX.md |
| `ghproxy-cli/` | GitHub 加速工具源码 |
| `AGENTS.md` | 项目规范与使用规则 |
| `env-template.txt` | 环境变量模板（填入自己的 API Key） |
| `restore_guide.md` | 恢复指南 |

---

## 换电脑恢复步骤

### 1. 安装 Hermes
下载并安装 Hermes 桌面版

### 2. 还原配置
将本仓库的 `config/config.yaml` 覆盖到：
```
C:\Users\<你的用户名>\AppData\Local\hermes\
```

### 3. 配置 API Key
复制 `env-template.txt` 为 `.env`，填入自己的 API Key

### 4. 还原早报脚本
```
cp scripts/daily_briefing.py C:\Users\<你的用户名>\AppData\Local\hermes\scripts\
```

### 5. 还原 ghproxy-cli
```
# 编译发布版
cd ghproxy-cli
dotnet publish -c Release -o bin/publish
```

### 6. 设置开机自启
```
# 复制启动脚本到启动文件夹
copy scripts/ghproxy-cli.bat "%APPDATA%\Microsoft\Windows\Start Menu\Programs\Startup\"
```

### 7. 配置环境变量
```
setx HTTPS_PROXY "http://127.0.0.1:16888"
setx NO_PROXY "localhost,127.0.0.1,.dashscope.aliyuncs.com,.deepseek.com,.bing.com,.baidu.com,.qq.com,.weixin.qq.com,.openstd.samr.gov.cn,pypi.org,files.pythonhosted.org"
```

### 8. 还原知识库
知识库文件较大（62篇PDF），需手动从旧电脑 `E:\13_WorkSpaceForHermes\01_bphe_knowledge_base\` 复制过来。

### 9. 配置微信网关
开机后执行：
```
hermes gateway install
```

---

## 注意事项
- API Key 已从配置中移除，请勿传回 GitHub
- 知识库 PDF 仅备份索引，原始文件需手动复制
- 换电脑后微信网关需要重新扫码绑定
- 建议使用私有仓库存放配置

---

*备份日期: 2026-07-02*
