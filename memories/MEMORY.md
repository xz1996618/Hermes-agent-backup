DashScope视觉模型配置要点：auxiliary.vision.provider 必须设为 "alibaba"（不是 "dashscope"），模型用 qwen3-vl-plus。关键：sk-ws- 国内版 Key 必须设 base_url 为 https://dashscope.aliyuncs.com/compatible-mode/v1，否则默认国际端点 dashscope-intl.aliyuncs.com 会报 401。qwen3-vl-235b-a22b-thinking 仅原生API可用。生图模型需在DashScope单独开通图像生成权限。
§
未经用户明确同意，不得在用户电脑上安装任何系统级软件（如 LibreOffice、Chrome 等），包括通过 winget/choco/npm -g 等方式。项目本地依赖（npm install 在项目目录内）也需先征得同意。工具类安装（如 pip install 到已有 venv）相对可接受但仍应告知用户。
§
用户是板式换热器选型软件开发者，工作涵盖：选型软件和关联式拟合软件开发、实验数据处理、文献阅读、板片结构设计和传热压降仿真。使用 Git，默认工作目录 E:\13_WorkSpaceForHermes。偏好先做再汇报。有多个 Hermes profile：senior_mechanical_engineer、senior_simulation_engineer、data_analyst_wps_office 等。
§
板换知识库在 E:\13_WorkSpaceForHermes\01_bphe_knowledge_base\，回答专业问题优先查此库，找不到再搜外部。引用必须注明文献名和数据来源(知识库/手册/估算)。所有 profile 可通过绝对路径共用此库。skill 按需安装到对应 profile 下。
§
微信已通过 iLink Bot 接入 gateway，安装为 Windows 计划任务（开机自启）。WeChat 禁止用 LaTeX $$/$ 公式（显示乱码），用纯文本描述替代（如 Δp∝ρ·U²）。Solikworks-auto skill 已安装（注意拼写缺了d）。FAL 生图余额已耗尽。DashScope sk-ws Key 只能 VL 识图（alibaba, qwen3-vl-plus），不能生图。
§
用户要求：所有参考性数据必须标注实验来源（什么设备/板型/工况）。非板式换热器的结论/数据必须着重标记告知。不可把圆管/其他设备的实验数据直接当PHE数据推荐。
§
GitHub备份仓库 xz1996618/Hermes-agent-backup。恢复skill=hermes-restore。完整会话需手动复制整个 hermes 目录。
§
每周五9:00自动备份到GitHub（cron任务ID=b69be9b422f6，脚本=github_backup.py）。换电脑后先克隆仓库，安装hermes-restore skill，再说"恢复我的配置"。
§
每日行业资讯早报 cron 任务（ID=58a6ea7fc938，脚本=daily_briefing.py）必须投递到微信（deliver: all）。iLink 有限流时等待冷却后再重试。用户不接受只发回当前会话的折中方案。
§
ghproxy-cli 端口16888，开机自启，HTTPS_PROXY+NO_PROXY 双环境体系。用户有股票分析需求（仅数据分析，不给买卖建议）。
§
量化交易助手 E:\13_WorkSpaceForHermes\quant-trading\quant-trading.html：实盘行情(腾讯/新浪JSONP)+历史回测(自定义资产+日期范围+手动导入+手续费计算)+ATR动态阈值策略。最终组合：300308中际旭创、002837英维克、300383光环新网、600845宝信软件。数据下载优先baostock(股票)，需绕过ghproxy(session.trust_env=False)。初始本金¥10000，基准线为持有不动。
§
用户对审批弹窗有顾虑：不确定批准框里的命令是在删除什么文件/干什么。以后需要审批删除操作时，先解释清楚路径和目的，让用户明白再请求批准。