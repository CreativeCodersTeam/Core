using System;
using Xunit;

namespace CreativeCoders.Core.UnitTests
{
    public class ThrowExtensionsTests
    {
        [Fact]
        public void ThrowTestException()
        {
            object obj = null;
            Assert.Throws<InvalidOperationException>(() => obj.ThrowIfNull(() => new InvalidOperationException()));
        }

        [Fact]
        public void ThrowTestNoException()
        {
            var obj = new object();
            obj.ThrowIfNull(() => new InvalidOperationException());
        }
    }
}