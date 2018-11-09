using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learn_180402_WorkStation
{
    class SqlMethod
    {
        public static DataSet GetData(string cmsql)
        {
            SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.Sql);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(cmsql, sqlConnection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.SelectCommand = sqlCommand;
            DataSet ds = new DataSet();
            sqlDataAdapter.Fill(ds);
            sqlConnection.Close();
            sqlConnection.Dispose();
            return ds;
        }

        public static int AffectRows(string cmsql)
        {
            SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.Sql);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(cmsql, sqlConnection);
            int count = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            sqlConnection.Dispose();
            return count;
        }
    }
}
