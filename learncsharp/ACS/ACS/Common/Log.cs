using System;
using System.IO;

namespace ACS
{
    public class Log
    {
        //软件本身异常信息，便于程序调试
        public string Path_Error { get; set; }

        //日志信息：记录详细的信息流过程
        public string Path_Log { get; set; }

        //自动导出的excel路径
        public string Path_Excel { get; set; }
        //设备异常信息
        public string Path_Machine_Log { get; set; }

        public Log()
        {
            Path_Error = System.Windows.Forms.Application.StartupPath + "\\ErrorMessage";
            Path_Excel = System.Windows.Forms.Application.StartupPath + "\\Excel";
            Path_Log = System.Windows.Forms.Application.StartupPath + "\\Log";
            Path_Machine_Log = System.Windows.Forms.Application.StartupPath + "\\MachineLog";
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

            if (!Directory.Exists(Path_Excel))//Excel信息
            {
                Directory.CreateDirectory(Path_Excel);
            }
            if (!Directory.Exists(Path_Machine_Log))//Excel信息
            {
                Directory.CreateDirectory(Path_Machine_Log);
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
            //一般都在Log文件夹
            MessageWriter(App.ExFile.Path_Log + "\\" + fileName + "_" + DateTime.Now.ToString("yyyy_MM_dd") + ".txt", true, Message);

            App.WinMain.TBox_EMessage.Dispatcher.BeginInvoke(new Action(() =>
            {
                App.WinMain.TBox_EMessage.Text += fileName + "：" + Message + "\n";
                App.WinMain.TBox_EMessage.PageDown();
                if (App.WinMain.TBox_EMessage.LineCount > 100)
                {
                    App.WinMain.TBox_EMessage.Clear();
                }

            }));
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="Message">信息内容</param>
        public void MessageError(string fileName, string Message)
        {
            DirectoryCheck();
            //一般在ErrorMessage文件夹
            MessageWriter(App.ExFile.Path_Error + "\\" + DateTime.Now.ToString("yyyy_MM_dd") + ".txt", true, Message);

            App.WinMain.TBox_EMessage.Dispatcher.BeginInvoke(new Action(() =>
            {
                App.WinMain.TBox_EMessage.Text += Message + "\n";
                App.WinMain.TBox_EMessage.PageDown();
                if (App.WinMain.TBox_EMessage.LineCount > 100)
                {
                    App.WinMain.TBox_EMessage.Clear();
                }

            }));
        }
    }
}
