using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastController.Extension
{
    public static class IContainerExtension
    {
        public static TServer GetServer<TServer>(this IContainer container)
        {
            return container.Resolve<TServer>();
        }

        public static TServer GetRequiredService<TServer>(this IContainer container)
        {
            var server = container.Resolve<TServer>();
            var resultServer = server != null ? server : throw new Exception($"Not TImplement:{typeof(TServer).FullName}");

            return resultServer;
        }
    }
}
