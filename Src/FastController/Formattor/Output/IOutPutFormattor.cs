namespace FastController.Formattor.Output
{
    public interface IOutPutFormattor
    {
        bool IsConvert(string contextType);
        string Serialization(object entity);
    }
}
