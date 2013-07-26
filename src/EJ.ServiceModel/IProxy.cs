namespace EJ.ServiceModel
{
    public interface IProxy
    {
        T Get<T>(string hostUrl) where T : class;
    }
}
