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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace map_181115
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        DAL_Comn_Sql sql = new DAL_Comn_Sql();
        List<PathPoint> PPointList = new List<PathPoint>();//地图点集合
        List<AGV> AGVList = new List<AGV>();//地图agv集合
        List<System.Windows.Shapes.Ellipse> PPSharp = new List<System.Windows.Shapes.Ellipse>();//地图点显示集合
        List<System.Windows.Shapes.Ellipse> AGVSharp = new List<System.Windows.Shapes.Ellipse>();//地图点显示集合
        int stepX = Properties.Settings.Default.GsWidth;
        int stepY = Properties.Settings.Default.GsHeight;
        public MainWindow()
        {
            InitializeComponent();
            InitPoint();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.WorkArea.Height;
            Left = (Width - this.Width) / 2;
            Top = (Height - this.Height) / 2;
            Canvas_Monitor.Width = (PPointList.Max(a => a.mapX) - PPointList.Min(a => a.mapX)) * stepX;
            Canvas_Monitor.Height = (PPointList.Max(a => a.mapY) - PPointList.Min(a => a.mapY)) * stepY;
            DrawPPoint();
            DrawAgv();
        }

        /// <summary>
        /// 设置路径点显示信息
        /// </summary>
        private void DrawPPoint()
        {
            foreach (PathPoint p in PPointList)
            {
                int pointSize=10;
                Ellipse elli = new Ellipse();
                elli.ToolTip = string.Format("x:{0} y:{1}",p.pointX,p.pointY);//鼠标悬浮到该点上时显示
                elli.Width = pointSize;//设置点的大小
                elli.Height = pointSize;//设置点的大小，点为圆时，width和height值一样
                elli.Fill = new SolidColorBrush(Colors.Black);//设置点填充色为黑色
                int[] xy = DrawPoint(
                  p.pointX, p.pointY, stepX, stepY, pointSize);
                elli.SetValue(Canvas.TopProperty, (double)xy[1]);//设置点距幕布顶端的距离
                elli.SetValue(Canvas.LeftProperty, (double)xy[0]);//设置点距幕布左端的距离
                Canvas_Monitor.Children.Add(elli);//将点添加到幕布中，显示
                PPSharp.Add(elli);//将点添加到地图点显示集合中
            }
        }

        /// <summary>
        /// 设置小车显示信息
        /// </summary>
        private void DrawAgv()
        {
            AGVList.Add(new AGV("0101",1,1));
            foreach (AGV agv in AGVList)
            {
                int Size = 80;
                Ellipse elli = new Ellipse();
                elli.ToolTip = string.Format("barcode:{0}\n x:{1} y:{2}", agv.barcode,agv.x, agv.y);//鼠标悬浮到该点上时显示
                elli.Width = Size;//设置点的大小
                elli.Height = Size;//设置点的大小，点为圆时，width和height值一样
                elli.Stroke = new SolidColorBrush(Colors.Tan);//设置边界颜色
                elli.StrokeThickness = 5;//设置边界宽度
                elli.Fill = new SolidColorBrush(Colors.Transparent);//设置点填充色为透明色
                int[] xy = DrawPoint(
                  agv.x, agv.y, stepX, stepY,Size);
                elli.SetValue(Canvas.TopProperty, (double)xy[1]);//设置点距幕布顶端的距离
                elli.SetValue(Canvas.LeftProperty, (double)xy[0]);//设置点距幕布左端的距离
                Canvas_Monitor.Children.Add(elli);//将点添加到幕布中，显示
                AGVSharp.Add(elli);//将点添加到地图点显示集合中
            }
        }

        /// <summary>
        /// 计算点在幕布中的位置（坐标）
        /// </summary>
        /// <param name="p1">点x坐标</param>
        /// <param name="p2">点y坐标</param>
        /// <param name="p3">间距宽</param>
        /// <param name="p4">间距高</param>
        /// <returns></returns>
        private int[] DrawPoint(int p1, int p2, int p3, int p4,int size)
        {
            int y = (PPointList.Max(a => a.mapY) - p2) * p4 - size/2;
            int x = (p1 - PPointList.Min(a => a.mapX)) * p3 - size / 2;
            return new int[] { x, y };
        }

        /// <summary>
        /// 获取数据库点信息
        /// </summary>
        private void InitPoint()
        {
            string selectP = "select * from dbo.T_PathPoint";
            DataSet ds = sql.SelectGet(Properties.Settings.Default.Sql, selectP);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PathPoint pp = new PathPoint();
                pp.pointNo = dr["PointNo"].ToString();
                pp.pointX = int.Parse(dr["PointX"].ToString());
                pp.pointY = int.Parse(dr["PointY"].ToString());
                pp.mapX = int.Parse(dr["x"].ToString());
                pp.mapY = int.Parse(dr["y"].ToString());
                //pp.pointType = int.Parse(dr["pointType"].ToString());
                if (PPointList.Find(a => a.pointNo == pp.pointNo) == null)
                {
                    PPointList.Add(pp);
                }
            }
        }

       
    }
}
