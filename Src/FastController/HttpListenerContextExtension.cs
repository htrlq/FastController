using System.Net;

namespace FastController
{
    public static class HttpListenerContextExtension
    {
        public static void Empty(this HttpListenerContext context)
        {
            var response = context.Response;
            var empty = "response is empty";
            byte[] emptyBytes = empty.ToBytes();

            response.OutputStream.Write(emptyBytes, 0, emptyBytes.Length);
            response.Close();
        }

        public static void Json(this HttpListenerContext context)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.Close();
        }

        public static void Json(this HttpListenerContext context, FastControllerConfiguration configuration, object data)
        {
            var response = context.Response;
            var formattor = configuration.Formattor;
            var json = formattor.Serialization(data);
            var jsonBytes = json.ToBytes();

            response.ContentType = "application/json";

            response.OutputStream.Write(jsonBytes, 0, jsonBytes.Length);
            response.Close();
        }

        public static void File(this HttpListenerContext context, string contextType, byte[] fileBytes)
        {
            var response = context.Response;

            response.ContentType = contextType;
            response.OutputStream.Write(fileBytes, 0, fileBytes.Length);
            response.Close();
        }

        public static void Error(this HttpListenerContext context,string error)
        {
            var response = context.Response;
            byte[] errorBytes = error.ToBytes();

            response.StatusCode = 404;
            response.OutputStream.Write(errorBytes, 0, errorBytes.Length);
            response.Close();
        }
    }
}
