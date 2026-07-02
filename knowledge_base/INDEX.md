# 板式换热器知识库 (BPHE Knowledge Base)

## 概况
- **总文件数**: 75篇
- **PDF**: 55+篇 | **Word**: 8篇 | **PPT**: 2篇 | **Markdown**: 6篇
- **关键刊物来源**: IJHMT, Applied Thermal Engineering, ScienceDirect, 国内期刊

---

## 一、板式换热器设计理论 (01_强化传热相关)

### 关联式汇总
| 文档 | 内容 | 关键参数 |
|------|------|--------|
| 01_Saturated boiling R-410A | 饱和沸腾传热+压降，垂直板换 | R-410A, 沸腾 |
| 02_Evaporation R-134a | 蒸发传热关联式 | R-134a, 蒸发 |
| 03_Survey correlations | **蒸发+冷凝关联式综述** (最全面) | 全制冷剂 |
| Condensation DTU | 热泵+ORC 冷凝关联式 | 冷凝 |
| Generalized correlations 2009 | **通用关联式**，任意几何板换 | f-Nu通用式 |
| Universal evaporation | 通用蒸发关联式 | 蒸发 |
| Inclination-friction 2020 | 波纹倾角-摩擦因子 | f-β关系 |
| Theoretical chevron 1996 | 人字波纹理论性能预测 | 理论模型 |
| 板壳式波纹倾角 | 中文期刊 | 波纹, 倾角 |
| 人字波纹倾角对性能影响 | 中文期刊, 2001 | 人字, 倾角 |

### 经典教材
| 书名 | 价值 |
|------|------|
| 王福军《CFD软件原理与应用》 | CFD实践指南 |
| 制冷与空气调节技术 (第5版) | 制冷基础 |
| 安德森《计算流体力学基础》 | CFD理论 |

---

## 二、换热系数拟合算法 (02)

| 方法 | 文献 |
|------|------|
| **Wilson Plot** | 通用综述 (2007), 综合新方法 |
| **遗传算法** | 电池包热管理流道优化 |
| **机器学习** | 强化学习, ML优化 (3本教材) |

---

## 三、BPHE 模型 (03)

| 文档 | 价值 |
|------|------|
| **BPHE Fundamental Models v1.0** (3392行) | ⭐ 核心模型文档 |
| New model generalized configurations | 通用流路+相变新模型 |
| ExplorerFastSolver (PPT) | 快速求解器说明 |
| D-R关联式拟合算法 (Word) | 关联式拟合流程 |

---

## 四、噪声与振动 (04)

| 文献 | 来源 |
|------|------|
| Acoustic resonance in PHE | IJHMT 1998 |
| Flow-Induced Noise in HX (28639行) | 专著级 |
| STHE pipeline noise (synergy) | Applied Thermal Eng 2017 |
| Noise-Vibration Case Study | IEEE |
| Single-to-noise PHE for EV | IJTS 2021 |

## 五、其他专题

| 分类 | 文献数 | 核心主题 |
|------|--------|--------|
| **05_工艺** | 1 | 不锈钢焊接冶金学 |
| **06_分配器压降** | **16** | 孔口压降、两相分配、分配器设计、红外分配量化、CFD数值模拟、R-1336mzz(Z)分配器、R1234ze(E)分配器 |
||| **新增文献** |
||| **01** [红外] A new method for estimating refrigerant distribution in plate evaporator using IR thermal imaging | IR图像→制冷剂分配估算新方法 (Int J Refrig 2021) |
||| **02** [实验+CFD] Experimental and simulation study on distributor design in plate evaporators | 分配器设计的实验+仿真 (Int J Refrig 2022, 上海交大) |
||| **03** [综述] Flow Maldistribution in PHE – Impact, Analysis, and Solutions | 流动不均匀性全面综述 |
||| **04** [R-1336mzz] Heat transfer and hydrodynamic characteristics of R-1336mzz(Z) in BPHE with distributor | R-1336mzz(Z)替代R-245fa+分配器性能 (ICHMT 2025) |
||| **05** [数值模型] Numerical model of two-phase refrigerant flow distribution in plate evaporator with distributors | 带分配器板式蒸发器两相分配数值模型 (ATE 2015) |
||| **06** [多通道CFD] Numerical study of two-phase flow in multi-channels PHE | 多通道PHE两相流CFD数值研究 (ICHMT 2022) |
||| **07** [综述] Two-phase flows downstream, upstream and within PHE | PHE上下游及内部两相流综述 (Int J Multiphase Flow 2025) |
||| **08** [综述] R1234ze(E)分配器孔径设计 | R1234ze(E)物性对分配器设计影响+孔径建议 (知识库新增) |
| **07_常识** | 2 | 制冷热泵核心知识、板换应用差异 |
| **08_传热压降** | 4 | R-1234ze(E)沸腾、干涸、两相流 |
| **09_角孔压降** | **3** | 流量分配不均匀性、角孔流速设计、进出口角孔设计探讨 |
| **10_设计与选型** | **1** | 角孔流速设计参考（8-10 m/s 来源考证） |

---

> 最后更新: 2026-07-02 | 工具: pdftotext + 批量提取
>
> **新增/更新**：
> - `06_分配器压降/分配器设计对板式换热器性能的影响_通俗总结.md` — 分配器工作原理、压降-分配平衡机制的通俗化整理
> - **数据补充**：S40 板型分配器实际参数 — 分配孔 d=1mm, 集管 D=14mm, β=0.071
> - **新增7篇文献**至 06_分配器压降（6→13篇）：红外分配量化法、分配器实验+CFD设计、流动不均匀性综述、R-1336mzz(Z)+分配器、两相分配数值模型、多通道CFD、PHE两相流综述
> - **新增** `10_设计与选型/角孔流速设计参考.md` — 角孔流速 8-10 m/s 经验值来源考证
> - **新增** `09_角孔压降/蒸发器进出口角孔大小设计探讨.md` — 蒸发器进出口角孔设计原理分析
> - **总文件数更新**: 69 → 75篇
