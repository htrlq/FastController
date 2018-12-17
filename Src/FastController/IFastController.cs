using System;
using System.Net;

namespace FastController
{
    public interface IFastController
    {
        void SetConfiguration(FastControllerConfiguration configuration);
        void Execute(HttpListenerContext context);
    }
}
