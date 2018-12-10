------整托入库：
----称重测积-----
----地址：127.0.0.1:8880  TCP/IP
----参数：FCFC000100000000000000000000PX17062700000000000199998888FDFD
----查询货物信息表
SELECT SN, Type, BDNO, AcqTime, Length, Width, Height, Weight, Hubno, OpTime, Pn
FROM dbo.HH_GOODSINFO
WHERE SN = 'PX170627000000000001'
GO
----材积数据上传---

----货物到达----
----地址：127.0.0.1:8880  TCP/IP
----参数：FCFC0002000000000088PX1706270000000000010088000000000000FDFD
----查询货物信息表中的位置信息
SELECT SN, Type, BDNO, AcqTime, Length, Width, Height, Weight, Hubno, OpTime, Pn
FROM dbo.HH_GOODSINFO
WHERE SN = 'PX170627000000000001'
GO
----任务池任务信息；
SELECT WorKNo, NodeCode, Type, TaskNum, TaskDeliveryedNum, Flag, OpTime, BZ
FROM dbo.HH_TASKPOOLMANAGE
WHERE WorKNo = '88'
GO

----------上架任务接口地址：
----------http://127.0.0.1:8080/wms-api/api/wms/loadTasks
----------参数：
[{'taskNo':'113';'SN':'PX170627000000000001';'planningNo':'';'isHengwen':'0';'IsPallet': '-1';'BDNO':'23';'pn':'PN';'weight':'99';'height':'88';'SN_OLD':'0';'opTime':'2017-05-26 14:24:30'}]
SELECT BussID, TaskNo, TaskType, SN, IsBlock, IsHengwen, Hubno, PlannningNo, BDNO, Pn, OUTNO, Height, Weight, PreBoxes, ErrorMessage, IsUnpackTray, IsBoxLable, IsMinpackLable, IsPipeline, SN_OLD, Priority, TaskState, State, OpTime, IsPallet
FROM dbo.HH_TASK
WHERE TaskNo = '113'
GO
----------查询任务分解情况
SELECT BussID, TaskNo, SubTaskNo, SN, TaskType, Hubno, OptParam, Priority, SeqNO, State, StartTime, EndTime, EndCode, EquipSysNo, SysCode, Falg
FROM dbo.HH_TASKPROCESS
WHERE TaskNo = '113'
GO
----------查询巷道情况
SELECT ChannelId, BInCode, BOutCode, AInCode, AOutCode, SysCode, state, stateinfo
FROM dbo.HH_CHANNELLDMANAGE
------木牛取货----
------http://127.0.0.1:8080/wms-api/webservice/AGVAPI?wsdl
------方法 PickupComplete
------参数：'00000000000000000000000000001892','88'
----任务池任务信息；已分配任务数减一
SELECT WorKNo, NodeCode, Type, TaskNum, TaskDeliveryedNum, Flag, OpTime, BZ
FROM dbo.HH_TASKPOOLMANAGE
WHERE WorKNo = '1002'
GO

------请求上货----
------http://127.0.0.1:8080/wms-api/webservice/AGVAPI?wsdl
------方法 RequestGoods
------参数：'00000000000000000000000000001892','88'
----任务池任务信息；目的站点已分配任务数小于总任务数
SELECT WorKNo, NodeCode, Type, TaskNum, TaskDeliveryedNum, Flag, OpTime, BZ
FROM dbo.HH_TASKPOOLMANAGE
WHERE WorKNo = '1002'
GO


------配送完成----
------http://127.0.0.1:8080/wms-api/webservice/AGVAPI?wsdl
------方法 DeliveryFinish
------参数：'00000000000000000000000000001892'
----任务池任务信息；目的站点已分配任务数加一
SELECT WorKNo, NodeCode, Type, TaskNum, TaskDeliveryedNum, Flag, OpTime, BZ
FROM dbo.HH_TASKPOOLMANAGE
WHERE WorKNo = '1002'
GO
----------查询任务完成状态情况
SELECT BussID, TaskNo, SubTaskNo, SN, TaskType, Hubno, OptParam, Priority, SeqNO, State, StartTime, EndTime, EndCode, EquipSysNo, SysCode, Falg
FROM dbo.HH_TASKPROCESS
WHERE SubTaskNo = '00000000000000000000000000001892'
GO




