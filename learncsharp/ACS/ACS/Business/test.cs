using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.Business
{
    class test
    {

        static void DownSTask()
        {
            string sql = string.Format(@"SELECT ts.*,c.ItemName FROM dbo.T_Task_Son ts
                                    LEFT JOIN dbo.T_Type_Config c ON ts.STaskType = c.ItemNo
                                    WHERE State IN(1, 2, 3) ORDER BY AgvNo, SerialNo");


            DataSet ds = DbHelperSQL.Query(sql);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                GetObject.GetSTask(dr);
            }
        }
    }
}
