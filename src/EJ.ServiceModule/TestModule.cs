using EJ.ServiceModel;

namespace EJ.ServiceModule
{
    public class TestModule : BaseModule, ITestModule
    {
        static readonly string modulePath = "/" + typeof(TestModule).Name.Replace("Module", "");
        public TestModule()
            : base(TestModule.modulePath)
        { }

        public TestModel GetIndex()
        {
            return new TestModel() { Id1 = "123", Id2 = "456" };
        }
    }
}
