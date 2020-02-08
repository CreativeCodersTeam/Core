using System;
using System.Linq;
using CreativeCoders.Net.XmlRpc;
using CreativeCoders.Net.XmlRpc.Model;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc
{
    public class XmlRpcRequestTests
    {
        [Fact]
        public void Ctor_WithNullMethods_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new XmlRpcRequest(null, false));
        }

        [Fact]
        public void Ctor_WithMethod_HasMethodSetToParameter()
        {
            var method = new XmlRpcMethodCall("test");

            var request = new XmlRpcRequest(new []{method}, false);

            Assert.Single(request.Methods);
            Assert.Same(method, request.Methods.First());
        }

        [Fact]
        public void Ctor_WithMethods_HasMethodsSetToParameter()
        {
            var method = new XmlRpcMethodCall("test");
            var method1 = new XmlRpcMethodCall("test1");

            var request = new XmlRpcRequest(new[] { method, method1 }, false);

            Assert.Equal(2, request.Methods.Count());
            Assert.Equal(request.Methods, new []{method, method1});
        }

        [Fact]
        public void Ctor_WithMultiCallFalse_MultiCallPropertyIsFalse()
        {
            var method = new XmlRpcMethodCall("test");

            var request = new XmlRpcRequest(new[] { method }, false);

            Assert.False(request.IsMultiCall);
        }

        [Fact]
        public void Ctor_WithMultiCallTrue_MultiCallPropertyIsTrue()
        {
            var method = new XmlRpcMethodCall("test");

            var request = new XmlRpcRequest(new[] { method }, true);

            Assert.True(request.IsMultiCall);
        }
    }
}