using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using XNWMS.Common;

namespace XNWMS
{
    public class SNInfoWcf : ISNInfoWcf
    {
        DAL_Comn_Log comnlog = new DAL_Comn_Log();
        public XNResultModel DoLoadTasks(Stream steam)
        {
            XNResultModel resmod=new XNResultModel();
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            WebHeaderCollection head = WebOperationContext.Current.IncomingRequest.Headers;
            if (head["systemCode"] == "HHWCS")
            {
                StreamReader sr = new StreamReader(steam);
                string s = sr.ReadToEnd();
                LogWrite.WriteLogToMain(s+"\n");
                comnlog.MessageLog("上传数据", s + "\n");
                //ResultModel resmod = DoService.Doserver(s, "palletPackInfo");
                resmod = DoService.Doserver(s, "palletPackInfo");
                return resmod;
            }
            else
            {
                resmod.Info = "systemCode键值不正确" + head["systemCode"];
                resmod.StatusCode = "405";
                return resmod;
                //throw new Exception("systemCode键值不正确" + head["systemCode"]);
            }
        }

        public XNResultModel DoLoadTasks1(Stream steam)
        {

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            WebHeaderCollection head = WebOperationContext.Current.IncomingRequest.Headers;
            if (head["systemCode"] == "HHWCS")
            {
                StreamReader sr = new StreamReader(steam);
                string s = sr.ReadToEnd();
                LogWrite.WriteLogToMain(s + "\n");
                comnlog.MessageLog("上传数据", s + "\n");
                //ResultModel resmod = DoService.Doserver(s, "SNInfo");
                XNResultModel resmod = DoService.Doserver(s, "SNInfo");
                return resmod;
            }
            else
            {
                throw new Exception("systemCode键值不正确" + head["systemCode"]);
            }
        }

        public XNResultModel DoLoadTasks2(Stream steam)
        {

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            WebHeaderCollection head = WebOperationContext.Current.IncomingRequest.Headers;
            //if (head["systemCode"] == "HHWCS")
            //{
                StreamReader sr = new StreamReader(steam);
                string s = sr.ReadToEnd();
                LogWrite.WriteLogToMain(s + "\n");
                comnlog.MessageLog("上传数据", s + "\n");
                //ResultModel resmod = DoService.Doserver(s, "loadTasksResult");
                XNResultModel resmod = DoService.Doserver(s, "loadTasksResult");
                return resmod;
            //}
            //else
            //{
            //    throw new Exception("systemCode键值不正确" + head["systemCode"]);
            //}
        }

        public XNResultModel DoLoadTasks3(Stream steam)
        {

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            WebHeaderCollection head = WebOperationContext.Current.IncomingRequest.Headers;
            if (head["systemCode"] == "HHWCS")
            {
                StreamReader sr = new StreamReader(steam);
                string s = sr.ReadToEnd();
                LogWrite.WriteLogToMain(s + "\n");
                comnlog.MessageLog("上传数据", s + "\n");
                //ResultModel resmod = DoService.Doserver(s, "stationInfo");
                XNResultModel resmod = DoService.Doserver(s, "stationInfo");
                return resmod;
            }
            else
            {
                throw new Exception("systemCode键值不正确" + head["systemCode"]);
            }

        }

        public XNResultModel DoLoadTasks4(Stream steam)
        {

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            WebHeaderCollection head = WebOperationContext.Current.IncomingRequest.Headers;
            if (head["systemCode"] == "HHWCS")
            {
                StreamReader sr = new StreamReader(steam);
                string s = sr.ReadToEnd();
                LogWrite.WriteLogToMain(s + "\n");
                comnlog.MessageLog("上传数据", s + "\n");
                //ResultModel resmod = DoService.Doserver(s, "deliveryTasksResult");
                XNResultModel resmod = DoService.Doserver(s, "deliveryTasksResult");
                return resmod;
            }
            else
            {
                throw new Exception("systemCode键值不正确" + head["systemCode"]);
            }
        }

    }
}
