using System;
using EJ.ServiceModel;
using Nancy;
using Nancy.ModelBinding;

namespace EJ.ServiceModule
{
    public class HelloModule : BaseModule, IHelloModule
    {
        static readonly string modulePath = "/" + typeof(HelloModule).Name.Replace("Module", "");
        public HelloModule()
            : base(HelloModule.modulePath)
        {
            Post["/NewModel/"] = p =>
            {
                var model = this.Bind<TestModel>();
                return model.Id1 + model.Id2;
            };
        }

        public string GetIndex()
        {
            return "Hello World";
        }

        public int GetCount()
        {
            return 404;
        }

        public DateTime GetDateTime()
        {
            return DateTime.Parse("2013-07-23 13:24:56");
        }

        public string GetMessage(string msg)
        {
            return "Hello " + msg;
        }

        public string GetMultiple(string str, int num)
        {
            return str + num;
        }
    }
}
