using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using ImpromptuInterface;
using ImpromptuInterface.Dynamic;
using RestSharp;

namespace EJ.ServiceModel
{
    public static class ObjectCreator
    {
        public static object CreateObject<T>() where T: class
        {
            dynamic tNew = new ImpromptuDictionary();
            var rets = GetNancyOperation(typeof (T));
            foreach (var ret in rets)
            {
                tNew[ret.Key] = ret.Value;
            }

            return Impromptu.ActLike<T>(tNew);
        }

        public static Dictionary<string, object> GetNancyOperation(Type type)
        {
            var baseUrl = "http://localhost:60770/";
            var modulePath = type.Name.Remove(0, 1).Replace("Module", "");

            var ret = new Dictionary<string, object>();
            object value = null;

            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (MethodInfo method in methods)
            {
                var methodName = method.Name;
                var parameters = method.GetParameters();
                var nancyAttr = method.GetCustomAttribute<NancyOperationContractAttribute>();
                if (nancyAttr == null)
                {
                    continue;
                }
                switch (nancyAttr.Method)
                {
                    case Method.GET:
                        if (methodName == "GetIndex")
                        {
                            methodName = string.Empty;
                            value = GetValue(baseUrl, modulePath, RestSharp.Method.GET);
                        }
                        Debug.WriteLine("[GET] " + baseUrl + modulePath + "/" + methodName);
                        break;
                    case Method.POST:
                        Debug.WriteLine("[POST] " + baseUrl + modulePath + "/" + methodName);
                        break;
                }

                ret.Add(method.Name, value);
            }

            return ret;
        }

        static Func<string> GetValue(string baseUrl, string modulePath, RestSharp.Method method)
        {
            var ret = RestServiceInvoke(baseUrl, modulePath, method);
            return () =>  "Hello World" ;
        }

        public static object RestServiceInvoke(string url, string resource, RestSharp.Method method)
        {
            //var baseUrl = "http://localhost:60770/";
            var client = new RestClient(url);

            var request = new RestSharp.RestRequest(resource, method);

            var ret = client.Execute(request);

            return ret;
        }
    }
}
