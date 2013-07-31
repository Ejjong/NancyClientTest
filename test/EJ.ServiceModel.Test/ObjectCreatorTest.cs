using System;
using Xunit;

namespace EJ.ServiceModel.Test
{
    public class ObjectCreatorTest
    {
        [Fact]
        public void TestCreateObject()
        {
            var ret = ObjectCreator.CreateObject<IHelloModule>("hostUrl");
            Assert.NotNull(ret);
            Assert.True(ret is IHelloModule);
        }

        [Fact]
        public void TestGetNancyOperation()
        {
            var type = typeof (IHelloModule);
            var ret = ObjectCreator.GetNancyOperation(type);
            Assert.NotNull(ret);
        }
    }
}
