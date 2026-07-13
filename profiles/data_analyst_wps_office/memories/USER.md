用户是板式换热器选型软件开发者，长期使用 WPS Office 而非 Excel。对图表交付质量要求高：单位必须精确（kW/kPa）、图例必须百分数（非小数）、图表必须左对齐等间距。偏好原生图表对象（非图片），不接受硬编码小数。严格遵循 skill 流程，批评过"你开始为啥没用这个skill"。
§
创建 Skill 前必须先给用户提案（简述用途、触发条件、核心内容），等待用户批准后才能创建，严禁自作主张直接创建。
§
视觉模型：qwen3-vl-plus（DashScope provider=alibaba）。sk-ws 工作空间 Key 只能 VL 识图，不能生图（图像生成需在DashScope单独开通）。FAL_KEY 已配但余额耗尽。微信已通过 iLink Bot 接入 gateway，开机自启。
§
每日行业早报 cron：ID=58a6ea7fc938，no_agent 脚本模式，scripts/daily_briefing.py 抓取17个中文行业网站，10:00自动执行，deliver=all。SolidWorks solikworks-auto skill 已安装，SW2025 COM接口拉伸参数不兼容。
§
创建 Skill 前必须先提交提案（说明用途、内容要点、价值），等待用户明确批准后才能创建，不得自作主张。
§
板式换热器知识库路径：E:\13_WorkSpaceForHermes\01_bphe_knowledge_base\。回答板换问题时优先查阅该知识库。
§
用户偏好：生成偏差图时必须先用 skill_view() 加载 experimental-vs-simulation-deviation-plot skill，确认变体后再动手。跳过 skill 直接写脚本会被用户指出并要求重做。本会话中有过实际教训。
§
非系统安全类操作（文件读写、脚本运行、pip安装等）无需请示，直接执行。仅在涉及系统级软件安装（如 winget/choco/LibreOffice）或高危操作时需确认。
§
生成偏差图这类常规数据分析任务，直接执行即可，无需请示确认。仅涉及系统级软件安装或高危操作时才需确认。
§
图表生成直接在原文件路径（attachments 目录）修改保存，不复制到桌面。