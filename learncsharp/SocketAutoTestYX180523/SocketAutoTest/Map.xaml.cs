using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SocketAutoTest
{
    /// <summary>
    /// Map.xaml 的交互逻辑
    /// </summary>
    public partial class Map : Window
    {
        Path path = new Path();
        List<Point> tempList = new List<Point> { };
        List<AgvClass> Alist = new List<AgvClass>();
        List<Point> cPointList = new List<Point> { };

        public static List<System.Windows.Shapes.Rectangle> WorkStationSharp = new List<System.Windows.Shapes.Rectangle>();   //地图货架显示集合
        public static List<System.Windows.Shapes.Ellipse> AgvSharp = new List<System.Windows.Shapes.Ellipse>(); //地图小车显示集合
        public static List<System.Windows.Shapes.Ellipse> PointSharp = new List<System.Windows.Shapes.Ellipse>();   //地图点显示集合

        public Map()
        {
            InitializeComponent();
            tempList = path.initPoint();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.WorkArea.Height;
            Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2;
            Top = (SystemParameters.WorkArea.Height - this.Height) / 2;
            Canvas_Monitor.Width = (tempList.Max(a => a.mapX) - tempList.Min(a => a.mapX)) *(Properties.Settings.Default.GsWidth+3);
            Canvas_Monitor.Height = (tempList.Max(a => a.mapY) - tempList.Min(a => a.mapY)) * (Properties.Settings.Default.GsHeight + 3);
            //Canvas_Monitor.Background = new SolidColorBrush(Colors.LightGreen); 
            DrawPath();
            PointSet();
            AgvPointSet();
            wsPointSet();
            
        }

        public double[] DrawSharp( double xpos, double ypos, int GsHeight, int GsWidth,int r)
        {
            double y = 0;
            double x = 0;
            x = xpos;
            y = ypos;
            y = (tempList.Max(a => a.mapY) - ypos + 2) * GsHeight+ 3+r/2;
            x = (xpos - tempList.Min(a => a.mapX) + 2) * GsWidth + 3 + r / 2;
            return new double[] { x, y };
        }

        public int[] DrawPoint(int xpos, int ypos, int GsHeight, int GsWidth)
        {
            int y = (tempList.Max(a => a.mapY) - ypos + 2) * GsHeight + (GsHeight) / 2 ;
            int x = (xpos - tempList.Min(a => a.mapX) + 2) * GsWidth+(GsWidth) / 2;
            return new int[] { x, y };
        }

        public void PointSet()
        {
            foreach (Point p in tempList)
            {
                Ellipse ellis = new Ellipse();
                //ellis.Name = p._BarCode;
                ellis.ToolTip =  string.Format("({0},{1}),({2},{3}),{4},{5},{6}"
                    ,p.mapX,p.mapY,p.currentX,p.currentY,p.pointDescription,p.intStationCode,p.wsType);

                ellis.Tag = "Transparent";
                ellis.Width = 4;
                ellis.Height = 4;
                /*
                 * DCol_Id	DCol_Name
                 * 0、普通路径点；（黑）
                 * 1、汇流口；（蓝）
                 * 2、导线桶（单）料台；（橙色）
                 * 3、导线桶（双）料台；（橙色）
                 * 4、路口点；（红）
                 * 5、释放路口点；（蓝）
                 * 6、充电点；（绿）
                 * 7、WCS输送线下料口；（浅蓝）
                 * 8、WCS输送线上料口；（浅蓝）
                 */
                switch (p.pointDescription)
                {
                    case 0:
                        ellis.Fill = new SolidColorBrush(Colors.Black);
                        ellis.Width = 4;
                        ellis.Height = 4;
                        break;
                    case 1:
                    case 5:
                        ellis.Fill = new SolidColorBrush(Colors.Blue);
                        ellis.Width = 8;
                        ellis.Height = 8;
                        break;
                    case 2:
                    case 3:
                        ellis.Fill = new SolidColorBrush(Colors.Orange);
                        ellis.Width = 6;
                        ellis.Height = 6;
                        break;
                    case 4:
                        ellis.Fill = new SolidColorBrush(Colors.Red);
                        ellis.Width = 8;
                        ellis.Height = 8;
                        break;
                    case 6:
                        ellis.Fill = new SolidColorBrush(Colors.Green);
                        ellis.Width = 6;
                        ellis.Height = 6;
                        break;
                    case 7:
                    case 8:
                        ellis.Fill = new SolidColorBrush(Colors.BlueViolet);
                        ellis.Width = 6;
                        ellis.Height = 6;
                        break;
                    default:
                        ellis.Fill = new SolidColorBrush(Colors.Black);
                        break;
                }
                int[] xy = DrawPoint(
                    p.mapX, p.mapY, Properties.Settings.Default.GsWidth, Properties.Settings.Default.GsHeight);

                ellis.SetValue(Canvas.TopProperty, (double)xy[1]);
                ellis.SetValue(Canvas.LeftProperty, (double)xy[0]);
                Canvas_Monitor.Children.Add(ellis);
                PointSharp.Add(ellis);
            }
        }

        public void AgvPointSet()
        {
            DAL_Comn_Sql DalSql = new DAL_Comn_Sql();
            string sql = string.Format(@"SELECT * FROM dbo.Agv");
            DataSet ds = DalSql.SelectGet(Properties.Settings.Default.Sql, sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AgvClass agv = new AgvClass();
                agv.IsEnable = bool.Parse(dr["isEnable"].ToString());
                {
                    if (agv.IsEnable)
                    {
                        agv.AgvName = dr["strAgvNo"].ToString();
                        agv.AgvType = int.Parse(dr["agvType"].ToString());
                        string currentBarcode = dr["strBarcode"].ToString();
                        string[] xy = currentBarcode.Substring(currentBarcode.IndexOf('(') + 1, (currentBarcode.IndexOf(')') - currentBarcode.IndexOf('(') - 1)).Split(new char[] { ',' });
                        agv.CurrentX = int.Parse(xy[0]);
                        agv.CurrentY = int.Parse(xy[1]);
                        agv.State = Int16.Parse(dr["agvState"].ToString());
                        agv.V = float.Parse(dr["currentCharge"].ToString());

                        agv.IsHaveGoods = bool.Parse(dr["agvCarry"].ToString());
                        agv.ChargeStation = int.Parse(dr["agvChargeStation"].ToString());
                        Alist.Add(agv);
                    }
                }
                
            }

            for (int i = 0; i < Alist.Count; i++)
            {

                Ellipse Elps = new Ellipse();
                Elps.Name = Alist[i].AgvName;
                Elps.ToolTip = Elps.Name;
                Elps.Width =Properties.Settings.Default.GsWidth;
                Elps.Height = Properties.Settings.Default.GsHeight;
                Elps.Fill = new SolidColorBrush(Colors.Transparent);
                Elps.Stroke = new SolidColorBrush(Colors.Tan);
                Elps.StrokeThickness = 3;

                Point p = tempList.Find(a=>a.currentX==Alist[i].CurrentX && a.currentY==Alist[i].CurrentY);

                if (p != null)
                {
                    int r=  Radius(p);
                    double[] xy = DrawSharp(p.mapX, p.mapY,Properties.Settings.Default.GsWidth,Properties.Settings.Default.GsHeight,r);
                    Elps.SetValue(Canvas.TopProperty, (double)xy[1]);
                    Elps.SetValue(Canvas.LeftProperty, (double)xy[0]);
                }

                if (!Alist[i].IsEnable)
                {
                    Elps.Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    Elps.Visibility = System.Windows.Visibility.Visible;
                }
                 Canvas_Monitor.Children.Add(Elps);
                 AgvSharp.Add(Elps);
            }
        }

        public void wsPointSet()
        {
            try
            {
                foreach (Point p in tempList)
                {
                    if (p.pointDescription == 2||p.pointDescription==3)
                    {
                        Rectangle rect = new Rectangle();
                        
                        if (p.wsType == 1)
                        {
                            rect.Stroke = new SolidColorBrush(Colors.LightSeaGreen);
                            rect.Name = "上料台" + p.intStationCode.ToString();

                        }
                        else if (p.wsType == 2)
                        {
                            rect.Stroke = new SolidColorBrush(Colors.SeaGreen);
                            rect.Name = "下料台" + p.intStationCode.ToString();
                        }
                        else if (p.wsType == 3)
                        {
                            rect.Stroke = new SolidColorBrush(Colors.LightGreen);
                            rect.Name = "料台" + p.intStationCode.ToString();
                        }
                        rect.ToolTip = rect.Name;
                        rect.Width = Properties.Settings.Default.GsWidth;
                        rect.Height = Properties.Settings.Default.GsHeight;
                        rect.Fill = new SolidColorBrush(Colors.Transparent);

                        rect.StrokeThickness = 3;
                        rect.RadiusX = 2;
                        rect.RadiusY = 2;
                        if (p.currentY == 4)
                        {
                            double[] xy = new double[2];
                            xy = DrawSharp(p.mapX, (p.mapY + 1), Properties.Settings.Default.GsWidth, Properties.Settings.Default.GsHeight, 0);
                            rect.SetValue(Canvas.TopProperty, (double)xy[1]);
                            rect.SetValue(Canvas.LeftProperty, (double)xy[0]);
                        }
                        else if (p.currentY == 2)
                        {
                            double[] xy = new double[2];
                            xy = DrawSharp(p.mapX, (p.mapY - 1), Properties.Settings.Default.GsWidth, Properties.Settings.Default.GsHeight, 0);
                            rect.SetValue(Canvas.TopProperty, (double)xy[1]);
                            rect.SetValue(Canvas.LeftProperty, (double)xy[0]);
                        }
                        Canvas_Monitor.Children.Add(rect);
                        WorkStationSharp.Add(rect);

                    }
                }
                foreach (Point p in tempList)
                {
                    if (p.pointDescription == 2 && p.wsType == 1)
                    {
                        Rectangle rect1 = new Rectangle();
                        rect1.Name = "中间"+p.intStationCode.ToString();
                        rect1.ToolTip = rect1.Name;
                        rect1.Width = Properties.Settings.Default.GsWidth;
                        rect1.Height = Properties.Settings.Default.GsHeight;
                        rect1.Fill = new SolidColorBrush(Colors.Transparent);
                        rect1.Stroke = new SolidColorBrush(Colors.Black);
                        rect1.StrokeThickness = 3;
                       
                        if (p.currentY == 4)
                        {
                            double[] xy = new double[2];
                            xy = DrawSharp(p.mapX+1, (p.mapY + 1), Properties.Settings.Default.GsWidth, Properties.Settings.Default.GsHeight, 0);
                            rect1.SetValue(Canvas.TopProperty, (double)xy[1]);
                            rect1.SetValue(Canvas.LeftProperty, (double)xy[0]);
                        }
                        else if (p.currentY == 2)
                        {
                            double[] xy = new double[2];
                            xy = DrawSharp(p.mapX-1, (p.mapY - 1), Properties.Settings.Default.GsWidth, Properties.Settings.Default.GsHeight, 0);
                            rect1.SetValue(Canvas.TopProperty, (double)xy[1]);
                            rect1.SetValue(Canvas.LeftProperty, (double)xy[0]);
                        }
                        Canvas_Monitor.Children.Add(rect1);
                        WorkStationSharp.Add(rect1);
                       
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            
            
        }

        public void DrawPath()
        {
            DrawLine(34, 4, 8, 4);
            DrawLine(34, 2, 5, 2);

            //DrawLine(7, 4,3, 4);
            //DrawLine(3, 4, 66, 32);
            DrawLine(7, 4, 66, 32);

            //DrawLine(63, 31, 4, 4);
            //DrawLine(4, 4, 4, 1);
            //DrawLine(4, 1, 63, 13);
            DrawLine(63, 31, 63, 13);

            //DrawLine(67, 31, 1, 4);
            //DrawLine(1, 4, 67, 13);
            DrawLine(67, 31, 67, 13);

            
            DrawLine(3, 26, 34, 2);
            DrawLine(2, 33, 2, 27);
            DrawLine(18, 33, 18, 27);
            DrawLine(29, 33, 29, 27);
            DrawLine(34, 33, 34, 27);
            DrawLine(42, 33, 42, 27);
            DrawLine(56, 33, 56, 27);
            DrawLine(8, 4, 7, 4);
            
            
            DrawLine(3, 1, 66, 24);
            DrawLine(15, 5, 66, 22);
            DrawLine(16, 5, 66, 20); 
            DrawLine(17, 5, 66, 18);
            DrawLine(18, 5, 66, 16);
            DrawLine(19, 5, 66, 14);
            DrawLine(20, 5, 66, 12);

            DrawArc(2, 33, 34, 4, 90,1);
            DrawArc(18, 33, 26, 4, 90,1);
            DrawArc(29, 33, 30,34, 90,1);//g
            DrawArc(34, 33, 35, 34, 90,1);//g
            DrawArc(42, 33, 18, 4, 90,1);//g
            DrawArc(56, 33, 9, 4, 90,1);
            //DrawArc(4, 4, 3, 4, 90,2);
            DrawArc(63, 31, 64, 32, 90, 1);//g

            DrawArc(3, 26, 2, 27, 90,1);
            DrawArc(44, 2, 18, 27, 90, 1);
            DrawArc(43, 2, 29, 27, 90, 1);
            DrawArc(42, 2, 34, 27, 90, 1);
            DrawArc(41, 2, 42, 27, 90, 1);
            DrawArc(40, 2, 56, 27, 90, 1);
            DrawArc(3, 1, 4, 2, 90, 1);

            DrawArc(15, 5, 4, 1, 90, 1);
            DrawArc(16, 5, 63, 21, 90, 1);
            DrawArc(17, 5, 63, 19, 90, 1);
            DrawArc(18, 5, 63, 17, 90, 1);
            DrawArc(19, 5, 63, 15, 90, 1);
            DrawArc(20, 5, 63, 13, 90, 1);

            DrawArc(1, 2, 66, 24, 90, 1);
            DrawArc(1, 1, 66, 22, 90, 1);
            DrawArc(1, 14, 66, 20, 90, 1);
            DrawArc(1, 13, 66, 18, 90, 1);
            DrawArc(1, 12, 66, 16, 90, 1);
            DrawArc(1, 11, 66, 14, 90, 1);
            DrawArc(67, 13, 66, 12, 90, 1);

            DrawArc(5, 2, 4, 2, 90, 1);
            DrawArc(66, 32, 67, 31, 90, 1);
            
        }

        public void DrawLine(int sCurrentX, int sCurrentY, int eCurrentX, int eCurrentY)
        {
            Line l1 = new Line();
            int b=6,c=6;
            int sx, sy, ex, ey;
            int GsHeight = Properties.Settings.Default.GsHeight;
            int GsWidth=Properties.Settings.Default.GsWidth;           
            Point startPoint = tempList.Find(a => a.currentX == sCurrentX && a.currentY == sCurrentY);
            if (startPoint != null)
            {
                b = Radius(startPoint);
                sx = startPoint.mapX;
                sy = startPoint.mapY;
            }
            else
            {
                sx = sCurrentX;
                sy = sCurrentY;
            }           
            double startPointX = ( sx+ 2 - tempList.Min(a => a.mapX)) * GsHeight + (GsHeight + b) / 2;
            double startPointY = (tempList.Max(a => a.mapY) - sy+ 2) * GsHeight + (GsHeight + b) / 2;
            Point endPoint = tempList.Find(a => a.currentX == eCurrentX && a.currentY == eCurrentY);
            if (endPoint != null)
            {
                c = Radius(endPoint);
                ex = endPoint.mapX;
                ey = endPoint.mapY;
            }
            else
            {
                ex = eCurrentX;
                ey = eCurrentY;
            }
            
            double endPointX = ( ex+ 2 - tempList.Min(a => a.mapX)) * GsHeight + (GsHeight + c) / 2;
            double endPointY = (tempList.Max(a => a.mapY) -ey  + 2) * GsHeight + (GsHeight + c) / 2;
            l1.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            l1.StrokeThickness = 2;
            l1.X1 = startPointX;
            l1.Y1 = startPointY;
            l1.X2 = endPointX;
            l1.Y2 = endPointY;
            Canvas_Monitor.Children.Add(l1);
        }

        public void DrawArc(int sCurrentX, int sCurrentY, int eCurrentX, int eCurrentY,double angle,int n)
        {
            int b = 6, c = 6;
            int sx, sy, ex, ey;
            int rHeight = Properties.Settings.Default.GsHeight*n;
            int rWidth=Properties.Settings.Default.GsWidth*n;
            int GsHeight = Properties.Settings.Default.GsHeight;
            int GsWidth = Properties.Settings.Default.GsWidth;      
            System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
            PathGeometry pathGeometry = new PathGeometry();
            System.Windows.Point ePoint = new System.Windows.Point();
            Point endPoint = tempList.Find(a => a.currentX == eCurrentX && a.currentY == eCurrentY);
            if (endPoint != null)
            {
                c = Radius(endPoint);
                ex = endPoint.mapX;
                ey = endPoint.mapY;
            }
            else
            {
                ex = eCurrentX;
                ey = eCurrentY;
            }
            ePoint.X = (ex + 2 - tempList.Min(a => a.mapX)) * GsHeight + (GsHeight + c) / 2;
            ePoint.Y = (tempList.Max(a => a.mapY) - ey + 2) * GsHeight + (GsHeight + c) / 2;
            ArcSegment arc = new ArcSegment(ePoint, new System.Windows.Size(rHeight, rWidth), angle, false, SweepDirection.Clockwise, true);
            PathFigure figure = new PathFigure();
            System.Windows.Point sPoint = new System.Windows.Point();
            Point startPoint = tempList.Find(a => a.currentX == sCurrentX && a.currentY == sCurrentY);
            if (startPoint != null)
            {
                b = Radius(startPoint);
                sx = startPoint.mapX;
                sy = startPoint.mapY;
            }
            else
            {
                sx = sCurrentX;
                sy = sCurrentY;
            }           
            sPoint.X = (sx + 2 - tempList.Min(a => a.mapX)) * GsHeight + (GsHeight + b) / 2;
            sPoint.Y = (tempList.Max(a => a.mapY) - sy + 2) * GsHeight + (GsHeight + b) / 2;
            figure.StartPoint = sPoint;
            figure.Segments.Add(arc);
            pathGeometry.Figures.Add(figure);
            path.Data = pathGeometry;
            path.Stroke = Brushes.LightSteelBlue;
            path.StrokeThickness = 2;
            Canvas_Monitor.Children.Add(path);
        }

        public int Radius(Point p)
        {
            int c = 0;
            switch (p.pointDescription)
            {
                case 0:
                    c = 4;
                    break;
                case 2:
                case 3:
                case 6:
                case 7:
                case 8:
                    c = 6;
                    break;
                case 1:
                case 4:
                case 5:
                    c = 8;
                    break;
                default:
                    c = 4;
                    break;
            }
            return c;
        }

        public void AgvChange(AgvClass Agv, int xpos, int ypos, int GsHeight, int GsWidth)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                System.Windows.Shapes.Ellipse Ell = AgvSharp.Find(
                  new Predicate<System.Windows.Shapes.Ellipse>(
                      a => { return a.Name == Agv.AgvName; }));

                if (Ell != null)
                {

                    double[] xy = DrawSharp(xpos, ypos, GsHeight, GsWidth,0);

                    Ell.SetValue(System.Windows.Controls.Canvas.LeftProperty, (double)xy[0]);

                    Ell.SetValue(System.Windows.Controls.Canvas.TopProperty, (double)xy[1]);

                }

            }));

        }

        public void AgvShelfStatusChange(AgvClass Agv)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                System.Windows.Shapes.Ellipse Ell = AgvSharp.Find(
                  new Predicate<System.Windows.Shapes.Ellipse>(
                      a => { return a.Name == Agv.AgvName; }));

                if (Ell != null)
                {
                    if (Agv.State == 11)
                    {
                        Ell.Stroke = new SolidColorBrush(Colors.Tan);
                    }
                    else if (Agv.State == 13)
                    {
                        Ell.Stroke = new SolidColorBrush(Colors.Red);
                    }
                    else if (Agv.State == 14)
                    {
                        Ell.Stroke = new SolidColorBrush(Colors.Green);
                    }

                    if (Agv.IsHaveGoods)
                    {
                        Ell.Fill = new SolidColorBrush(Colors.LightGreen);
                    }
                    else
                    {
                        Ell.Fill = new SolidColorBrush(Colors.Transparent);
                    }
                }

            }));
        }

        public void wsStateChange(WorkStationControlClass wsc)
        {

            foreach (WorkStation ws in wsc.ws)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    System.Windows.Shapes.Rectangle Ell = WorkStationSharp.Find(
                      new Predicate<System.Windows.Shapes.Rectangle>(
                          a => { return a.Name == "上料台" + ws.wsNo; }));

                    if (Ell != null)
                    {
                        if (ws.IsLoadingFinish)
                        {
                            Ell.Fill = new SolidColorBrush(Colors.LightGreen);
                        }
                        else
                        {
                            Ell.Fill = new SolidColorBrush(Colors.Transparent);
                        }
                        if (ws.IsLoadingWork)
                        {
                            Ell.Fill = new LinearGradientBrush(Colors.LightGreen, Colors.Transparent, 90);
                        }
                        
                    }

                }));
                this.Dispatcher.Invoke(new Action(() =>
                {
                    System.Windows.Shapes.Rectangle Ell = WorkStationSharp.Find(
                      new Predicate<System.Windows.Shapes.Rectangle>(
                          a => { return  a.Name == "下料台" + ws.wsNo; }));

                    if (Ell != null)
                    {
                        if (ws.IsUnloadingRequest)
                        {
                            Ell.Fill = new SolidColorBrush(Colors.LightGreen);
                        }
                        else
                        {
                            Ell.Fill = new SolidColorBrush(Colors.Transparent);
                        }
                        
                    }

                }));
                this.Dispatcher.Invoke(new Action(() =>
                {
                    System.Windows.Shapes.Rectangle Ell = WorkStationSharp.Find(
                      new Predicate<System.Windows.Shapes.Rectangle>(
                          a => { return a.Name == "料台" + ws.wsNo; }));

                    if (Ell != null)
                    {
                        if (ws.IsLoadingFinish)
                        {
                            Ell.Fill = new SolidColorBrush(Colors.LightGreen);
                        }
                        else if (ws.IsLoadingWork)
                        {
                            Ell.Fill = new LinearGradientBrush(Colors.LightGreen, Colors.Transparent, 90);
                        }                      
                        else if (ws.IsUnloadingRequest)
                        {
                            Ell.Fill = new SolidColorBrush(Colors.LightGreen);
                        }
                        else
                        {
                            Ell.Fill = new SolidColorBrush(Colors.Transparent);
                        }
                    }

                }));
            }
           
        }
    }
}
