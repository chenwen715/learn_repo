using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using XNWMS.Common;

namespace XNWMS
{
    public class SNInfoWcf : ISNInfoWcf
    {
        DAL_Comn_Log comnlog = new DAL_Comn_Log();
        public ResultModel DoLoadTasks(Stream steam)
        {

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            StreamReader sr = new StreamReader(steam);
            string s = sr.ReadToEnd();
            LogWrite.WriteLogToMain(s+"\n");
            comnlog.MessageLog("上传数据", s + "\n");
            ResultModel resmod = DoService.Doserver(s, "palletPackInfo");
            return resmod;
        }

        public ResultModel DoLoadTasks1(Stream steam)
        {

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            StreamReader sr = new StreamReader(steam);
            string s = sr.ReadToEnd();
            LogWrite.WriteLogToMain(s + "\n");
            comnlog.MessageLog("上传数据", s + "\n");
            ResultModel resmod = DoService.Doserver(s, "SNInfo");
            return resmod;
        }

        public ResultModel DoLoadTasks2(Stream steam)
        {

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            StreamReader sr = new StreamReader(steam);
            string s = sr.ReadToEnd();
            LogWrite.WriteLogToMain(s + "\n");
            comnlog.MessageLog("上传数据", s + "\n");
            ResultModel resmod = DoService.Doserver(s, "loadTasksResult");
            return resmod;
        }

        public ResultModel DoLoadTasks3(Stream steam)
        {

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            StreamReader sr = new StreamReader(steam);
            string s = sr.ReadToEnd();
            LogWrite.WriteLogToMain(s + "\n");
            comnlog.MessageLog("上传数据", s + "\n");
            ResultModel resmod = DoService.Doserver(s, "stationInfo");
            return resmod;

        }

        public ResultModel DoLoadTasks4(Stream steam)
        {

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            StreamReader sr = new StreamReader(steam);
            string s = sr.ReadToEnd();
            LogWrite.WriteLogToMain(s + "\n");
            comnlog.MessageLog("上传数据", s + "\n");
            ResultModel resmod = DoService.Doserver(s, "deliveryTasksResult");
            return resmod;
        }

    }
}
