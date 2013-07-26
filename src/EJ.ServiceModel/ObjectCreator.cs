using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using ImpromptuInterface;
using ImpromptuInterface.Dynamic;
using RestSharp;
using System.Linq;

namespace EJ.ServiceModel
{
    public static class ObjectCreator
    {
        private static string baseUrl;

        public static object CreateObject<T>(string hostUrl) where T : class
        {
            baseUrl = hostUrl;
            dynamic impromptuDictionary = new ImpromptuDictionary();
            var nOps = GetNancyOperation(typeof(T));
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
                var nancyAttr = method.GetCustomAttribute<NancyOperationContractAttribute>();
                if (nancyAttr == null)
                {
                    continue;
                }
                switch (nancyAttr.Method)
                {
                    case Method.GET:
                        if (parameters.Length == 0)
                        {
                            if (methodName == "GetIndex")
                            {
                                methodName = string.Empty;
                            }

                            Type ex = typeof(ObjectCreator);
                            MethodInfo mi = ex.GetMethod("GetValue");
                            Type returnType = method.ReturnParameter.ParameterType;
                            MethodInfo miConstructed = mi.MakeGenericMethod(returnType);

                            object[] args = { modulePath + "/" + methodName.Replace("Get", string.Empty), RestSharp.Method.GET };
                            value = miConstructed.Invoke(null, args);
                        }
                        else
                        {
                            Type ex = typeof(ObjectCreator);
                            var _methods = ex.GetMethods();
                            var mi = _methods.Where(m => m.Name == "GetValue2" && m.GetGenericArguments().Count() == method.GetParameters().Count() + 1).SingleOrDefault();
                            Type returnType = method.ReturnParameter.ParameterType;
                            Type[] types = new Type[parameters.Length + 1];
                            string[] pStrings = new string[parameters.Length];
                            string query = "";
                            for (int i = 0; i < parameters.Length; i++)
                            {
                                var curP = parameters[i];
                                types[i] = curP.ParameterType;
                                pStrings[i] = curP.Name;
                                query += "/{" + curP.Name + "}";
                            }
                            types[parameters.Length] = returnType;

                            MethodInfo miConstructed = mi.MakeGenericMethod(types);

                            object[] args = { modulePath + "/" + methodName.Replace("Get", string.Empty) + query, RestSharp.Method.GET, pStrings };
                            value = miConstructed.Invoke(null, args);
                        }
                        Debug.WriteLine("[GET] " + baseUrl + modulePath + "/" + methodName);
                        break;
                    case Method.POST:
                        break;
                }
                ret.Add(method.Name, value);
            }

            return ret;
        }

        public static Func<T1, TR> GetValue2<T1, TR>(string modulePath, RestSharp.Method method, string[] paramNames)
        {
            return (p1) =>
            {
                var ret = GetResult<TR>(modulePath, method, paramNames, p1);
                return ret;
            };
        }

        public static Func<T1, T2, TR> GetValue2<T1, T2, TR>(string modulePath, RestSharp.Method method, string[] paramNames)
        {
            return (p1, p2) =>
            {
                var ret = GetResult<TR>(modulePath, method, paramNames, new object[] { p1, p2});
                return ret;
            };
        }

        public static Func<TR> GetValue<TR>(string modulePath, RestSharp.Method method)
        {
            return () =>
            {
                var ret = GetResult<TR>(modulePath, method, null, null);
                return ret;
            };
        }

        public static TR GetResult<TR>(string modulePath, RestSharp.Method method, string[] paramNames, params object[] parameters)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(modulePath , method);

            if (paramNames != null && parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    request.AddUrlSegment(paramNames[i], parameters[i].ToString());
                }
            }
            
            IRestResponse response = client.Execute(request);

            if (typeof(TR) == typeof(int))
            {
                return (TR)Convert.ChangeType(response.StatusCode, typeof(TR));
            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.ContentType == "application/json; charset=utf8")
                {
                    var ret = ServiceStack.Text.JsonSerializer.DeserializeFromString<TR>(response.Content);
                    return ret;
                }
                return (TR)Convert.ChangeType(response.Content, typeof(TR));
            }

            return default(TR);
        }
    }
}
