using System.Collections.Generic;

namespace ACS
{
    public class AgvMsg
    {
        /// <summary>
        /// 小车发送的小车号
        /// </summary>
        public string AgvNo;
        
        /// <summary>
        /// 小车发送的当前码值
        /// </summary>
        public string Barcode;

        /// <summary>
        /// 小车发送的当前电压
        /// </summary>
        public int Voltage;

        /// <summary>
        /// 小车发送的自身状态
        /// </summary>
        public int State;

        /// <summary>
        /// 小车发送的当前顶升状态
        /// </summary>
        public HeightEnum Height;

        /// <summary>
        /// 小车发送的错误代码1
        /// </summary>
        public int ErrCode1;

        /// <summary>
        /// 小车发送的错误代码2
        /// </summary>
        public int ErrCode2;

        /// <summary>
        /// 小车发送的错误代码3
        /// </summary>
        public int ErrCode3;

        public int SID;

        public STaskType sTaskType;
    }

    public class ClientOrder
    {
        //任务号对于WMS/MES为父任务号
        //对于执行机构为子任务号
        public string TaskNo { get; set; }
        //任务类型针对WMS/MES或ExecUnit为不同考虑
        public string TaskType { get; set; }

        //MES/WMS
        public string OrderNo { get; set; }
        public string CInvCode { get; set; }
        public float Count { get; set; }
        public Shelf Shelf { get; set; }


        //执行单元
        public string ECode1 { get; set; }
        public string ECode2 { get; set; }
        public string ECode3 { get; set; }
        public int FuncCode { get; set; }
        public string OrderType { get; set; }
        public string Address { get; set; }
        public string Reserve1 { get; set; }
        public string Reserve2 { get; set; }
        public string TaskStatus { get; set; }
        public Point SBarCode { get; set; }//begin Code
        public Point EBarCode { get; set; }// end code
        public string Status { get; set; }
        public int Dtype { get; set; }
        public string BarCodeCurrent { get; set; }//当前码值
        public int Ori { get; set; }//东南西北
        public int SOri { get; set; }//begin orientation
        public int EOri { get; set; }//end orientation
        public float Vol { get; set; }
        public bool Crc { get; set; }//指令校验，判断指令信息是否残缺
        public int ShelfBe { get; set; }

        /// <summary>
        /// AGV状态
        /// </summary>
        public Agv Eu { get; set; }
        //动作集合
        public List<Motion> LstMotion { get; set; }

        public bool Error { get; set; }
    }
}
