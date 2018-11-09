using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learn_180330_Graphics_1
{
    class Method
    {
        public static int ConnectSql(string s)
        {
            try
            {
                string sqlString = "server=.;database=Vote;uid=sa;pwd=abc123*";
                SqlConnection sConnection = new SqlConnection(sqlString);
                sConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(s, sConnection);
                int num = int.Parse(sqlCommand.ExecuteScalar().ToString());
                sConnection.Close();
                return num;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
           
        }

        public static DataSet GetData(string s)
        {
            try
            {
                string sqlString = "server=.;database=Vote;uid=sa;pwd=abc123*";
                SqlConnection sConnection = new SqlConnection( sqlString);
                sConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(s, sConnection);
                SqlDataAdapter sdApart = new SqlDataAdapter();
                sdApart.SelectCommand = sqlCommand;
                DataSet ds = new DataSet();
                sdApart.Fill(ds, "DATA");
                //sConnection.Close();
                return ds;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            


        }
    }
}
