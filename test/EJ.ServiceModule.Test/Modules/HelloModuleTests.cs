﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EJ.ServiceModel;
using Xunit;

namespace EJ.ServiceModule.Test.Modules
{
    public class HelloModuleTests : IUseFixture<ProxyFixture>
    {
        private IHelloModule _target;

        public void SetFixture(ProxyFixture fixture)
        {
            if (_target != null) return;

            var proxy = fixture.CreateProxy();
            var hostUrl = fixture.HostUrl;

            _target = proxy.Get<IHelloModule>(hostUrl);
        }

        [Fact(Skip = "Self Hosting Test")]
        public void GetIndex()
        {
            using (NancyHostUtil.CreateAndOpenSelfHost())
            {
                var ret = _target.GetIndex();
                Assert.Equal("Hello World", ret);
            }
        }

        [Fact(Skip = "Self Hosting Test")]
        public void GetCount()
        {
            using (NancyHostUtil.CreateAndOpenSelfHost())
            {
                var ret = _target.GetCount();
                Assert.Equal(404, ret);
            }
        }

        [Fact(Skip = "Self Hosting Test")]
        public void GetDateTime()
        {
            using (NancyHostUtil.CreateAndOpenSelfHost())
            {
                var ret = _target.GetDateTime();
                Assert.Equal("2013-07-23 13:24:56", ret.ToString("yyyy-MM-dd HH:mm:ss"));
            }
        }

        [Fact(Skip = "Self Hosting Test")]
        public void GetMessage()
        {
            using (NancyHostUtil.CreateAndOpenSelfHost())
            {
                var ret = _target.GetMessage("Nancy");
                Assert.Equal("Hello Nancy", ret);
            }
        }

        [Fact(Skip = "Self Hosting Test")]
        public void GetMultiple()
        {
            using (NancyHostUtil.CreateAndOpenSelfHost())
            {
                var ret = _target.GetMultiple("Nancy", 123);
                Assert.Equal("Nancy123", ret);
            }
        }
    }
}
