Hermes storage paths on Windows: ~/AppData/Local/hermes/profiles/default/memories/ (memory files), ~/AppData/Local/hermes/skills/ (skill directories with SKILL.md), ~/AppData/Local/hermes/config.yaml (config). Full paths resolve under C:\Users\<user>\AppData\Local\hermes.
§
DashScope视觉模型配置要点：auxiliary.vision.provider 必须设为 "alibaba"（不是 "dashscope"），模型用 qwen3-vl-plus（兼容API下最强）。qwen3-vl-235b-a22b-thinking 仅原生API可用，Hermes兼容模式用不了。生图模型（qwen-image-2.0-pro等）在DashScope"图像生成"标签页单独开启，不在"全模态"标签下。
§
Hermes Agent 离线 Windows 安装器：已完成，输出 ~/Desktop/HermesOfflineSetup_v0.16.0.exe (~373MB)。所有架构、踩坑、构建流程已记录在 offline-hermes-installer-win skill 中（SKILL.md + references/ 下含 build_staging.py / assemble.py）。下次构建直接加载该 skill。
§
Hermes 离线安装器 Windows 踩坑全集：
1) 中文 Win cmd.exe 用 GBK(936)，UTF-8 BOM→"锘緻"乱码破坏首行。.bat 必须纯 ASCII 无 BOM，open('wb') + encode('ascii')。
2) 中文 Win PATH 可能残缺：xcopy/powershell 找不到。系统命令用 %SystemRoot%\System32\xxx.exe 全路径。
3) 创建桌面快捷方式用 cscript+VBScript（Win 全版本自带），不依赖 powershell。
4) C# 自解压桩原理：桩.exe + MARKER + 8字节ZIP长度(LE) + ZIP。桩必须从1MB偏移全文扫描标记，不能只扫末尾。
5) 桌面版路径：apps\desktop\release\win-unpacked\Hermes.exe，需先设 PATH 含 hermes\bin 再启动。
§
Alfa Laval 产品搜索策略：主站 alfalaval.com 的产品页面经常 404（网站改版后旧路径失效），但地区子站（alfalaval.sg/.com.au/.co.uk 等）可能保留了完整的产品页面。应优先搜索：1) 地区子站 + 产品路径 2) media/news 新闻发布页（常有详细参数）3) globalassets/documents 下的 PDF 产品手册（产品 leaflet 含完整技术规格）。AC900 实际路径为 /products/.../ac/ac900/（不是 /ac-series/ac900/），产品 leaflet 文件名格式为 `ac900_-product-leaflet_en.pdf`。PDF 文本提取用 pdftotext 命令行工具（Windows Git-Bash 自带）。
§
未经用户明确同意，不得在用户电脑上安装任何系统级软件（如 LibreOffice、Chrome 等），包括通过 winget/choco/npm -g 等方式。项目本地依赖（npm install 在项目目录内）也需先征得同意。工具类安装（如 pip install 到已有 venv）相对可接受但仍应告知用户。
§
视觉模型配置：auxiliary.vision.provider 必须设为 alibaba（不是 dashscope），模型用 qwen3-vl-plus（兼容API下最强）。qwen3-vl-235b-a22b-thinking 仅原生API可用，Hermes兼容模式用不了。生图模型（qwen-image-2.0-pro等）在DashScope"图像生成"标签页单独开启，不在"全模态"标签下。
§
每日早报 cron：脚本 scripts/daily_briefing.py 抓取17个中文行业网站，no_agent=true 避免审批卡住，deliver=all 投递微信+会话。arXiv 在热工程领域精准度不够，改用中文行业网站直接爬取更有效。
§
工作目录：E:\13_WorkSpaceForHermes\yang_HermesAgent。后续工作产生的文件（脚本、数据、输出等）优先保存在此目录，非必要不存C盘以节省空间。