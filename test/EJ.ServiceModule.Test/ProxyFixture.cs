using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EJ.ServiceModel;

namespace EJ.ServiceModule.Test
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
}
