using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ACS
{
    /// <summary>
    /// 与任务处理交互部分
    /// </summary>
    public class Task
    {
        public static void DoWork()
        {
            //ClearPointDirection();
            ClearPointOccupy();

            Charge();
            TaskManage();
            DownTask();
            DeleteTask();
            AssignePath();
        }

        /// <summary>
        /// 清理无任务小车锁定的方向信息
        /// </summary>
        static void ClearPointDirection()
        {
            //如果被Agv锁的点，不在AGV的路径中，则移除。
            
            List<AgvPoint> listAgvPoint = new List<AgvPoint>();
            foreach (Agv agv in App.AgvList)
            {
                if (agv.sTaskList.Count != 0)
                {
                    foreach (PathPoint pp in agv.sTaskList[0].pathList)
                    {
                        listAgvPoint.Add(new AgvPoint(agv.agvNo, pp.point));
                    }
                }
            }

            List<Point> listPointDirectionLock = App.pointList.Where(
                a => a.listTmpDirection.Count != 0).ToList();

            foreach (Point point in listPointDirectionLock)
            {
                for (int i = 0; i < point.listTmpDirection.Count; i++)
                {
                    TmpDirection td = point.listTmpDirection[i];
                    
                    bool isExists = listAgvPoint.Exists(a => a.agv == td.agvNo && a.point == point);
                    if (!isExists)
                        point.listTmpDirection.RemoveAll(a => a.agvNo == td.agvNo);
                }
            }
        }

        /// <summary>
        /// 清理点的占用
        /// </summary>
        static void ClearPointOccupy()
        {
            try
            {
                //统一清理点占用（不是AGV当前点，也不是任务终点）（内存/数据库）
                //查找忙碌小车
                List<string> listBarcode = new List<string>();
                foreach (Agv agv in App.AgvList)
                {
                    listBarcode.Add(agv.barcode);

                    if (agv.sTaskList.Count != 0)
                    {
                        //agv.sTaskList.Reverse();
                        listBarcode.Add(agv.sTaskList[0].endPoint.barCode);
                    }
                }

                List<Point> listPoint = App.pointList.Where(a => a.isOccupy).ToList();
                foreach (Point point in listPoint)
                {
                    if (!listBarcode.Exists(a => a == point.barCode))
                    {
                        string sql = string.Format("UPDATE dbo.T_Base_Point SET IsOccupy = 0,OccupyAgvNo = '' WHERE BarCode = '{0}';", point.barCode);
                        DbHelperSQL.ExecuteSql(sql);

                        point.isOccupy = false;
                        point.occupyAgvNo = "";
                    }
                }
            }
            catch (Exception ex)
            {
                App.ExFile.MessageLog("ClearPointOccupy", ex.Message + "\r");
            }
        }

        static void Charge()
        {
            //try
            //{
                //DbHelperSQL.RunProc("P_Charge", null);
            //}
            //catch (Exception ex)
            //{
            //    App.ExFile.MessageLog("P_Charge", ex.Message + "\r");
            //}
        }
        
        /// <summary>
        /// 处理任务
        /// </summary>
        static void TaskManage()
        {
            try
            {
                DbHelperSQL.RunProc("P_Task", null);
            }
            catch (Exception ex)
            {
                App.ExFile.MessageLog("P_Task", ex.Message + "\r");
            }
        }

        /// <summary>
        /// 下载任务
        /// </summary>
        static void DownTask()
        {
            try
            {
                string sql = @"SELECT ts.*,c.ItemName  FROM dbo.T_Task_Son ts
                                    LEFT JOIN dbo.T_Type_Config c ON ts.STaskType = c.ItemNo
                                    WHERE State = 0 ORDER BY AgvNo, SerialNo";
                DataSet ds = DbHelperSQL.Query(sql);

                DataRowCollection drc = ds.Tables[0].Rows;
                foreach (DataRow dr in drc)
                {
                    STask s = GetObject.GetSTask(dr);

                    string sql2 = string.Format("UPDATE dbo.T_Task_Son SET State = 1 WHERE SID = '{0}'", s.sID);
                    DbHelperSQL.ExecuteSql(sql2);
                    s.state = TaskState.Down;
                }
            }
            catch (Exception ex)
            {
                App.ExFile.MessageLog("DownTask", ex.Message + "\r");
            }
        }
        
        /// <summary>
        /// 删除任务
        /// </summary>
        static void DeleteTask()
        {
            //从数据库中查询到待删除的任务
            //按照任务号和AGV编号，删除内存任务信息
            //删除内存路径锁
            //删除内存路径
            //删除内存点占用
            //删除内存点的锁定
            //更新数据库
            try
            {
                string sql = string.Format("SELECT * FROM dbo.T_Task_Delete WHERE State = 0");
                DataSet ds = DbHelperSQL.Query(sql);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string taskNo = dr["TaskNo"].ToString();
                    string agvNo = dr["AgvNo"].ToString();

                    Agv agv = App.AgvList.FirstOrDefault(a => a.agvNo == agvNo);
                    agv.sTaskList.RemoveAll(a => a.taskNo == taskNo);


                }
            }
            catch (Exception ex)
            {
                App.ExFile.MessageLog("DeleteTask", ex.Message + "\r");
            }
        }

        /// <summary>
        /// 分配路径
        /// </summary>
        static void AssignePath()
        {
            //try
            //{
            //遍历所有AGV，找agv中的第一条任务
            //如果状态是已下载，则分配路径
            //如果分配成功，则更新数据库任务状态，插入路径
            //如果更新成功，则更新内存已分配路径,更新任务状态

            foreach (Agv agv in App.AgvList)
            {
                List<STask> listSTask = agv.sTaskList;
                if (listSTask.Count == 0)
                    continue;
                STask sTask = listSTask[0];
                if (sTask.state != TaskState.Down)
                    continue;

                string sql = "";
                List<PathPoint> listPathPoint = new List<PathPoint>();
                //如果起点和终点一致，且是行走，则直接结束任务
                if (sTask.beginPoint == sTask.endPoint && sTask.sTaskType == STaskType.D1)
                {
                    FinishTask(agv, sTask);
                    continue;
                }

                //因未实现路径重建，临时使用此方式来避免 将任务创建在别的小车路径上
                //如果不是最后一个任务且是行走，如果终点被锁，则先不查找路径
                if (listSTask.Count > 1 && sTask.sTaskType == STaskType.D1)
                    if (sTask.endPoint.listTmpDirection.Count != 0)
                        continue;

                if(sTask.sTaskType == STaskType.D25)
                {
                    FinishTask(agv, sTask);
                    continue;
                }


                listPathPoint = PathGet.GetPath(agv);
                if (listPathPoint == null)
                    continue;

                int serialNo = 1;
                foreach (PathPoint pp in listPathPoint)
                {
                    //待修改，需将点的方向也丢到路径中
                    sql += string.Format("INSERT INTO T_Base_PathList (SID,Barcode,serialNo,direction) VALUES('{0}','{1}','{2}','{3}');", agv.sTaskList[0].sID, pp.point.barCode, serialNo, 0);
                    serialNo++;
                    
                }

                sql += string.Format("UPDATE dbo.T_Task_Son SET State = 2  WHERE SID = '{0}'", sTask.sID);
                DbHelperSQL.ExecuteSql(sql);

                sTask.pathList = listPathPoint;
                sTask.state = TaskState.HavePath;
            }
            //}
            //catch (Exception ex)
            //{
            //    App.ExFile.MessageLog("AssignePath", ex.Message + "\r");
            //}
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="sTask"></param>
        public static void DoingTask(STask sTask)
        {
            //更新数据库发送的子任务状态
            string sql = string.Format(@"UPDATE dbo.T_Task_Son SET State = '3' WHERE SID = '{0}'", sTask.sID);
            DbHelperSQL.ExecuteSql(sql);

            sTask.state = TaskState.Doing;
        }

        /// <summary>
        /// 完成任务
        /// </summary>
        public static void FinishTask(Agv agv, STask sTask)
        {
            SqlParameter[] para = new SqlParameter[1];
            para[0] = new SqlParameter("SID", SqlDbType.Int);
            para[0].Value = sTask.sID;
            DbHelperSQL.RunProc("P_Task_Finish", para);

            agv.sTaskList.Remove(sTask);
        }

        public static void ResetPath(Agv agv)
        {
            //删除当前路径
            //任务状态变为1

            STask sTask = agv.sTaskList[0];
            string sql = string.Format("DELETE FROM dbo.T_Base_PathList WHERE SID = '{0}';", sTask.sID);
            sql += string.Format("UPDATE dbo.T_Task_Son SET State = 1 WHERE SID = '{0}' ;", sTask.sID);
            DbHelperSQL.ExecuteSql(sql);

            sTask.pathList.Clear();
            sTask.state = TaskState.Down;
        }
    }
}
