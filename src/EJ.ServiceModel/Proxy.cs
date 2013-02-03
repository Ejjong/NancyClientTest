using System;
using System.Linq.Expressions;
using System.Reflection;

namespace EJ.ServiceModel
{
    public class Proxy : IProxy
    {
        public T Get<T>() where T : class
        {
            T Obj = default(T);
            if (Obj == null)
            {
                if (typeof (T).IsInterface)
                {
                    var mock = new Moq.Mock<IHelloModule>();
                    mock.Setup(foo => foo.GetIndex()).Returns("Hello World");
                    return (T) mock.Object;
                }

                Obj = (T) ObjectCreator.CreateObject(typeof (T));
            }
            return Obj;
        }
    }
}
