using FastController.Unitwork;
using System.Net;

namespace FastController
{
    public static class HttpListenerContextExtension
    {
        public static void Error(this HttpListenerContext context, string error)
        {
            try
            {
                var response = context.Response;
                byte[] errorBytes = error.ToBytes();

                response.StatusCode = 404;
                response.OutputStream.Write(errorBytes, 0, errorBytes.Length);
                response.Close();
            }
            catch
            {

            }
        }

        public static void Empty(this HttpListenerContext Context)
        {
            try
            {
                var response = Context.Response;
                var empty = "response is empty";
                byte[] emptyBytes = empty.ToBytes();

                response.OutputStream.Write(emptyBytes, 0, emptyBytes.Length);
                response.Close();
            }
            catch
            {

            }
        }

        public static void Json(this HttpListenerContext Context)
        {
            try
            {
                var response = Context.Response;
                response.ContentType = "application/json";
                response.Close();
            }
            catch
            {

            }
        }

        public static void Json(this HttpListenerContext Context, object data)
        {
            try
            {
                var response = Context.Response;

                var contextType = "application/json";

                var Configuration = AutofacUnitwork.Instance.GetServer<FastControllerConfiguration>();

                foreach (var formattor in Configuration.OutPutFormattors)
                {
                    if (formattor.IsConvert(contextType))
                    {
                        var json = formattor.Serialization(data);
                        var jsonBytes = json.ToBytes();
                        response.ContentType = contextType;

                        response.OutputStream.Write(jsonBytes, 0, jsonBytes.Length);
                        response.Close();
                    }
                }
            }
            catch
            {

            }
        }

        public static void File(this HttpListenerContext Context, string contextType, byte[] fileBytes)
        {
            try
            {
                var response = Context.Response;

                response.ContentType = contextType;
                response.OutputStream.Write(fileBytes, 0, fileBytes.Length);
                response.Close();
            }
            catch
            {

            }
        }
    }
}
