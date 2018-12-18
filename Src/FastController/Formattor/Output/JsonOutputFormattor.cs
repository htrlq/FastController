using Newtonsoft.Json;

namespace FastController.Formattor.Output
{
    public class JsonOutputFormattor:IOutPutFormattor
    {
        private string ContextType { get; } = "application/json";

        public bool IsConvert(string contextType)
        {
            return ContextType.Contains(contextType);
        }

        public string Serialization(object entity)
        {
            return JsonConvert.SerializeObject(entity);
        }
    }
}
