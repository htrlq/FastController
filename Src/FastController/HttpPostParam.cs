using System.Net;

namespace FastController
{
    public class HttpPostParam : HttpParam
    {
        public HttpPostParam(HttpListenerContext context) : base(context, "Post")
        {
        }
    }
}
