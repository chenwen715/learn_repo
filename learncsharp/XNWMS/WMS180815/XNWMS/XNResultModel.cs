using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace XNWMS
{
    //返回ZWCS的结果
    [DataContract]
    public class XNResultModel
    {
        [DataMember]
        public string StatusCode { get; set; }

        [DataMember]
        private dynamic Data;

        [DataMember]
        public string Info { get; set; }
    }
}
