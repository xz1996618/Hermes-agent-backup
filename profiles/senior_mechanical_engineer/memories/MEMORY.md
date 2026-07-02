DashScope视觉模型配置要点：auxiliary.vision.provider 必须设为 "alibaba"（不是 "dashscope"），模型用 qwen3-vl-plus（兼容API下最强）。qwen3-vl-235b-a22b-thinking 仅原生API可用，Hermes兼容模式用不了。生图模型（qwen-image-2.0-pro等）在DashScope"图像生成"标签页单独开启，不在"全模态"标签下。
§
未经用户明确同意，不得在用户电脑上安装任何系统级软件（如 LibreOffice、Chrome 等），包括通过 winget/choco/npm -g 等方式。项目本地依赖（npm install 在项目目录内）也需先征得同意。工具类安装（如 pip install 到已有 venv）相对可接受但仍应告知用户。
§
工作目录：E:\13_WorkSpaceForHermes\senior_mechanical_engineer_HermesAgent，所有工作产生的文件优先保存到此目录，避免占用C盘空间。
§
用户：板换选型开发。SW2024(E盘)COM残损。偏好：旋转优先、直接执行、停止即止、创建Skill先提案、建模前清场、文件锁定时换名。识图直连DashScope。技能层级：sw-auto→sw_bphe→bphe_design。知识库：E:\…\01_HeatExchanger\01_FeatureInformation。
§
SW VBA宏→插件执行工作流（已验证）：1) 必须完整.swb格式（注释头+模块级Dim+Sub main）原样写入ANSI+CRLF 2) 变量用 As Object，禁止强类型 3) 尺寸用 Parameter.SystemValue，GetDimension/SetSystemValue3/SetUserPreferenceToggle 会挂死 4) 插件 HTTP POST localhost:51920，返回 JSON{status,feature_tree} 5) SKILL:sw_auto_vba 存储NL↔VBA映射，持续更新
§
用户偏好：先给思路方案再执行（不要直接干）。盲写没验证过的API调用前先问用户要录制宏。SW建模走sw_auto_vba skill，从NL→VBA→插件执行→反馈修正闭环。不用C#翻译、不绕弯路。
§
sw_auto_vba 工作流：用户录VBA宏→我分析学习→更新skill映射表→下次用户只给自然语言需求→我查skill生成宏→发SwRunMacroPlugin(HTTP:51920)→SW内RunMacro执行→返特征树。所有尺寸修改我处理，用户不碰代码。
§
sw_auto_vba 工作流限定：只能走用户手动启动SW→插件自动加载→我发宏。由AI（Python COM）启动SW时插件不会自动加载，端口51920不通。不可用Python COM直连建模。
§
用户明确：后续除非用户指示试其他方法，建模只走sw_auto_vba（VBA宏→插件），不走Python COM、C# Interop等旁路。
§
SW 建模优先顺序：sw_auto_vba(VBA宏插件) > sw2014-auto(Python COM)。任何建模指令必须先检测插件:51920是否在线，在线就用 VBA 宏全程走插件，Python COM 仅作 fallback。2026-06-21 因跳过此项被用户纠正。
§
sw_auto_vba VBA 插件 CreateSpline 限制：单个坐标参数形式（CreateSpline x1,y1,z1,...）导致 SW 挂死；数组形式（CreateSpline Array(x1,y1,z1,...)）短测试通过但含 18 齿齿轮的全宏仍挂死。CreateLine/CreateArc/CreateCircle 可靠。插件挂死后 taskkill /f /im SLDWORKS.exe 杀进程 → 用户重启 SW → 工具→插件勾选 SwRunMacroPlugin 恢复。
§
齿轮（渐开线）待学习：用户后续会录制SolidWorks渐开线齿轮VBA宏供分析。到时解析宏中的CreateSpline/CreateArc/CreateLine参数、FeatureExtrusion2拉伸、FeatureCircularPattern阵列等API调用，存入sw_auto_vba skill映射表。参数：模数m、齿数z、压力角α=20°、齿宽B、渐开线极坐标方程 x=r_b(cos t+t sin t)。