using System.Net;

namespace FastController
{
    public class HttpPostParam : HttpParam
    {
        public HttpPostParam(HttpListenerContext context, FastControllerConfiguration configuration) : base(context,configuration, "Post")
        {
        }
    }
}
