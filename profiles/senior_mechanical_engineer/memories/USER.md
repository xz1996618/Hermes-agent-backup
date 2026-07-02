用户是板式换热器选型软件开发者，工作包括：选型软件和关联式拟合软件开发、原始实验数据处理和拟合数据整理、文献阅读。后续涉及板片结构设计和传热压降仿真。常用 Git 管理代码。默认工作目录 E:\13_WorkSpaceForHermes。对成本敏感，关注 token 消耗和模型性价比。创建 Skill 前必须先提案批准。
§
用户是板式换热器选型软件开发者，工作涵盖：选型软件和关联式拟合软件开发、实验数据处理和拟合数据整理、文献阅读、板片结构设计和传热压降仿真。使用 Git。默认工作目录 E:\13_WorkSpaceForHermes。偏好直接执行而非过多规划，先做再汇报。对 DeepSeek Chat 成本和速度敏感。创建 Skill 前须提案批准。
§
视觉模型：qwen3-vl-plus（DashScope provider=alibaba）。sk-ws 工作空间 Key 只能 VL 识图，不能生图（图像生成需在DashScope单独开通）。FAL_KEY 已配但余额耗尽。微信已通过 iLink Bot 接入 gateway，开机自启。
§
创建 Skill 前必须先向用户提交提案，说明要创建什么Skill、内容和理由，等用户批准后再执行。不得自作主张直接创建。
§
板式换热器知识库路径：E:\13_WorkSpaceForHermes\01_bphe_knowledge_base\。回答板换相关问题时优先检索此目录下的文档。
§
SW建模偏好：1)全程自动勿手动；2)圆锥首选旋转（FeatureRevolve2虚线轴），拒绝拔模替代；3)圆筒用薄壁拉伸（FeatureExtrusionThin）不用切除；4)切除永不勾选反向，贯穿需选背面切向实体；5)每次建模完 CloseAllDocuments+ExitApp，建模前 taskkill 旧进程；6)创建Skill前先提案。通过放示例文件教新功能。知识库：E:\13_WorkSpaceForHermes\01_bphe_knowledge_base\。
§
所有测试/调试生成的 SolidWorks 零件(.SLDPRT)文件，必须保存到 E:\13_WorkSpaceForHermes\senior_mechanical_engineer_HermesAgent\Test\ 目录下。正式产品文件才放在上层目录。
§
建模走 sw_auto_vba（用户录VBA宏→我调参→插件RunMacro）。sw_auto Python COM只做辅助，MCP桥接不用。变量用As Object，尺寸用Parameter.SystemValue，禁止GetDimension/SetSystemValue3和SetUserPreferenceToggle。
§
SwRunMacroPlugin已部署（DLL+regasm注册）。每次执行前检查:51920在线，不在线则提示用户重启SW+勾选插件。
§
sw_auto_vba skill存储NL↔VBA映射。工作流：用户录宏→我调参→POST插件(:51920)→SW内RunMacro→返特征树。禁止自行写未验证宏，必须从用户给的宏学习后生成。