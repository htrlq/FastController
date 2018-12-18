using System;
using System.Linq;
using System.Net;

namespace FastController.Formattor.Input
{

    public class ParamInputFormattor : IInputFormattor
    {
        public TEntity Deserialization<TEntity>(HttpListenerContext context) where TEntity : class
        {
            var query = context.Request.QueryString;
            var entity = Activator.CreateInstance<TEntity>();
            var type = entity.GetType();

            foreach (var propety in type.GetProperties())
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

        public bool IsConvert(HttpListenerContext context)
        {
            return context.Request.QueryString != null && context.Request.QueryString.Count > 0;
        }
    }
}
