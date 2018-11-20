using com.force.json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XNWMS.Common;
using XNWMS.InfoModel_S;

namespace XNWMS
{
    public class DoService
    {
        public static XNResultModel Doserver(string s, string ServerName)
        {
            DAL_Comn_Log comnlog = new DAL_Comn_Log();
            DAL_Comn_Sql comnsql = new DAL_Comn_Sql();
            string Emessage = null;
            //ResultModel resmod = new ResultModel();
            XNResultModel resmod = new XNResultModel();
            string table = "";
            string insertSql = "";
            List<deliveryTasksResult> deliveryTasksResultlist=null;
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
VALUES ('{1}', '{2}',{3}, {4}, {5}, {6}, getdate(),0) ", table, SNInfolist[i].SN, SNInfolist[i].BDNO, SNInfolist[i].length,
                                                        SNInfolist[i].width, SNInfolist[i].height, SNInfolist[i].weight);
                        if (SNInfolist[i].SN.StartsWith("P"))
                        {
                            insertSql += string.Format(@"UPDATE dbo.T_Biz_Container_PalletDtl
SET Length = {0},
	Width = {1},
	Height = {2},
	Weight = {3},
	UpdateTime = getdate() where PalletNo='{4}' ", SNInfolist[i].length, SNInfolist[i].width, SNInfolist[i].height
                                                               , SNInfolist[i].weight, SNInfolist[i].SN);
                        }
                        else if (SNInfolist[i].SN.StartsWith("C"))
                        {
                            insertSql += string.Format(@"UPDATE dbo.T_Biz_Container_BulksDtl
SET Length = {0}
	, Width = {1}
	, Height = {2}
	, Weight = {3}
    ,UpdateTime=getdate()
WHERE BoxNo = '{4}'", SNInfolist[i].length, SNInfolist[i].width, SNInfolist[i].height, SNInfolist[i].weight, SNInfolist[i].SN);
                        }
                        
                    }
                }
                else if (ServerName == "deliveryTasksResult")
                {
                    deliveryTasksResultlist = ChoseServer<deliveryTasksResult>(s);
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
                            insertSql += string.Format("UPDATE T_Tmp_Task SET TaskNo='{1}' ,State=7,FinishTime=getdate() where PalletNo='{0}' and State=6", deliveryTasksResultlist[i].SN, deliveryTasksResultlist[i].taskNo);
                        }

