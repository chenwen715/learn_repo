------������⣺
----���ز��-----
----��ַ��127.0.0.1:8880  TCP/IP
----������FCFC000100000000000000000000PX17062700000000000199998888FDFD
----��ѯ������Ϣ��
SELECT SN, Type, BDNO, AcqTime, Length, Width, Height, Weight, Hubno, OpTime, Pn
FROM dbo.HH_GOODSINFO
WHERE SN = 'PX170627000000000001'
GO
----�Ļ������ϴ�---

----���ﵽ��----
----��ַ��127.0.0.1:8880  TCP/IP
----������FCFC0002000000000088PX1706270000000000010088000000000000FDFD
----��ѯ������Ϣ���е�λ����Ϣ
SELECT SN, Type, BDNO, AcqTime, Length, Width, Height, Weight, Hubno, OpTime, Pn
FROM dbo.HH_GOODSINFO
WHERE SN = 'PX170627000000000001'
GO
----�����������Ϣ��
SELECT WorKNo, NodeCode, Type, TaskNum, TaskDeliveryedNum, Flag, OpTime, BZ
FROM dbo.HH_TASKPOOLMANAGE
WHERE WorKNo = '88'
GO

----------�ϼ�����ӿڵ�ַ��
----------http://127.0.0.1:8080/wms-api/api/wms/loadTasks
----------������
[{'taskNo':'113';'SN':'PX170627000000000001';'planningNo':'';'isHengwen':'0';'IsPallet': '-1';'BDNO':'23';'pn':'PN';'weight':'99';'height':'88';'SN_OLD':'0';'opTime':'2017-05-26 14:24:30'}]
SELECT BussID, TaskNo, TaskType, SN, IsBlock, IsHengwen, Hubno, PlannningNo, BDNO, Pn, OUTNO, Height, Weight, PreBoxes, ErrorMessage, IsUnpackTray, IsBoxLable, IsMinpackLable, IsPipeline, SN_OLD, Priority, TaskState, State, OpTime, IsPallet
FROM dbo.HH_TASK
WHERE TaskNo = '113'
GO
----------��ѯ����ֽ����
SELECT BussID, TaskNo, SubTaskNo, SN, TaskType, Hubno, OptParam, Priority, SeqNO, State, StartTime, EndTime, EndCode, EquipSysNo, SysCode, Falg
FROM dbo.HH_TASKPROCESS
WHERE TaskNo = '113'
GO
----------��ѯ������
SELECT ChannelId, BInCode, BOutCode, AInCode, AOutCode, SysCode, state, stateinfo
FROM dbo.HH_CHANNELLDMANAGE
------ľţȡ��----
------http://127.0.0.1:8080/wms-api/webservice/AGVAPI?wsdl
------���� PickupComplete
------������'00000000000000000000000000001892','88'
----�����������Ϣ���ѷ�����������һ
SELECT WorKNo, NodeCode, Type, TaskNum, TaskDeliveryedNum, Flag, OpTime, BZ
FROM dbo.HH_TASKPOOLMANAGE
WHERE WorKNo = '1002'
GO

------�����ϻ�----
------http://127.0.0.1:8080/wms-api/webservice/AGVAPI?wsdl
------���� RequestGoods
------������'00000000000000000000000000001892','88'
----�����������Ϣ��Ŀ��վ���ѷ���������С����������
SELECT WorKNo, NodeCode, Type, TaskNum, TaskDeliveryedNum, Flag, OpTime, BZ
FROM dbo.HH_TASKPOOLMANAGE
WHERE WorKNo = '1002'
GO


------�������----
------http://127.0.0.1:8080/wms-api/webservice/AGVAPI?wsdl
------���� DeliveryFinish
------������'00000000000000000000000000001892'
----�����������Ϣ��Ŀ��վ���ѷ�����������һ
SELECT WorKNo, NodeCode, Type, TaskNum, TaskDeliveryedNum, Flag, OpTime, BZ
FROM dbo.HH_TASKPOOLMANAGE
WHERE WorKNo = '1002'
GO
----------��ѯ�������״̬���
SELECT BussID, TaskNo, SubTaskNo, SN, TaskType, Hubno, OptParam, Priority, SeqNO, State, StartTime, EndTime, EndCode, EquipSysNo, SysCode, Falg
FROM dbo.HH_TASKPROCESS
WHERE SubTaskNo = '00000000000000000000000000001892'
GO




