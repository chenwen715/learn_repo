using System;
using System.Collections.Generic;

namespace ACS
{
    public class AStar
    {
        List<Point> OpenList = new List<Point>();
        List<Point> CloseList = new List<Point>();
        List<Point> SurPointsList = new List<Point>();
        

        public const int CONER = 8;
        public const int LINE = 4;
        public const int TOEND = 4;

        /// <summary>
        /// 寻找路径
        /// </summary>
        public Point PathGet(List<Point> listApoint, Point beginPoint, Point endPoint, Agv agv)
        {
            if (beginPoint == null || endPoint == null)
                throw new Exception(" 查找路径失败，起点终点不能为空");
            
            OpenList.Add(beginPoint);

            int x = 0;
            while (OpenList.Count != 0)
            {
                x++;                        
                //按照F值从小到大排序
                //将此点从Open表剔除，并加入Close表中
                OpenList.Sort();
                Point currentPoint = OpenList[0];
                OpenList.RemoveAt(0);
                CloseList.Add(currentPoint);

                //找出它相邻的点
                List<Point> aroundPoints = GetAroundPoint(listApoint, currentPoint);
                if (agv.agvNo == Commond.TestAgvNo)
                {
                    string sql1 = string.Format(@"UPDATE dbo.T_Base_Point SET PathNum = '{0}',G={2},H={3} WHERE BarCode = '{1}'", x, currentPoint.barCode,currentPoint.Value_G,currentPoint.Value_H);
                    DbHelperSQL.ExecuteSql(sql1);
                    System.Threading.Thread.Sleep(50);
                }
                
                foreach (Point judgePoint in aroundPoints)
                {
                    if (OpenList.Contains(judgePoint))
                    {
                        var G = currentPoint.Value_G + ConditionG(judgePoint, currentPoint);
                        if (G < judgePoint.Value_G)
                        {
                            judgePoint.ParentPoint = currentPoint;
                            judgePoint.Value_G = G;
                            judgePoint.Value_H = CalcH(judgePoint, endPoint);
                            judgePoint.Value_F = judgePoint.Value_G + judgePoint.Value_H;
                        }
                    }
                    else
                    {
                        //如果它们不在开始列表里, 就加入, 并设置父节点,并计算GHF
                        judgePoint.ParentPoint = currentPoint;
                        judgePoint.Value_G = currentPoint.Value_G + ConditionG(judgePoint, currentPoint);
                        judgePoint.Value_H = CalcH(judgePoint, endPoint);
                        judgePoint.Value_F = judgePoint.Value_G + judgePoint.Value_H;
                        OpenList.Add(judgePoint);

                        if (judgePoint.barCode == endPoint.barCode)
                            //return null;
                            return judgePoint;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 寻找四周点
        /// </summary>
        List<Point> GetAroundPoint(List<Point> listApoint, Point currentPoint)
        {
            List<Point> listPoint = new List<Point>();

            Point xPos = currentPoint.xPosPoint;
            if (listApoint.Contains(xPos))
                if (CanReach(xPos,currentPoint, 1))
                    listPoint.Add(xPos);

            Point xNeg = currentPoint.xNegPoint;
            if (listApoint.Contains(xNeg))
                if (CanReach(xNeg, currentPoint, 2))
                    listPoint.Add(xNeg);

            Point yPos = currentPoint.yPosPoint;
            if (listApoint.Contains(yPos))
                if (CanReach(yPos, currentPoint, 3))
                    listPoint.Add(yPos);

            Point yNeg = currentPoint.yNegPoint;
            if (listApoint.Contains(yNeg))
                if (CanReach(yNeg, currentPoint, 4))
                    listPoint.Add(yNeg);

            return listPoint;
        }

        /// <summary>
        /// 是否能选择此点
        /// </summary>
        bool CanReach(Point judgePoint, Point currentPoint, int flag)
        {
            if (judgePoint == null)
                return false;

            //关掉的点，排除
            if (CloseList.Exists(a => a.barCode == judgePoint.barCode))
                return false;
            
            switch (flag)
            {
                case 1:
                    //临时方向，如果X-允许走，则不准走
                    if (judgePoint.listTmpDirection.Exists(a => a.direction == 2))
                        return false;

                    //固定方向，如果不能往X+
                    if (!currentPoint.isXPos)
                        return false;
                    break;
                case 2:
                    if (judgePoint.listTmpDirection.Exists(a => a.direction == 1))
                        return false;
                    if (!currentPoint.isXNeg)
                        return false;
                    break;
                case 3:
                    if (judgePoint.listTmpDirection.Exists(a => a.direction == 4))
                        return false;
                    if (!currentPoint.isYPos)
                        return false;
                    break;
                case 4:
                    if (judgePoint.listTmpDirection.Exists(a => a.direction == 3))
                        return false;
                    if (!currentPoint.isYNeg)
                        return false;
                    break;
                default:
                    throw new Exception("创建路径失败：找不到此点的方向。");
            }
            
            return true;
        }

        /// <summary>
        /// 获取方向
        /// </summary>
        /// <param name="judgePoint">要判断的点</param>
        /// <param name="currentPoint">当前点</param>
        /// <returns>返回方向。1：东。2：南。3：西。4：北</returns>
        public int GetDircition(Point judgePoint, Point currentPoint)
        {
            if (judgePoint.x == currentPoint.x)
            {
                if (judgePoint.y == currentPoint.y + 1)
                    return 3;
                else if (judgePoint.y == currentPoint.y - 1)
                    return 4;
            }
            if (judgePoint.y == currentPoint.y)
            {
                if (judgePoint.x == currentPoint.x + 1)
                    return 1;
                else if (judgePoint.x == currentPoint.x - 1)
                    return 2;
            }

            throw new Exception("无法找到此两点的路径方向：" + judgePoint.barCode + "-" + currentPoint.barCode);
        }

        /// <summary>
        /// 点在OpenList
        /// </summary>
        /// <param name="currentPoint"></param>
        /// <param name="judgePoint"></param>
        void FoundPoint(Point judgePoint, Point currentPoint)
        {

        }

        double ConditionG(Point judgePoint, Point currentPoint)
        {
            double value = LINE;
            Point parentPoint = null;

            if (currentPoint.ParentPoint != null)
            {
                parentPoint = currentPoint.ParentPoint;

                //如果三点在一个直线上，则等于5，否则等于10
                if ((parentPoint.x == currentPoint.x && judgePoint.x == parentPoint.x)
                    || (parentPoint.y == currentPoint.y && judgePoint.y == parentPoint.y))
                    value = LINE;
                else value = CONER;
            }

            //返回值加上将要行走的小车数量
            return value + judgePoint.listTmpDirection.Count;
        }
 
        double CalcH(Point judgePoint, Point endPoint)
        {
            //查找距离目标点的距离（X+Y）*7
            int dtX = Math.Abs(judgePoint.x - endPoint.x);
            int dtY = Math.Abs(judgePoint.y - endPoint.y);
            return (dtX + dtY) * TOEND;
        }
    }
}