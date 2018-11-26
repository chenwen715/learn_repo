using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACSsocket
{
    class SendData
    {
        /// <summary>
        /// 获取新动作
        /// </summary>
        /// <param name="agvMsg">小车信息</param>
        /// <param name="listMotion">动作列表</param>
        /// <returns></returns>
        //public static byte[] GetNewActionData(int SID, List<Motion> listMotion)
        public static byte[] GetNewActionData(AgvMsg agvMsg)
        {
            byte[] Data = new byte[73];
            Data[0] = 0;
            Data[1] = 1;    //功能码
            Data[2] = 69;

           
            ///动作报文拼接

            //任务号
            byte[] TaskNo = new byte[4];
            
            TaskNo = BitConverter.GetBytes(UInt32.Parse(Form2.motionL[0].sTaskNo));
            Form1.sid = Form2.motionL[0].sTaskNo;
            
            //TaskNo = BitConverter.GetBytes(UInt32.Parse(agvMsg.SID.ToString()));
            Data[3] = TaskNo[0];
            Data[4] = TaskNo[1];
            Data[5] = TaskNo[2];
            Data[6] = TaskNo[3];
            Data[7] = (byte)Form2.motionL[0].sTaskType; //动作类型
            int DTypeIndex = 0;
            foreach (Motion newMotion in Form2.motionL)
            {
                byte[] BarCode1 = new byte[4];
                byte[] DXpos = new byte[2];
                byte[] DYpos = new byte[2];
                byte[] DXdis = new byte[2];
                byte[] DYdis = new byte[2];



                UInt32 barCode1 = UInt32.Parse(newMotion.barcode);    //码值
                BarCode1 = BitConverter.GetBytes(barCode1);
                Data[8 + DTypeIndex] = BarCode1[0];
                Data[9 + DTypeIndex] = BarCode1[1];
                Data[10 + DTypeIndex] = BarCode1[2];
                Data[11 + DTypeIndex] = BarCode1[3];

                Int16 xpos = (Int16)newMotion.x; //X坐标
                DXpos = BitConverter.GetBytes(xpos);
                Data[12 + DTypeIndex] = DXpos[0];
                Data[13 + DTypeIndex] = DXpos[1];

                Int16 ypos = (Int16)newMotion.y;  //Y坐标
                DYpos = BitConverter.GetBytes(ypos);
                Data[14 + DTypeIndex] = DYpos[0];
                Data[15 + DTypeIndex] = DYpos[1];

                Int16 xdis = (Int16)newMotion.xLength; //X间距
                DXdis = BitConverter.GetBytes(xdis);
                Data[16 + DTypeIndex] = DXdis[0];
                Data[17 + DTypeIndex] = DXdis[1];

                Int16 ydis = (Int16)newMotion.yLength; //Y间距
                DYdis = BitConverter.GetBytes(ydis);
                Data[18 + DTypeIndex] = DYdis[0];
                Data[19 + DTypeIndex] = DYdis[1];

                Data[20 + DTypeIndex] = (byte)newMotion.OriAgv;//车头方向
                Data[21 + DTypeIndex] = (byte)newMotion.pointType;//点属性
                Data[22 + DTypeIndex] = (byte)newMotion.AntiCollision;//防撞属性
                Data[23 + DTypeIndex] = (byte)newMotion.OriDial;//托盘属性

                DTypeIndex = DTypeIndex + 16;
            }

            Data[72] = MsgManage.CRC(Data, 73);
            string s = Encoding.ASCII.GetString(Data, 0, Data.Length);
            //App.ExFile.MessageLog("DataTranslate", s + "\r");
            return Data;
        }

        /// <summary>
        /// 重发，要求AGV，间隔1秒后在发送
        /// </summary>
        /// <param name="agvNo"></param>
        /// <returns></returns>
        public static byte[] GetRepeatData(string agvNo)
        {
            return GetData(agvNo, TypeEnum.Repeat);
        }

        public static byte[] GetFinishData(string agvNo)
        {
            return GetData(agvNo, TypeEnum.Finish);
        }

        public static byte[] GetData(string agvNo, TypeEnum te)
        {
            byte[] Data = new byte[4];
            Data[0] = 0;
            Data[1] = (byte)te;
            Data[2] = 0;


            Data[3] = MsgManage.CRC(Data, 4);
            return Data;
        }
    }
}

