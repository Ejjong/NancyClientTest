using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using ImpromptuInterface;
using ImpromptuInterface.Dynamic;
using RestSharp;

namespace EJ.ServiceModel
{
    public static class ObjectCreator
    {
        static readonly string baseUrl = "http://localhost:60770/";

        public static object CreateObject<T>() where T: class
        {
            dynamic impromptuDictionary = new ImpromptuDictionary();
            var nOps = GetNancyOperation(typeof (T));
            foreach (var nOp in nOps)
            {
                impromptuDictionary[nOp.Key] = nOp.Value;
            }

            return Impromptu.ActLike<T>(impromptuDictionary);
        }

        public static Dictionary<string, object> GetNancyOperation(Type type)
        {
           
            var modulePath = type.Name.Remove(0, 1).Replace("Module", "");

            var ret = new Dictionary<string, object>();
            object value = null;

            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (MethodInfo method in methods)
            {
                var methodName = method.Name;
                var parameters = method.GetParameters();
                var returnParameter = method.ReturnParameter;
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
                        else if (methodName == "GetID")
                        {
                            value = GetValue2(baseUrl, modulePath + "/" + methodName.Replace("Get", string.Empty), RestSharp.Method.GET);
                        }
                        else if (methodName == "GetMessage")
                        {
                            value = GetValue3(baseUrl, modulePath + "/" + methodName.Replace("Get", string.Empty), RestSharp.Method.GET);
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
            var client = new RestClient(baseUrl);
            var request = new RestRequest(modulePath, method);

            IRestResponse response = client.Execute(request);
            var ret = response.StatusCode == HttpStatusCode.OK ? response.Content : null;

            if (ret != null)
            {
                return () => ret;
            }

            return () => null;
        }

        static Func<int, string> GetValue2(string baseUrl, string modulePath, RestSharp.Method method)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(modulePath + @"/{id}", method);

            return (id) =>
                {
                    request.AddUrlSegment("id", id.ToString());
                    IRestResponse response = client.Execute(request);

                    var ret = response.StatusCode == HttpStatusCode.OK ? response.Content : null;

                    if (ret != null)
                    {
                        return ret;
                    }
                    return null;
                };
        }

        static Func<string, string> GetValue3(string baseUrl, string modulePath, RestSharp.Method method)
        {
            ParameterExpression _baseUrl = Expression.Parameter(typeof(string), "baseUrl");
            ParameterExpression _msg = Expression.Parameter(typeof(string), "msg");

            Expression<Func<string, string>> func = Expression.Lambda<Func<string, string>>
            (
                Expression.Call(typeof(ObjectCreator), "GetResult", null, _baseUrl, _msg),
                new[] { _msg }
            );

           var ret = func.Compile();

            return ret;
        }

        public static string GetResult(string _baseUrl, string msg)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest("Hello/Message" + @"/{msg}", RestSharp.Method.GET);
            request.AddUrlSegment("msg", msg);
            IRestResponse response = client.Execute(request);

            var ret = response.StatusCode == HttpStatusCode.OK ? response.Content : null;

            if (ret != null)
            {
                return ret;
            }
            return null;
        }
    }
}
