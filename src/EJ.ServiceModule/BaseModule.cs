using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EJ.ServiceModel;
using Nancy;

namespace EJ.ServiceModule
{
    public class BaseModule : NancyModule
    {
        public BaseModule() : this(String.Empty) { }
        public BaseModule(string modulePath)
            : base(modulePath)
        {
            var type = this.GetType();
            var interf = type.GetInterface("I" + type.Name);
            if (interf == null) return;
            var methods = interf.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (MethodInfo method in methods)
            {
                var attr = method.GetCustomAttribute<NancyOperationContractAttribute>();
                if (attr == null || attr.Method != Method.GET)
                {
                    continue;
                }
                var parameters = method.GetParameters();
                var name = method.Name.Remove(0, 3);
                var path = name == "Index" ? "/" : "/" + name;
                {
                    if (!parameters.Any())
                    {
                        Get[path] = p => method.Invoke(this, null);
                    }
                    else
                    {
                        var paramStr = string.Empty;
                        var paramInfos = new Dictionary<Type, object>();
                        foreach (var parameter in parameters)
                        {
                            paramStr += "/{" + parameter.Name + "}";
                            paramInfos.Add(parameter.ParameterType, parameter.Name);
                        }
                        path += paramStr;

                        Get[path] = p => method.Invoke(this, GenerateParams(p, paramInfos));
                    }
                }
            }
        }

        public object[] GenerateParams(dynamic param, Dictionary<Type, object> pInfos)
        {
            var ret = new object[pInfos.Count];
            int i = 0;
            foreach (var pInfo in pInfos)
            {
                Type type = pInfo.Key;
                if (type == typeof(string))
                {
                    ret[i] = (string)param[pInfo.Value.ToString()];
                }
                else if (type == typeof(int))
                {
                    ret[i] = (int)param[pInfo.Value.ToString()];
                }
                else if (type == typeof(TestModel))
                {
                    ret[i] = (TestModel)param[pInfo.Value.ToString()];
                }
                i++;
            }
            return ret;
        }
    }
}
