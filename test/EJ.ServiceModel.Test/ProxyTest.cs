using RestSharp;
using Xunit;

namespace EJ.ServiceModel.Test
{
    public class ProxyTest
    {
        [Fact]
        public void ValidateGet()
        {
            IProxy proxy = new Proxy();
            var impromptuInterface = proxy.Get<IHelloModule>();
            Assert.NotNull(impromptuInterface);
        }

        [Fact]
        public void GetIndexReturnString()
        {
            IProxy proxy = new Proxy();
            var ret = proxy.Get<IHelloModule>().GetIndex();
            Assert.Equal("Hello World", ret);
        }

        [Fact]
        public void GetCountReturnInteger()
        {
            IProxy proxy = new Proxy();
            var ret = proxy.Get<IHelloModule>().GetCount();
            Assert.Equal(404, ret);
        }

        [Fact]
        public void GetIndexReturnObject()
        {
            IProxy proxy = new Proxy();
            var ret = proxy.Get<ITestModule>().GetIndex();
            Assert.NotNull(ret);
            Assert.Equal("123", ret.Id1);
            Assert.Equal("456", ret.Id2);
        }

        [Fact]
        public void GetDateTimeReturnDateTime()
        {
            IProxy proxy = new Proxy();
            var ret = proxy.Get<IHelloModule>().GetDateTime();
            Assert.Equal("2013-07-23 13:24:56", ret.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        [Fact]
        public void GetMessage()
        {
            IProxy proxy = new Proxy();
            var ret = proxy.Get<IHelloModule>().GetMessage("Nancy");
            Assert.Equal("Hello Nancy", ret);
        }

        [Fact]
        public void GetMultipleParams()
        {
            IProxy proxy = new Proxy();
            var ret = proxy.Get<IHelloModule>().GetMultiple("Nancy", 2013);
            Assert.Equal("Nancy2013", ret);
        }

        [Fact]
        public void PostTest()
        {
            string baseUrl = "http://localhost:60770/";
            var client = new RestClient(baseUrl);
            var request = new RestRequest("/Hello/NewModel/", RestSharp.Method.POST);
            request.AddBody(new TestModel { Id1 = "12", Id2 = "34" });
            IRestResponse response = client.Execute(request);
            Assert.NotNull(response);
            Assert.Equal("1234", response.Content);
        }
    }
}
