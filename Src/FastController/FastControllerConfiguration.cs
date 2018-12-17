using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastController
{
    public class FastControllerConfiguration
    {
        public string[] Methods { get; }
        public string ResponseContextType { get; }
        public IFormattor Formattor { get; }

        public static FastControllerConfiguration Default { get; set; } = new FastControllerConfiguration("application/json", new JsonFormattor(), "Post", "Get");

        public FastControllerConfiguration(string responseContextType, IFormattor formattor)
        {
            ResponseContextType = responseContextType;
            Formattor = formattor;
        }

        public FastControllerConfiguration(string responseContextType, IFormattor formattor, params string[] methods)
        {
            Methods = methods;
            ResponseContextType = responseContextType;
            Formattor = formattor;
        }
    }
}
