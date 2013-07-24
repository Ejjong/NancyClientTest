using System;
using Xunit;

namespace EJ.ServiceModel.Test
{
    public class ObjectCreatorTest
    {
        [Fact]
        public void TestMethod1()
        {
            Type type = typeof (IHelloModule);
            var ret = ObjectCreator.GetNancyOperation(type);
            Assert.NotNull(ret);
        }
    }
}
