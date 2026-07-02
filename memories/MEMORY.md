DashScope视觉模型配置要点：auxiliary.vision.provider 必须设为 "alibaba"（不是 "dashscope"），模型用 qwen3-vl-plus。关键：sk-ws- 国内版 Key 必须设 base_url 为 https://dashscope.aliyuncs.com/compatible-mode/v1，否则默认国际端点 dashscope-intl.aliyuncs.com 会报 401。qwen3-vl-235b-a22b-thinking 仅原生API可用。生图模型需在DashScope单独开通图像生成权限。
§
未经用户明确同意，不得在用户电脑上安装任何系统级软件（如 LibreOffice、Chrome 等），包括通过 winget/choco/npm -g 等方式。项目本地依赖（npm install 在项目目录内）也需先征得同意。工具类安装（如 pip install 到已有 venv）相对可接受但仍应告知用户。
§
用户是板式换热器选型软件开发者，工作涵盖：选型软件和关联式拟合软件开发、实验数据处理、文献阅读、板片结构设计和传热压降仿真。使用 Git，默认工作目录 E:\13_WorkSpaceForHermes。偏好先做再汇报。有多个 Hermes profile：senior_mechanical_engineer、senior_simulation_engineer、data_analyst_wps_office 等。
§
板换知识库在 E:\13_WorkSpaceForHermes\01_bphe_knowledge_base\，回答专业问题优先查此库，找不到再搜外部。引用必须注明文献名和数据来源(知识库/手册/估算)。所有 profile 可通过绝对路径共用此库。skill 按需安装到对应 profile 下。
§
SolidWorks 2025 SP1（版本32.1），COM接口 FeatureExtrusion2 参数不兼容。solikworks-auto skill 已装到 senior_mechanical_engineer profile。solidworks-automation-skill（⭐412）也已装，自检通过，依赖 pywin32+comtypes 已装。Auto-Modeling-Agent 仓库为空壳。
§
微信已通过 iLink Bot 接入 gateway，安装为 Windows 计划任务（开机自启）。WeChat 禁止用 LaTeX $$/$ 公式（显示乱码），用纯文本描述替代（如 Δp∝ρ·U²）。Solikworks-auto skill 已安装（注意拼写缺了d）。FAL 生图余额已耗尽。DashScope sk-ws Key 只能 VL 识图（alibaba, qwen3-vl-plus），不能生图。
§
用户要求：所有参考性数据必须标注实验来源（什么设备/板型/工况）。非板式换热器的结论/数据必须着重标记告知。不可把圆管/其他设备的实验数据直接当PHE数据推荐。
§
GitHub加速：ghproxy-cli(端口16888)开机自启(.bat已加固：independent window + port-ready wait)。ProxyServer.cs已修复：3s超时+IP fallback(仅140.82段)+禁DNS(被劫持)+新建TcpClient每次重试。桌面版update check走REST API->git fetch 10s超时，3s×4IP=12s略超边界，网络波动时可能再失败需重试。HTTPS_PROXY+NO_PROXY双环境体系(.env+setx)。参考hermes-config-china skill的ghproxy-git-debugging.md。
§
GitHub 加速用 ghproxy-cli（E:\13_WorkSpaceForHermes\02 for some project\ghproxy-cli\），系统环境变量 HTTPS_PROXY + NO_PROXY（含 weixin.qq.com、pypi.org 白名单）需与 .env 同步。PyPI 清华镜像 PIP_INDEX_URL 已配。Hermes 更新后快捷方式会失效需重建。系统环境变量 NO_PORXY 可能丢失需重新 setx。
§
GitHub备份仓库 xz1996618/Hermes-agent-backup。恢复skill=hermes-restore。完整会话需手动复制整个 hermes 目录。