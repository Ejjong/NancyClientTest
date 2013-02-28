using System;

namespace EJ.ServiceModel
{
    public interface IHelloModule
    {
        [NancyOperationContract(Method = Method.GET)]
        string GetIndex();

        [NancyOperationContract(Method = Method.GET)]
        string GetMessage(string msg);

        [NancyOperationContract(Method = Method.GET)]
        string GetID(int id);

        [NancyOperationContract(Method = Method.POST)]
        TestModel GetModel();

        [NancyOperationContract(Method = Method.GET)]
        int GetNum(string str);
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
