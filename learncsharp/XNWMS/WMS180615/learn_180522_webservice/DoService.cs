using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learn_180522_webservice
{
    public class DoService
    {
        public static ResultModel Doserver(string s, string ServerName)
        {
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
                    }
                }
                else if (ServerName == "loadTasksResult")
                {
                    List<loadTasksResult> loadTasksResultlist = ChoseServer<loadTasksResult>(s);
                    table = ChoseTableName(ServerName);
                    for (int i = 0; i < loadTasksResultlist.Count; i++)
                    {
                        insertSql += string.Format(@"INSERT INTO {0} (tNo, PalletOrBoxNo, ToWareCell, CreateTime, Flag)
VALUES ('{1}', '{2}','{3}',getdate(), 0)", table, loadTasksResultlist[i].taskNo, loadTasksResultlist[i].SN,
                                                      loadTasksResultlist[i].hubno);
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
                        insertSql += string.Format(@"INSERT INTO {0} (tNo, PalletOrBoxNo, AreaNo, CreateTime, Flag)
VALUES ('{1}', '{2}','{3}',  getdate(), 0)", table, stationInfolist[i].taskNo,stationInfolist[i].SN,stationInfolist[i].hubno);
                    }
                }
                else
                {
                    throw new Exception("不知道是什么方法！");
                }
                int successNumber=comnsql.DataOperator(Properties.Settings.Default.sql, insertSql);
                LogWrite.WriteLogToMain(DateTime.Now.ToString() + " " + ServerName + "插入[" + table + "]"+successNumber+"条数据成功！\n");
                //返回成功的Json至wcf服务
                resmod = ResultModel.GteRemark(true, DateTime.Now, null);
            }
            catch (Exception e)
            {
                Emessage = DateTime.Now.ToString() + " " + ServerName + "插入表失败！" + e.Message + "\n";
                resmod = ResultModel.GteRemark(false, DateTime.Now, Emessage);
            }
            LogWrite.WriteResult(ServerName+"=="+JsonConvert.SerializeObject(resmod));
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
