
-----ɢ�����
-----���ز�������������Ϣ
INSERT INTO dbo.WCS_UPLOAD_TRACK (BARCODE, BS_NO, WEIGHT, LENGHT, WIDE, HIGH, CREATE_TIME, SPARE1, TYPE, SPARE2)
VALUES ('CX170628000003', 'P0004', 100, 100, 100, 100, '2011-10-10 10:10:10', '0', 4, NULL)
GO
-----��ѯ������Ϣ
SELECT SN, Type, BDNO, AcqTime, Length, Width, Height, Weight, Hubno, OpTime, Pn
FROM dbo.HH_GOODSINFO
WHERE SN = 'CX170628000003'
GO
-----��ʽ�ߵ�ַ����
INSERT INTO dbo.WCS_APPLY_DEST (BARCODE, BS_NO, BCR_NO, TO_POS, STATUS, CREATE_TIME, UPDATE_TIME, SPARE1, SPARE2)
VALUES ('CX170628000003', '1014', '10', 'pws003', 0, NULL, NULL, NULL, NULL)
GO

-----��ѯ������Ϣ��
SELECT SN, Type, BDNO, AcqTime, Length, Width, Height, Weight, Hubno, OpTime, Pn
FROM dbo.HH_GOODSINFO
WHERE SN = 'CX170628000003'
GO
-----��ѯ�����ʾʱ����
SELECT BussID, TaskNo, TaskType, SN, IsBlock, IsHengwen, Hubno, PlannningNo, BDNO, Pn, OUTNO, Height, Weight, PreBoxes, ErrorMessage, IsUnpackTray, IsBoxLable, IsMinpackLable, IsPipeline, SN_OLD, Priority, TaskState, State, OpTime, IsPallet
FROM dbo.HH_TASK
WHERE TaskType = 'CCIN' AND SN = 'CX170628000003'
GO

-----��ѯ�����������[��Ҫ���ͻ��ܲŻ�����������]
SELECT BussID, TaskNo, SubTaskNo, SN, TaskType, Hubno, OptParam, Priority, SeqNO, State, StartTime, EndTime, EndCode, EquipSysNo, SysCode, Falg
FROM dbo.HH_TASKPROCESS
WHERE TaskType = 'CCIN' 
GO
-----��ѯ����ʹ�����
SELECT ChannelorShelfNo, Type, Area, AvailableNum, UsedNum, RestNum, State
FROM dbo.HH_HUBNOSTATISTICS

-----��ѯ��ǰ����̨������
SELECT WorKNo, NodeCode, Type, TaskNum, TaskDeliveryedNum, Flag, OpTime, BZ
FROM dbo.HH_TASKPOOLMANAGE
WHERE WorKNo = 'pws005' AND Type = '21'
GO
-----��ѯ����̨��ϸ
SELECT WorKNo, NodeCode, SubTaskNo, TaskNo, TaskType, SN, OpTime
FROM dbo.HH_TASKINFO


