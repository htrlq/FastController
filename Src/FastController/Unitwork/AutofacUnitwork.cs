using Autofac;
using FastController.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastController.Unitwork
{
    public class AutofacUnitwork
    {
        public static AutofacUnitwork Instance { get; } = new AutofacUnitwork();

        private IContainer Container { get; set; }

        private ContainerBuilder Builder { get; } = new ContainerBuilder();

        public AutofacUnitwork AddScope<TServer, TImplement>() where TImplement : class, TServer
        {
            Builder.AddScope<TServer, TImplement>();

            return this;
        }

        public AutofacUnitwork AddTransient<TServer, TImplement>() where TImplement : class, TServer
        {
            Builder.AddTransient<TServer, TImplement>();

            return this;
        }

        public AutofacUnitwork AddSingleton<TServer, TImplement>() where TImplement : class, TServer
        {
            Builder.AddSingleton<TServer, TImplement>();

            return this;
        }

        public AutofacUnitwork AddSingleton<TImplement>(TImplement implement) where TImplement : class
        {
            Builder.AddSingleton<TImplement>(implement);

            return this;
        }

        public TServer GetServer<TServer>()
        {
            return Container.Resolve<TServer>();
        }

        public TServer GetRequiredService<TServer>()
        {
            var server = Container.Resolve<TServer>();
            var resultServer = server != null ? server : throw new Exception($"Not TImplement:{typeof(TServer).FullName}");

            return resultServer;
        }

        public IContainer Build()
        {
            Container = Builder.Build();

            return Container;
        }
    }
}
