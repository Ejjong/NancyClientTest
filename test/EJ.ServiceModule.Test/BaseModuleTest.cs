﻿using EJ.ServiceModel;
using Nancy;
using Nancy.Testing;
using Xunit;

namespace EJ.ServiceModule.Test
{
    public class BaseModuleTest
    {
        [Fact]
        public void Should_return_status_ok_when_route_exists()
        {
            // Given
            var browser = new Browser(with => with.Module<EJ.ServiceModule.HelloModule>());

            // When
            var result = browser.Get("/Hello");

            // Then
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Hello World", result.Body.AsString());
        }
    }
}
