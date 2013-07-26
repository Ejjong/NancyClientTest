using EJ.ServiceModel;
using EJ.ServiceModule;
using Nancy;

namespace ExtendModule
{
    public class ExtModule : BaseModule, IExtModule
    {
        static readonly string modulePath = "/" + typeof(ExtModule).Name.Replace("Module", "");
        public ExtModule()
            : base(ExtModule.modulePath)
        {
            Get["/Test"] = _ => Response.AsJson(new TestModel() { Id1 = "12", Id2 = "34" });
        }

        public string GetIndex()
        {
            return "Hello Ext";
        }
    }
}
