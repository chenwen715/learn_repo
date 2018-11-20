using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNWMS.Common;

namespace XNWMS
{
    public class DoService
    {
        public static ResultModel Doserver(string s, string ServerName)
        {
            DAL_Comn_Log comnlog = new DAL_Comn_Log();
            DAL_Comn_Sql comnsql = new DAL_Comn_Sql();
            string Emessage = null;
            ResultModel resmod = new ResultModel();
            string table = "";
            string insertSql = "";
            try
            {
                if (ServerName == "SNInfo")
                {
                    List<SNInfo> SNInfolist = ChoseServer<SNInfo>(s);
                    table = ChoseTableName(ServerName);
                    for (int i = 0; i < SNInfolist.Count; i++)
                    {
                        insertSql += string.Format(@"INSERT INTO {0} (PalletOrBoxNo, BDNo, Length, Width, Height, Weight, CreateTime, Flag)
VALUES ('{1}', '{2}',{3}, {4}, {5}, {6}, getdate(),0)", table, SNInfolist[i].SN, SNInfolist[i].BDNO, SNInfolist[i].length,
                                                        SNInfolist[i].width, SNInfolist[i].height, SNInfolist[i].weight);
                    }
                }
                else if (ServerName == "deliveryTasksResult")
                {
                    List<deliveryTasksResult> deliveryTasksResultlist = ChoseServer<deliveryTasksResult>(s);
                    table = ChoseTableName(ServerName);
                    for (int i = 0; i < deliveryTasksResultlist.Count; i++)
                    {
                        insertSql += string.Format(@"INSERT INTO {0} (TaskNo, TaskType, PalletOrBoxNo, Station, CreateTime, Flag)
VALUES ('{1}', '{2}','{3}', '{4}', getdate(), 0)", table, deliveryTasksResultlist[i].taskNo, deliveryTasksResultlist[i].taskType,
                                                                    deliveryTasksResultlist[i].SN,deliveryTasksResultlist[i].hubno);
                        
                        insertSql += string.Format("UPDATE dbo.T_Task_UnLoad_Old SET State = 99 WHERE TaskNo =='{0}'", deliveryTasksResultlist[i].taskNo);
                    }
                }
                else if (ServerName == "loadTasksResult")
                {
                    List<loadTasksResult> loadTasksResultlist = ChoseServer<loadTasksResult>(s);
                    table = ChoseTableName(ServerName);
                    for (int i = 0; i < loadTasksResultlist.Count; i++)
                    {
                        if (loadTasksResultlist[i].SN.StartsWith("P"))
                        {                           
                            string sl = string.Format(@"select wcState from dbo.T_Biz_Container_Pallet where wcName='{0}'",loadTasksResultlist[i].hubno);
                            if (comnsql.GetData(Properties.Settings.Default.sql, sl).ToString() == "wcState_Empty")
                            {
                                insertSql += string.Format(@"INSERT INTO {0} (tNo, PalletOrBoxNo, ToWareCell, CreateTime, Flag)
VALUES ('{1}', '{2}','{3}',getdate(), 0)", table, loadTasksResultlist[i].taskNo, loadTasksResultlist[i].SN,
                                                      loadTasksResultlist[i].hubno);
                                insertSql += string.Format(@"INSERT INTO dbo.T_Biz_Container_PalletDtl (Warecell, PalletNo, BDNo, PNo, Number, IsOverWeight, IsHengWen, IsQualified, CreateTime,flag)
SELECT '{0}','{1}',BDNO, Pn, Number,case WHEN Weight>300 then 1 else 0 end ,IsHengwen,  IsBlock,getdate(),0
FROM dbo.T_Task_Load_Old WHERE TaskNo='{2}'", loadTasksResultlist[i].hubno, loadTasksResultlist[i].SN, loadTasksResultlist[i].taskNo);
                                insertSql += string.Format("update dbo.T_Biz_Container_Pallet set wcState='wcState_Store',StoreType='Container_Pallet',PalletNo='{1}', UpdateTime=getdate() where wcName='{0}'", loadTasksResultlist[i].hubno, loadTasksResultlist[i].SN);
                            }
                            else
                            {
                                throw new Exception("仓位已存在托盘");
                            }
                        }
                        else if (loadTasksResultlist[i].SN.StartsWith("C"))
                        {
                            insertSql += string.Format(@"INSERT INTO dbo.T_Biz_Container_BulksDtl (ShelfNo, BoxNo, BDNo, PNo, Number, Weight, IsQualified, CreateTime)
SELECT '{0}','{1}',BDNO, Pn, Number,case WHEN Weight>300 then 1 else 0 end ,IsHengwen, IsBlock,getdate()
FROM dbo.T_Task_Load_Old WHERE TaskNo='{2}'", loadTasksResultlist[i].hubno, loadTasksResultlist[i].SN, loadTasksResultlist[i].taskNo);
                            string sl = string.Format(@"select cState from dbo.T_Biz_Container_Bulks where cShelfDtl='{0}'", loadTasksResultlist[i].hubno);
                            if (comnsql.GetData(Properties.Settings.Default.sql, sl).ToString() == "wcState_Empty")
                            {
                                insertSql += string.Format("update dbo.T_Biz_Container_Bulks set cState='wcState_Store',cType='Container_Box' where cShelfDtl='{0}'", loadTasksResultlist[i].hubno);
                            }
                        }
                        insertSql += string.Format("UPDATE dbo.T_Task_Load_Old SET State = 99 WHERE TaskNo =='{0}'", loadTasksResultlist[i].taskNo);
                    }
                }
                else if (ServerName == "palletPackInfo")
                {
                    List<palletPackInfo> palletPackInfolist = ChoseServer<palletPackInfo>(s);
                    table = ChoseTableName(ServerName);
                    for (int i = 0; i < palletPackInfolist.Count; i++)
                    {
                        for (int j = 0; j < palletPackInfolist[i].SN_BoxList.Count; j++)
                        {
                            insertSql += string.Format(@"INSERT INTO {0} (PalletNo, BoxNo, CreateTime, Flag)
VALUES ('{1}', '{2}',getdate(), 0)", table, palletPackInfolist[i].SN_Pallet, palletPackInfolist[i].SN_BoxList[j]);
                        }
                            
                    }
                }
                else if (ServerName == "stationInfo")
                {
                    List<stationInfo> stationInfolist = ChoseServer<stationInfo>(s);
                    table = ChoseTableName(ServerName);
                    for (int i = 0; i < stationInfolist.Count; i++)
                    {
                        string sl = string.Format(@"SELECT * FROM dbo.T_Biz_Container_PalletDtl
WHERE PalletNo ='{0}' and flag=1", stationInfolist[i].SN);
                        if (comnsql.SelectGet(Properties.Settings.Default.sql, sl) != null)
                        {
                            insertSql += string.Format("DELETE FROM dbo.T_Biz_Container_PalletDtl WHERE PalletNo ='{0}'", stationInfolist[i].SN);
                            insertSql += string.Format("UPDATE dbo.T_Biz_Container_Pallet SET wcState = 'wcState_Empty',StoreType='',PalletNo='',UpdateTime=getdate() WHERE PalletNo='{0}'", stationInfolist[i].SN);
                        }
                        insertSql += string.Format(@"INSERT INTO {0} (tNo, PalletOrBoxNo, AreaNo, CreateTime, Flag)
VALUES ('{1}', '{2}','{3}',  getdate(), 0)", table, stationInfolist[i].taskNo,stationInfolist[i].SN,stationInfolist[i].hubno);
                    }
                }
                else
                {
                    throw new Exception("不知道是什么方法！");
                }
                int successNumber=comnsql.DataOperator(Properties.Settings.Default.sql, insertSql);
                LogWrite.WriteLogToMain(DateTime.Now.ToString() + " " + ServerName + "插入[" + table + "]数据成功！\n");
                comnlog.MessageLog("上传数据结果", ServerName + "插入[" + table + "]数据成功！\n");
                //返回成功的Json至wcf服务
                resmod = ResultModel.GteRemark(true, DateTime.Now, null);
            }
            catch (Exception e)
            {
                Emessage = DateTime.Now.ToString() + " " + ServerName + "插入表失败！" + e.Message + "\n";
                resmod = ResultModel.GteRemark(false, DateTime.Now, Emessage);
            }
            LogWrite.WriteResult(ServerName+"=="+JsonConvert.SerializeObject(resmod));
            comnlog.MessageLog("上传数据结果", ServerName + "==" + JsonConvert.SerializeObject(resmod) + "\n");
            return resmod;
        }

 
        public static List<T> ChoseServer<T>(string s)
        {
            List<T> list = JsonConvert.DeserializeObject<List<T>>(s);
            return list;          
        }

        #region
        //public static List<SNInfo> SNInfoService(string s)
        //{
        //    List<SNInfo> list = JsonConvert.DeserializeObject<List<SNInfo>>(s);
        //    return list;
        //}

        //public static List<deliveryTasksResult> deliveryTasksResultService(string s)
        //{
        //    List<deliveryTasksResult> list = JsonConvert.DeserializeObject<List<deliveryTasksResult>>(s);
        //    return list;
        //}

        //public static List<loadTasksResult> loadTasksResultService(string s)
        //{
        //    List<loadTasksResult> list = JsonConvert.DeserializeObject<List<loadTasksResult>>(s);
        //    return list;
        //}

        //public static List<palletPackInfo> palletPackInfoService(string s)
        //{
        //    List<palletPackInfo> list = JsonConvert.DeserializeObject<List<palletPackInfo>>(s);
        //    return list;
        //}

        //public static List<stationInfo> stationInfoService(string s)
        //{
        //    List<stationInfo> list = JsonConvert.DeserializeObject<List<stationInfo>>(s);
        //    return list;
        //}
        #endregion

        public static string ChoseTableName(string ServerName)
        {
            switch (ServerName)
            {
                case "SNInfo":
                    return "dbo.I_Relay_CargoInfo";
                case "deliveryTasksResult":
                    return "dbo.I_Relay_DeliveryResult";
                case "loadTasksResult":
                    return "dbo.I_Relay_LoadResult";
                case "palletPackInfo":
                    return "dbo.I_Relay_PalletPackInfo";
                case "stationInfo":
                    return "dbo.I_Relay_StationInfo";
                default:
                    throw new Exception("不知道是什么方法！");
            }
        }
    }
}
