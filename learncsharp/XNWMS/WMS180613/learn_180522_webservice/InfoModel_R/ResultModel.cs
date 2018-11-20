using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace learn_180522_webservice
{
    [DataContract]
    public class ResultModel
    {
        [DataMember]
        public bool result { get; set; }

        [DataMember]
        private string dateTime;
        public DateTime GetDateTime
        {
            get
            {
                return DateTime.ParseExact(this.dateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            }
            set
            {
                this.dateTime = value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            }
        }

        [DataMember]
        public string remark { get; set; }

        public static ResultModel GteRemark(bool GResult, DateTime GdateTime, string GRemark)
        {
            ResultModel resmod = new ResultModel();
            resmod.result = GResult;
            resmod.GetDateTime = GdateTime;
            resmod.remark = GRemark;
            return resmod;
        }
    }
}
