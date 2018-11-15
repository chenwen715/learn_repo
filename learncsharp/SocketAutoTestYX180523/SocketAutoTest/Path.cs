using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketAutoTest
{
    class Path
    {
      
        List<Point> pList = new List<Point> { };
        DataSet ds = new DataSet();
        public List<Point> initPoint()
        { 
            DAL_Comn_Sql DalSql=new DAL_Comn_Sql();
            string sql = string.Format(@"SELECT * FROM dbo.T_PathPoint");
            ds = DalSql.SelectGet(Properties.Settings.Default.Sql,sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Point p = new Point();
                p.mapX = int.Parse(dr["PointX"].ToString());
                p.mapY = int.Parse(dr["PointY"].ToString());
                p.currentX = int.Parse(dr["x"].ToString());
                p.currentY = int.Parse(dr["y"].ToString());
                p.pointDescription = int.Parse(dr["pointType"].ToString());
                if (!string.IsNullOrEmpty(dr["intStationCode"].ToString()))
                {
                    p.intStationCode = int.Parse(dr["intStationCode"].ToString());
                }
                else
                {
                    p.intStationCode = 0;
                }
                p.wsType = int.Parse(dr["wsType"].ToString());
                if (!string.IsNullOrEmpty(dr["beforex"].ToString()))
                {
                    p.beforex = int.Parse(dr["beforex"].ToString());
                }
                else
                {
                    p.beforex = 0;
                }
                if (!string.IsNullOrEmpty(dr["beforey"].ToString()))
                {
                    p.beforey = int.Parse(dr["beforey"].ToString());
                }
                else
                {
                    p.beforey = 0;
                }
                p.isOccuppy = bool.Parse(dr["isOccupy"].ToString());
                p.occupyAgv = dr["occupyAgv"].ToString();

                pList.Add(p);
            }
            return pList;
        }
        public List<PathPoint> findPath(List<Point> pointList,string sid,int destinationNo,int taskType,int currentX,int currentY)
        {
            List<PathPoint> pp = new List<PathPoint> { };
            try
            {
                int routeNo = 0;
                int flag = 0;
                int count = 0;
                while (flag == 0)
                {

                    foreach (Point p in pointList)
                    {
                        PathPoint pathPoint = new PathPoint();
                        count++;
                        if (p.currentX == currentX && p.currentY == currentY)
                        {
                            
                            routeNo++;
                            //routeNo = AddPoint(pp, p, routeNo, sid);
                            pathPoint.routeNo = routeNo;
                            pathPoint.sid = sid;
                            pathPoint.point = p;
                            pp.Add(pathPoint);
                            if (p.intStationCode == destinationNo && p.pointDescription != 1 && p.pointDescription != 5
                                &&(((taskType == 2 || taskType == 1) && taskType == p.wsType) || (taskType != 1 && taskType != 2) || p.pointDescription != 2))
                            {
                                flag = 1;
                                break;
                            }
                            if (p.beforex != 0 && p.beforey != 0)
                            {
                                currentX = p.beforex;
                                currentY = p.beforey;
                            }
                            else if (currentX == 1 && currentY == 2 && taskType != 7)
                            {
                                currentX = 3;
                                currentY = 1;
                            }
                            else if (currentX == 1 && currentY == 2 && taskType == 7)
                            {
                                currentX = 1;
                                currentY = 1;
                            }
                            else if (currentX == 1 && currentY == 1 && taskType == 7&&destinationNo==155)
                            {
                                currentX = 2;
                                currentY = 1;
                            }
                            else if (currentX == 1 && currentY == 1 && taskType == 7 && destinationNo != 155)
                            {
                                currentX = 1;
                                currentY = 14;
                            }
                            else if (currentX == 1 && currentY == 14 && taskType == 7 && destinationNo == 165)
                            {
                                currentX = 2;
                                currentY =14;
                            }
                            else if (currentX == 1 && currentY == 14 && taskType == 7 && destinationNo != 165)
                            {
                                currentX = 1;
                                currentY = 13;
                            }
                            else if (currentX == 1 && currentY == 13 && taskType == 7 && destinationNo == 175)
                            {
                                currentX = 2;
                                currentY = 13;
                            }
                            else if (currentX == 1 && currentY == 13 && taskType == 7 && destinationNo != 175)
                            {
                                currentX = 1;
                                currentY = 12;
                            }
                            else if (currentX == 1 && currentY == 12 && taskType == 7 && destinationNo == 185)
                            {
                                currentX = 2;
                                currentY = 12;
                            }
                            else if (currentX == 1 && currentY == 12 && taskType == 7 && destinationNo != 185)
                            {
                                currentX = 1;
                                currentY = 11;
                            }
                            else if (currentX == 1 && currentY == 11 && taskType == 7 && destinationNo == 195)
                            {
                                currentX = 2;
                                currentY = 11;
                            }
                            else if (currentX == 1 && currentY == 11 && taskType == 7 && destinationNo != 195)
                            {
                                currentX = 20;
                                currentY = 5;
                            }
                            else if (currentX == 4 && currentY == 2 && (taskType == 7 || destinationNo == 45 || destinationNo == 46))
                            {
                                currentX = 3;
                                currentY = 3;
                            }
                            else if (currentX == 4 && currentY == 2 && (!(taskType == 7 || destinationNo == 45 || destinationNo == 46)))
                            {
                                currentX = 5;
                                currentY = 2;
                            }
                            else if (currentX == 40 && currentY == 2 && (taskType == 7 || destinationNo == 45 || destinationNo == 46))
                            {
                                currentX = 4;
                                currentY = 3;
                            }
                            else if (currentX == 40 && currentY == 2 && (!(taskType == 7 || destinationNo == 45 || destinationNo == 46)))
                            {
                                currentX = 11;
                                currentY = 2;
                            }
                            else if (currentX == 41 && currentY == 2 && (taskType == 7 || destinationNo == 45 || destinationNo == 46 || destinationNo >54))
                            {
                                currentX = 5;
                                currentY = 3;
                            }
                            else if (currentX == 41 && currentY == 2 && (!(taskType == 7 || destinationNo == 45 || destinationNo == 46 || destinationNo > 54)))
                            {
                                currentX = 19;
                                currentY = 2;
                            }
                            else if (currentX == 42 && currentY == 2 && (taskType == 7 || destinationNo == 45 || destinationNo == 46 || destinationNo > 50))
                            {
                                currentX = 6;
                                currentY = 3;
                            }
                            else if (currentX == 42 && currentY == 2 && (!(taskType == 7 || destinationNo == 45 || destinationNo == 46 || destinationNo > 50)))
                            {
                                currentX = 43;
                                currentY = 2;
                            }
                            else if (currentX == 43 && currentY == 2 && (taskType == 7 || destinationNo == 45 || destinationNo == 46 || destinationNo > 50))
                            {
                                currentX = 7;
                                currentY = 3;
                            }
                            else if (currentX == 43 && currentY == 2 && (!(taskType == 7 || destinationNo == 45 || destinationNo == 46 || destinationNo > 50)))
                            {
                                currentX = 23;
                                currentY = 2;
                            }
                            else if (currentX == 44 && currentY == 2 && (taskType == 7 || destinationNo == 45 || destinationNo == 46 || destinationNo > 10))
                            {
                                currentX = 8;
                                currentY = 3;
                            }
                            else if (currentX == 44 && currentY == 2 && (!(taskType == 7 || destinationNo == 45 || destinationNo == 46 || destinationNo > 10)))
                            {
                                currentX = 27;
                                currentY = 2;
                            }
                            break;
                        }
                       
                    }
                    //if(count==tempList.Count)
                    //{
                    //    throw new Exception("(" + currentX + "," + currentY + ") 不存在该点");
                    //}
                }


                return pp;
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            
        }

        private int AddPoint(List<PathPoint> pp, Point p,int n,string sid)
        {
            if (pp.Count != 0)
            {
                int lastPointX=pp[pp.Count - 1].point.mapX;
                int lastPointY=pp[pp.Count - 1].point.mapY;
                int PointX = p.mapX;
                int PointY = p.mapY;
                if (lastPointX == PointX && Math.Abs(PointY - lastPointY) > 1)
                {
                    int a = Math.Abs(PointY - lastPointY);
                    if (PointY < lastPointY)
                    {
                        for (int i = 1; i < a; i++)
                        {
                            PathPoint pathPoint = new PathPoint();
                            Point addPoint = new Point();
                            addPoint.mapX = lastPointX;
                            addPoint.mapY = lastPointY - i;
                            addPoint.currentX = lastPointX;
                            addPoint.currentY = lastPointY - i;
                            addPoint.pointDescription = 0;
                            addPoint.wsType = 0;
                            addPoint.intStationCode = 0;
                            pathPoint.routeNo = n;
                            pathPoint.sid = sid;
                            pathPoint.point = addPoint;
                            pp.Add(pathPoint);
                            n++;
                        }
                    }
                    else if (PointY > lastPointY)
                    {
                        for (int i = 1; i < a; i++)
                        {
                            PathPoint pathPoint = new PathPoint();
                            Point addPoint = new Point();
                            addPoint.mapX = lastPointX;
                            addPoint.mapY = lastPointY + i;
                            addPoint.currentX = lastPointX;
                            addPoint.currentY = lastPointY + i;
                            addPoint.pointDescription = 0;
                            addPoint.wsType = 0;
                            addPoint.intStationCode = 0;
                            pathPoint.routeNo = n;
                            pathPoint.sid = sid;
                            pathPoint.point = addPoint;
                            pp.Add(pathPoint);
                            n++;
                        }
                    }
                }
                else if (lastPointY == PointY && Math.Abs(PointX - lastPointX) > 1)
                {
                    int a = Math.Abs(PointX - lastPointX);
                    if (PointX< lastPointX)
                    {
                        for (int i = 1; i < a; i++)
                        {
                            PathPoint pathPoint = new PathPoint();
                            Point addPoint = new Point();
                            addPoint.mapX = lastPointX-i;
                            addPoint.mapY = lastPointY ;
                            addPoint.currentX = lastPointX-i;
                            addPoint.currentY = lastPointY ;
                            addPoint.pointDescription = 0;
                            addPoint.wsType = 0;
                            addPoint.intStationCode = 0;
                            pathPoint.routeNo = n;
                            pathPoint.sid = sid;
                            pathPoint.point = addPoint;
                            pp.Add(pathPoint);
                            n++;
                        }
                    }
                    else if (PointX > lastPointX)
                    {
                        for (int i = 1; i < a; i++)
                        {
                            PathPoint pathPoint = new PathPoint();
                            Point addPoint = new Point();
                            addPoint.mapX = lastPointX+i;
                            addPoint.mapY = lastPointY ;
                            addPoint.currentX = lastPointX+i;
                            addPoint.currentY = lastPointY;
                            addPoint.pointDescription = 0;
                            addPoint.wsType = 0;
                            addPoint.intStationCode = 0;
                            pathPoint.routeNo = n;
                            pathPoint.sid = sid;
                            pathPoint.point = addPoint;
                            pp.Add(pathPoint);
                            n++;
                        }
                    }
                }
                else if (lastPointY != PointY && lastPointX != PointX)
                {

                }
            }
            return n;
        }
    }
}
