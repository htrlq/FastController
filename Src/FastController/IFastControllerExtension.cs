using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastController
{
    public interface IFastControllerFactory
    {
        bool RegisterInstance<TService>(TService service) where TService : IFastController;
        bool Register<TService>() where TService : IFastController;
    }

    public static class IFastControllerExtension
    {
        public static TAttribute GetAttribute<TAttribute>(this IFastController service)
                                                                                      where TAttribute:Attribute
        {
            var type = service.GetType();
            return type.GetCustomAttributes(true).FirstOrDefault(item => item.GetType() == typeof(TAttribute)) as TAttribute;
        }
    }
}
