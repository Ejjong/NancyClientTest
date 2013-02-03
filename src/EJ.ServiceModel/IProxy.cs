namespace EJ.ServiceModel
{
    public interface IProxy
    {
        T Get<T>() where T : class;
    }
}
