using System;
using CreativeCoders.Net.XmlRpc.Model.Values;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Model.Values
{
    public class DoubleValueTests
    {
        [Fact]
        public void Ctor_DoubleValue_ValueIsDouble()
        {
            const double doubleValue = 1234.56;

            var value = new DoubleValue(doubleValue);

            Assert.True(Math.Abs(doubleValue - value.Value) < 0.0000001);
        }
    }
}