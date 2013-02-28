using System.Net;
using RestSharp;
using Xunit;

namespace EJ.ServiceModel.Test
{
    public class ProxyTest
    {
        [Fact]
        public void ValidGet()
        {
            IProxy proxy = new Proxy();
            var impromptuInterface = proxy.Get<IHelloModule>();
        }

        [Fact]
        public void GetIndex()
        {
            IProxy proxy = new Proxy();
            var ret = proxy.Get<IHelloModule>().GetIndex();
            Assert.Equal("Hello World", ret);
        }

        [Fact]
        public void GetID()
        {
            IProxy proxy = new Proxy();
            var ret = proxy.Get<IHelloModule>().GetID(17);
            Assert.Equal("17", ret);
        }

        [Fact]
        public void GetMessage()
        {
            IProxy proxy = new Proxy();
            var ret = proxy.Get<IHelloModule>().GetMessage("World");
            Assert.Equal("Hello World", ret);
        }

        [Fact]
        public void DirectGetMessage()
        {
            var client = new RestClient("http://nancy.nanuminet.co.kr/nancy/");
            var request = new RestRequest("Hello/Message" + @"/{msg}", RestSharp.Method.GET);
            request.AddUrlSegment("msg", "World");
            IRestResponse response = client.Execute(request);
            var ret = response.StatusCode == HttpStatusCode.OK ? response.Content : null;
            Assert.Equal("Hello World", ret);
        }

        [Fact]
        public void GetNum()
        {
            IProxy proxy = new Proxy();
            var ret = proxy.Get<IHelloModule>().GetNum("23");
            Assert.Equal(23, ret);
        }
    }
}
