using System;
using System.Collections.Generic;
using System.Text;

namespace ACS
{
    /// <summary>
    /// 发送Agv的数据
    /// </summary>
    public class SendData
    {
        /// <summary>
        /// 获取新任务数据
        /// </summary>
        /// <param name="agvNo"></param>
        /// <param name="sId"></param>
        /// <param name="SOri"></param>
        /// <param name="EOri"></param>
        /// <returns></returns>
        public static byte[] GetNewTaskData(string agvNo, int sId, byte SOri, byte EOri)
        {
            byte[] Data = new byte[36];
            Data[0] = 0;
            Data[1] = 1;    //功能码
            Data[2] = 22;

            byte[] DataAdress = Encoding.ASCII.GetBytes(agvNo);

            for (int index = 3; index < 3 + DataAdress.Length; index++)
            {
                Data[index] = DataAdress[index - 3];
            }

            byte[] DataTaskNo = Encoding.ASCII.GetBytes(sId.ToString());

            for (int index = 13; index < 13 + DataTaskNo.Length; index++)
            {
                Data[index] = DataTaskNo[index - 13];
            }
            Data[33] = SOri;
            Data[34] = EOri;
            Data[35] = Commond.CRC(Data, 34);
            
            string s = Encoding.ASCII.GetString(Data,0 , Data.Length);
            App.ExFile.MessageLog("DataTranslate", s + "\r");
            return Data;
        }

        /// <summary>
        /// 获取新动作
        /// </summary>
        /// <param name="agvMsg">小车信息</param>
        /// <param name="listMotion">动作列表</param>
        /// <returns></returns>
        public static byte[] GetNewActionData(AgvMsg agvMsg, List<Motion> listMotion)
        {
            byte[] Data = new byte[89];
            Data[0] = 0;
            Data[1] = 3;    //功能码
            Data[2] = 75;

            byte[] DataAdress = Encoding.ASCII.GetBytes(agvMsg.AgvNo);
            for (int index = 3; index < 3 + DataAdress.Length; index++)
            {
                Data[index] = DataAdress[index - 3];
            }

            //动作报文拼接
            int DTypeIndex = 0;
            foreach (Motion newMotion in listMotion)
            {
                byte[] BarCode1 = new byte[4];
                byte[] DXpos = new byte[2];
                byte[] DYpos = new byte[2];
                byte[] DXdis = new byte[2];
                byte[] DYdis = new byte[2];

                Data[13 + DTypeIndex] = (byte)newMotion.sTaskType; //动作类型

                UInt32 barCode1 = UInt32.Parse(newMotion.barcode);    //码值
                BarCode1 = BitConverter.GetBytes(barCode1);
                Data[14 + DTypeIndex] = BarCode1[0];
                Data[15 + DTypeIndex] = BarCode1[1];
                Data[16 + DTypeIndex] = BarCode1[2];
                Data[17 + DTypeIndex] = BarCode1[3];

                Int16 xpos = (Int16)newMotion.x; //X坐标
                DXpos = BitConverter.GetBytes(xpos);
                Data[18 + DTypeIndex] = DXpos[0];
                Data[19 + DTypeIndex] = DXpos[1];

                Int16 ypos = (Int16)newMotion.y;  //Y坐标
                DYpos = BitConverter.GetBytes(ypos);
                Data[20 + DTypeIndex] = DYpos[0];
                Data[21 + DTypeIndex] = DYpos[1];

                Int16 xdis = (Int16)newMotion.xLength; //X间距
                DXdis = BitConverter.GetBytes(xdis);
                Data[22 + DTypeIndex] = DXdis[0];
                Data[23 + DTypeIndex] = DXdis[1];

                Int16 ydis = (Int16)newMotion.yLength; //Y间距
                DYdis = BitConverter.GetBytes(xdis);
                Data[24 + DTypeIndex] = DYdis[0];
                Data[25 + DTypeIndex] = DYdis[1];

                Data[26 + DTypeIndex] = (byte)newMotion.pointType;//点属性
                Data[27 + DTypeIndex] = 1;//托盘属性        是否需要？？

                DTypeIndex = DTypeIndex + 15;
            }

            Data[88] = Commond.CRC(Data, 87);
            string s = Encoding.ASCII.GetString(Data, 0, Data.Length);
            App.ExFile.MessageLog("DataTranslate", s + "\r");
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

        /// <summary>
        /// 动作完成
        /// </summary>
        /// <param name="agvNo"></param>
        /// <returns></returns>
        public static byte[] GetFinishData(string agvNo)
        {
            return GetData(agvNo, TypeEnum.Finish);
        }

        public static byte[] GetData(string agvNo, TypeEnum te)
        {
            byte[] Data = new byte[14];
            Data[0] = 0;
            Data[1] = (byte)te;
            Data[2] = 0;

            //小车号
            Byte[] DataAdress = ASCIIEncoding.ASCII.GetBytes(agvNo);
            for (int index = 3; index < 3 + DataAdress.Length; index++)
            {
                Data[index] = DataAdress[index - 3];
            }
            Data[13] = Commond.CRC(Data, 12);
            return Data;
        }
    }
}
