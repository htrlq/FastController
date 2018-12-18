using System;
using System.Linq;
using System.Net;

namespace FastController
{
    public class HttpGetParam : HttpParam
    {
        public HttpGetParam(HttpListenerContext context) : base(context, "Get")
        {
        }

        protected override TEntity Converter<TEntity>()
        {
            if (Context.Request.HttpMethod.Equals(Method, StringComparison.InvariantCultureIgnoreCase))
            {
                var query = Context.Request.QueryString;
                var entity = Activator.CreateInstance<TEntity>();
                var type = entity.GetType();

                foreach(var propety in type.GetProperties())
                {
                    var findKey = query.AllKeys.FirstOrDefault(key => key.Equals(propety.Name, StringComparison.InvariantCultureIgnoreCase));

                    if (!string.IsNullOrWhiteSpace(findKey))
                    {
                        var value = Convert.ChangeType(query[findKey], propety.PropertyType);

                        propety.SetValue(entity, value, null);
                    }
                }

                return entity;
            }

            throw new Exception("Http Methos Check is fail");
        }
    }
}
