using System;
using RestSharp;
using Xunit;

namespace EJ.ServiceModel.Test
{
    public class ProxyFixture : IDisposable
    {
        public IProxy CreateProxy()
        {
            return new Proxy();
        }

        public string HostUrl
        {
            get
            {
                return "http://localhost:1234/base/";
            }
        }

        public void Dispose()
        {
            //test tear down code
        }
    }

    public class ProxyTest : IUseFixture<ProxyFixture>
    {
        private IProxy _proxy;
        private string _hostUrl;

        public void SetFixture(ProxyFixture Fixture)
        {
            _proxy = Fixture.CreateProxy();
            _hostUrl = Fixture.HostUrl;
        }

        [Fact]
        public void ValidateGet()
        {
            var impromptuInterface = _proxy.Get<IHelloModule>(_hostUrl);
            Assert.NotNull(impromptuInterface);
        }

        //[Fact]
        //public void PostTest()
        //{
        //    string baseUrl = "http://localhost:60770/";
        //    var client = new RestClient(baseUrl);
        //    var request = new RestRequest("/Hello/NewModel/", RestSharp.Method.POST);
        //    request.AddBody(new TestModel { Id1 = "12", Id2 = "34" });
        //    IRestResponse response = client.Execute(request);
        //    Assert.NotNull(response);
        //    Assert.Equal("1234", response.Content);
        //}
    }
}
