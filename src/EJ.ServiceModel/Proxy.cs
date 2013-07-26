
namespace EJ.ServiceModel
{
    public class Proxy : IProxy
    {
        public T Get<T>(string hostUrl) where T : class
        {
            var obj = (T)ObjectCreator.CreateObject<T>(hostUrl);
            return obj;
        }
    }
}
