using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageTranslate
{
   
    public class LogWrite
    {
        public void WriteLog(string filePath, string content)
        {
            try
            {
                string FilePath = "D:\\Log\\" + DateTime.Now.ToString("yyyyMMdd") + filePath;

                if (File.Exists(FilePath))
                {
                    FileStream aFile = new FileStream(FilePath, FileMode.Append);
                    StreamWriter sw = new StreamWriter(aFile, System.Text.Encoding.UTF8);
                    sw.WriteLine(DateTime.Now.ToLongTimeString() + ": " + content);
                    sw.Close();
                }
                else//文件不存在
                {
                    FileStream aFile = new FileStream(FilePath, FileMode.Create);
                    StreamWriter sw = new StreamWriter(aFile, System.Text.Encoding.UTF8);
                    sw.WriteLine(DateTime.Now.ToLongTimeString() + ": " + content);
                    sw.Close();
                }
            }
            catch { }
        }

        public delegate void ErrorLogDelegate(string log);
        public static ErrorLogDelegate ErrorLog;
        public static bool WriteError(string content)
        {
            if (ErrorLog != null)
                ErrorLog(content);
            return true;
        }


        public delegate void WriteLogDelegate(string log);
        public static WriteLogDelegate WriteLogBack;
        public static bool WriteLogToMain(string content)
        {
            if (WriteLogBack != null)
                WriteLogBack(content);
            return true;
        }

        public static ErrorLogDelegate ErrorLogStk;
        public static bool WriteErrorStk(string content)
        {
            if (ErrorLogStk != null)
                ErrorLogStk(content);
            return true;
        }


        public static WriteLogDelegate WriteLogBackStk;
        public static bool WriteLogToMainStk(string content)
        {
            if (WriteLogBackStk != null)
                WriteLogBackStk(content);
            return true;
        }


    }
    
}
