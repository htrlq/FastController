using Autofac;

namespace FastController.Extension
{
    public static class ContainerBuilderExtension
    {
        public static ContainerBuilder AddScope<TServer, TImplement>(this ContainerBuilder builder) where TImplement : class, TServer
        {
            builder.RegisterType<TImplement>().As<TServer>().InstancePerLifetimeScope();

            return builder;
        }

        public static ContainerBuilder AddTransient<TServer, TImplement>(this ContainerBuilder builder) where TImplement : class, TServer
        {
            builder.RegisterType<TImplement>().As<TServer>().InstancePerDependency();

            return builder;
        }

        public static ContainerBuilder AddSingleton<TServer, TImplement>(this ContainerBuilder builder) where TImplement : class, TServer
        {
            builder.RegisterType<TImplement>().As<TServer>().SingleInstance();

            return builder;
        }

        public static ContainerBuilder AddSingleton<TImplement>(this ContainerBuilder builder, TImplement implement) where TImplement : class
        {
            builder.Register(c=> implement).SingleInstance();

            return builder;
        }
    }
}
