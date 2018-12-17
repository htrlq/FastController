using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace FastController
{
    public interface IFormattor
    {
        string ContextType { get; }
        string Serialization(object entity);
        TEntity Deserialization<TEntity>(HttpListenerContext context) where TEntity : class;
    }

    public class JsonFormattor : IFormattor
    {
        public string ContextType => "application/json";

        public TEntity Deserialization<TEntity>(HttpListenerContext context) where TEntity : class
        {
            var request = context.Request;

            if (request.ContentType.Contains(ContextType))
            {
                try
                {
                    if (request.HasEntityBody)
                    {
                        var body = request.InputStream;

                        using (var textRead = new StreamReader(body))
                        {
                            using (var jsonReadStream = new JsonTextReader(textRead))
                            {
                                JsonSerializer jsonSerializer = new JsonSerializer();
                                return jsonSerializer.Deserialize<TEntity>(jsonReadStream);
                            }
                        }
                    }
                    else
                    {
                        var query = request.QueryString;
                        var keys = query.AllKeys;
                        var entity = Activator.CreateInstance<TEntity>();
                        var type = typeof(TEntity);

                        foreach (var property in type.GetProperties())
                        {
                            var findKey = keys.FirstOrDefault(key => key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));
                            if (!string.IsNullOrWhiteSpace(findKey))
                            {
                                var value = Convert.ChangeType(query[findKey], property.PropertyType);
                                property.SetValue(entity, value, null);
                            }
                        }
                    }
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        public string Serialization(object entity)
        {
            return JsonConvert.SerializeObject(entity);
        }
    }
}
