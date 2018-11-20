using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNWMS
{
    class DAL_Comn_Sql
    {
        //public static string connectionString = Properties.Settings.Default.sql;
        public DataSet SelectGet(string CnString, string CmString)
        {
            DataSet ds = new System.Data.DataSet();
            SqlConnection sqlCn = null;
            SqlDataAdapter sqlDa = null;

            using (sqlCn = new SqlConnection(CnString))
            {
                sqlCn.Open();
                sqlDa = new SqlDataAdapter(CmString, sqlCn);
                sqlDa.Fill(ds);
                sqlCn.Close();
            }

            sqlCn.Dispose();
            sqlDa.Dispose();
            return ds;
        }
        /// <summary>
        /// 数据操作
        /// </summary>
        public int DataOperator(string CnString, string Cmstring)
        {
            int Line = 0;
            SqlCommand sqlCm = null;
            SqlConnection sqlCn = null;

            using (sqlCn = new SqlConnection(CnString))
            {
                sqlCn.Open();
                sqlCm = new SqlCommand(Cmstring, sqlCn);
                Line = sqlCm.ExecuteNonQuery();
                sqlCn.Close();
            }
            sqlCn.Dispose();
            sqlCm.Dispose();
            return Line;
        }

        public object GetData(string CnString, string Cmstring)
        {
            SqlCommand sqlCm = null;
            SqlConnection sqlCn = null;
            object a = null;

            using (sqlCn = new SqlConnection(CnString))
            {
                sqlCn.Open();
                sqlCm = new SqlCommand(Cmstring, sqlCn);
                a = sqlCm.ExecuteScalar();
                sqlCn.Close();
            }
            sqlCn.Dispose();
            sqlCm.Dispose();
            return a;
        }

        /// <summary>
        /// 调用存储过程Output参数方法
        /// </summary>
        /// <param name="procName">执行的存储过程语句</param>
        /// <param name="para">数据库的参数</param>
        /// <param name="OutName">定义Output参数</param>
        /// <returns>返回值</returns>
        public object ProcOutput(string CnString,string procName, SqlParameter[] paras, string OutName)
        {
            SqlConnection conn = new SqlConnection(CnString);//创建数据库连接对象
            conn.Open();//打开数据库连接

            //SqlCommand cmd = conn.CreateCommand();//创建并执行与此连接关联的T-Sql语句命令的对象，可以不用这段用下面两段
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            cmd.CommandType = CommandType.StoredProcedure;//设置此类型是存储过程类型
            cmd.CommandText = procName;//设置执行的存储过程//添加para参数
            if (paras != null)
            {
                foreach (SqlParameter parameter in paras)
                    cmd.Parameters.Add(parameter);
            }
            int n = cmd.ExecuteNonQuery();//执行语句并返回受影响的行数
            object o = cmd.Parameters[OutName].Value;//获取@CardID的值
            cmd.Parameters.Clear();
            conn.Close();//关闭连接
            return o;
        }

        public int  ProcOutput(string CnString, string procName, SqlParameter[] paras)
        {
            SqlConnection conn = new SqlConnection(CnString);//创建数据库连接对象
            conn.Open();//打开数据库连接

            //SqlCommand cmd = conn.CreateCommand();//创建并执行与此连接关联的T-Sql语句命令的对象，可以不用这段用下面两段
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            cmd.CommandType = CommandType.StoredProcedure;//设置此类型是存储过程类型
            cmd.CommandText = procName;//设置执行的存储过程//添加para参数
            if (paras != null)
            {
                foreach (SqlParameter parameter in paras)
                    cmd.Parameters.Add(parameter);
            }
            int n = cmd.ExecuteNonQuery();//执行语句并返回受影响的行数          
            conn.Close();//关闭连接
            return n;
        }

        public List<string> ProcOutput(string CnString, string procName, SqlParameter[] paras, string OutName1, string OutName2)
        {
            List<string> result = new List<string>();
            SqlConnection conn = new SqlConnection(CnString);//创建数据库连接对象
            conn.Open();//打开数据库连接

            //SqlCommand cmd = conn.CreateCommand();//创建并执行与此连接关联的T-Sql语句命令的对象，可以不用这段用下面两段
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            cmd.CommandType = CommandType.StoredProcedure;//设置此类型是存储过程类型
            cmd.CommandText = procName;//设置执行的存储过程//添加para参数
            if (paras != null)
            {
                foreach (SqlParameter parameter in paras)
                    cmd.Parameters.Add(parameter);
            }
            int n = cmd.ExecuteNonQuery();//执行语句并返回受影响的行数
            result.Add( cmd.Parameters[OutName1].Value.ToString());//获取@CardID的值
            result.Add(cmd.Parameters[OutName2].Value.ToString());
            //cmd.Parameters.Clear();
            conn.Close();//关闭连接
            return result;
        }
       
    }
}

