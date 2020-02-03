using System;
using System.Text;
using CreativeCoders.Net.XmlRpc.Model.Values;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Model.Values
{
    public class Base64ValueTests
    {
        [Fact]
        public void Ctor_BytesData_ValueIsSetCorrect()
        {
            var bytes = Encoding.UTF8.GetBytes("Test1234");
            var value = new Base64Value(bytes, Encoding.UTF8);

            Assert.Equal(bytes, value.Value);
        }

        [Fact]
        public void Ctor_Base64StringData_ValueIsSetCorrect()
        {
            var bytes = Encoding.UTF8.GetBytes("Test1234");
            var data = Convert.ToBase64String(bytes);
            var value = new Base64Value(data, Encoding.UTF8);

            Assert.Equal(bytes, value.Value);
        }

        [Fact]
        public void GetEncodedString_Call_ReturnsBytesAsString()
        {
            var bytes = Encoding.UTF8.GetBytes("Test1234");
            var value = new Base64Value(bytes, Encoding.UTF8);

            Assert.Equal("Test1234", value.GetEncodedString());
        }

        [Fact]
        public void GetEncodedString_CallWithEncodingASCII_ReturnsBytesAsASCIIString()
        {
            var bytes = Encoding.Default.GetBytes("ÄÜ");
            var value = new Base64Value(bytes, Encoding.Default);

            Assert.Equal("ÄÜ", value.GetEncodedString());
        }
    }
}