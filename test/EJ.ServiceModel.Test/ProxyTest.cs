using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
