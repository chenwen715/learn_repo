using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace XNWMS
{
    [ServiceContract]
    public interface ISNInfoWcf
    {
        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "palletPackInfo")]
        //ResultModel DoLoadTasks(Stream steam);
        XNResultModel DoLoadTasks(Stream steam);

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "SNInfo")]
        //ResultModel DoLoadTasks1(Stream steam);
        XNResultModel DoLoadTasks1(Stream steam);

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "loadTasksResult")]
        //ResultModel DoLoadTasks2(Stream steam);
        XNResultModel DoLoadTasks2(Stream steam);

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "stationInfo")]
        //ResultModel DoLoadTasks3(Stream steam);
        XNResultModel DoLoadTasks3(Stream steam);

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "deliveryTasksResult")]
        //ResultModel DoLoadTasks4(Stream steam);
        XNResultModel DoLoadTasks4(Stream steam);

    }
}
