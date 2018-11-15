using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketAutoTest
{
    class WriteLog
    {
        public string Path_Log = string.Format(@"C:\Users\0322\Desktop\友星\友星ACS\友星ACS\log");
        

        ///<summary>
        /// 目录检查
        /// </summary>
        public void DirectoryCheck()
        {
           
            //Log信息
            if (!Directory.Exists(Path_Log)) Directory.CreateDirectory(Path_Log);
            
        }

        /// <summary>
        /// 向指定文件写入信息
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <param name="Appended">是否追加文件信息</param>
        /// <param name="LogInfo">信息内容</param>
        public void MessageWriter(string fileName, bool Appended, string LogInfo)
        {
            DirectoryCheck();
            try
            {
                string pathName = Path_Log + "\\" + fileName+"_"+DateTime.Now.ToString("yyyy_MM_dd_HH") + ".txt";
                using (StreamWriter sw = new StreamWriter(pathName, Appended))
                {
                    sw.WriteLine(LogInfo);
                    //sw.WriteLine(DateTime.Now.ToString("HH:mm:ss fff") + "==" + LogInfo);
                    sw.Close();
                }
            }
            catch (Exception Ex)
            {
                
            }
        }

       
      
    }
}
