USE [NCYouXinACS]
GO
/****** Object:  Table [dbo].[Agv]    Script Date: 2018/11/20 8:39:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Agv](
	[strAgvNo] [nvarchar](50) NULL,
	[agvType] [int] NULL,
	[strBarcode] [nvarchar](50) NULL,
	[agvState] [int] NULL,
	[currentCharge] [numeric](10, 2) NULL,
	[isEnable] [bit] NULL,
	[agvCarry] [bit] NULL,
	[agvHeart] [datetime] NULL,
	[agvChargeStation] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ConfigMsg]    Script Date: 2018/11/20 8:39:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConfigMsg](
	[itemID] [int] NULL,
	[itemName] [nvarchar](100) NULL,
	[configType] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FTask]    Script Date: 2018/11/20 8:39:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FTask](
	[strTaskNo] [nvarchar](50) NULL,
	[taskType] [int] NULL,
	[agvType] [int] NULL,
	[intTaskSplitStep] [int] NULL,
	[strBarcode] [nvarchar](50) NULL,
	[startStation] [nvarchar](50) NULL,
	[endStation] [nvarchar](50) NULL,
	[strAcceptTime] [datetime] NULL,
	[isTaskSubmit] [bit] NULL,
	[taskAgv] [nvarchar](50) NULL,
	[strBeginTime] [datetime] NULL CONSTRAINT [DF_FTask_strBeginTime]  DEFAULT (NULL),
	[strEndTime] [datetime] NULL CONSTRAINT [DF_FTask_strEndTime]  DEFAULT (NULL),
	[taskErrMsg] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Junction]    Script Date: 2018/11/20 8:39:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Junction](
	[intJuncCode] [int] NULL,
	[strJuncName] [nvarchar](50) NULL,
	[siteType] [int] NULL,
	[isJuncLocked] [bit] NULL,
	[isJuncLockedInfo] [nvarchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SonTask]    Script Date: 2018/11/20 8:39:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SonTask](
	[strSonTaskNo] [nvarchar](50) NULL,
	[strTaskNo] [nvarchar](50) NULL,
	[sonTaskType] [int] NULL,
	[intSonTaskStep] [int] NULL,
	[endSonTaskStation] [nvarchar](50) NULL,
	[sonTaskState] [int] NULL,
	[strSonTaskBeginTime] [datetime] NULL CONSTRAINT [DF_SonTask_strSonTaskBeginTime]  DEFAULT (NULL),
	[strSonTaskEndTime] [datetime] NULL CONSTRAINT [DF_SonTask_strSonTaskEndTime]  DEFAULT (NULL)
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Station]    Script Date: 2018/11/20 8:39:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Station](
	[intStationCode] [int] NULL,
	[strStatoinName] [nvarchar](50) NULL,
	[siteType] [int] NULL,
	[isRequestIn] [bit] NULL,
	[isAllowedOut] [bit] NULL,
	[isOutSuccess] [bit] NULL,
	[inState] [int] NULL,
	[outState] [int] NULL,
	[isStationErr] [bit] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_PathList_Tmp]    Script Date: 2018/11/20 8:39:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_PathList_Tmp](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Agv] [nvarchar](50) NULL,
	[Point] [nvarchar](50) NOT NULL,
	[X] [int] NOT NULL,
	[Y] [int] NOT NULL,
	[RouteNo] [int] NOT NULL,
	[Sid] [nvarchar](50) NOT NULL,
	[PointType] [int] NOT NULL,
	[StationNo] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_PathPoint]    Script Date: 2018/11/20 8:39:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_PathPoint](
	[PointNo] [int] IDENTITY(1,1) NOT NULL,
	[PointX] [int] NOT NULL,
	[PointY] [int] NOT NULL,
	[x] [int] NOT NULL,
	[y] [int] NOT NULL,
	[pointType] [int] NOT NULL,
	[intStationCode] [int] NULL,
	[wsType] [int] NULL,
	[beforex] [int] NULL,
	[beforey] [int] NULL,
	[afterx] [int] NULL,
	[aftery] [int] NULL,
	[isOccupy] [bit] NULL,
	[occupyAgv] [nvarchar](50) NULL,
	[area] [int] NULL
) ON [PRIMARY]

GO
INSERT [dbo].[Agv] ([strAgvNo], [agvType], [strBarcode], [agvState], [currentCharge], [isEnable], [agvCarry], [agvHeart], [agvChargeStation]) VALUES (N'A03_001', 2, N'(16,5)', 11, CAST(99.00 AS Numeric(10, 2)), 1, 0, CAST(N'2018-05-24 14:17:05.000' AS DateTime), 165)
INSERT [dbo].[Agv] ([strAgvNo], [agvType], [strBarcode], [agvState], [currentCharge], [isEnable], [agvCarry], [agvHeart], [agvChargeStation]) VALUES (N'A03_002', 2, N'(17,5)', 11, CAST(99.00 AS Numeric(10, 2)), 1, 0, CAST(N'2018-05-24 14:17:05.000' AS DateTime), 175)
INSERT [dbo].[Agv] ([strAgvNo], [agvType], [strBarcode], [agvState], [currentCharge], [isEnable], [agvCarry], [agvHeart], [agvChargeStation]) VALUES (N'A03_003', 2, N'(15,5)', 11, CAST(99.00 AS Numeric(10, 2)), 1, 0, CAST(N'2018-05-24 14:17:05.000' AS DateTime), 155)
INSERT [dbo].[Agv] ([strAgvNo], [agvType], [strBarcode], [agvState], [currentCharge], [isEnable], [agvCarry], [agvHeart], [agvChargeStation]) VALUES (N'A03_004', 2, N'(18,5)', 11, CAST(99.00 AS Numeric(10, 2)), 1, 0, CAST(N'2018-05-24 14:17:05.000' AS DateTime), 185)
INSERT [dbo].[Agv] ([strAgvNo], [agvType], [strBarcode], [agvState], [currentCharge], [isEnable], [agvCarry], [agvHeart], [agvChargeStation]) VALUES (N'A03_005', 2, N'(19,5)', 11, CAST(99.00 AS Numeric(10, 2)), 1, 0, CAST(N'2018-05-24 14:17:04.000' AS DateTime), 195)
INSERT [dbo].[Agv] ([strAgvNo], [agvType], [strBarcode], [agvState], [currentCharge], [isEnable], [agvCarry], [agvHeart], [agvChargeStation]) VALUES (N'A03_006', 2, N'(20,5)', 14, CAST(99.00 AS Numeric(10, 2)), 1, 0, CAST(N'2018-05-24 14:17:04.000' AS DateTime), 205)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (1, N'99', 4)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (2, N'90', 3)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (3, N'70', 2)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (100, N'155', 7)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (100, N'165', 7)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (100, N'175', 7)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (100, N'185', 7)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (100, N'195', 7)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (100, N'205', 7)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (1, N'任务不合理（回家初始位置不对）', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (2, N'任务不合理（动作类型无法识别）', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (3, N'任务不合理（目标工位不在地图中）', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (4, N'通讯校验位有误', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (5, N'启动点不合理', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (6, N'任务中无磁条', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (7, N'无', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (8, N'任务不合理（小车上料任务车上有物料）', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (9, N'任务不合理（小车下料任务车上无物料）', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (10, N'无', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (11, N'回家错误', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (12, N'DCS暂停时间过长', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (13, N'行走丢失多码', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (14, N'Tim等待时间过长', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (15, N'左电机报错', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (16, N'右电机报错', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (17, N'辊道电机报错', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (18, N'多电机报错', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (19, N'充电异常', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (20, N'任务中急停按钮被拍下', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (21, N'无', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (22, N'无', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (23, N'无', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (24, N'获取车号失败', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (25, N'无', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (26, N'无', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (27, N'无', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (28, N'无', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (29, N'无', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (30, N'无', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (31, N'AGV小车上料错误', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (32, N'AGV小车下料错误', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (33, N'无', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (34, N'无', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (35, N'无', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (36, N'无', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (37, N'无', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (38, N'无', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (39, N'无', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (40, N'无', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (41, N'无', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (42, N'无', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (43, N'无', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (44, N'无', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (45, N'小车脱离轨道', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (46, N'模块不在线', 1)
INSERT [dbo].[ConfigMsg] ([itemID], [itemName], [configType]) VALUES (47, N'Tim故障', 1)
INSERT [dbo].[FTask] ([strTaskNo], [taskType], [agvType], [intTaskSplitStep], [strBarcode], [startStation], [endStation], [strAcceptTime], [isTaskSubmit], [taskAgv], [strBeginTime], [strEndTime], [taskErrMsg]) VALUES (N'BA1081C3-9167-434F-9840-2704308775A9', 8, 2, 1, N'', N'0', N'205', CAST(N'2018-05-24 14:17:03.000' AS DateTime), 1, N'A03_006', NULL, NULL, 0)
INSERT [dbo].[Junction] ([intJuncCode], [strJuncName], [siteType], [isJuncLocked], [isJuncLockedInfo]) VALUES (1, N'交通路口1', 1, 0, N'')
INSERT [dbo].[Junction] ([intJuncCode], [strJuncName], [siteType], [isJuncLocked], [isJuncLockedInfo]) VALUES (2, N'交通路口2', 1, 0, N'')
INSERT [dbo].[Junction] ([intJuncCode], [strJuncName], [siteType], [isJuncLocked], [isJuncLockedInfo]) VALUES (3, N'交通路口3', 1, 0, N'')
INSERT [dbo].[Junction] ([intJuncCode], [strJuncName], [siteType], [isJuncLocked], [isJuncLockedInfo]) VALUES (4, N'交通路口4', 1, 0, N'')
INSERT [dbo].[Junction] ([intJuncCode], [strJuncName], [siteType], [isJuncLocked], [isJuncLockedInfo]) VALUES (5, N'交通路口5', 1, 0, N'')
INSERT [dbo].[Junction] ([intJuncCode], [strJuncName], [siteType], [isJuncLocked], [isJuncLockedInfo]) VALUES (6, N'交通路口6', 1, 0, N'')
INSERT [dbo].[Junction] ([intJuncCode], [strJuncName], [siteType], [isJuncLocked], [isJuncLockedInfo]) VALUES (7, N'交通路口7', 1, 0, N'')
INSERT [dbo].[Junction] ([intJuncCode], [strJuncName], [siteType], [isJuncLocked], [isJuncLockedInfo]) VALUES (8, N'交通路口8', 1, 0, N'')
INSERT [dbo].[Junction] ([intJuncCode], [strJuncName], [siteType], [isJuncLocked], [isJuncLockedInfo]) VALUES (9, N'交通路口9', 1, 0, N'')
INSERT [dbo].[Junction] ([intJuncCode], [strJuncName], [siteType], [isJuncLocked], [isJuncLockedInfo]) VALUES (10, N'交通路口10', 1, 0, N'')
INSERT [dbo].[Junction] ([intJuncCode], [strJuncName], [siteType], [isJuncLocked], [isJuncLockedInfo]) VALUES (11, N'交通路口11', 1, 0, N'')
INSERT [dbo].[Junction] ([intJuncCode], [strJuncName], [siteType], [isJuncLocked], [isJuncLockedInfo]) VALUES (12, N'交通路口12', 1, 0, N'')
INSERT [dbo].[Junction] ([intJuncCode], [strJuncName], [siteType], [isJuncLocked], [isJuncLockedInfo]) VALUES (13, N'交通路口13', 1, 0, N'')
INSERT [dbo].[SonTask] ([strSonTaskNo], [strTaskNo], [sonTaskType], [intSonTaskStep], [endSonTaskStation], [sonTaskState], [strSonTaskBeginTime], [strSonTaskEndTime]) VALUES (N'BA1081C3-9167-434F-9840-2704308775A9_1', N'BA1081C3-9167-434F-9840-2704308775A9', 4, 1, N'205', 1, NULL, NULL)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (1, N'D03_065', 2, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (2, N'D03_065', 2, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (3, N'D03_065', 2, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (4, N'D03_065', 2, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (5, N'D03_065', 2, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (6, N'D03_065', 2, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (7, N'D03_065', 2, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (8, N'D03_065', 2, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (9, N'D03_065', 2, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (10, N'D03_065', 2, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (11, N'D03_065', 2, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (12, N'D03_065', 2, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (13, N'D03_065', 2, 0, 0, 0, 0, 0, 1)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (14, N'D03_065', 2, 0, 0, 0, 0, 0, 1)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (15, N'D03_065', 2, 0, 0, 0, 0, 0, 1)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (16, N'D03_065', 2, 0, 0, 0, 0, 0, 1)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (17, N'D03_065', 2, 0, 0, 0, 0, 0, 1)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (18, N'D03_065', 2, 0, 0, 0, 0, 0, 1)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (19, N'D03_065', 2, 0, 0, 0, 0, 0, 1)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (20, N'D03_065', 2, 0, 0, 0, 0, 0, 1)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (21, N'D03_065', 2, 0, 0, 0, 0, 0, 1)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (22, N'D03_065', 2, 0, 0, 0, 0, 0, 1)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (23, N'D03_065', 2, 0, 0, 0, 0, 0, 1)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (24, N'D03_065', 2, 0, 0, 0, 0, 0, 1)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (25, N'D03_065', 2, 0, 0, 0, 0, 0, 1)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (26, N'D03_065', 2, 0, 0, 0, 0, 0, 1)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (27, N'D03_065', 2, 0, 0, 0, 0, 0, 1)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (28, N'D03_065', 2, 0, 0, 0, 0, 0, 1)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (29, N'D03_065', 2, 0, 0, 0, 0, 0, 1)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (30, N'D03_065', 2, 0, 0, 0, 0, 0, 1)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (51, N'D03_067', 3, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (45, N'WCS', 8, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (46, N'WCS', 7, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (205, N'Charge', 6, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (52, N'D03_067', 3, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (53, N'D03_067', 3, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (54, N'D03_067', 3, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (55, N'D03_067', 3, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (56, N'D03_067', 3, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (57, N'D03_067', 3, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (58, N'D03_067', 3, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (59, N'D03_067', 3, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (60, N'D03_067', 3, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (61, N'D03_067', 3, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (62, N'D03_067', 3, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (63, N'D03_067', 3, 0, 0, 0, 0, 0, 1)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (64, N'D03_067', 3, 0, 0, 0, 0, 0, 1)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (65, N'D03_067', 3, 0, 0, 0, 0, 0, 1)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (66, N'D03_067', 3, 0, 0, 0, 0, 0, 1)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (67, N'D03_067', 3, 0, 0, 0, 0, 0, 1)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (68, N'D03_067', 3, 0, 0, 0, 0, 0, 1)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (165, N'Charge', 6, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (175, N'Charge', 6, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (155, N'Charge', 6, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (185, N'Charge', 6, 0, 0, 0, 0, 0, 0)
INSERT [dbo].[Station] ([intStationCode], [strStatoinName], [siteType], [isRequestIn], [isAllowedOut], [isOutSuccess], [inState], [outState], [isStationErr]) VALUES (195, N'Charge', 6, 0, 0, 0, 0, 0, 0)
SET IDENTITY_INSERT [dbo].[T_PathList_Tmp] ON 

INSERT [dbo].[T_PathList_Tmp] ([ID], [Agv], [Point], [X], [Y], [RouteNo], [Sid], [PointType], [StationNo]) VALUES (14840, N'A03_006', N'205', 20, 5, 1, N'BA1081C3-9167-434F-9840-2704308775A9_1', 6, 205)
SET IDENTITY_INSERT [dbo].[T_PathList_Tmp] OFF
SET IDENTITY_INSERT [dbo].[T_PathPoint] ON 

INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (99, 71, 26, 1, 3, 7, 46, 2, 1, 2, 1, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (100, 71, 25, 1, 2, 4, NULL, 0, NULL, NULL, 1, 3, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (101, 71, 23, 1, 1, 4, NULL, 0, NULL, NULL, 1, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (102, 71, 21, 1, 14, 4, NULL, 0, NULL, NULL, 1, 1, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (103, 71, 19, 1, 13, 4, NULL, 0, NULL, NULL, 1, 14, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (104, 71, 17, 1, 12, 4, NULL, 0, NULL, NULL, 1, 13, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (105, 71, 15, 1, 11, 4, NULL, 0, NULL, NULL, 1, 12, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (106, 69, 22, 2, 1, 0, NULL, 0, 15, 5, 1, 1, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (107, 69, 20, 2, 14, 0, 0, 0, 16, 5, 1, 14, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (108, 69, 18, 2, 13, 0, 0, 0, 17, 5, 1, 13, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (109, 69, 16, 2, 12, 0, 0, 0, 18, 5, 1, 12, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (110, 69, 14, 2, 11, 0, 0, 0, 19, 5, 1, 11, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (111, 71, 28, 1, 4, 8, 45, 1, 1, 3, 3, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (112, 67, 24, 4, 1, 1, 1, 0, 4, 2, NULL, NULL, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (113, 68, 24, 3, 1, 1, 1, 0, 4, 2, 1, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (114, 67, 25, 4, 2, 4, NULL, 0, NULL, NULL, NULL, NULL, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (115, 67, 28, 3, 3, 5, 1, 0, 4, 4, 4, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (116, 66, 26, 5, 2, 5, 1, 0, 40, 2, 4, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (117, 67, 30, 4, 4, 1, 2, 0, 3, 4, 3, 3, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (118, 64, 32, 5, 4, 1, 2, 0, 3, 4, 6, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (119, 69, 32, 3, 4, 5, 2, 0, 1, 4, NULL, NULL, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (120, 62, 32, 6, 4, 5, 8, 0, 5, 4, NULL, NULL, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (121, 60, 32, 7, 4, 1, 8, 0, 6, 4, 8, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (122, 57, 26, 40, 2, 4, NULL, 0, NULL, NULL, 5, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (123, 58, 34, 8, 4, 0, NULL, 0, 7, 4, 9, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (124, 57, 34, 9, 4, 5, 3, 0, 8, 4, NULL, NULL, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (125, 56, 32, 4, 3, 1, 3, 0, 9, 4, 40, 2, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (126, 55, 34, 10, 4, 1, 3, 0, 9, 4, 11, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (127, 53, 34, 11, 4, 3, 62, 3, 10, 4, 12, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (128, 52, 34, 12, 4, 3, 61, 3, 11, 4, 13, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (129, 50, 34, 13, 4, 3, 60, 3, 12, 4, 14, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (130, 49, 34, 14, 4, 3, 59, 3, 13, 4, 15, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (131, 47, 34, 15, 4, 3, 58, 3, 14, 4, 16, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (132, 46, 34, 16, 4, 3, 57, 3, 15, 4, 17, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (133, 44, 34, 17, 4, 3, 56, 3, 16, 4, 18, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (134, 43, 34, 18, 4, 3, 55, 3, 17, 4, 38, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (135, 41, 34, 19, 4, 3, 54, 3, 38, 4, 39, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (136, 40, 34, 20, 4, 3, 53, 3, 19, 4, 21, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (137, 38, 34, 21, 4, 3, 52, 3, 20, 4, 22, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (138, 37, 34, 22, 4, 3, 51, 3, 21, 4, 40, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (139, 36, 34, 40, 4, 5, 5, 0, 22, 4, NULL, NULL, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (140, 33, 34, 41, 4, 1, 5, 0, 40, 4, 42, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (141, 31, 34, 42, 4, 5, 6, 0, 41, 4, NULL, NULL, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (142, 28, 34, 43, 4, 1, 6, 0, 42, 4, 23, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (143, 29, 32, 7, 3, 1, 6, 0, 42, 4, 43, 2, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (144, 25, 34, 23, 4, 2, 12, 2, 43, 4, 24, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (145, 23, 34, 24, 4, 2, 12, 1, 23, 4, 25, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (146, 21, 34, 25, 4, 2, 11, 2, 24, 4, 44, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (147, 20, 34, 44, 4, 5, 7, 0, 25, 4, NULL, NULL, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (148, 19, 34, 26, 4, 2, 11, 1, 44, 4, 27, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (149, 18, 32, 8, 3, 1, 7, 0, 26, 4, 44, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (150, 17, 34, 27, 4, 2, 10, 2, 26, 4, 45, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (151, 16, 34, 45, 4, 1, 7, 0, 27, 4, 28, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (152, 15, 34, 28, 4, 2, 10, 1, 45, 4, 29, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (153, 13, 34, 29, 4, 2, 9, 2, 28, 4, 30, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (154, 11, 34, 30, 4, 2, 9, 1, 29, 4, 31, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (155, 9, 34, 31, 4, 2, 8, 2, 30, 4, 32, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (156, 7, 34, 32, 4, 2, 8, 1, 31, 4, 33, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (157, 5, 34, 33, 4, 2, 7, 2, 32, 4, 34, 4, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (158, 3, 34, 34, 4, 2, 7, 1, 33, 4, 34, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (159, 4, 26, 34, 2, 2, 6, 2, 34, 4, 33, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (160, 6, 26, 33, 2, 2, 6, 1, 34, 2, 32, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (161, 8, 26, 32, 2, 2, 5, 2, 33, 2, 31, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (162, 10, 26, 31, 2, 2, 5, 1, 32, 2, 30, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (163, 12, 26, 30, 2, 2, 4, 2, 31, 2, 29, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (164, 14, 26, 29, 2, 2, 4, 1, 30, 2, 28, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (165, 16, 26, 28, 2, 2, 3, 2, 29, 2, 27, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (166, 18, 26, 27, 2, 2, 3, 1, 28, 2, 44, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (167, 19, 26, 44, 2, 4, NULL, 0, NULL, NULL, 26, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (168, 20, 26, 26, 2, 2, 2, 2, 44, 2, 25, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (169, 22, 26, 25, 2, 2, 2, 1, 26, 2, 24, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (170, 24, 26, 24, 2, 2, 1, 2, 25, 2, 23, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (171, 26, 26, 23, 2, 2, 1, 1, 24, 2, 43, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (172, 30, 26, 43, 2, 4, NULL, 0, NULL, NULL, 42, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (173, 35, 26, 42, 2, 4, NULL, 0, NULL, NULL, 22, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (174, 38, 26, 22, 2, 0, 0, 3, 42, 2, 21, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (175, 39, 26, 21, 2, 0, 0, 3, 22, 2, 20, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (176, 41, 26, 20, 2, 0, 0, 3, 21, 2, 41, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (177, 43, 26, 41, 2, 4, NULL, 0, NULL, NULL, 19, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (178, 42, 26, 19, 2, 0, 0, 3, 20, 2, 18, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (179, 44, 26, 18, 2, 0, 0, 3, 41, 2, 17, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (180, 45, 26, 17, 2, 0, 0, 3, 18, 2, 16, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (181, 47, 26, 16, 2, 0, 0, 3, 17, 2, 15, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (182, 48, 26, 15, 2, 0, 0, 3, 16, 2, 14, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (183, 50, 26, 14, 2, 0, 0, 3, 15, 2, 13, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (184, 51, 26, 13, 2, 0, 0, 3, 14, 2, 12, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (185, 53, 26, 12, 2, 0, 0, 3, 13, 2, 11, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (186, 54, 26, 11, 2, 0, 0, 3, 12, 2, 40, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (187, 42, 32, 5, 3, 1, 4, 0, 38, 4, 41, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (188, 34, 32, 6, 3, 1, 5, 0, 40, 4, 42, 2, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (189, 41, 34, 39, 4, 1, 4, 0, 19, 4, 20, 4, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (190, 43, 34, 38, 4, 5, 4, 0, 18, 4, NULL, NULL, 0, N'', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (191, 68, 22, 15, 5, 6, 155, 0, 4, 1, 2, 1, 1, N'A03_003', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (192, 68, 20, 16, 5, 6, 165, 0, 4, 1, 2, 14, 1, N'A03_001', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (193, 68, 18, 17, 5, 6, 175, 0, 4, 1, 2, 13, 1, N'A03_002', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (194, 68, 16, 18, 5, 6, 185, 0, 4, 1, 2, 12, 1, N'A03_004', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (195, 68, 14, 19, 5, 6, 195, 0, 4, 1, 2, 11, 1, N'A03_005', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (196, 68, 12, 20, 5, 6, 205, 0, 4, 1, 1, 11, 1, N'A03_006', 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (197, 61, 28, 30, 30, 1, 89, 0, 40, 40, NULL, NULL, 0, NULL, 1)
GO
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (198, 63, 34, 40, 40, 5, 9, 0, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (199, 61, 36, 20, 20, 1, 9, 0, 40, 40, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (200, 62, 24, 11, 3, 6, 113, 0, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (201, 62, 23, 12, 3, 6, 123, 0, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (202, 62, 22, 13, 3, 6, 133, 0, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (203, 62, 21, 14, 3, 6, 143, 0, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (204, 62, 20, 15, 3, 6, 153, 0, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (205, 62, 19, 16, 3, 6, 163, 0, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (206, 62, 18, 17, 3, 6, 173, 0, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (207, 62, 17, 18, 3, 6, 183, 0, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (208, 62, 16, 19, 3, 6, 193, 0, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (209, 62, 15, 20, 3, 6, 203, 0, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (210, 62, 14, 21, 3, 6, 213, 0, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (211, 62, 12, 22, 3, 6, 223, 0, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (212, 64, 9, 23, 3, 0, NULL, 0, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (213, 58, 44, 11, 7, 9, NULL, 1, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (214, 57, 44, 12, 7, 9, NULL, 2, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (215, 56, 44, 45, 2, 4, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (216, 53, 44, 13, 7, 9, NULL, 1, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (217, 52, 44, 14, 7, 9, NULL, 2, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (218, 51, 44, 46, 2, 4, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (220, 48, 44, 15, 7, 9, NULL, 1, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (221, 47, 44, 16, 7, 9, NULL, 2, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (222, 46, 44, 47, 2, 4, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (223, 43, 44, 17, 7, 9, NULL, 1, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (224, 42, 44, 18, 7, 9, NULL, 2, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (225, 41, 44, 48, 2, 4, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (226, 38, 44, 19, 7, 9, NULL, 1, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (227, 37, 44, 20, 7, 9, NULL, 2, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (228, 36, 44, 49, 2, 4, 0, 0, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (230, 33, 44, 21, 7, 9, NULL, 1, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (231, 32, 44, 22, 7, 9, NULL, 2, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (233, 28, 44, 23, 7, 9, NULL, 1, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (234, 27, 44, 24, 7, 9, NULL, 2, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (235, 23, 44, 25, 7, 9, NULL, 1, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (236, 22, 44, 26, 7, 9, NULL, 2, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (237, 18, 44, 27, 7, 9, NULL, 1, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (238, 17, 44, 28, 7, 9, NULL, 2, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (239, 13, 44, 29, 7, 9, NULL, 1, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (240, 12, 44, 30, 7, 9, NULL, 2, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (241, 8, 44, 31, 7, 9, NULL, 1, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (242, 7, 44, 32, 7, 9, NULL, 2, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (243, 3, 44, 33, 7, 9, NULL, 1, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (244, 2, 44, 34, 7, 9, NULL, 2, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (245, 58, 50, 41, 7, 9, NULL, 1, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (246, 2, 50, 63, 7, 9, NULL, 2, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (247, 58, 11, 71, 7, 9, NULL, 1, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (248, 2, 11, 93, 7, 9, NULL, 2, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (249, 58, 5, 101, 7, 9, NULL, 1, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (250, 2, 5, 123, 7, 9, NULL, 2, NULL, NULL, NULL, NULL, 0, NULL, 1)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (251, 90, 33, 90, 33, 7, 49, 2, NULL, NULL, NULL, NULL, 0, NULL, 1)
SET IDENTITY_INSERT [dbo].[T_PathPoint] OFF
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'小车号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agv', @level2type=N'COLUMN',@level2name=N'strAgvNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'小车类型 1成品配送 2导线桶配送' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agv', @level2type=N'COLUMN',@level2name=N'agvType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'小车码值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agv', @level2type=N'COLUMN',@level2name=N'strBarcode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'小车状态 10：自动、无码（报警）；11:自动、空闲；12:自动、歪码（报警、占位）13：忙碌；14:充电中；20:手动、无码；21:手动、有码（占位）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agv', @level2type=N'COLUMN',@level2name=N'agvState'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'小车电量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agv', @level2type=N'COLUMN',@level2name=N'currentCharge'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'小车是否可用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agv', @level2type=N'COLUMN',@level2name=N'isEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否载货' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agv', @level2type=N'COLUMN',@level2name=N'agvCarry'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最近一次上报心跳的时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agv', @level2type=N'COLUMN',@level2name=N'agvHeart'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'小车匹配的充电桩' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agv', @level2type=N'COLUMN',@level2name=N'agvChargeStation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配置信息序号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ConfigMsg', @level2type=N'COLUMN',@level2name=N'itemID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配置信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ConfigMsg', @level2type=N'COLUMN',@level2name=N'itemName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配置类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ConfigMsg', @level2type=N'COLUMN',@level2name=N'configType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTask', @level2type=N'COLUMN',@level2name=N'strTaskNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务类型 1导线桶配送  2导线桶回收 3半成品配送 4空周转箱回收 5成品入库 6零箱入库 7充电 8取消充电 9 回家' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTask', @level2type=N'COLUMN',@level2name=N'taskType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'小车类型 1成品配送 2导线桶配送' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTask', @level2type=N'COLUMN',@level2name=N'agvType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务开始分解序号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTask', @level2type=N'COLUMN',@level2name=N'intTaskSplitStep'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'导线桶条码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTask', @level2type=N'COLUMN',@level2name=N'strBarcode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务的起点站台' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTask', @level2type=N'COLUMN',@level2name=N'startStation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务的终点站台' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTask', @level2type=N'COLUMN',@level2name=N'endStation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务的接收时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTask', @level2type=N'COLUMN',@level2name=N'strAcceptTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务是否提交' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTask', @level2type=N'COLUMN',@level2name=N'isTaskSubmit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务执行小车' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTask', @level2type=N'COLUMN',@level2name=N'taskAgv'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务的开始时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTask', @level2type=N'COLUMN',@level2name=N'strBeginTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务的结束时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTask', @level2type=N'COLUMN',@level2name=N'strEndTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务异常信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTask', @level2type=N'COLUMN',@level2name=N'taskErrMsg'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'汇流站点序号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Junction', @level2type=N'COLUMN',@level2name=N'intJuncCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'汇流站点名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Junction', @level2type=N'COLUMN',@level2name=N'strJuncName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'汇流站点类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Junction', @level2type=N'COLUMN',@level2name=N'siteType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'汇流站点是否锁定' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Junction', @level2type=N'COLUMN',@level2name=N'isJuncLocked'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'汇流站点锁定小车' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Junction', @level2type=N'COLUMN',@level2name=N'isJuncLockedInfo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SonTask', @level2type=N'COLUMN',@level2name=N'strSonTaskNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务类型  1小车下料 2小车上料 3充电 4取消充电 5小车下料故障恢复 6小车上料故障恢复 7回家' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SonTask', @level2type=N'COLUMN',@level2name=N'sonTaskType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务序号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SonTask', @level2type=N'COLUMN',@level2name=N'intSonTaskStep'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务的终点站台' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SonTask', @level2type=N'COLUMN',@level2name=N'endSonTaskStation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务状态 1初始化  2待发送  3已发送  4已完成' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SonTask', @level2type=N'COLUMN',@level2name=N'sonTaskState'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务的开始时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SonTask', @level2type=N'COLUMN',@level2name=N'strSonTaskBeginTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务的结束时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SonTask', @level2type=N'COLUMN',@level2name=N'strSonTaskEndTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'站台站点序号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Station', @level2type=N'COLUMN',@level2name=N'intStationCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'站台控制器名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Station', @level2type=N'COLUMN',@level2name=N'strStatoinName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'站台站点类型 1汇流口  2导线桶  3成品 4半成品 5WCS' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Station', @level2type=N'COLUMN',@level2name=N'siteType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否请求上料(DCS)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Station', @level2type=N'COLUMN',@level2name=N'isRequestIn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否允许下料(DCS)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Station', @level2type=N'COLUMN',@level2name=N'isAllowedOut'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否下料成功(DCS)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Station', @level2type=N'COLUMN',@level2name=N'isOutSuccess'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上料区状态 1上料运转中  2上料已完成' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Station', @level2type=N'COLUMN',@level2name=N'inState'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'下料区状态 3下料请求' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Station', @level2type=N'COLUMN',@level2name=N'outState'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'下料区是否有货(DCS)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Station', @level2type=N'COLUMN',@level2name=N'isStationErr'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'点序号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_PathPoint', @level2type=N'COLUMN',@level2name=N'PointNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地图上x点坐标' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_PathPoint', @level2type=N'COLUMN',@level2name=N'PointX'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地图上y坐标' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_PathPoint', @level2type=N'COLUMN',@level2name=N'PointY'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CAD图上x坐标' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_PathPoint', @level2type=N'COLUMN',@level2name=N'x'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CAD图上y坐标' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_PathPoint', @level2type=N'COLUMN',@level2name=N'y'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CAD图上该点的性质：1、汇流口；2、导线桶（单）料台；3、导线桶（双）料台；4、路口点；5、释放路口点；6、充电点；7、WCS输送线下料口；8、WCS输送线上料口' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_PathPoint', @level2type=N'COLUMN',@level2name=N'pointType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CAD图上该点的站点序号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_PathPoint', @level2type=N'COLUMN',@level2name=N'intStationCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CAD图上该料台是上料台还是下料台：1、上料台；2、下料台；0、非料台' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_PathPoint', @level2type=N'COLUMN',@level2name=N'wsType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CAD图上该点的前一点x坐标' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_PathPoint', @level2type=N'COLUMN',@level2name=N'beforex'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CAD图上该点的前一点y坐标' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_PathPoint', @level2type=N'COLUMN',@level2name=N'beforey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CAD图上该点的后一点x坐标' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_PathPoint', @level2type=N'COLUMN',@level2name=N'afterx'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CAD图上该点的后一点y坐标' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_PathPoint', @level2type=N'COLUMN',@level2name=N'aftery'
GO
