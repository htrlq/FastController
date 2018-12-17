using System.Net;

namespace FastController
{
    public class HttpGetParam : HttpParam
    {
        public HttpGetParam(HttpListenerContext context, FastControllerConfiguration configuration) : base(context,configuration, "Get")
        {
        }
    }
}
