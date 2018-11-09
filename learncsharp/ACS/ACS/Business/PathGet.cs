using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS
{
    public class PathGet
    {
        /// <summary>
        /// 生成路径
        /// </summary>
        /// <returns>路径集合</returns>
        public static List<PathPoint> GetPath(Agv agv)
        {
            STask sTask = agv.sTaskList[0];
            Point beginPoint = sTask.beginPoint;
            Point endPoint = sTask.endPoint;
            STaskType sTaskType = sTask.sTaskType;
            
            switch (sTask.sTaskType)
            {
                case STaskType.D1:
                    if (beginPoint == endPoint)
                        return new List<PathPoint>() { GetObject.GetPathPoint(beginPoint), GetObject.GetPathPoint(endPoint) };
                    
                    AStar aStart = new AStar();
                    List<Point> listApoint = GetCanUseApoint(agv);
                    ClearParentPoint();
                    Point Parent = aStart.PathGet(listApoint, beginPoint, endPoint, agv);
                    if (Parent == null)
                        return null;

                    List<PathPoint> listPathPoint = GetPathList(Parent);
                    AddCorner(listPathPoint);
                    OriLock(listPathPoint, agv);

                    return listPathPoint;
                case STaskType.D2:
                case STaskType.D3:
                case STaskType.D6:
                case STaskType.D15:
                    return new List<PathPoint>() { GetObject.GetPathPoint(beginPoint), GetObject.GetPathPoint(endPoint) };
                case STaskType.D13:
                case STaskType.D14:
                    //左弧右弧进
                    break;
                case STaskType.D17:
                case STaskType.D19:
                    //左弧右弧出
                    break;
                default:
                    throw new Exception("无此任务类型:" + sTask.sTaskType);
            }

            return null;
        }

        /// <summary>
        /// 清所有点
        /// </summary>
        static void ClearParentPoint()
        {
            foreach (Point point in App.pointList)
            {
                if (point.ParentPoint != null)
                    point.ParentPoint = null;
            }
        }

        /// <summary>
        ///获取可用的路径点
        /// </summary>
        static List<Point> GetCanUseApoint(Agv agv)
        {
            STask sTask = agv.sTaskList[0];
            Point beginPoint = sTask.beginPoint;
            Point endPoint = sTask.endPoint;

            //获取可用点
            List<Point> listApoint = new List<Point>();
            foreach (Point point in App.pointList)
            {
                if (point != beginPoint && point != endPoint)
                {
                    //如果是货架点
                    if (point.pointType == PointType.D2)
                    {
                        //如果是顶升，则禁止使用货架点
                        if (agv.height == HeightEnum.High)
                            continue;

                        //如果是其他任务的起点/终点，则禁止通过
                        bool isTask = App.AgvList.Exists(a =>
                                            a.sTaskList.Exists(b =>
                                            b.beginPoint == point || b.endPoint == point));
                        if (isTask)
                            continue;
                    }

                    //以下点类型不使用
                    if (point.pointType == PointType.D4
                    || point.pointType == PointType.D6
                    || point.pointType == PointType.D8 
                    || point.pointType == PointType.D9
                    || point.pointType == PointType.D10
                    || point.pointType == PointType.D11
                    || point.pointType == PointType.D14
                    || point.pointType == PointType.D15
                    )
                        continue;

                    //被占用且不是终点，不使用
                    if (point.isOccupy)
                        continue;

                    //异常车上的点不申请
                    if (point.lockedAgv != null && point.lockedAgv.errorMsg != 0)
                        continue;
                    
                    //不包含agv的区域，则不选择
                    if (!point.areaNo.Contains(agv.areaNo))
                        continue;
                }
                
                listApoint.Add(point);
            }

            return listApoint;
        }

        static List<PathPoint> GetPathList(Point Parent)
        {
            List<PathPoint> listPathPoint = new List<PathPoint>();

            while (Parent != null)
            {
                listPathPoint.Add(GetObject.GetPathPoint(Parent));
                Parent = Parent.ParentPoint;
            }
            
            listPathPoint.Reverse();
            for (int i = 0; i < listPathPoint.Count; i++)
            {
                listPathPoint[i].serialNo = i + 1;
            }

            return listPathPoint;
        }

        /// <summary>
        /// 赋予拐点属性
        /// </summary>
        static void AddCorner(List<PathPoint> Path)
        {
            for (int i = 1; i < Path.Count; i++)
            {
                if (i + 1 < Path.Count)
                {
                    //三点在一条线上
                    if ((Path[i - 1].point.x == Path[i].point.x && Path[i].point.x == Path[i + 1].point.x)
                        || (Path[i - 1].point.y == Path[i].point.y && Path[i].point.y == Path[i + 1].point.y))
                    {
                        Path[i].isCorner = false;
                    }
                    else
                    {
                        //如果是旋转点，则不是拐点
                        if (Path[i].point.pointType == PointType.D10)
                            Path[i].isCorner = false;
                        else
                            Path[i].isCorner = true;
                    }
                }
            }
        }

        /// <summary>
        /// 锁定路径方向
        /// </summary>
        /// <param name="Path"></param>
        public static void OriLock(List<PathPoint> Path, Agv agv)
        {
            //终点不比较
            for (int i = 0; i < Path.Count - 1; i++)
            {
                Point point = Path[i].point;
                Point nextPoint = Path[i + 1].point;
                if (point == nextPoint)
                    continue;

                AStar aStart = new AStar();
                int direction = aStart.GetDircition(nextPoint, point);

                TmpDirection td = new TmpDirection();
                td.direction = direction;
                td.agvNo = agv.agvNo;
                point.listTmpDirection.Add(td);
            }
        }
    }
}
