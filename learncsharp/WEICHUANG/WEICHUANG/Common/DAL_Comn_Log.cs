using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEICHUANG.Common
{
    class DAL_Comn_Log
    {
        //软件本身异常信息，便于程序调试
        public string Path_Error { get; set; }
        //日志信息：记录详细的信息流过程
        public string Path_Log { get; set; }

        public DAL_Comn_Log()
        {
            Path_Error = System.Windows.Forms.Application.StartupPath + "\\ErrorMessage";
            Path_Log = System.Windows.Forms.Application.StartupPath + "\\Log";
        }


        ///<summary>
        /// 目录检查
        /// </summary>
        public void DirectoryCheck()
        {

            if (!Directory.Exists(Path_Error))//异常信息
            {
                Directory.CreateDirectory(Path_Error);
            }

            if (!Directory.Exists(Path_Log))//Log信息
            {
                Directory.CreateDirectory(Path_Log);
            }
        }

        /// <summary>
        /// 向指定文件写入信息
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <param name="Appended">是否追加文件信息</param>
        /// <param name="LogInfo">信息内容</param>
        public void MessageWriter(string FilePath, bool Appended, string LogInfo)
        {
            using (StreamWriter sw = new StreamWriter(FilePath, Appended))
            {
                sw.WriteLine(DateTime.Now.ToString("HH:mm:ss fff") + " " + LogInfo);
                sw.Close();
            }
        }

        /// <summary>
        /// 日志信息
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="Message">信息内容</param>
        public void MessageLog(string fileName, string Message)
        {
            DirectoryCheck();
            string pathName = Path_Log + "\\"
                + DateTime.Now.ToString("yyyy_MM_dd")
                + fileName + ".txt";
            MessageWriter(pathName, true, Message);
        }
    }
}


