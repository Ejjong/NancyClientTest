using EJ.ServiceModel;
using EJ.ServiceModule;

namespace ExtendModule
{
    public class ExtModule : BaseModule, IExtModule
    {
        static readonly string modulePath = "/" + typeof(ExtModule).Name.Replace("Module", "");
        public ExtModule()
            : base(ExtModule.modulePath)
        {
            Get["/Test"] = _ => "Ext Test";
        }

        public string GetIndex()
        {
            return "Hello Ext";
        }
    }
}
