using EJ.ServiceModel;
using Nancy;
using Nancy.ModelBinding;

namespace EJ.ServiceModule
{
    public class HelloModule : BaseModule, IHelloModule
    {
        public HelloModule()
            : base("/Hello")
        {
            Post["/ModelId/{model}"] = p =>
                {
                    var model = this.Bind<TestModel>();
                    return HttpStatusCode.OK;
                };
        }

        public string GetIndex()
        {
            return "Hello World";
        }

        public string GetMessage(string msg)
        {
            return "Hello " + msg;
        }

        public string GetID(int id)
        {
            return id.ToString();
        }

        public TestModel GetModel()
        {
            return new TestModel() { Id1 = "jonglee1", Id2 = "jonglee2" };
        }

        public string GetModelId(TestModel model)
        {
            return model.Id1;
        }
    }

}
