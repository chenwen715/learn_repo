using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace XNWMS
{
    public class SNInfoWcf : ISNInfoWcf
    {
        public ResultModel DoLoadTasks(Stream steam)
        {

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            StreamReader sr = new StreamReader(steam);
            string s = sr.ReadToEnd();
            LogWrite.WriteLogToMain(s+"\n");
            ResultModel resmod = DoService.Doserver(s, "palletPackInfo");
            return resmod;
        }

        public ResultModel DoLoadTasks1(Stream steam)
        {

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            StreamReader sr = new StreamReader(steam);
            string s = sr.ReadToEnd();
            LogWrite.WriteLogToMain(s + "\n");
            ResultModel resmod = DoService.Doserver(s, "SNInfo");
            return resmod;
        }

        public ResultModel DoLoadTasks2(Stream steam)
        {

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            StreamReader sr = new StreamReader(steam);
            string s = sr.ReadToEnd();
            LogWrite.WriteLogToMain(s + "\n");
            ResultModel resmod = DoService.Doserver(s, "loadTasksResult");
            return resmod;
        }

        public ResultModel DoLoadTasks3(Stream steam)
        {

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            StreamReader sr = new StreamReader(steam);
            string s = sr.ReadToEnd();
            LogWrite.WriteLogToMain(s + "\n");
            ResultModel resmod = DoService.Doserver(s, "stationInfo");
            return resmod;

        }

        public ResultModel DoLoadTasks4(Stream steam)
        {

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            StreamReader sr = new StreamReader(steam);
            string s = sr.ReadToEnd();
            LogWrite.WriteLogToMain(s + "\n");
            ResultModel resmod = DoService.Doserver(s, "deliveryTasksResult");
            return resmod;
        }
    }
}
