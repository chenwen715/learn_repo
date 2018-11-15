using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map_181115
{
    class DAL_Comn_Sql
    {
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
    }
}
