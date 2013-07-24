using System;

namespace EJ.ServiceModel
{
    public interface IHelloModule
    {
        [NancyOperationContract(Method = Method.GET)]
        string GetIndex();

        [NancyOperationContract(Method = Method.GET)]
        int GetCount();

        [NancyOperationContract(Method = Method.GET)]
        DateTime GetDateTime();

        [NancyOperationContract(Method = Method.GET)]
        string GetMessage(string msg);

        [NancyOperationContract(Method = Method.GET)]
        string GetMultiple(string str, int num);
    }

    public interface ITestModule
    {
        [NancyOperationContract(Method = Method.GET)]
        TestModel GetIndex();
    }

    public interface IExtModule
    {
        [NancyOperationContract(Method = Method.GET)]
        string GetIndex();
    }

    public class TestModel
    {
        public string Id1 { get; set; }
        public string Id2 { get; set; }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class NancyOperationContractAttribute : Attribute
    {
        public Method Method { get; set; }
    }

    public enum Method
    {
        GET,
        POST,
        PUT,
        DELETE,
        HEAD,
        OPTIONS,
        PATCH,
    }
}
