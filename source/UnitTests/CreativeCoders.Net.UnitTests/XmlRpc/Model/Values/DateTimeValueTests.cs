using System;
using CreativeCoders.Net.XmlRpc.Model.Values;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Model.Values
{
    public class DateTimeValueTests
    {
        [Fact]
        public void Ctor_DateTimeNowValue_ValueIsDateTimeNow()
        {
            var now = DateTime.Now;

            var value = new DateTimeValue(now);

            Assert.Equal(now, value.Value, TimeSpan.FromMilliseconds(1));
        }
    }
}