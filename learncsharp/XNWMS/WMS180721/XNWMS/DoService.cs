﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNWMS.Common;
using XNWMS.InfoModel_S;

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
            string date = DateTime.Now.Year.ToString().PadLeft(4, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');
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
                        insertSql += string.Format(@"UPDATE dbo.T_Biz_Container_PalletDtl
SET Length = {0},
	Width = {1},
	Height = {2},
	Weight = {3},
	UpdateTime = getdate() where PalletNo='{4}' and BDNo='{5}'", SNInfolist[i].length, SNInfolist[i].width, SNInfolist[i].height
                                                               , SNInfolist[i].weight, SNInfolist[i].SN, SNInfolist[i].BDNO);
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
                                                                    deliveryTasksResultlist[i].SN, deliveryTasksResultlist[i].hubno);
                        if (deliveryTasksResultlist[i].taskType.ToUpper() == "BIND")
                        {
                            insertSql += string.Format(@"UPDATE dbo.T_Biz_Container_PalletDtl
SET Warecell = '',
	State = 2,
	Station = '{0}',
	UpdateTime = getdate()
WHERE PalletNo = '{1}'", deliveryTasksResultlist[i].hubno, deliveryTasksResultlist[i].SN);
                        }
                        else
                        {
                            insertSql += string.Format("DELETE FROM dbo.T_Biz_Container_PalletDtl WHERE PalletNo = '{0}'", deliveryTasksResultlist[i].SN);
                        }

                        insertSql += string.Format("UPDATE dbo.T_Task_UnLoad_Old SET State = 99 WHERE TaskNo ='{0}'", deliveryTasksResultlist[i].taskNo);
                    }
                }
                else if (ServerName == "loadTasksResult")
                {
                    List<loadTasksResult> loadTasksResultlist = ChoseServer<loadTasksResult>(s);
                    table = ChoseTableName(ServerName);
                    for (int i = 0; i < loadTasksResultlist.Count; i++)
                    {
                        string sel = string.Format(@"SELECT BDNO, Pn FROM dbo.T_Task_Load_Old WHERE TaskNo='{0}' AND SN='{1}'", loadTasksResultlist[i].taskNo, loadTasksResultlist[i].SN);
                        if (loadTasksResultlist[i].SN.StartsWith("P"))
                        {
                            string sl = string.Format(@"select wcState from dbo.T_Biz_Container_Pallet where wcName='{0}'", loadTasksResultlist[i].hubno);
                            if (comnsql.GetData(Properties.Settings.Default.sql, sl).ToString() == "wcState_Empty")
                            {
                                insertSql += string.Format(@"INSERT INTO {0} (tNo, PalletOrBoxNo, ToWareCell, CreateTime, Flag)
VALUES ('{1}', '{2}','{3}',getdate(), 0)", table, loadTasksResultlist[i].taskNo, loadTasksResultlist[i].SN,
                                                      loadTasksResultlist[i].hubno);
                                insertSql += string.Format(@"UPDATE dbo.T_Biz_Container_PalletDtl
SET Warecell = '{0}',
    State=1,
	UpdateTime = getdate()
WHERE PalletNo = '{1}'", loadTasksResultlist[i].hubno, loadTasksResultlist[i].SN);
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
                        insertSql += string.Format("UPDATE dbo.T_Task_Load_Old SET State = 99 WHERE TaskNo ='{0}'", loadTasksResultlist[i].taskNo);
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
                            insertSql += string.Format(@"INSERT INTO dbo.T_Biz_Container_PalletDtl (PalletNo, BoxNo, BDNo, PNo, Number, IsOverWeight, IsHengWen, IsQualified, State, Station, CreateTime)
SELECT '{0}','{1}',BDNo, PNo,Number,0,0,1,2,'areaTogether',getdate()
FROM T_Task_UnLoad_Old WHERE SN ='{1}' and State < 99", palletPackInfolist[i].SN_Pallet, palletPackInfolist[i].SN_BoxList[j]);
                            insertSql += string.Format("UPDATE dbo.T_Task_UnLoad_Old SET State = 99 WHERE SN ='{0}'", palletPackInfolist[i].SN_BoxList[j]);
                        }


                    }
                }
                else if (ServerName == "stationInfo")
                {
                    int t = (DateTime.Now.Day - 1) * 86400 + DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
                    List<stationInfo> stationInfolist = ChoseServer<stationInfo>(s);
                    table = ChoseTableName(ServerName);
                    for (int i = 0; i < stationInfolist.Count; i++)
                    {
                        string sl = string.Format(@"SELECT * FROM dbo.T_Biz_Container_PalletDtl
WHERE PalletNo ='{0}'", stationInfolist[i].SN);
                        if (comnsql.SelectGet(Properties.Settings.Default.sql, sl) != null)
                        {
                            insertSql += string.Format(@"UPDATE dbo.T_Biz_Container_PalletDtl
SET Warecell = '',
	State = 2,
	Station = '{0}',
	UpdateTime = getdate()
WHERE PalletNo = '{1}'", stationInfolist[i].hubno, stationInfolist[i].SN);
                            insertSql += string.Format("UPDATE dbo.T_Biz_Container_Pallet SET wcState = 'wcState_Empty',StoreType='',PalletNo='',UpdateTime=getdate() WHERE PalletNo='{0}'", stationInfolist[i].SN);
                        }
                        else
                        {
                            throw new Exception("库存中无托盘号为：" + stationInfolist[i].SN + "的数据");
                        }
                        if (stationInfolist[i].hubno.Contains("Buffer"))
                        {
                            CreateBufferAreaTask(stationInfolist[i], date, t);
                        }
                        else if (stationInfolist[i].hubno.Contains("Pick"))
                        {
                            CreateApartAreaTask(stationInfolist[i], date, t);
                        }
                        else if (stationInfolist[i].hubno.Contains("Apart"))
                        {
                            CreatePickAreaTask(stationInfolist[i], date, t);
                        }
                        insertSql += string.Format(@"INSERT INTO {0} (tNo, PalletOrBoxNo, AreaNo, CreateTime, Flag)
VALUES ('{1}', '{2}','{3}',  getdate(), 0)", table, stationInfolist[i].taskNo, stationInfolist[i].SN, stationInfolist[i].hubno);
                        insertSql += string.Format("UPDATE dbo.T_Task_UnLoad_Old SET State = 99 WHERE TaskNo ='{0}'", stationInfolist[i].taskNo);
                    }
                }
                else
                {
                    throw new Exception("不知道是什么方法！");
                }
                int successNumber = comnsql.DataOperator(Properties.Settings.Default.sql, insertSql);
                LogWrite.WriteLogToMain(DateTime.Now.ToString() + " " + ServerName + "插入[" + table + "]数据成功！\n");
                comnlog.MessageLog("上传数据", ServerName + "插入[" + table + "]数据成功！\n");
                //返回成功的Json至wcf服务
                resmod = ResultModel.GteRemark(true, DateTime.Now, null);
            }
            catch (Exception e)
            {
                Emessage = DateTime.Now.ToString() + " " + ServerName + "插入表失败！" + e.Message + "\n";
                resmod = ResultModel.GteRemark(false, DateTime.Now, Emessage);
            }
            LogWrite.WriteResult(ServerName + "==" + JsonConvert.SerializeObject(resmod));
            comnlog.MessageLog("上传数据", ServerName + "==" + JsonConvert.SerializeObject(resmod) + "\n");
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

        public static string GetTaskNo(string date)
        {
            DAL_Comn_Sql comnsql = new DAL_Comn_Sql();
            SqlParameter[] paras = new SqlParameter[1];
            paras[0] = new SqlParameter("@NEW_ID", SqlDbType.NVarChar, 50);
            paras[0].Direction = ParameterDirection.Output;
            string taskno = comnsql.ProcOutput(Properties.Settings.Default.sql, "P_GetNewUnLoadTaskNo", paras, "@NEW_ID").ToString();
            string number = "";
            if (!string.IsNullOrEmpty(taskno))
            {
                number = date + (int.Parse(taskno.Substring(taskno.Length - 8, 8)) + 1).ToString().PadLeft(8, '0');
            }
            else
            {
                number = date + "1".PadLeft(8, '0');
            }
            return number;
        }

        private static string GetLoadTaskNo(string date)
        {
            DAL_Comn_Sql comnsql = new DAL_Comn_Sql();
            SqlParameter[] paras = new SqlParameter[1];
            paras[0] = new SqlParameter("@NEW_ID", SqlDbType.NVarChar, 50);
            paras[0].Direction = ParameterDirection.Output;
            string taskno = comnsql.ProcOutput(Properties.Settings.Default.sql, "P_GetNewLoadTaskNo", paras, "@NEW_ID").ToString();

            string number = "";
            if (!string.IsNullOrEmpty(taskno))
            {
                number = date + (Convert.ToInt16(taskno.Substring(taskno.Length - 8, 8)) + 1).ToString().PadLeft(8, '0');
            }
            else
            {
                number = date + "1".PadLeft(8, '0');
            }
            return number;
        }

        public static string GetBox(string date)
        {
            DAL_Comn_Sql comnsql = new DAL_Comn_Sql();
            string boxNo = "";
            string selectboxstring = string.Format("SELECT No FROM dbo.T_Tmp_Record WHERE Type = 'CBox' and isnull(No,'') like '%{0}%'", date);
            if (comnsql.GetData(Properties.Settings.Default.sql, selectboxstring) != null)
            {
                string box = comnsql.GetData(Properties.Settings.Default.sql, selectboxstring).ToString();
                if (!string.IsNullOrEmpty(box))
                {
                    boxNo = date + (Convert.ToInt32(box.Substring((box.Length - 8), 8)) + 1).ToString().PadLeft(8, '0');
                }
                else
                {
                    boxNo = date + "00000001";
                }
            }
            else
            {
                boxNo = date + "00000001";
            }
            string updateboxstring = string.Format("update dbo.T_Tmp_Record set No='{}' WHERE Type = 'CBox' ", boxNo);
            comnsql.DataOperator(Properties.Settings.Default.sql, updateboxstring);
            return boxNo;
        }

        public static void CreateApartAreaTask(stationInfo SI, string date, int t)
        {
            DAL_Comn_Sql comnsql = new DAL_Comn_Sql();
            DAL_Comn_Log comnlog = new DAL_Comn_Log();
            string selectstring = string.Format(@"SELECT ult.SN,ult.Sign,ult.OutNo,ult.IsPipeline,ult.Number AS NeedNumber,ult.BDNo AS NeedBDNo,ult.PNo AS NeedpNo,pd.BoxNo,pd.BDNo,pd.PNo,pd.Number,pd.IsOverWeight,pd.IsHengWen,pd.IsQualified,mn.miniPerBag
FROM dbo.T_Task_UnLoad_Old ult
LEFT JOIN T_Biz_Container_PalletDtl pd ON SN=pd.PalletNo
LEFT JOIN T_Biz_PNo_MiniNumber mn ON ult.PNo=mn.pNo
WHERE TaskNo = '{0}' AND SN='{1}' AND IsUnpackTray=1", SI.taskNo, SI.SN);
            DataSet ds = comnsql.SelectGet(Properties.Settings.Default.sql, selectstring);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string boxNo = "CC" + GetBox(date);
                string TaskNo = "ULTaskNo" + GetTaskNo(date);
                if (dr["NeedBDNo"].ToString() == dr["BDNo"].ToString() && dr["NeedpNo"].ToString() == dr["PNo"].ToString())
                {

                    int boxcount = Convert.ToInt32(Convert.ToInt32(dr["NeedNumber"]) / Convert.ToInt32(dr["miniPerBag"]));
                    int lastcount = Convert.ToInt32(Convert.ToInt32(dr["NeedNumber"]) % Convert.ToInt32(dr["miniPerBag"]));
                    if (lastcount == 0)
                    {
                        string insertstring = string.Format(@"INSERT INTO dbo.T_Task_UnLoad (Times, State, Sign, TaskNo, TaskType, TaskState, Priority, OutNo, PlanningNo, SN, HubNo, IsUnpackTray, IsBoxLable, IsMinpackLable, IsPipeline, SN_OLD, Number, BusinessType, BDNo, PNo, Station, CreateTime)
VALUES ({9}, 0, '{0}', '{1}', 'OUT', 'NORMAL', 1, '{2}', '', '{3}', '', 0, 0, -1, {4}, '{5}', {6}, '', '{7}', '{8}', '', getdate()"
               , dr["Sign"].ToString(), TaskNo, dr["OutNo"].ToString(), dr["BoxNo"].ToString(), Convert.ToInt16(dr["IsPipeline"]), dr["SN"].ToString(), Convert.ToInt32(dr["NeedNumber"]), dr["BDNo"].ToString(), dr["PNo"].ToString(), t + 5);
                        int affect = comnsql.DataOperator(Properties.Settings.Default.sql, insertstring);
                        if (affect == 1)
                        {
                            LogWrite.WriteLogToMain(DateTime.Now.ToString() + "插入拆托区至分拣区（出库专道）的任务成功，任务号：" + TaskNo + "，箱号：" + dr["BoxNo"].ToString());
                            comnlog.MessageLog("任务创建", DateTime.Now.ToString() + "插入拆托区至分拣区（出库专道）的任务成功，任务号：" + TaskNo + "，箱号：" + dr["BoxNo"].ToString());
                            insertUnloadTask(t, dr["Sign"].ToString(), TaskNo, dr["OutNo"].ToString(), dr["BoxNo"].ToString(), "", false, false, false, Convert.ToBoolean(dr["IsPipeline"]), dr["SN"].ToString(), dr["BDNo"].ToString(), dr["PNo"].ToString(),"");
                            string insertMsg = string.Format(@"INSERT INTO dbo.T_Tmp_ApartData (OutNo, BDNo, PNo, PalletNo, HaveCount, BoxNo, NeedCount, MiniPerBox, BoxCount, BoxRemainCount,PalletRemainCount,Sign)
VALUES ('{0}', '{1}', '{2}', '{3}', {4}, '{5}', {6}, {7}, {8}, {9})", dr["OutNo"].ToString(), dr["BDNo"].ToString(), dr["PNo"].ToString(), dr["SN"].ToString(), Convert.ToInt32(dr["Number"]), boxNo
                                                                    , Convert.ToInt32(dr["NeedNumber"]), Convert.ToInt32(dr["miniPerBag"]), boxcount, 0, Convert.ToInt32(dr["Number"]) - Convert.ToInt32(dr["NeedNumber"]), dr["Sign"].ToString());
                            int affect1 = comnsql.DataOperator(Properties.Settings.Default.sql, insertMsg);
                        }
                    }
                    else
                    {
                        string insertstring = string.Format(@"INSERT INTO dbo.T_Task_UnLoad (Times, State, Sign, TaskNo, TaskType, TaskState, Priority, OutNo, PlanningNo, SN, HubNo, IsUnpackTray, IsBoxLable, IsMinpackLable, IsPipeline, SN_OLD, Number, BusinessType, BDNo, PNo, Station, CreateTime)
VALUES ({9}, 0, '{0}', '{1}', 'OUT', 'NORMAL', 1, '{2}', '', '{3}', '', 1, 0, -1, {4}, '{5}', {6}, '', '{7}', '{8}', '', getdate()"
               , dr["Sign"].ToString(), TaskNo, dr["OutNo"].ToString(), dr["BoxNo"].ToString(), Convert.ToInt16(dr["IsPipeline"]), dr["SN"].ToString(), Convert.ToInt32(dr["miniPerBag"]) * (boxcount + 1), dr["BDNo"].ToString(), dr["PNo"].ToString(), t + 5);
                        int affect = comnsql.DataOperator(Properties.Settings.Default.sql, insertstring);
                        if (affect == 1)
                        {
                            LogWrite.WriteLogToMain(DateTime.Now.ToString() + "插入拆托区至分拣区（入库专道）的任务成功，任务号：" + TaskNo + "，箱号：" + dr["BoxNo"].ToString());
                            comnlog.MessageLog("任务创建", DateTime.Now.ToString() + "插入拆托区至分拣区（入库专道）的任务成功，任务号：" + TaskNo + "，箱号：" + dr["BoxNo"].ToString());
                            insertUnloadTask(t, dr["Sign"].ToString(), TaskNo, dr["OutNo"].ToString(), dr["BoxNo"].ToString(), "", true, false, false, Convert.ToBoolean(dr["IsPipeline"]), dr["SN"].ToString(), dr["BDNo"].ToString(), dr["PNo"].ToString(), "");
                            string insertMsg = string.Format(@"INSERT INTO dbo.T_Tmp_ApartData (OutNo, BDNo, PNo, PalletNo, HaveCount, BoxNo, NeedCount, MiniPerBox, BoxCount, BoxRemainCount,PalletRemainCount,Sign)
VALUES ('{0}', '{1}', '{2}', '{3}', {4}, '{5}', {6}, {7}, {8}, {9})", dr["OutNo"].ToString(), dr["BDNo"].ToString(), dr["PNo"].ToString(), dr["SN"].ToString(), Convert.ToInt32(dr["Number"]), boxNo
                                                                    , Convert.ToInt32(dr["NeedNumber"]), Convert.ToInt32(dr["miniPerBag"]), (boxcount + 1)
                                                                    , (Convert.ToInt32(dr["miniPerBag"]) * (boxcount + 1) - Convert.ToInt32(dr["NeedNumber"])), Convert.ToInt32(dr["Number"]) - Convert.ToInt32(dr["miniPerBag"]) * (boxcount + 1)
                                                                    , dr["Sign"].ToString());
                            int affect1 = comnsql.DataOperator(Properties.Settings.Default.sql, insertMsg);
                        }
                    }
                    if (ds.Tables[0].Rows.Count > 1 || ((Convert.ToInt32(dr["Number"]) - Convert.ToInt32(dr["NeedNumber"])) / Convert.ToInt32(dr["miniPerBag"]) > 0))
                    {
                        int loadnumber = 0;
                        string numberdtl = "";
                        string update = "";
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            string ss = string.Format(@"SELECT PalletRemainCount FROM dbo.T_Tmp_ApartData WHERE PalletNo='{0}'", dr["SN"].ToString());
                            int prn = Convert.ToInt32(comnsql.GetData(Properties.Settings.Default.sql, ss));
                            update = string.Format(@"UPDATE dbo.T_Biz_Container_PalletDtl
SET Number = {0},State=1,UpdateTime=getdate()
WHERE PalletNo = '{1}' AND BDNo='{2}' AND PNo='{3}'", prn, dr["SN"].ToString(), dr["BDNo"].ToString(), dr["PNo"].ToString());

                        }
                        else
                        {
                            string ss = string.Format(@"SELECT PalletRemainCount FROM dbo.T_Tmp_ApartData WHERE PalletNo='{0}'", dr["SN"].ToString());
                            int prn = Convert.ToInt32(comnsql.GetData(Properties.Settings.Default.sql, ss));
                            if (prn != 0)
                            {
                                update += string.Format(@"UPDATE dbo.T_Biz_Container_PalletDtl
SET Number = {0},State=1,UpdateTime=getdate()
WHERE PalletNo = '{1}' AND BDNo='{2}' AND PNo='{3}' and Number={4}", prn, dr["SN"].ToString(), dr["BDNo"].ToString(), dr["PNo"].ToString(), Convert.ToInt32(dr["Number"]));
                            }
                            else
                            {
                                update += string.Format(@"delete from  dbo.T_Biz_Container_PalletDtl
WHERE PalletNo = '{1}' AND BDNo='{2}' AND PNo='{3}' and Number={0}", Convert.ToInt32(dr["Number"]), dr["SN"].ToString(), dr["BDNo"].ToString(), dr["PNo"].ToString());
                            }
                            update += string.Format(@"UPDATE dbo.T_Biz_Container_PalletDtl SET State=1,UpdateTime=getdate()
WHERE PalletNo = '{0}' ", dr["SN"].ToString());
                        }
                        comnsql.DataOperator(Properties.Settings.Default.sql, update);


                        string select = string.Format(@"SELECT PalletNo, BDNo, PNo, Number FROM dbo.T_Biz_Container_PalletDtl WHERE PalletNo = '{0}'", dr["SN"].ToString());
                        DataSet ds1 = comnsql.SelectGet(Properties.Settings.Default.sql, select);
                        foreach (DataRow dr1 in ds1.Tables[0].Rows)
                        {
                            loadnumber += Convert.ToInt32(dr1["Number"]);
                            if (string.IsNullOrEmpty(numberdtl))
                            {
                                numberdtl = Convert.ToString(loadnumber);
                            }
                            else
                            {
                                numberdtl += "," + Convert.ToString(loadnumber);
                            }
                        }
                        string TaskNo1 = "LTaskNo" + GetLoadTaskNo(date);
                        string insertstring1 = string.Format(@"INSERT INTO dbo.T_Task_Load (Times, State, TaskNo, SN, PlanningNo, IsHengwen, IsBLOCK, TaskType, BDNO, Pn, Weight, Height, SN_OLD, Number, NumberDtl, CreateTime)
VALUES ({8}, 0, '{0}', '{1}', '', {2}, {3}, -1, '{4}', '{5}', 0, 0, '{1}', {6}, '{7}', getdate()", TaskNo1, dr["SN"].ToString(), Convert.ToInt32(dr["IsHengwen"]), Convert.ToInt32(dr["IsBLOCK"]), dr["BDNo"].ToString(), dr["PNo"].ToString(), loadnumber, numberdtl, t + 5);
                        int affect1 = comnsql.DataOperator(Properties.Settings.Default.sql, insertstring1);
                        if (affect1 == 1)
                        {
                            LogWrite.WriteLogToMain(DateTime.Now.ToString() + "插入拆托区至立库的任务成功，任务号：" + TaskNo1 + "，托盘号：" + dr["SN"].ToString());
                            comnlog.MessageLog("任务创建", DateTime.Now.ToString() + "插入拆托区至立库的任务成功，任务号：" + TaskNo1 + "，托盘号：" + dr["SN"].ToString());
                            insertLoadTask(t, TaskNo1, dr["SN"].ToString(), Convert.ToBoolean(dr["IsHengwen"]), dr["BDNo"].ToString(), dr["PNo"].ToString(), 0, 0);
                        }
                    }
                }
            }
        }

        public static void CreatePickAreaTask(stationInfo SI, string date, int t)
        {
            DAL_Comn_Sql comnsql = new DAL_Comn_Sql();
            DAL_Comn_Log comnlog = new DAL_Comn_Log();
            string insertstring = "";
            string ss = string.Format(@"SELECT OutNo,BDNo, PNo,NeedCount,BoxRemainCount,Sign FROM dbo.T_Tmp_ApartData WHERE BoxNo='{0}'", SI.SN);
            DataSet ds = comnsql.SelectGet(Properties.Settings.Default.sql, ss);
            if (ds != null)
            {
                string ulTaskNo = "ULTaskNo" + GetTaskNo(date);
                string lTaskNo = "LTaskNo" + GetLoadTaskNo(date);
                string boxNo = "CF" + GetBox(date);
                int outNumber = Convert.ToInt32(ds.Tables[0].Rows[0]["NeedCount"]);
                int inNumber = Convert.ToInt32(ds.Tables[0].Rows[0]["BoxRemainCount"]);
                bool[] ispipe = { true, false };
                Random rr = new Random();
                int IsPipeline = Convert.ToInt16(ispipe[rr.Next(0, ispipe.Length)]);
                insertstring += string.Format(@"INSERT INTO dbo.T_Task_Load (Times, State, TaskNo, SN, PlanningNo, IsHengwen, IsBLOCK, TaskType, BDNO, Pn, Weight, Height, SN_OLD, Number, NumberDtl, CreateTime)
VALUES ({8}, 0, '{0}', '{1}', '', {2}, {3}, -1, '{4}', '{5}', 0, 0, '{8}', {6}, '{7}', getdate()"
                    , lTaskNo, boxNo, 0, 1, ds.Tables[0].Rows[0]["BDNo"].ToString(), ds.Tables[0].Rows[0]["PNo"].ToString(), inNumber, inNumber.ToString(), SI.SN, t + 5);
                insertstring += string.Format(@"INSERT INTO dbo.T_Task_UnLoad (Times, State, Sign, TaskNo, TaskType, TaskState, Priority, OutNo, PlanningNo, SN, HubNo, IsUnpackTray, IsBoxLable, IsMinpackLable, IsPipeline, SN_OLD, Number, BusinessType, BDNo, PNo, Station, CreateTime)
VALUES ({8}, 0, '{0}', '{1}', 'OUT', 'NORMAL', 1, '{2}', '', '{3}', '', 1, 0, -1, {4}, '{3}', {5}, '', '{6}', '{7}', '', getdate()"
                  , ds.Tables[0].Rows[0]["Sign"].ToString(), ulTaskNo, ds.Tables[0].Rows[0]["OutNo"].ToString(), SI.SN, IsPipeline, outNumber
                  , ds.Tables[0].Rows[0]["BDNo"].ToString(), ds.Tables[0].Rows[0]["PNo"].ToString(), t + 5);
                insertstring += string.Format(@"DELETE FROM dbo.T_Tmp_ApartData WHERE BoxNo='{0}'", SI.SN);
                int affect = comnsql.DataOperator(Properties.Settings.Default.sql, insertstring);
                if (affect == 3)
                {
                    LogWrite.WriteLogToMain(DateTime.Now.ToString() + "插入分拣区至拼托区的任务成功，任务号：" + ulTaskNo + "，箱号：" + SI.SN);
                    comnlog.MessageLog("任务创建", DateTime.Now.ToString() + "插入分拣区至拼托区的任务成功，任务号：" + ulTaskNo + "，箱号：" + SI.SN);
                    insertUnloadTask(t, ds.Tables[0].Rows[0]["Sign"].ToString(), ulTaskNo, ds.Tables[0].Rows[0]["OutNo"].ToString(), SI.SN, "", true, false, false, Convert.ToBoolean(IsPipeline), SI.SN, ds.Tables[0].Rows[0]["BDNo"].ToString(), ds.Tables[0].Rows[0]["PNo"].ToString(), "");
                    LogWrite.WriteLogToMain(DateTime.Now.ToString() + "插入分拣区至散件库的任务成功，任务号：" + lTaskNo + "，箱号：" + boxNo);
                    comnlog.MessageLog("任务创建", DateTime.Now.ToString() + "插入分拣区至散件库的任务成功，任务号：" + lTaskNo + "，箱号：" + boxNo);
                    insertLoadTask(t, lTaskNo, boxNo, false, ds.Tables[0].Rows[0]["BDNo"].ToString(), ds.Tables[0].Rows[0]["PNo"].ToString(), 0, 0);
                }
            }
        }

        public static void CreateBufferAreaTask(stationInfo SI, string date, int t)
        {
            DAL_Comn_Sql comnsql = new DAL_Comn_Sql();
            DAL_Comn_Log comnlog = new DAL_Comn_Log();
            string number = GetTaskNo(date);
            string TaskNo = "DTaskNo" + number;
            string taskstring = string.Format("INSERT INTO dbo.T_Task_UnLoad (Times, State, TaskNo, TaskType, SN, CreateTime) VALUES ({0}, 0, '{1}', '{2}', '{3}',getdate())", t = 5, TaskNo, "Load", SI.SN);
            int sNumber = comnsql.DataOperator(Properties.Settings.Default.sql, taskstring);
            if (sNumber == 1)
            {
                LogWrite.WriteLogToMain(DateTime.Now.ToString() + "插入至出口的任务成功，任务号：" + TaskNo + "，托盘号：" + SI.SN);
                comnlog.MessageLog("任务创建", DateTime.Now.ToString() + "插入至出口的任务成功，任务号：" + TaskNo + "，托盘号：" + SI.SN);
                DeliveryTasks dtask = new DeliveryTasks();
                dtask.Times = t + 5;
                dtask.taskNo = TaskNo;
                dtask.taskType = "Load";
                dtask.SN[0] = SI.SN;
                dtask.state = 0;
                Form1.sendDeliveryTasksList.Add(dtask);
                //Form1.SendDeliveryTask(dtask);
            }
        }

        private static void insertLoadTask(int t,string taskNo,string SN,bool isHengWen,string BDNo,string PNo,int weight,int height)
        {
            LoadData pclt = new LoadData();
            pclt.times = t+5;
            pclt.state = 0;
            pclt.taskNo = taskNo;
            pclt.palletOrBoxNo = SN;
            pclt.isHengWen = isHengWen;
            pclt.bdNo = BDNo;
            pclt.pNo = PNo;
            pclt.weight = weight;
            pclt.height = height;
            if (Form1.sendLoadTasksList.Find(a => a.taskNo == pclt.taskNo) == null)
            {
                Form1.sendLoadTasksList.Add(pclt);              
            }
        }

        private static void insertUnloadTask(int t, string sign, string taskNo, string OutNo, string palletOrBoxNo, string hubno, bool isUnpackTray, bool isBoxLable, bool isMinpackLable, bool isPipeline,
            string SN_OLD,string bdno,string pno,string station)
        {
            UnLoadData ult = new UnLoadData();
            ult.times = t+5;
            ult.state = 0;
            ult.sign = sign;
            ult.taskNo = taskNo;
            ult.taskType = "OUT";
            ult.taskState = "NORMAL";
            ult.priority = 1;
            ult.outNo = OutNo;
            ult.planningNo = "";
            ult.palletOrBoxNo = palletOrBoxNo;
            ult.hubno = hubno;
            ult.isUnpackTray = isUnpackTray;
            ult.isBoxLable = isBoxLable;
            ult.isMinpackLable = isMinpackLable;
            ult.isPipeline = isPipeline;
            ult.SN_OLD = SN_OLD;
            ult.bdNo = bdno;
            ult.pNo = pno;
            ult.station = station;
            if (Form1.insertSendUnloadTasksList.Find(a => a.taskNo == ult.taskNo) == null)
            {
                Form1.insertSendUnloadTasksList.Add(ult);
                
            }
        }
    }
    
}
