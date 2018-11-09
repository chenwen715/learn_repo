using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learn_180328_file
{
    class WriteLog
    {
        public static void createFile(string path, string filename)
        {
            string filePath = path + "\\" + filename;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
                Console.WriteLine("创建成功");
            }
            else
            {
                Console.WriteLine("已存在该文件");
            }
            Console.ReadKey();          
        }

        public static void createFileInfo(string path, string filename)
        {
            string filePath = path + "\\" + filename;
            DirectoryInfo dirctoryInfo = new DirectoryInfo(path);
            FileInfo fileInfo=new FileInfo(filePath);
            if (!dirctoryInfo.Exists)
            {
                dirctoryInfo.Create();
            }
            if (!fileInfo.Exists)
            {
                fileInfo.Create();
                Console.WriteLine("创建成功");
            }
            else
            {
                Console.WriteLine("已存在该文件");
            }
            
            //string filename1=fileInfo.FullName;
            //Console.WriteLine(filename1);
            //string fileextend = fileInfo.Extension.Trim();
            //Console.WriteLine(fileextend.Substring(1,fileextend.Length-1));
            //string fileparent = dirctoryInfo.Parent.FullName;
            //Console.WriteLine(fileparent);
            //Console.ReadKey();
        }

        public static void LogWrite(string file,string content)
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Append, FileAccess.Write);
                StreamWriter sWriter = new StreamWriter(fs);
                sWriter.Write(content);
                sWriter.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                FileInfo fileInfo = new FileInfo(file);
                string name=DateTime.Now.ToString().Replace(" ","_").Replace(":","_").Replace("/","_")+"_log.txt";
                //createFileInfo(fileInfo.DirectoryName,name );
                //FileStream fs = new FileStream(fileInfo.DirectoryName + "\\" + "log.txt", FileMode.Append, FileAccess.Write);
                StreamWriter sWriterLog = new StreamWriter(fileInfo.DirectoryName + "\\" + "log.txt",true);
                sWriterLog.WriteLine(DateTime.Now.ToString() + ":" + ex.ToString());
                sWriterLog.Close();

            }
            



        }
    }
}
