using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EJ.ServiceModel.Test.Modules
{
    public class HelloModuleTests : IUseFixture<ProxyFixture>
    {
        private IHelloModule _target;

        public void SetFixture(ProxyFixture fixture)
        {
            IProxy proxy = fixture.CreateProxy();
            string hostUrl = fixture.GetHostUrl;

            _target = proxy.Get<IHelloModule>(hostUrl);
        }
    }
}
