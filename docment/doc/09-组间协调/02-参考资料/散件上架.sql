
-----散件入库
-----称重测积、插入货物信息
INSERT INTO dbo.WCS_UPLOAD_TRACK (BARCODE, BS_NO, WEIGHT, LENGHT, WIDE, HIGH, CREATE_TIME, SPARE1, TYPE, SPARE2)
VALUES ('CX170628000003', 'P0004', 100, 100, 100, 100, '2011-10-10 10:10:10', '0', 4, NULL)
GO
-----查询货物信息
SELECT SN, Type, BDNO, AcqTime, Length, Width, Height, Weight, Hubno, OpTime, Pn
FROM dbo.HH_GOODSINFO
WHERE SN = 'CX170628000003'
GO
-----箱式线地址申请
INSERT INTO dbo.WCS_APPLY_DEST (BARCODE, BS_NO, BCR_NO, TO_POS, STATUS, CREATE_TIME, UPDATE_TIME, SPARE1, SPARE2)
VALUES ('CX170628000003', '1014', '10', 'pws003', 0, NULL, NULL, NULL, NULL)
GO

-----查询货物信息表
SELECT SN, Type, BDNO, AcqTime, Length, Width, Height, Weight, Hubno, OpTime, Pn
FROM dbo.HH_GOODSINFO
WHERE SN = 'CX170628000003'
GO
-----查询任务表示时数据
SELECT BussID, TaskNo, TaskType, SN, IsBlock, IsHengwen, Hubno, PlannningNo, BDNO, Pn, OUTNO, Height, Weight, PreBoxes, ErrorMessage, IsUnpackTray, IsBoxLable, IsMinpackLable, IsPipeline, SN_OLD, Priority, TaskState, State, OpTime, IsPallet
FROM dbo.HH_TASK
WHERE TaskType = 'CCIN' AND SN = 'CX170628000003'
GO

-----查询待处理任务表[需要配送货架才会有配送任务]
SELECT BussID, TaskNo, SubTaskNo, SN, TaskType, Hubno, OptParam, Priority, SeqNO, State, StartTime, EndTime, EndCode, EquipSysNo, SysCode, Falg
FROM dbo.HH_TASKPROCESS
WHERE TaskType = 'CCIN' 
GO
-----查询货架使用情况
SELECT ChannelorShelfNo, Type, Area, AvailableNum, UsedNum, RestNum, State
FROM dbo.HH_HUBNOSTATISTICS

-----查询当前工作台任务数
SELECT WorKNo, NodeCode, Type, TaskNum, TaskDeliveryedNum, Flag, OpTime, BZ
FROM dbo.HH_TASKPOOLMANAGE
WHERE WorKNo = 'pws005' AND Type = '21'
GO
-----查询工作台明细
SELECT WorKNo, NodeCode, SubTaskNo, TaskNo, TaskType, SN, OpTime
FROM dbo.HH_TASKINFO