                        insertSql += string.Format("UPDATE dbo.T_Task_UnLoad_Old SET State = 99,FinishTime=getdate() WHERE TaskNo ='{0}' and State < 99", deliveryTasksResultlist[i].taskNo);
                    }
                }
                else if (ServerName == "loadTasksResult")
                {
                    List<loadTasksResult> loadTasksResultlist = ChoseServer<loadTasksResult>(s);
                    table = ChoseTableName(ServerName);
                    for (int i = 0; i < loadTasksResultlist.Count; i++)
                    {
                        //string sel = string.Format(@"SELECT BDNO, Pn FROM dbo.T_Task_Load_Old WHERE TaskNo='{0}' AND SN='{1}'", loadTasksResultlist[i].taskNo, loadTasksResultlist[i].SN);
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
    Station='',
	UpdateTime = getdate()
WHERE PalletNo = '{1}' ", loadTasksResultlist[i].hubno, loadTasksResultlist[i].SN);
                                insertSql += string.Format(@"update dbo.T_Biz_Container_Pallet set wcState='wcState_Store',StoreType='Container_Pallet',PalletNo='{1}', UpdateTime=getdate() where wcName='{0}'", loadTasksResultlist[i].hubno, loadTasksResultlist[i].SN);
                            }
                            else
                            {
                                throw new Exception("仓位已存在托盘");
                            }
                        }
                        else if (loadTasksResultlist[i].SN.StartsWith("C"))
                        {
                            sendBulkContainer(loadTasksResultlist[i]);
                        }
                        insertSql += string.Format("UPDATE dbo.T_Task_Load_Old SET State = 99,FinishTime=getdate() WHERE TaskNo ='{0}' and State < 99", loadTasksResultlist[i].taskNo);
                    }
                }
                else if (ServerName == "palletPackInfo")
                {
                    List<palletPackInfo> palletPackInfolist = ChoseServer<palletPackInfo>(s);
                    table = ChoseTableName(ServerName);
                    for (int i = 0; i < palletPackInfolist.Count; i++)
                    {
                        //int tNumber = 0;
                        for (int j = 0; j < palletPackInfolist[i].SN_BoxList.Count; j++)
                        {
                            insertSql += string.Format(@"INSERT INTO {0} (PalletNo, BoxNo, CreateTime, Flag)
VALUES ('{1}', '{2}',getdate(), 0)", table, palletPackInfolist[i].SN_Pallet, palletPackInfolist[i].SN_BoxList[j].SN);
                            insertSql += string.Format(@"INSERT INTO dbo.T_Biz_Container_PalletDtl (PalletNo, BoxNo, BDNo, PNo, Number, IsOverWeight, IsHengWen, IsQualified, State, Station, CreateTime)
SELECT '{0}','{1}',BDNo, PNo,Number,0,0,1,2,'areaTogether',getdate()
FROM T_Task_UnLoad_Old WHERE SN ='{1}' and State < 99 ", palletPackInfolist[i].SN_Pallet, palletPackInfolist[i].SN_BoxList[j].SN );
                            insertSql += string.Format(" UPDATE dbo.T_Task_UnLoad_Old SET State = 99,FinishTime=getdate() WHERE SN ='{0}' and State < 99", palletPackInfolist[i].SN_BoxList[j].SN);
                            insertSql += string.Format(@"UPDATE T_Tmp_Task SET TaskNo = '',
	    PalletNo = '{1}',
        BoxNo='',
	    State = 5,
	    UpdateTime = getdate() WHERE BoxNo='{0}' and State=4", palletPackInfolist[i].SN_BoxList[j].SN, palletPackInfolist[i].SN_Pallet);
                            insertSql += string.Format(@"DELETE FROM dbo.T_Tmp_TogetherData WHERE BoxNo = '{0}' ", palletPackInfolist[i].SN_BoxList[j].SN );
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
                        if (!string.IsNullOrEmpty(stationInfolist[i].taskNo))
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
                                if (!string.IsNullOrEmpty(stationInfolist[i].taskNo))
                                {
                                    insertSql += string.Format("UPDATE dbo.T_Tmp_Task SET State = 6,UpdateTime = GETDATE() WHERE PalletNo='{0}' AND TaskNo='{1}' and State=0", stationInfolist[i].SN, stationInfolist[i].taskNo);
                                }
                                else
                                {
                                    insertSql += string.Format("UPDATE dbo.T_Tmp_Task SET State = 6,UpdateTime = GETDATE() WHERE PalletNo='{0}' and State=5", stationInfolist[i].SN, stationInfolist[i].taskNo);
                                }

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
                            insertSql += string.Format(@"INSERT INTO {0} (tNo, PalletOrBoxNo, AreaNo,outPort, CreateTime, Flag)
VALUES ('{1}', '{2}','{3}', '{4}', getdate(), 0)", table, stationInfolist[i].taskNo, stationInfolist[i].SN, stationInfolist[i].hubno,stationInfolist[i].outPort);
                            if (stationInfolist[i].hubno.Contains("Together"))
                            {
                                insertSql += string.Format(@"UPDATE dbo.T_Tmp_TogetherData SET State = 1 WHERE BoxNo = '{0}' ", stationInfolist[i].SN);
                                insertSql += string.Format("UPDATE dbo.T_Task_UnLoad_Old SET State = 98,FinishTime=getdate() WHERE TaskNo ='{0}' and State < 98", stationInfolist[i].taskNo);
                            }
                            else if (!stationInfolist[i].hubno.Contains("Apart"))
                            {
                                insertSql += string.Format("UPDATE dbo.T_Task_UnLoad_Old SET State = 99,FinishTime=getdate() WHERE TaskNo ='{0}' and State < 99", stationInfolist[i].taskNo);
                            }
                        }
                        else
                        {
                            insertSql += string.Format(@"update dbo.T_Biz_Container_Bulks 
set cState='wcState_Empty',cType='' ,cUpdateTime=getdate()
where cShelfDtl IN (SELECT HubNo FROM T_Biz_Container_BulksDtl WHERE BoxNo='{0}')", stationInfolist[i].SN);
                            insertSql += string.Format(@"DELETE FROM dbo.T_Biz_Container_BulksDtl WHERE BoxNo ='{0}'AND Flag=1", stationInfolist[i].SN);
                            insertSql += string.Format(@"INSERT INTO {0} (tNo, PalletOrBoxNo, AreaNo, CreateTime, Flag)
VALUES ('{1}', '{2}','{3}',  getdate(), 0)", table, stationInfolist[i].taskNo, stationInfolist[i].SN, stationInfolist[i].hubno);
                            insertSql += string.Format("UPDATE dbo.T_Task_UnLoad_Old SET State = 99,FinishTime=getdate() WHERE SN ='{0}' and State = 1", stationInfolist[i].SN);
                            CreatePickAreaTask(stationInfolist[i], date, t);
                        }
                        
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
                //resmod = ResultModel.GteRemark(true, DateTime.Now, null);
                resmod.StatusCode = "200";
                resmod.Info = "处理成功";


                if (ServerName == "deliveryTasksResult")
                {
                     for (int i = 0; i < deliveryTasksResultlist.Count; i++)
                    {
                        string sels = string.Format(@"SELECT distinct Sign,OutNo FROM dbo.T_Tmp_Task WHERE PalletNo='{0}' and TaskNo='{1}'", deliveryTasksResultlist[i].SN, deliveryTasksResultlist[i].taskNo);
                        DataSet ds1 = comnsql.SelectGet(Properties.Settings.Default.sql, sels);
                        foreach (DataRow dr in ds1.Tables[0].Rows)
                        {
                            string sign = dr["Sign"].ToString();
                            string outno = dr["OutNo"].ToString();
                            string sels1 = string.Format(@"SELECT TotalNumber, Number FROM dbo.T_Tmp_Task WHERE Sign = '{0}' AND State=7", sign);
                            DataSet ds2 = comnsql.SelectGet(Properties.Settings.Default.sql, sels1);
                            int tnb = 0;
                            foreach (DataRow dr1 in ds2.Tables[0].Rows)
                            {
                                tnb += Convert.ToInt32(dr1["Number"]);
                            }
                            if(Convert.ToInt32(ds2.Tables[0].Rows[0]["TotalNumber"])==tnb)
                            {
                                string ups = string.Format(@"UPDATE dbo.T_Task_UnLoad_Origin SET State = 99,FinishTime=getdate() WHERE State=1 AND Sign='{0}'", sign);
                                int sn = comnsql.DataOperator(Properties.Settings.Default.sql, ups);
                                if (sn == 1)
                                {
                                    string sels2 = string.Format(@"SELECT u.*,o.Count FROM T_Task_UnLoad_Origin u LEFT JOIN T_Tmp_Order o ON o.OutNo=u.OutNo WHERE u.State=99 AND u.OutNo='{0}'", outno);
                                    DataSet ds3 = comnsql.SelectGet(Properties.Settings.Default.sql, sels2);
                                    if (ds3 != null && ds3.Tables[0].Rows.Count != 0 && (ds3.Tables[0].Rows.Count == Convert.ToInt16(ds3.Tables[0].Rows[0]["Count"])))
                                    {
                                        string ups1 = string.Format(@"UPDATE dbo.T_Tmp_Order SET State = 99,FinishTime=getdate() WHERE State=0 AND OutNo='{0}'", outno);
                                        int sn1 = comnsql.DataOperator(Properties.Settings.Default.sql, ups1);
                                    }
                                }
                            }
                        }
                        
                     }
                }
            }
            catch (Exception e)
            {
                Emessage = DateTime.Now.ToString() + " " + ServerName + "插入表失败！" + e.Message + "\n";
                //resmod = ResultModel.GteRemark(false, DateTime.Now, Emessage);
                resmod.StatusCode = "404";
                resmod.Info = "处理失败";
            }
            LogWrite.WriteResult(ServerName + "==" + JsonConvert.SerializeObject(resmod));
            comnlog.MessageLog("上传数据", ServerName + "==" + JsonConvert.SerializeObject(resmod) + "\n");
            return resmod;
        }

        private static void sendBulkContainer(loadTasksResult loadTasksResult)
        {
            DAL_Comn_Log comnlog = new DAL_Comn_Log();
            DAL_Comn_Sql comnsql = new DAL_Comn_Sql();
            string str = string.Format(@"[{{""taskNo"":""{0}"",
""SN"":""{1}"",
""hubno"":""{2}"",
""shelves"":""{3}"",
""opTime"":""{4}"",
""location"":""{5}"",
""remark"":""{6}""}}]", loadTasksResult.taskNo, loadTasksResult.SN, loadTasksResult.hubno, loadTasksResult.shelfNo, DateTime.Now.ToString(), loadTasksResult.location, loadTasksResult.remark);

            JSONArray jsarray = new JSONArray(str);
            string url = Properties.Settings.Default.url + "/loadTasksResult";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.KeepAlive = true;
            request.AllowAutoRedirect = false;
            request.ContentType = "application/json";
            request.Headers.Add("systemCode:HHWCS");
            byte[] postdatabtyes = Encoding.UTF8.GetBytes(jsarray.ToString());
            request.ContentLength = postdatabtyes.Length;
            try
            {
                Stream requeststream = request.GetRequestStream();
                requeststream.Write(postdatabtyes, 0, postdatabtyes.Length);
                requeststream.Close();
                string resp;

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    resp = sr.ReadToEnd();
                }


                if (resp.Contains("true"))
                {
                    LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 上传散件库存成功，箱号：" + loadTasksResult.SN + "\n" + "========" + resp + "\n");
                    comnlog.MessageLog("散件上架结果",DateTime.Now.ToString() + " 上传散件库存成功，箱号：" + loadTasksResult.SN + "\n" + "========" + resp + "\n");
                    string insertSql = string.Format(@"UPDATE dbo.T_Biz_Container_BulksDtl
SET ShelfNo = '{0}'
	, HubNo = '{1}'
	, flag = 0
    , UpdateTime=getdate()
WHERE BoxNo = '{2}'",loadTasksResult.shelfNo , loadTasksResult.hubno, loadTasksResult.SN);
                    int aa=comnsql.DataOperator(Properties.Settings.Default.sql, insertSql);
                    if (aa != 1)
                    {
                        throw new Exception("更新散件库存失败");
                    }

                    string sl = string.Format(@"select cState from dbo.T_Biz_Container_Bulks where cShelfDtl='{0}'", loadTasksResult.hubno);
                    if (comnsql.GetData(Properties.Settings.Default.sql, sl).ToString() == "wcState_Empty")
                    {
                        string insertSql1 = string.Format("update dbo.T_Biz_Container_Bulks set cState='wcState_Store',cType='Container_Box',cUpdateTime=getdate() where cShelfDtl='{0}'", loadTasksResult.hubno);
                        comnsql.DataOperator(Properties.Settings.Default.sql, insertSql1);
                    }                   
                }
                else
                {
                    LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 上传散件库存失败，箱号：" + loadTasksResult.SN + "\n" + "========" + resp + "\n");
                    comnlog.MessageLog("散件上架结果", DateTime.Now.ToString() + " 上传散件库存失败，箱号：" + loadTasksResult.SN + "\n" + "========" + resp + "\n");                
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.Replace(Environment.NewLine, ""));
            }
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

        public static string GetDeliveryTaskNo()
        {
            DAL_Comn_Sql comnsql = new DAL_Comn_Sql();
            SqlParameter[] paras = new SqlParameter[1];
            paras[0] = new SqlParameter("@NEW_ID", SqlDbType.NVarChar, 50);
            paras[0].Direction = ParameterDirection.Output;
            string taskno = comnsql.ProcOutput(Properties.Settings.Default.sql, "[P_GetNewDeliveryTaskNo]", paras, "@NEW_ID").ToString();
            return taskno;
        }

        public static string GetUnLoadTaskNo()
        {
            DAL_Comn_Sql comnsql = new DAL_Comn_Sql();
            SqlParameter[] paras = new SqlParameter[1];
            paras[0] = new SqlParameter("@NEW_ID", SqlDbType.NVarChar, 50);
            paras[0].Direction = ParameterDirection.Output;
            string taskno = comnsql.ProcOutput(Properties.Settings.Default.sql, "P_GetNewUnLoadTaskNo", paras, "@NEW_ID").ToString();
            return taskno;
        }

        private static string GetLoadTaskNo()
        {
            DAL_Comn_Sql comnsql = new DAL_Comn_Sql();
            SqlParameter[] paras = new SqlParameter[1];
            paras[0] = new SqlParameter("@NEW_ID", SqlDbType.NVarChar, 50);
            paras[0].Direction = ParameterDirection.Output;
            string taskno = comnsql.ProcOutput(Properties.Settings.Default.sql, "P_GetNewLoadTaskNo", paras, "@NEW_ID").ToString();
            return taskno;
        }

        public static string GetBox(string area)
        {
            DAL_Comn_Sql comnsql = new DAL_Comn_Sql();
            SqlParameter[] paras = new SqlParameter[2];
            paras[0] = new SqlParameter("@Area", SqlDbType.NVarChar, 50);
            paras[1] = new SqlParameter("@NEW_ID", SqlDbType.NVarChar, 50);
            paras[0].Value = area;
            paras[1].Direction = ParameterDirection.Output;
            string boxNo = comnsql.ProcOutput(Properties.Settings.Default.sql, "P_GetNewBoxNo", paras, "@NEW_ID").ToString();
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
               
                if (dr["NeedBDNo"].ToString() == dr["BDNo"].ToString() && dr["NeedpNo"].ToString() == dr["PNo"].ToString())
                {
                    string boxNo = GetBox("Apart");
                    string TaskNo = GetUnLoadTaskNo();
                    int boxcount = Convert.ToInt32(Convert.ToInt32(dr["NeedNumber"]) / Convert.ToInt32(dr["miniPerBag"]));
                    int lastcount = Convert.ToInt32(Convert.ToInt32(dr["NeedNumber"]) % Convert.ToInt32(dr["miniPerBag"]));
                    if (lastcount == 0)
                    {
                        string insertstring = string.Format(@"INSERT INTO dbo.T_Task_UnLoad (Times, State, Sign, TaskNo, TaskType, TaskState, Priority, OutNo, PlanningNo, SN, HubNo, IsUnpackTray, IsBoxLable, IsMinpackLable, IsPipeline, SN_OLD, Number, BusinessType, BDNo, PNo, Station, CreateTime)
VALUES ({9}, 0, '{0}', '{1}', 'OUT', 'NORMAL', 1, '{2}', '', '{3}', '', 0, 0, -1, {4}, '{5}', {6}, '', '{7}', '{8}', '', getdate())"
               , dr["Sign"].ToString(), TaskNo, dr["OutNo"].ToString(), boxNo, Convert.ToInt16(dr["IsPipeline"]), dr["SN"].ToString(), Convert.ToInt32(dr["NeedNumber"]), dr["BDNo"].ToString(), dr["PNo"].ToString(), t + 5);
                        int affect = comnsql.DataOperator(Properties.Settings.Default.sql, insertstring);
                        if (affect == 1)
                        {
                            LogWrite.WriteLogToMain(DateTime.Now.ToString() + "插入拆托区至分拣区（出库专道）的任务成功，任务号：" + TaskNo + "，箱号：" + boxNo+"\n");
                            comnlog.MessageLog("任务创建", DateTime.Now.ToString() + "插入拆托区至分拣区（出库专道）的任务成功，任务号：" + TaskNo + "，箱号：" + boxNo);
                            insertUnloadTask(t, dr["Sign"].ToString(), TaskNo, dr["OutNo"].ToString(), boxNo, "", false, false, false, Convert.ToBoolean(dr["IsPipeline"]), dr["SN"].ToString(), dr["BDNo"].ToString(), dr["PNo"].ToString(), "");
                            string insertMsg = string.Format(@"INSERT INTO dbo.T_Tmp_ApartData (OutNo, BDNo, PNo, PalletNo, HaveCount, BoxNo, NeedCount, MiniPerBox, BoxCount, BoxRemainCount,PalletRemainCount,Sign,flag)
VALUES ('{0}', '{1}', '{2}', '{3}', {4}, '{5}', {6}, {7}, {8}, {9},{10},'{11}',0)", dr["OutNo"].ToString(), dr["BDNo"].ToString(), dr["PNo"].ToString(), dr["SN"].ToString(), Convert.ToInt32(dr["Number"]), boxNo
                                                                    , Convert.ToInt32(dr["NeedNumber"]), Convert.ToInt32(dr["miniPerBag"]), boxcount, 0, Convert.ToInt32(dr["Number"]) - Convert.ToInt32(dr["NeedNumber"]), dr["Sign"].ToString());
                            insertMsg += string.Format(@"UPDATE T_Tmp_Task SET TaskNo = '{2}',
	    PalletNo = '',
	    BoxNo = '{3}',
	    Number ={4},
	    State = 3,
	    UpdateTime = getdate() WHERE PalletNo='{0}' AND TaskNo='{1}' and State = 0", SI.SN, SI.taskNo, TaskNo, boxNo, Convert.ToInt32(dr["NeedNumber"]));
                            insertMsg += string.Format(@"UPDATE dbo.T_Tmp_TogetherData SET PalletNo ='',BoxNo = '{0}' WHERE PalletNo = '{1}' ", boxNo, SI.SN);
                            int affect1 = comnsql.DataOperator(Properties.Settings.Default.sql, insertMsg);
                        }
                    }
                    else
                    {
                        int count1 = 0;
                        if(Convert.ToInt32(dr["Number"])<Convert.ToInt32(dr["miniPerBag"]) * (boxcount + 1))
                        {
                            count1 = Convert.ToInt32(dr["Number"]);
                        }
                        else
                        {
                            count1 = Convert.ToInt32(dr["miniPerBag"]) * (boxcount + 1);
                        }
                        string insertstring = string.Format(@"INSERT INTO dbo.T_Task_UnLoad (Times, State, Sign, TaskNo, TaskType, TaskState, Priority, OutNo, PlanningNo, SN, HubNo, IsUnpackTray, IsBoxLable, IsMinpackLable, IsPipeline, SN_OLD, Number, BusinessType, BDNo, PNo, Station, CreateTime)
VALUES ({9}, 0, '{0}', '{1}', 'OUT', 'NORMAL', 1, '{2}', '', '{3}', '', 1, 0, -1, {4}, '{5}', {6}, '', '{7}', '{8}', '', getdate())"
               , dr["Sign"].ToString(), TaskNo, dr["OutNo"].ToString(), boxNo, Convert.ToInt16(dr["IsPipeline"]), dr["SN"].ToString(), count1, dr["BDNo"].ToString(), dr["PNo"].ToString(), t + 5);
                        int affect = comnsql.DataOperator(Properties.Settings.Default.sql, insertstring);
                        if (affect == 1)
                        {
                            LogWrite.WriteLogToMain(DateTime.Now.ToString() + "插入拆托区至分拣区（入库专道）的任务成功，任务号：" + TaskNo + "，箱号：" + boxNo + "\n");
                            comnlog.MessageLog("任务创建", DateTime.Now.ToString() + "插入拆托区至分拣区（入库专道）的任务成功，任务号：" + TaskNo + "，箱号：" + boxNo);
                            insertUnloadTask(t, dr["Sign"].ToString(), TaskNo, dr["OutNo"].ToString(), boxNo, "", true, false, false, Convert.ToBoolean(dr["IsPipeline"]), dr["SN"].ToString(), dr["BDNo"].ToString(), dr["PNo"].ToString(), "");
                            string insertMsg = string.Format(@"INSERT INTO dbo.T_Tmp_ApartData (OutNo, BDNo, PNo, PalletNo, HaveCount, BoxNo, NeedCount, MiniPerBox, BoxCount, BoxRemainCount,PalletRemainCount,Sign,flag)
VALUES ('{0}', '{1}', '{2}', '{3}', {4}, '{5}', {6}, {7}, {8}, {9},{10},'{11}',0)", dr["OutNo"].ToString(), dr["BDNo"].ToString(), dr["PNo"].ToString(), dr["SN"].ToString(), Convert.ToInt32(dr["Number"]), boxNo
                                                                    , Convert.ToInt32(dr["NeedNumber"]), Convert.ToInt32(dr["miniPerBag"]), (boxcount + 1)
                                                                    , (count1 - Convert.ToInt32(dr["NeedNumber"])), Convert.ToInt32(dr["Number"]) - count1
                                                                    , dr["Sign"].ToString());
                            insertMsg += string.Format(@"UPDATE T_Tmp_Task SET TaskNo = '{2}',
	    PalletNo = '',
	    BoxNo = '{3}',
	    Number ={4},
	    State = 3,
	    UpdateTime = getdate() WHERE PalletNo='{0}' AND TaskNo='{1}' and State=0 ", SI.SN, SI.taskNo, TaskNo, boxNo, count1);
                            insertMsg += string.Format(@"UPDATE dbo.T_Tmp_TogetherData SET PalletNo='',BoxNo = '{0}' WHERE PalletNo = '{1}' ", boxNo, SI.SN);
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
                            string ss = string.Format(@"SELECT PalletRemainCount FROM dbo.T_Tmp_ApartData WHERE PalletNo='{0}' and OutNo='{1}' and flag=0", dr["SN"].ToString(), dr["OutNo"].ToString());
                            int prn = Convert.ToInt32(comnsql.GetData(Properties.Settings.Default.sql, ss));
                            update = string.Format(@"UPDATE dbo.T_Biz_Container_PalletDtl
SET Number = {0},State=0,UpdateTime=getdate()
WHERE PalletNo = '{1}' AND BDNo='{2}' AND PNo='{3}'", prn, dr["SN"].ToString(), dr["BDNo"].ToString(), dr["PNo"].ToString());

                        }
                        else
                        {
                            string ss = string.Format(@"SELECT PalletRemainCount FROM dbo.T_Tmp_ApartData WHERE PalletNo='{0}' and OutNo='{1}' and flag=0", dr["SN"].ToString(), dr["OutNo"].ToString());
                            int prn = Convert.ToInt32(comnsql.GetData(Properties.Settings.Default.sql, ss));
                            if (prn != 0)
                            {
                                update += string.Format(@"UPDATE dbo.T_Biz_Container_PalletDtl
SET Number = {0},State=0,UpdateTime=getdate()
WHERE PalletNo = '{1}' AND BDNo='{2}' AND PNo='{3}' and Number={4}", prn, dr["SN"].ToString(), dr["BDNo"].ToString(), dr["PNo"].ToString(), Convert.ToInt32(dr["Number"]));
                            }
                            else
                            {
                                update += string.Format(@"delete from  dbo.T_Biz_Container_PalletDtl
WHERE PalletNo = '{1}' AND BDNo='{2}' AND PNo='{3}' and Number={0}", Convert.ToInt32(dr["Number"]), dr["SN"].ToString(), dr["BDNo"].ToString(), dr["PNo"].ToString());
                            }
                            update += string.Format(@"UPDATE dbo.T_Biz_Container_PalletDtl SET State=0,UpdateTime=getdate()
WHERE PalletNo = '{0}' ", dr["SN"].ToString());
                        }
                        update += string.Format(@"UPDATE dbo.T_Tmp_ApartData  SET flag=1 WHERE PalletNo='{0}' and OutNo='{1}' and flag=0", dr["SN"].ToString(), dr["OutNo"].ToString());
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
                                numberdtl += "," + Convert.ToString(dr1["Number"]);
                            }
                        }
                        string TaskNo1 = GetLoadTaskNo();
                        string insertstring1 = string.Format(@"INSERT INTO dbo.T_Task_Load (Times, State, TaskNo, SN, PlanningNo, IsHengwen, IsBLOCK, TaskType, BDNO, Pn, Weight, Height, SN_OLD, Number, NumberDtl, CreateTime)
VALUES ({8}, 0, '{0}', '{1}', '', {2}, {3}, -1, '{4}', '{5}', 0, 0, '{1}', {6}, '{7}', getdate())", TaskNo1, dr["SN"].ToString(), Convert.ToInt16(dr["IsHengwen"]), Convert.ToInt16(dr["IsQualified"])
                                                                                                  , dr["BDNo"].ToString(), dr["PNo"].ToString(), loadnumber, numberdtl, t + 5);
                        int affect1 = comnsql.DataOperator(Properties.Settings.Default.sql, insertstring1);
                        if (affect1 == 1)
                        {
                            LogWrite.WriteLogToMain(DateTime.Now.ToString() + "插入拆托区至立库的任务成功，任务号：" + TaskNo1 + "，托盘号：" + dr["SN"].ToString() + "\n");
                            comnlog.MessageLog("任务创建", DateTime.Now.ToString() + "插入拆托区至立库的任务成功，任务号：" + TaskNo1 + "，托盘号：" + dr["SN"].ToString());
                            insertLoadTask(t, TaskNo1, dr["SN"].ToString(), Convert.ToBoolean(dr["IsHengwen"]), dr["BDNo"].ToString(), dr["PNo"].ToString(), 0, 0, dr["SN"].ToString());
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
            string ss = string.Format(@"SELECT OutNo,BDNo, PNo,PalletNo,NeedCount,BoxRemainCount,Sign FROM dbo.T_Tmp_ApartData WHERE BoxNo='{0}' and flag=1", SI.SN);
            DataSet ds = comnsql.SelectGet(Properties.Settings.Default.sql, ss);
            if (ds != null&&ds.Tables[0].Rows.Count!=0)
            {
               
                int outNumber = Convert.ToInt32(ds.Tables[0].Rows[0]["NeedCount"]);
                int inNumber = Convert.ToInt32(ds.Tables[0].Rows[0]["BoxRemainCount"]);
                bool[] ispipe = { true, false };
                Random rr = new Random();
                int IsPipeline = Convert.ToInt16(ispipe[rr.Next(0, ispipe.Length)]);
                if (inNumber != 0)
                {
                    string ulTaskNo = GetUnLoadTaskNo();
                    //string lTaskNo = GetLoadTaskNo();
                    string boxNo = GetBox("Pick");
//                    insertstring += string.Format(@"INSERT INTO dbo.T_Task_Load (Times, State, TaskNo, SN, PlanningNo, IsHengwen, IsBLOCK, TaskType, BDNO, Pn, Weight, Height, SN_OLD, Number, NumberDtl, CreateTime)
//VALUES ({9}, 0, '{0}', '{1}', '', {2}, {3}, -1, '{4}', '{5}', 0, 0, '{8}', {6}, '{7}', getdate())"
//                    , lTaskNo, SI.SN, 0, 1, ds.Tables[0].Rows[0]["BDNo"].ToString(), ds.Tables[0].Rows[0]["PNo"].ToString(), inNumber, inNumber.ToString(), SI.SN, t + 5);
                    insertstring += string.Format(@"INSERT INTO dbo.T_Task_UnLoad (Times, State, Sign, TaskNo, TaskType, TaskState, Priority, OutNo, PlanningNo, SN, HubNo, IsUnpackTray, IsBoxLable, IsMinpackLable, IsPipeline, SN_OLD, Number, BusinessType, BDNo, PNo, Station, CreateTime)
VALUES ({8}, 0, '{0}', '{1}', 'OUT', 'NORMAL', 1, '{2}', '', '{3}', '', 1, 0, -1, {4}, '{9}', {5}, '', '{6}', '{7}', '', getdate())"
                  , ds.Tables[0].Rows[0]["Sign"].ToString(), ulTaskNo, ds.Tables[0].Rows[0]["OutNo"].ToString(), boxNo, IsPipeline, outNumber
                  , ds.Tables[0].Rows[0]["BDNo"].ToString(), ds.Tables[0].Rows[0]["PNo"].ToString(), t + 5,SI.SN);
                    insertstring += string.Format(@"DELETE FROM dbo.T_Tmp_ApartData WHERE BoxNo='{0}' ", SI.SN);
                    insertstring += string.Format(@"UPDATE dbo.T_Task_UnLoad_Old SET State = 99,FinishTime=getdate() WHERE TaskNo ='{0}' and State < 99 ", SI.taskNo);
                    insertstring += string.Format(@"UPDATE dbo.T_Tmp_TogetherData SET PalletNo ='',BoxNo = '{0}' WHERE BoxNo = '{1}' ", boxNo, SI.SN);
                    insertstring += string.Format(@"INSERT INTO dbo.T_Biz_Container_BulksDtl (ShelfNo, HubNo, BoxNo, BDNo, PNo, Number, Length, Width, Height, Weight, IsQualified, CreateTime, flag)
VALUES ('', '', '{0}', '{1}', '{2}', {3}, 0, 0, 0, 0, 1, getdate(),2) ", SI.SN, ds.Tables[0].Rows[0]["BDNo"].ToString(), ds.Tables[0].Rows[0]["PNo"].ToString(), inNumber);
                    int affect = comnsql.DataOperator(Properties.Settings.Default.sql, insertstring);
                    if (affect == 5||affect == 4)
                    {
                        LogWrite.WriteLogToMain(DateTime.Now.ToString() + "插入分拣区至拼托区的任务成功，任务号：" + ulTaskNo + "，箱号：" + boxNo + "\n");
                        comnlog.MessageLog("任务创建", DateTime.Now.ToString() + "插入分拣区至拼托区的任务成功，任务号：" + ulTaskNo + "，箱号：" + boxNo);
                        insertUnloadTask(t, ds.Tables[0].Rows[0]["Sign"].ToString(), ulTaskNo, ds.Tables[0].Rows[0]["OutNo"].ToString(), boxNo, "", true, false, false, Convert.ToBoolean(IsPipeline), SI.SN, ds.Tables[0].Rows[0]["BDNo"].ToString(), ds.Tables[0].Rows[0]["PNo"].ToString(), "");
                        //LogWrite.WriteLogToMain(DateTime.Now.ToString() + "插入分拣区至散件库的任务成功，任务号：" + lTaskNo + "，箱号：" + SI.SN);
                        //comnlog.MessageLog("任务创建", DateTime.Now.ToString() + "插入分拣区至散件库的任务成功，任务号：" + lTaskNo + "，箱号：" + SI.SN);
                        //insertLoadTask(t, lTaskNo, SI.SN, false, ds.Tables[0].Rows[0]["BDNo"].ToString(), ds.Tables[0].Rows[0]["PNo"].ToString(), 0, 0, SI.SN);
                        string insertMsg = string.Format(@"UPDATE T_Tmp_Task SET TaskNo = '{1}',
	        Number ={2},
            BoxNo='{3}',
	        State = 4,
	        UpdateTime = getdate() WHERE BoxNo='{0}' and (State=0 or State=3) ", SI.SN, ulTaskNo, outNumber, boxNo);
                        int affect1 = comnsql.DataOperator(Properties.Settings.Default.sql, insertMsg);
                    }
                }
                else if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PalletNo"].ToString()))
                {
                    string insertMsg = string.Format(@"DELETE FROM dbo.T_Tmp_ApartData WHERE BoxNo='{0}'", SI.SN);
                    insertMsg += string.Format(@"UPDATE T_Tmp_Task SET 
	        State = 4,
	        UpdateTime = getdate() WHERE BoxNo='{0}' AND TaskNo='{1}' AND State=3 ", SI.SN, SI.taskNo);
                    insertMsg += string.Format("UPDATE dbo.T_Task_UnLoad_Old SET State = 2,FinishTime=getdate() WHERE TaskNo ='{0}' and State < 99", SI.taskNo);
                    int affect1 = comnsql.DataOperator(Properties.Settings.Default.sql, insertMsg);
                }
                else
                {
                    string ulTaskNo = GetUnLoadTaskNo();
                    insertstring += string.Format(@"INSERT INTO dbo.T_Task_UnLoad (Times, State, Sign, TaskNo, TaskType, TaskState, Priority, OutNo, PlanningNo, SN, HubNo, IsUnpackTray, IsBoxLable, IsMinpackLable, IsPipeline, SN_OLD, Number, BusinessType, BDNo, PNo, Station, CreateTime)
VALUES ({8}, 0, '{0}', '{1}', 'OUT', 'NORMAL', 1, '{2}', '', '{3}', '', 1, 0, -1, {4}, '{3}', {5}, '', '{6}', '{7}', '', getdate())"
                  , ds.Tables[0].Rows[0]["Sign"].ToString(), ulTaskNo, ds.Tables[0].Rows[0]["OutNo"].ToString(), SI.SN, IsPipeline, outNumber
                  , ds.Tables[0].Rows[0]["BDNo"].ToString(), ds.Tables[0].Rows[0]["PNo"].ToString(), t + 5);
                    insertstring += string.Format(@"DELETE FROM dbo.T_Tmp_ApartData WHERE BoxNo='{0}'", SI.SN);
                    insertstring += string.Format(@"UPDATE T_Tmp_Task SET 
	        State = 4,
	        UpdateTime = getdate() WHERE BoxNo='{0}' AND State=0 ", SI.SN);
                    int affect = comnsql.DataOperator(Properties.Settings.Default.sql, insertstring);
                    if (affect == 2)
                    {
                        LogWrite.WriteLogToMain(DateTime.Now.ToString() + "插入分拣区至拼托区的任务成功，任务号：" + ulTaskNo + "，箱号：" + SI.SN + "\n");
                        comnlog.MessageLog("任务创建", DateTime.Now.ToString() + "插入分拣区至拼托区的任务成功，任务号：" + ulTaskNo + "，箱号：" + SI.SN);
                        insertUnloadTask(t, ds.Tables[0].Rows[0]["Sign"].ToString(), ulTaskNo, ds.Tables[0].Rows[0]["OutNo"].ToString(), SI.SN, "", true, false, false, Convert.ToBoolean(IsPipeline), SI.SN, ds.Tables[0].Rows[0]["BDNo"].ToString(), ds.Tables[0].Rows[0]["PNo"].ToString(), "");
                    }
                }
            }
        }

        public static void CreateBufferAreaTask(stationInfo SI, string date, int t)
        {
            DAL_Comn_Sql comnsql = new DAL_Comn_Sql();
            DAL_Comn_Log comnlog = new DAL_Comn_Log();
            string number = GetDeliveryTaskNo();
            string TaskNo = "DTaskNo" + number;
            string taskstring = string.Format("INSERT INTO dbo.T_Task_UnLoad (Times, State, TaskNo, TaskType, SN, CreateTime) VALUES ({0}, 0, '{1}', '{2}', '{3}',getdate())", t + 5, TaskNo, "Load", SI.SN);
            int sNumber = comnsql.DataOperator(Properties.Settings.Default.sql, taskstring);
            if (sNumber == 1)
            {
                LogWrite.WriteLogToMain(DateTime.Now.ToString() + "插入至出口的任务成功，任务号：" + TaskNo + "，托盘号：" + SI.SN + "\n");
                comnlog.MessageLog("任务创建", DateTime.Now.ToString() + "插入至出口的任务成功，任务号：" + TaskNo + "，托盘号：" + SI.SN);
                DeliveryTasks dtask = new DeliveryTasks();
                dtask.Times = t + 5;
                dtask.taskNo = TaskNo;
                dtask.taskType = "Load";
                dtask.SN = new List<string>();
                if (dtask.SN.FindAll(a => a == SI.SN.Replace("\0", "")).Count == 0)
                {
                    dtask.SN.Add(SI.SN.Replace("\0", ""));
                }
                dtask.state = 0;
                if (Form1.sendDeliveryTasksList.Find(a => a.taskNo == dtask.taskNo) == null)
                {
                    Form1.sendDeliveryTasksList.Add(dtask);
                }
            }
        }

        private static void insertLoadTask(int t, string taskNo, string SN, bool isHengWen, string BDNo, string PNo, int weight, int height, string SN_OLD)
        {
            LoadData pclt = new LoadData();
            pclt.times = t + 5;
            pclt.state = 0;
            pclt.taskNo = taskNo;
            pclt.palletOrBoxNo = SN;
            pclt.isHengWen = isHengWen;
            pclt.bdNo = BDNo;
            pclt.pNo = PNo;
            pclt.weight = weight;
            pclt.height = height;
            pclt.palletOrBoxNo_Old = SN_OLD;
            if (Form1.sendLoadTasksList.Find(a => a.taskNo == pclt.taskNo) == null)
            {
                Form1.sendLoadTasksList.Add(pclt);
            }
        }

        private static void insertUnloadTask(int t, string sign, string taskNo, string OutNo, string palletOrBoxNo, string hubno, bool isUnpackTray, bool isBoxLable, bool isMinpackLable, bool isPipeline,
            string SN_OLD, string bdno, string pno, string station)
        {
            UnLoadData ult = new UnLoadData();
            ult.times = t + 5;
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
