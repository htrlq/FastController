using System.Net;

namespace FastController.Formattor.Input
{
    public interface IInputFormattor
    {
        bool IsConvert(HttpListenerContext context);
        TEntity Deserialization<TEntity>(HttpListenerContext context) where TEntity : class;
    }
}
