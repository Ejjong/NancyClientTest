using System;
using System.Linq.Expressions;
using System.Reflection;

namespace EJ.ServiceModel
{
    public class Proxy : IProxy
    {
        public T Get<T>() where T : class
        {
            var obj = (T)ObjectCreator.CreateObject<T>();
            return obj;
        }
    }
}
