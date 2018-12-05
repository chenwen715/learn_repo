using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ACSsocket
{
    class MsgManage
    {
        /// <summary>
        /// 指令解析
        /// </summary>
        /// <param name="socketOpt">自定义Socket对象</param>
        public static void DataTranslate(SocketOpt socketOpt)
        {
            string errMsg = "";

            byte[] DataRecv = socketOpt.recData;
            string AgvNo = Encoding.ASCII.GetString(DataRecv, 3, 10).Replace("\0", "");
            socketOpt.agv=new Agv();
            socketOpt.agv.agvNo = AgvNo;
            //记录回给小车的信息
            //App.ExFile.MessageLog("Get" + AgvNo, BitConverter.ToString(DataRecv, 0, 31));

            AgvMsg agvMsg = new AgvMsg();
            int DataLength = DataRecv[2];
            agvMsg.AgvNo = AgvNo;           
            agvMsg.Barcode = BitConverter.ToUInt32(DataRecv, 13).ToString();
            agvMsg.Voltage = BitConverter.ToInt16(DataRecv, 17) / 100;
            agvMsg.State = DataRecv[19];
            agvMsg.Height = (HeightEnum)DataRecv[20];
            agvMsg.ErrCode1 = DataRecv[21];
            agvMsg.ErrCode2 = DataRecv[22];
            agvMsg.ErrCode3 = DataRecv[23];

            string sid = BitConverter.ToUInt32(DataRecv, 26).ToString();
            if (string.IsNullOrEmpty(sid))
            {
                agvMsg.SID = 0;
            }

            else
            {
                agvMsg.SID = int.Parse(sid);
            }
            agvMsg.sTaskStatus = DataRecv[30];
           
            int CrcR = DataRecv[31];
            int CrcC =CRC(DataRecv, DataLength + 14);

            if (CrcC == CrcR)
            {
                UpdateAgv(agvMsg);
                if (Form1.agvList.Find(a => a.agvNo == agvMsg.AgvNo).sTaskList[0].taskNo == "0" || string.IsNullOrEmpty(Form1.agvList.Find(a => a.agvNo == agvMsg.AgvNo).sTaskList[0].taskNo))
                {
                    Form1.sid = "";
                }
                else
                {
                    Form1.sid = Form1.agvList.Find(a => a.agvNo == agvMsg.AgvNo).sTaskList[0].taskNo;
                }
                Form1.currentbarcode = Form1.agvList.Find(a => a.agvNo == agvMsg.AgvNo).barcode;
                if (Form1.responseType == 0)
                {
                    Form1.showMessage(AgvNo, "请选择回复报文类型"); 
                }
                while  (Form1.responseType == 0)
                {                 
                   Thread.Sleep(500);
                }
                if (Form1.responseType == rTypeEnum.Repeat)
                {
                    socketOpt.SendData = SendData.GetRepeatData(AgvNo);
                }
                else if (Form1.responseType == rTypeEnum.Finish)
                {
                    socketOpt.SendData = SendData.GetFinishData(AgvNo);
                }
                else if (Form1.responseType == rTypeEnum.NewAction)
                {
                    socketOpt.SendData = SendData.GetNewActionData(agvMsg);
                }
                                                              
            }
            else
            {
                errMsg = "AGV数据校验失败！";
            }
           

            if (!string.IsNullOrEmpty(errMsg))
            {
                Log.MessageError("Error" + agvMsg.AgvNo, errMsg);
            }


            TCP.Send(socketOpt);
        }

        private static void UpdateAgv(AgvMsg agvMsg)
        {
            if (Form1.agvList.Find(a => a.agvNo == agvMsg.AgvNo) == null)
            {
                Agv agv = new Agv();
                agv.agvNo = agvMsg.AgvNo;
                agv.barcode = agvMsg.Barcode;
                agv.currentCharge = agvMsg.Voltage;
                agv.height = agvMsg.Height;
                agv.sTaskList = new List<STask>();
                STask stask = new STask();                 
                stask.taskNo = agvMsg.SID.ToString();
                agv.sTaskList.Add(stask);
                agv.state = (AgvState)agvMsg.State;
                Form1.agvList.Add(agv);
            }
            else
            {
                Agv agv = Form1.agvList.Find(a => a.agvNo == agvMsg.AgvNo);
                agv.barcode = agvMsg.Barcode;
                agv.currentCharge = agvMsg.Voltage;
                agv.height = agvMsg.Height;
                agv.sTaskList[0].taskNo = agvMsg.SID.ToString();
                agv.state = (AgvState)agvMsg.State;
            }
        }

        public static byte CRC(byte[] data, int length)
        {
            byte crc = (byte)(data[0] ^ data[1]);
            for (int i = 2; i < length - 1; i++)
            {
                crc = (byte)(crc ^ data[i]);
            }
            return crc;
        }
    }
}

