Hermes storage paths on Windows: ~/AppData/Local/hermes/profiles/default/memories/ (memory files), ~/AppData/Local/hermes/skills/ (skill directories with SKILL.md), ~/AppData/Local/hermes/config.yaml (config). Full paths resolve under C:\Users\<user>\AppData\Local\hermes.
§
DashScope视觉模型配置要点：auxiliary.vision.provider 必须设为 "alibaba"（不是 "dashscope"），模型用 qwen3-vl-plus（兼容API下最强）。qwen3-vl-235b-a22b-thinking 仅原生API可用，Hermes兼容模式用不了。生图模型（qwen-image-2.0-pro等）在DashScope"图像生成"标签页单独开启，不在"全模态"标签下。
§
离线安装器：输出 ~/Desktop/HermesOfflineSetup_v0.16.0.exe，细节见 offline-hermes-installer-win skill。
§
未经用户明确同意，不得在用户电脑上安装任何系统级软件（如 LibreOffice、Chrome 等），包括通过 winget/choco/npm -g 等方式。项目本地依赖（npm install 在项目目录内）也需先征得同意。工具类安装（如 pip install 到已有 venv）相对可接受但仍应告知用户。
§
每日早报 cron：脚本 scripts/daily_briefing.py 抓取17个中文行业网站，no_agent=true 避免审批卡住，deliver=all 投递微信+会话。arXiv 在热工程领域精准度不够，改用中文行业网站直接爬取更有效。
§
工作目录：E:\13_WorkSpaceForHermes\data_analyst_wps_office_HermesAgent\。所有数据分析相关产出文件（脚本、报告、数据文件等）默认保存到此目录，非必要不写C盘以节省空间。
§
Hermes venv Python：C:\Users\Administrator\AppData\Local\hermes\hermes-agent\venv\Scripts\python.exe，含 numpy 2.3.5+scipy 1.17.1，数据分析脚本用此运行。
§
Windows路径注意：write_file 用 /e/... 会解析到 C:\e\...，E盘文件必须用 E:/... 格式。
§
台达B220板换：厚度<190mm下非对称不可行（冷DP翻2-3倍），对称设计最优。用户成本敏感：片数少+波高小=便宜。