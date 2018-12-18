using FastController.Unitwork;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace FastController
{
    public partial class FastControllerFactory : IFastControllerFactory
    {
        private ConcurrentDictionary<string, IFastController> dictionary = new ConcurrentDictionary<string, IFastController>();

        static FastControllerFactory()
        {
            AutofacUnitwork.Instance.AddSingleton(FastControllerConfiguration.Default);
            AutofacUnitwork.Instance.Build();
        }

        public bool RegisterInstance<TService>(TService service) where TService : IFastController
        {
            var attribute = service.GetAttribute<RouteAttribute>();
            return dictionary.TryAdd(attribute?.Url ?? "Home", service);
        }

        public bool Register<TService>() where TService : IFastController
        {
            var service = Activator.CreateInstance<TService>();
            var attribute = service.GetAttribute<RouteAttribute>();
            return dictionary.TryAdd(attribute?.Url ?? "Home", service);
        }

        public void UseUrl(string url)
        {
            HttpListener listener = new HttpListener();

            listener.Prefixes.Add(url);
            BeginStart(listener);
        }

        public void Use(string host,int port)
        {
            HttpListener listener = new HttpListener();

            listener.Prefixes.Add($"http://{host}:{port}/");
            BeginStart(listener);
        }

        delegate void StartAsyncCallback(HttpListener listener);

        private void BeginStart(HttpListener listener)
        {
            var asyncCallback = new StartAsyncCallback(Start);

            asyncCallback.BeginInvoke(listener, null, null);
        }

        private void Start(HttpListener listener)
        {
            listener.Start();

            while (true)
            {
                listener.BeginGetContext(Context, listener);

                Thread.Sleep(1000);
            }
        }

        delegate void ExecuteAsyncCallback(HttpListenerContext context);

        private void Context(IAsyncResult ar)
        {
            if (ar.AsyncState is HttpListener listener)
            {
                var context = listener.EndGetContext(ar);
                var rawUrl = context.Request.RawUrl;
                var first = dictionary.First(item => rawUrl.Contains(item.Key));

                if (!string.IsNullOrWhiteSpace(first.Key))
                {
                    var controller = first.Value;

                    var call = new ExecuteAsyncCallback(controller.Execute);
                    var beginInvoke = call.BeginInvoke(context, null, null);

                    call.EndInvoke(beginInvoke);
                }
            }
        }
    }

    public static class ConcurrentDictionaryExtension
    {
        public static KeyValuePair<TKey,TValue> First<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, bool> where)
        {
            foreach(var pair in dictionary)
            {
                if (where(pair))
                    return pair;
            }

            return new KeyValuePair<TKey, TValue>();
        }
    }
}
