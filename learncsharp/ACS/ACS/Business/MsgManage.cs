using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ACS
{
    public class MsgManage
    {
        /// <summary>
        /// 指令解析
        /// </summary>
        /// <param name="socketOpt">自定义Socket对象</param>
        public static void DataTranslate(SocketOpt socketOpt)
        {
            byte[] DataRecv = socketOpt.Data;
            string AgvNo = Encoding.ASCII.GetString(DataRecv, 3, 10).Replace("\0", "");

            //try
            //{
            AgvMsg agvMsg = new AgvMsg();
            int DataLength = DataRecv[2];
            agvMsg.AgvNo = Encoding.ASCII.GetString(DataRecv, 3, 10).Replace("\0", "");

            socketOpt.agv = App.AgvList.FirstOrDefault(a => a.agvNo == agvMsg.AgvNo);
            if (socketOpt.agv == null)
                throw new Exception("更新状态失败：找不到小车号。");

            agvMsg.Barcode = BitConverter.ToUInt32(DataRecv, 13).ToString();
            agvMsg.Voltage = BitConverter.ToInt16(DataRecv, 17) / 100;
            agvMsg.State = DataRecv[19];
            agvMsg.Height = (HeightEnum)DataRecv[20];
            agvMsg.ErrCode1 = DataRecv[21];
            agvMsg.ErrCode2 = DataRecv[22];
            agvMsg.ErrCode3 = DataRecv[23];

            string sid = Encoding.ASCII.GetString(DataRecv, 26, 20).Replace("\0", "");
            if (string.IsNullOrEmpty(sid))
                agvMsg.SID = 0;
            else
                agvMsg.SID = int.Parse(sid);
            agvMsg.sTaskType = (STaskType)DataRecv[46];

            int CrcR = DataRecv[47];
            int CrcC = Commond.CRC(DataRecv, DataLength + 14);
            if (CrcC == CrcR)
            {
                Agv agv = socketOpt.agv;
                UpdateAgvShelfPoint(agvMsg, agv);

                if (agvMsg.SID == 0)
                    SendTask(agvMsg, socketOpt);
                else
                {
                    UpdataPointOri(agvMsg, agv);
                    AgvDoTask(agvMsg, socketOpt);
                }
            }
            //}
            //catch (Exception ex)
            //{
            //    App.ExFile.MessageLog("DataTranslate", ex.Message + "\r"
            //              + Encoding.Default.GetString(DataRecv));
            //}

            socketOpt.Data = SendData.GetRepeatData(AgvNo);
            ManageTcp.Send(socketOpt);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        static void SendTask(AgvMsg agvMsg, SocketOpt socketOpt)
        {
            Agv agv = socketOpt.agv;
            if (agv.state != AgvState.D11 && agv.state != AgvState.D12)
                return;
            if (agv.errorMsg != 0 && agv.errorMsg != 19 && agv.errorMsg != 1900 && agv.errorMsg != 190000)
                return;

            if (agv.sTaskList.Count == 0)
                return;
            STask sTask = agv.sTaskList[0];
            if (sTask.state != TaskState.HavePath)
                return;

            byte SOri, EOri;
            if (sTask.dialDirection == 1)
                SOri = 1;
            else
                SOri = 0;
            EOri = (byte)sTask.dialDirection;
            socketOpt.Data = SendData.GetNewTaskData(agvMsg.AgvNo, sTask.sID, SOri, EOri);
            ManageTcp.Send(socketOpt);
            Task.DoingTask(sTask);
        }

        /// <summary>
        /// 接收小车动作完成操作
        /// </summary>
        static void AgvDoTask(AgvMsg agvMsg, SocketOpt socketOpt)
        {
            Agv agv = socketOpt.agv;
            STask sTask = agv.sTaskList.FirstOrDefault(a => a.sID == agvMsg.SID);
            if (sTask == null) throw new Exception("无法找到子任务！");

            PathPoint pPoint = sTask.pathList.FirstOrDefault(a => a.point.barCode == agvMsg.Barcode);
            switch (agvMsg.sTaskType)
            {
                case STaskType.D1:
                case STaskType.D20:
                case STaskType.D21:
                case STaskType.D24:
                    List<Motion> listMotion = GetNextListMotion(sTask, pPoint, agv);
                    if (listMotion.Count == 0)
                        throw new Exception("找到0条路径");
                    else if (listMotion.Count == 1)
                        socketOpt.Data = SendData.GetRepeatData(agv.agvNo);
                    else
                        socketOpt.Data = SendData.GetNewActionData(agvMsg, listMotion);
                    //除发送给AGV的点外，其它点全部解锁方向
                    NoLock(agv, listMotion);
                    break;
                case STaskType.D2:
                case STaskType.D3:
                case STaskType.D6:
                case STaskType.D15:
                case STaskType.D18:
                case STaskType.D22:
                case STaskType.D23:
                    Task.FinishTask(agv, sTask);
                    socketOpt.Data = SendData.GetFinishData(agv.agvNo);
                    break;
                case STaskType.D25:
                    //if (beginPoint.barCode != endPoint.barCode)
                    throw new Exception("原地旋转，起点和终点必须一致！");
                    break;
                default:
                    throw new Exception("未找到此任务类型");
            }

            ManageTcp.Send(socketOpt);
        }

        /// <summary>
        /// 获取行走后面的点
        /// </summary>
        static List<Motion> GetNextListMotion(STask sTask, PathPoint pathPoint, Agv agv)
        {
            List<Motion> listMotion = new List<Motion>();
            List<PathPoint> listPathPoint = sTask.pathList;

            //移除当前点之前的路径点
            sTask.pathList.RemoveAll(a => a.serialNo < pathPoint.serialNo);

            int i = 0;
            foreach (PathPoint pp in listPathPoint)
            {
                i++;
                if (i == 5)
                    break;

                Point nowPoint = pp.point;

                if (Commond.IsTest && nowPoint.barCode == "2280")
                {
                    listMotion.Add(GetObject.GetMotion(sTask, pp));
                    continue;
                }
                if (nowPoint.lockedAgv == agv)
                {
                    listMotion.Add(GetObject.GetMotion(sTask, pp));
                    continue;
                }

                if (nowPoint.lockedAgv != null)
                {
                    //找到占用此点的小车
                    //如果小车没任务且在低位，则让其回家

                    Agv nextAgv = nowPoint.lockedAgv;

                    if (nextAgv.sTaskList.Count == 0)
                    {
                        //创建一个回家任务
                    }
                    else
                    {
                        List<PathPoint> lpp = nextAgv.sTaskList[0].pathList;
                        if (lpp.Exists(a => a.point.barCode == agv.barcode))
                        {
                            //发送取消任务
                            //agv回复取消成果，则删除路径及更新任务状态
                            //Task.ResetPath(agv);
                        }
                    }

                    break;
                }

                if (IsLoop(nowPoint, new List<Agv>() { agv }))
                    break;

                //如果当前点货架，且小车是顶升   不走
                Shelf shelf = App.ShelfList.FirstOrDefault(a => a.currentBarcode == nowPoint.barCode);
                if (agv.height != HeightEnum.Low && shelf != null)
                    break;

                nowPoint.lockedAgv = agv;
                listMotion.Add(GetObject.GetMotion(sTask, pp));

                if (pp.isCorner)
                    break;
            }

            return listMotion;
        }
     
        /// <summary>
        /// 检查是否有回旋死锁
        /// </summary>
        static bool IsLoop(Point nowPoint, List<Agv> listAgv)
        {
            //找最新的AGV
            Agv lastAgv = listAgv.LastOrDefault();
            PathPoint nextPoint = GetNextPathPoint(lastAgv, nowPoint);
            if (nextPoint != null)
            {
                //获取占用此点的AGV
                Agv agv = nextPoint.point.lockedAgv;
                //此点被另外的AGV占用了
                if (agv != null && agv != lastAgv)
                {
                    listAgv.Add(agv);

                    //如果还没找到第四个点，则继续找
                    if (listAgv.Count != 4)
                        IsLoop(nextPoint.point, listAgv);
                    else
                    {
                        //都不相同时，返回True
                        if (listAgv[0] != listAgv[3] && listAgv[0] != listAgv[4])
                            return true;
                    }
                }
            }
            return false;
        }
        
        /// <summary>
        ///获取下面的路径点
        /// </summary>
        static PathPoint GetNextPathPoint(Agv agv, Point p)
        {
            int i = 0;
            if (agv.sTaskList.Count == 0)
                return null;
            foreach (PathPoint pp in agv.sTaskList[0].pathList)
            {
                if (pp.point.barCode == p.barCode)
                {
                    i = 1000;
                    continue;
                }
                i++;
                if (i == 1001)
                    return pp;
            }
            return null;
        }

        static void NoLock(Agv agv, List<Motion> listMotion)
        {
            //除要发送给AGV的点外，其他被此agv占用的点全部解除
            List<Point> listPoint = App.pointList.Where(a => a.lockedAgv == agv).ToList();
            foreach (Point point in listPoint)
            {
                if (!listMotion.Exists(a => a.barcode == point.barCode))
                    point.lockedAgv = null;
            }
        }
        
        /// <summary>
        /// 更新小车货架信息
        /// </summary>
        static void UpdateAgvShelfPoint(AgvMsg agvMsg, Agv agv)
        {
            string oldBarcode = agv.barcode;
            string newBarcode = agvMsg.Barcode;
            //刚开机，没扫到码，给的是0
            if (newBarcode == "0")
                agvMsg.Barcode = newBarcode = oldBarcode;

            UpdatePoint(oldBarcode, newBarcode, agv);

            ////如果AGV的位置状态不变，不更新Agv状态
            if (newBarcode == oldBarcode
                && agv.height == agvMsg.Height
                && agv.currentCharge == agvMsg.Voltage
                && agv.state == (AgvState)agvMsg.State
                && agv.errorMsg == agvMsg.ErrCode1 + agvMsg.ErrCode2 + agvMsg.ErrCode3)
                return;

            //配送与仓库不同，此判断条件需要考量。   低位/中位/高位的问题
            if (agvMsg.Height == HeightEnum.High)
            {
                Shelf shelf = App.ShelfList.FirstOrDefault(a => a.currentBarcode == oldBarcode);
                if (shelf != null)
                {
                    //货架的当前点要变更为现在的点
                    string shelfSql = string.Format(@"UPDATE T_Base_Shelf SET CurrentBarcode = '{0}' 
                                            WHERE CurrentBarcode = '{1}';", newBarcode, oldBarcode);
                    DbHelperSQL.ExecuteSql(shelfSql);

                    shelf.currentBarcode = newBarcode;
                }
                else
                    throw new Exception("小车顶升是高位，但是无货架点！");
            }

            //更新数据库中小车的状态
            int error = agvMsg.ErrCode1*10000 + agvMsg.ErrCode2*100 + agvMsg.ErrCode3;
            string sql = string.Format(@"UPDATE dbo.T_Base_AGV SET Barcode = '{0}',ErrorMsg = '{1}',
                Height = '{2}',CurrentCharge = '{3}',UpdateTime = GETDATE(),State = '{4}' WHERE AgvNo = '{5}'",
                newBarcode, error, (int)agvMsg.Height, agvMsg.Voltage, agvMsg.State, agvMsg.AgvNo);
            DbHelperSQL.ExecuteSql(sql);

            agv.barcode = newBarcode;
            agv.height = agvMsg.Height;
            agv.currentCharge = agvMsg.Voltage;
            agv.state = (AgvState)agvMsg.State;
            agv.errorMsg = agvMsg.ErrCode1 + agvMsg.ErrCode2 + agvMsg.ErrCode3;
        }

        /// <summary>
        /// 更新点信息
        /// </summary>
        static void UpdatePoint(string oldBarcode,string newBarcode,Agv agv)
        {
            //占用当前点，解除旧点占用
            Point oldPoint = App.pointList.FirstOrDefault(a => a.barCode == oldBarcode);
            Point currentPoint = App.pointList.FirstOrDefault(a => a.barCode == newBarcode);

            //如果当前AGV无任务
            if (agv.sTaskList.Count == 0)
            {
                //如果当前点未被占用
                if (!currentPoint.isOccupy)
                {
                    string sql2 = string.Format("Update dbo.T_Base_Point set IsOccupy = 1,OccupyAgvNo = '{0}' where PointNo = '{1}'", agv.agvNo, currentPoint.barCode);
                    DbHelperSQL.ExecuteSql(sql2);

                    currentPoint.isOccupy = true;
                    currentPoint.occupyAgvNo = agv.agvNo;
                }
            }

            if (oldPoint != currentPoint)
            {
                oldPoint.lockedAgv = null;
                currentPoint.lockedAgv = agv;
            }
        }

        /// <summary>
        /// 更新点方向锁信息
        /// </summary>
        static void UpdataPointOri(AgvMsg agvMsg, Agv agv)
        {
            //找到小车当前的任务
            STask sTask = agv.sTaskList.FirstOrDefault(a => a.sID == agvMsg.SID);
            if (sTask == null)
                return;
            
            PathPoint pathCurrent = sTask.pathList.FirstOrDefault(a => a.point.barCode == agv.barcode);
            if (pathCurrent == null)
                throw new Exception("AGV当前点不在路径中！" + agv.agvNo);

            //查找所有被当前小车锁定的点
            List<Point> listPoint = App.pointList.Where(a => a.listTmpDirection.Exists(b => b.agvNo == agv.agvNo)).ToList();

            foreach (Point point in listPoint)
            {
                //如果当前点不在小车的路径中
                //如果当前点是小车所在的点,当前点不能解除方向锁。解除会导致对面的车冲突
                //则清理当前点的路径锁
                
                if (!sTask.pathList.Exists(a => a.point == point))
                    point.listTmpDirection.RemoveAll(a => a.agvNo == agv.agvNo);
            }
        }
    }
}
