用户是板式换热器选型软件开发者，工作涵盖：选型软件和关联式拟合软件开发、原始实验数据处理和拟合数据整理、文献阅读。后续涉及板片结构设计和传热压降仿真。常用 Git 管理代码。默认工作目录 E:\13_WorkSpaceForHermes。对成本敏感，关注 token 消耗和模型性价比。创建 Skill 前必须先提案批准。视觉模型 qwen3-vl-plus（DashScope provider=alibaba），sk-ws 工作空间 Key 只能识图不能生图。微信已通过 iLink Bot 接入 gateway。SolidWorks solikworks-auto skill 已安装（SW2025 COM 接口拉伸参数不兼容）。所有 profile 共用 E:\13_WorkSpaceForHermes\01_bphe_knowledge_base 知识库。回答优先查知识库并注明文献来源。
§
对于从零搭建的新工具/项目，偏好先评估可行性再批准动工（先不要执行，评估可行性）。对于常规任务/已有框架内的工作，偏好直接执行先做再汇报。CLI 工具优先于图形界面，临时调用模式优先于常驻服务。
§
视觉模型：qwen3-vl-plus（DashScope provider=alibaba）。sk-ws 工作空间 Key 只能 VL 识图，不能生图（图像生成需在DashScope单独开通）。FAL_KEY 已配但余额耗尽。微信已通过 iLink Bot 接入 gateway，开机自启。
§
每日行业早报 cron：ID=58a6ea7fc938，no_agent 脚本模式，scripts/daily_briefing.py 抓取17个中文行业网站，10:00自动执行，deliver=all。SolidWorks solikworks-auto skill 已安装，SW2025 COM接口拉伸参数不兼容。
§
User prefers explanation-first approach: when proposing something new (like Kanban multi-profile setup), explain the workflow and options in detail first before actually building/running it. They'll say "搭" (build) when ready to proceed.
§
对量化交易有深入了解——能提出详细的策略评估报告（ATR动态阈值、大盘过滤、持仓再平衡等专业概念）。注重细节，会指出UI瑕疵（输入框太小）和逻辑bug（硬编码天数、固定收益率）。偏好自适应/动态参数方案优于固定阈值。
§
关注中文A股市场（中际旭创、英维克等），熟悉中国金融数据源（腾讯行情qt.gtimg.cn、新浪财经hq.sinajs.cn）。偏好从file://直接打开HTML文件工作，不接受依赖后端代理的方案。