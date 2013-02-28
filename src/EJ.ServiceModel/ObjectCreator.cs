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
        //static readonly string baseUrl = "http://nancy.nanuminet.co.kr/nancy/";

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
                            //value = GetValue(baseUrl, modulePath, RestSharp.Method.GET);
                        }
                        //else if (methodName == "GetID" || methodName == "GetMessage" || methodName == "GetNum")
                        else {
                            if (parameters[0].ParameterType == typeof(string) &&
                                returnParameter.ParameterType == typeof (int))
                            {
                                value = GetValue<string, int>(modulePath + "/" + methodName.Replace("Get", string.Empty), RestSharp.Method.GET);
                            }
                            else if (parameters[0].ParameterType == typeof(string) && 
                                returnParameter.ParameterType == typeof(string))
                            {
                                value = GetValue<string, string>(modulePath + "/" + methodName.Replace("Get", string.Empty), RestSharp.Method.GET);
                            }
                            else if (parameters[0].ParameterType == typeof(int) &&
                                returnParameter.ParameterType == typeof(string))
                            {
                                value = GetValue<int, string>(modulePath + "/" + methodName.Replace("Get", string.Empty), RestSharp.Method.GET);
                            }
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

        static Func<T1, TR> GetValue<T1,TR>(string modulePath, RestSharp.Method method)
        {
            return (msg) =>
                {
                    var ret = GetResult<TR>(modulePath, method, msg);
                    return (TR)Convert.ChangeType(ret, typeof(TR)); 
                };
        }

        public static TR GetResult<TR>(string modulePath, RestSharp.Method method , params object[] parameters)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(modulePath + @"/{msg}", method);
            request.AddUrlSegment("msg", parameters[0].ToString());
            IRestResponse response = client.Execute(request);

            if (typeof (TR) == typeof(int))
            {
                return (TR)Convert.ChangeType(response.StatusCode, typeof(TR)); 
            }
            var ret = response.StatusCode == HttpStatusCode.OK ? response.Content : null;

            if (ret != null)
            {
                return (TR)Convert.ChangeType(ret, typeof(TR)); 
            }
            return default(TR);
        }
    }
}
