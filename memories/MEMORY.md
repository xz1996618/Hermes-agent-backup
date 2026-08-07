DashScope视觉模型配置要点：auxiliary.vision.provider 必须设为 "alibaba"（不是 "dashscope"），模型用 qwen3-vl-plus。关键：sk-ws- 国内版 Key 必须设 base_url 为 https://dashscope.aliyuncs.com/compatible-mode/v1，否则默认国际端点 dashscope-intl.aliyuncs.com 会报 401。qwen3-vl-235b-a22b-thinking 仅原生API可用。生图模型需在DashScope单独开通图像生成权限。
§
未经用户明确同意，不得在用户电脑上安装任何系统级软件（如 LibreOffice、Chrome 等），包括通过 winget/choco/npm -g 等方式。项目本地依赖（npm install 在项目目录内）也需先征得同意。工具类安装（如 pip install 到已有 venv）相对可接受但仍应告知用户。
§
用户是板式换热器选型软件开发者，工作涵盖：选型软件和关联式拟合软件开发、实验数据处理、文献阅读、板片结构设计和传热压降仿真。使用 Git，默认工作目录 E:\13_WorkSpaceForHermes。偏好先做再汇报。有多个 Hermes profile：senior_mechanical_engineer、senior_simulation_engineer、data_analyst_wps_office 等。
§
板换知识库在 E:\13_WorkSpaceForHermes\01_bphe_knowledge_base\，回答专业问题优先查此库，找不到再搜外部。引用必须注明文献名和数据来源(知识库/手册/估算)。所有 profile 可通过绝对路径共用此库。skill 按需安装到对应 profile 下。
§
用户偏好 DeepSeek 做主模型，不要在他不知情时自动切到千问等替代模型。遇到模型超时问题时，先问他要不要切再执行。
§
GitHub备份仓库 xz1996618/Hermes-agent-backup。恢复skill=hermes-restore。完整会话需手动复制整个 hermes 目录。
§
每周五9:00自动备份到GitHub（cron任务ID=b69be9b422f6，脚本=github_backup.py）。换电脑后先克隆仓库，安装hermes-restore skill，再说"恢复我的配置"。
§
中国网络：GitHub靠hosts+ghproxy(16888，只配git config proxy，严禁系统级HTTPS_PROXY，sslBackend=schannel)绕过DNS劫持(206.188.193.176)；Python包靠uv清华镜像；hermes update失败手动git fetch+merge+uv pip install -e .。
§
量化交易助手 quant-trading.html：实盘行情+历史回测+ATR动态阈值。组合：300308中际旭创、002837英维克、300383光环新网、600845宝信软件。数据用baostock(绕过ghproxy)。
§
板换专题文献在知识库根目录 板式换热器专题文献总结.md，含角孔压降/接管压降/板内分配/板间分配/低GWP冷媒五专题。说"更新专题文献"时重新扫描更新。
§
审批模式设为smart，不涉及系统安全的操作（Python脚本、数据分析等）自动批准。
§
用户对审批弹窗有顾虑：不确定批准框里的命令是在删除什么文件/干什么。以后需要审批删除操作时，先解释清楚路径和目的，让用户明白再请求批准。
§
智谱 GLM API Key 已配置，全免费。Provider=Z.AI（不是 zhipu），model=glm-4.7。域名 open.bigmodel.cn 已加 NO_PROXY。
§
每日早报 cron(58a6ea7fc938)：脚本 daily_briefing.py fetch()用 ProxyHandler({})绕过系统代理，deliver=all。iLink 限流等待冷却后重试。
§
PHE物性数据提取：严格按源PDF(TDS/SDS)填写，文档有什么填什么、没有就留空，禁止自行估算/插值/发挥（用户明确要求）。格式：温度K+密度/比热/热导率/粘度四表。注意单位陷阱(kcal→W、kJ→J、运动→动态粘度)，用表格值验证。
§
Tesseract 5.5.0已装(chi_sim+eng)。扫描PDF用pdf2txt.py本地OCR。
§
金融计算教训：金额/利率/日期必须先逐步验算再报数，禁止凭感觉手写数字（曾把月供错写成5573实为7764）。
§
工作库 E:\13_WorkSpaceForHermes\03_bphe_work_base\：02_产品手册(自家SANHUA) + 03_竞争对手板换(Alfa-laval/Kelvion/SWEP/宝得/高力/唯益)。用户问产品信息优先查此库。