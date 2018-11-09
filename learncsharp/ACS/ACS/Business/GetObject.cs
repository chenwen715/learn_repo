using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ACS
{
    /// <summary>
    /// 获取对象
    /// </summary>
    public class GetObject
    {
        public static Shelf GetShelf(DataRow dr)
        {
            Shelf s = new Shelf();
            s.areaNo = dr["areaNo"].ToString();
            s.barcode = dr["barcode"].ToString();
            s.currentBarcode = dr["currentBarcode"].ToString();
            s.isEnable = bool.Parse(dr["isEnable"].ToString());
            s.isLocked = bool.Parse(dr["isLocked"].ToString());
            s.shelfDirection = dr["shelfDirection"].ToString();
            s.shelfNo = dr["shelfNo"].ToString();

            return s;
        }

        public static Agv GetAgv(DataRow dr)
        {
            Agv a = new Agv();
            a.agvNo = dr["agvNo"].ToString();
            a.barcode = dr["barcode"].ToString();
            a.currentCharge = float.Parse(dr["currentCharge"].ToString());
            a.errorMsg = int.Parse(dr["errorMsg"].ToString());
            a.height = (HeightEnum)int.Parse(dr["height"].ToString());
            a.isEnable = bool.Parse(dr["isEnable"].ToString());
            a.sTaskList = new List<STask>();
            a.state = (AgvState)int.Parse(dr["state"].ToString());
            a.areaNo = dr["areaNo"].ToString();

            return a;
        }

        public static Point GetPoint(DataRow dr)
        {
            Point p = new Point();
            p.isXPos = bool.Parse(dr["isXPos"].ToString());
            p.isYPos = bool.Parse(dr["isYPos"].ToString());
            p.isYNeg = bool.Parse(dr["isYNeg"].ToString());
            p.isXNeg = bool.Parse(dr["isXNeg"].ToString());
            p.listTmpDirection = new List<TmpDirection>();
            p.areaNo = dr["AreaNo"].ToString();
            p.barCode = dr["Barcode"].ToString();
            p.isEnable = bool.Parse(dr["isEnable"].ToString());
            p.isOccupy = bool.Parse(dr["isOccupy"].ToString());
            p.lockedAgv = null;
            p.occupyAgvNo = dr["occupyAgvNo"].ToString();
            p.pointType = (PointType)int.Parse(dr["PointType"].ToString());
            p.x = int.Parse(dr["X"].ToString());
            p.y = int.Parse(dr["Y"].ToString());
            p.xLength = int.Parse(dr["xLength"].ToString());
            p.yLength = int.Parse(dr["yLength"].ToString());

            return p;
        }

        public static STask GetSTask(DataRow dr)
        {
            STask sTask = new STask();
            sTask.serialNo = int.Parse(dr["serialNo"].ToString()); ;
            sTask.sID = int.Parse(dr["SID"].ToString());
            sTask.taskNo = dr["TaskNo"].ToString();
            sTask.sTaskType = (STaskType)int.Parse(dr["ItemName"].ToString());
            sTask.agv = App.AgvList.FirstOrDefault(a => a.agvNo == dr["AgvNo"].ToString()); 
            sTask.beginPoint = App.pointList.FirstOrDefault(a => a.barCode == dr["BeginPoint"].ToString());
            sTask.endPoint = App.pointList.FirstOrDefault(a => a.barCode == dr["EndPoint"].ToString());
            sTask.dialDirection = int.Parse(dr["DialDirection"].ToString());
            sTask.agvDirection = int.Parse(dr["AgvDirection"].ToString());
            sTask.state = (TaskState)(int.Parse(dr["State"].ToString()));
            sTask.pathList = new List<PathPoint>();
            sTask.agv.sTaskList.Add(sTask);
            return sTask;
        }

        public static Motion GetMotion(STask st, PathPoint ppCurrent)
        {
            Motion motion = new Motion();
            motion.sTaskType = st.sTaskType;
            if (st.sTaskType == STaskType.D1)
            {
                //如果是子任务的终点
                if (ppCurrent.serialNo == st.pathList[st.pathList.Count - 1].serialNo)
                    motion.sTaskType = STaskType.D18;
                //如果是拐点
                if (ppCurrent.isCorner)
                    motion.sTaskType = STaskType.D20;
            }
            
            motion.barcode = ppCurrent.point.barCode;
            motion.x = ppCurrent.point.x;
            motion.y = ppCurrent.point.y;
            motion.xLength = ppCurrent.point.xLength;
            motion.yLength = ppCurrent.point.xLength;
            motion.pointType = (int)ppCurrent.point.pointType;
            return motion;
        }

        public static PathPoint GetPathPoint(Point point)
        {
            PathPoint pp = new PathPoint();
            pp.point = point;
            return pp;
        }
    }
}
