using FastController.Unitwork;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Reflection;

namespace FastController
{
    public class FastControllerBase : IFastController
    {
        private ConcurrentDictionary<string, MethodInfo> dictionary = new ConcurrentDictionary<string, MethodInfo>();

        public FastControllerBase()
        {
            var type = GetType();
            var selfAttribute = this.GetAttribute<RouteAttribute>();
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            foreach(var method in methods)
            {
                var attribute = method.GetCustomAttributes(true).FirstOrDefault(item => item.GetType() == typeof(RouteAttribute)) as RouteAttribute;

                if (attribute != null && CheckMethod(method))
                {
                    var url = attribute.Url;

                    if (!url.Contains($"{selfAttribute.Url}"))
                    {
                        url = $"{selfAttribute.Url}/{attribute.Url}";
                    }

                    dictionary.TryAdd(url, method);
                }
            }
        }

        private bool CheckMethod(MethodInfo method)
        {
            var parameters = method.GetParameters();

            return parameters.Length == 1 && ( parameters[0].ParameterType == typeof(HttpListenerContext) || typeof(HttpParam).IsAssignableFrom(parameters[0].ParameterType));
        }


        delegate void ExecuteContextCallback(MethodInfo method, HttpListenerContext context, object instance);

        private void ExecuteContextAsync(MethodInfo method, HttpListenerContext context, object instance)
        {
            method.Invoke(instance, new[]{
                context
            });
        }

        delegate void ExecuteHttpCallback(MethodInfo method, HttpParam context, object instance);

        private void ExecuteHttpAsync(MethodInfo method, HttpParam context, object instance)
        {
            method.Invoke(instance, new[]{
                context
            });
        }

        public void Execute(HttpListenerContext context)
        {
            var request = context.Request;
            var rawUrl = request.RawUrl.Contains("?") ? request.RawUrl.Split("?".ToCharArray())[0] : request.RawUrl;

            if (dictionary.ContainsKey(rawUrl))
            {
                try
                {
                    var method = dictionary[rawUrl];
                    var firstParams = method.GetParameters().FirstOrDefault();

                    if (firstParams.ParameterType == typeof(HttpListenerContext))
                    {
                        var callback = new ExecuteContextCallback(ExecuteContextAsync);
                        var beginInvoke = callback.BeginInvoke(method, context, this, null, null);

                        callback.EndInvoke(beginInvoke);
                    }
                    else if (typeof(HttpParam).IsAssignableFrom(firstParams.ParameterType))
                    {
                        var Configuration = AutofacUnitwork.Instance.GetServer<FastControllerConfiguration>();

                        if (Configuration.Methods.Any(item => item.Equals(request.HttpMethod, StringComparison.InvariantCultureIgnoreCase)))
                        {

                            var paramContext = Activator.CreateInstance(firstParams.ParameterType, new object[]{
                                context
                            }) as HttpParam;

                            var callback = new ExecuteHttpCallback(ExecuteHttpAsync);
                            var beginInvoke = callback.BeginInvoke(method, paramContext, this, null, null);

                            callback.EndInvoke(beginInvoke);
                        }
                    }
                    else
                    {
                        context.Error("Not impl param");
                    }
                }
                catch(Exception ex)
                {
                    context.Error(ex.InnerException.Message);
                }
            }
        }
    }
}
