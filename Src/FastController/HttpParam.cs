using FastController.Unitwork;
using System;
using System.Net;
using System.Text;

namespace FastController
{
    public abstract class HttpParam
    {
        protected HttpListenerContext Context { get; set; }
        protected string Method { get; }

        public HttpParam(HttpListenerContext context, string method)
        {
            Context = context;
            Method = method;
        }

        protected virtual TEntity Converter<TEntity>() where TEntity : class
        {
            if (Context.Request.HttpMethod.Equals(Method, StringComparison.InvariantCultureIgnoreCase))
            {
                var Configuration = AutofacUnitwork.Instance.GetServer<FastControllerConfiguration>();

                foreach (var formattor in Configuration.InputFormattors)
                {
                    if (formattor.IsConvert(Context))
                    {
                        return formattor.Deserialization<TEntity>(Context);
                    }
                }
            }

            throw new Exception("Http Methos Check is fail");
        }

        public TEntity GetValue<TEntity>() where TEntity : class
        {
            return Converter<TEntity>();
        }

        public void Empty()
        {
            Context.Empty();
        }

        public void Json()
        {
            Context.Json();
        }

        public void Json(object data)
        {
            Context.Json(data);
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
