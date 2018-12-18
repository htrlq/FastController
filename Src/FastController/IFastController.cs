using System;
using System.Net;

namespace FastController
{
    public interface IFastController
    {
        void Execute(HttpListenerContext context);
    }
}
