DashScope视觉模型配置要点：auxiliary.vision.provider 必须设为 "alibaba"（不是 "dashscope"），模型用 qwen3-vl-plus。关键：sk-ws- 国内版 Key 必须设 base_url 为 https://dashscope.aliyuncs.com/compatible-mode/v1，否则默认国际端点 dashscope-intl.aliyuncs.com 会报 401。qwen3-vl-235b-a22b-thinking 仅原生API可用。生图模型需在DashScope单独开通图像生成权限。
§
未经用户明确同意，不得在用户电脑上安装任何系统级软件（如 LibreOffice、Chrome 等），包括通过 winget/choco/npm -g 等方式。项目本地依赖（npm install 在项目目录内）也需先征得同意。工具类安装（如 pip install 到已有 venv）相对可接受但仍应告知用户。
§
用户是板式换热器选型软件开发者，工作涵盖：选型软件和关联式拟合软件开发、实验数据处理、文献阅读、板片结构设计和传热压降仿真。使用 Git，默认工作目录 E:\13_WorkSpaceForHermes。偏好先做再汇报。有多个 Hermes profile：senior_mechanical_engineer、senior_simulation_engineer、data_analyst_wps_office 等。
§
板换知识库在 E:\13_WorkSpaceForHermes\01_bphe_knowledge_base\，回答专业问题优先查此库，找不到再搜外部。引用必须注明文献名和数据来源(知识库/手册/估算)。所有 profile 可通过绝对路径共用此库。skill 按需安装到对应 profile 下。
§
SolidWorks 2025 SP1（版本32.1），COM接口 FeatureExtrusion2 参数不兼容。solikworks-auto skill 已安装（注意拼写缺了字母d）。微信已通过 iLink Bot 接入 gateway，安装为 Windows 计划任务（开机自启）。FAL 生图余额已耗尽。DashScope sk-ws Key 只能 VL 识图（alibaba provider, qwen3-vl-plus），不能生图（需在DashScope单独开通图像生成权限）。
§
WeChat 平台禁止用 LaTeX $$/$ 公式（显示为乱码），用纯文本描述替代（如 Δp∝ρ·U²），通俗文档一律文字版。
§
用户要求：所有参考性数据必须标注实验来源（什么设备/板型/工况）。非板式换热器的结论/数据必须着重标记告知。不可把圆管/其他设备的实验数据直接当PHE数据推荐。
§
solidworks-automation-skill（⭐412，GitHub Python COM 自动化工具包）已安装到 senior_mechanical_engineer profile 的 skills/ 下，自检通过。依赖 pywin32 + comtypes 已安装。SolidWorks-Auto-Modeling-Agent 仓库为空壳，未安装。
§
GitHub加速：所有profile、所有会话中访问GitHub时，统一使用 E:\13_WorkSpaceForHermes\02_for some project\ghproxy-cli\bin\Release\net8.0\win-x64\ghproxy-cli.exe 程序。需先运行 ghproxy-cli.exe start 启动服务（端口16888），.env 中已配 HTTPS_PROXY=http://127.0.0.1:16888 + NO_PROXY 白名单。Google Scholar 等其他被墙站点需 SOCKS5 节点，暂未配置。
§
Hermes 更新后快捷方式会失效（新版本替换旧 exe），更新后需重新建桌面快捷方式。ghproxy-cli 开机自启脚本不受影响。
§
Hermes update 需连 pypi.org，在国内会失败导致桌面版被删。已在 .env 配清华镜像 PIP_INDEX_URL=https://pypi.tuna.tsinghua.edu.cn/simple。Windows 环境变量 NO_PROXY 已添加 pypi.org、files.pythonhosted.org。ghproxy-cli 开机自启脚本在启动文件夹。更新后快捷键会失效需重建。
§
小红书系列内容工作目录：E:\13_WorkSpaceForHermes\internet_influencer_HermesAgent\01_English\，所有素材/文案/图片都放此目录。PIL Pillow（msjh.ttc 中文）可生成教育信息图，马卡龙四色（绿/蓝/黄/紫）四宫格排版已验证可用。