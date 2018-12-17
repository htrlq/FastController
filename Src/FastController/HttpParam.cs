using System;
using System.Net;
using System.Text;

namespace FastController
{
    public abstract class HttpParam
    {
        protected HttpListenerContext Context { get; set; }
        protected FastControllerConfiguration Configuration { get; }
        protected string Method { get; }

        public HttpParam(HttpListenerContext context, FastControllerConfiguration configuration, string method)
        {
            Context = context;
            Configuration = configuration;
            Method = method;
        }

        protected virtual TEntity Converter<TEntity>() where TEntity : class
        {
            if (Context.Request.HttpMethod.Equals(Method, StringComparison.InvariantCultureIgnoreCase))
            {
                var formattor = Configuration.Formattor;
                return formattor.Deserialization<TEntity>(Context);
            }

            throw new Exception("Http Methos Check is fail");
        }

        public TEntity GetValue<TEntity>() where TEntity : class
        {
            return Converter<TEntity>();
        }

        public void Empty()
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

        public void Json()
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

        public void Json(object data)
        {
            try
            {
                var response = Context.Response;
                var formattor = Configuration.Formattor;
                var json = formattor.Serialization(data);
                var jsonBytes = json.ToBytes();

                response.ContentType = "application/json";

                response.OutputStream.Write(jsonBytes, 0, jsonBytes.Length);
                response.Close();
            }
            catch
            {

            }
        }

        public void File(string contextType, byte[] fileBytes)
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

    internal static class StringExtension
    {
        public static byte[] ToBytes(this string str, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            return encoding.GetBytes(str);
        }

    }
}
