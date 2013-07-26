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

        public string GetHostUrl
        {
            get
            {
                return "http://localhost:60770/";
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
            _hostUrl = Fixture.GetHostUrl;
        }

        [Fact]
        public void ValidateGet()
        {
            var impromptuInterface = _proxy.Get<IHelloModule>(_hostUrl);
            Assert.NotNull(impromptuInterface);
        }

        [Fact]
        public void GetIndexReturnString()
        {
            var ret = _proxy.Get<IHelloModule>(_hostUrl).GetIndex();
            Assert.Equal("Hello World", ret);
        }

        [Fact]
        public void GetCountReturnInteger()
        {
            var ret = _proxy.Get<IHelloModule>(_hostUrl).GetCount();
            Assert.Equal(404, ret);
        }

        [Fact]
        public void GetIndexReturnObject()
        {
            var ret = _proxy.Get<ITestModule>(_hostUrl).GetIndex();
            Assert.NotNull(ret);
            Assert.Equal("123", ret.Id1);
            Assert.Equal("456", ret.Id2);
        }

        [Fact]
        public void GetDateTimeReturnDateTime()
        {
            var ret = _proxy.Get<IHelloModule>(_hostUrl).GetDateTime();
            Assert.Equal("2013-07-23 13:24:56", ret.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        [Fact]
        public void GetMessage()
        {
            var ret = _proxy.Get<IHelloModule>(_hostUrl).GetMessage("Nancy");
            Assert.Equal("Hello Nancy", ret);
        }

        [Fact]
        public void GetMultipleParams()
        {
            var ret = _proxy.Get<IHelloModule>(_hostUrl).GetMultiple("Nancy", 2013);
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
