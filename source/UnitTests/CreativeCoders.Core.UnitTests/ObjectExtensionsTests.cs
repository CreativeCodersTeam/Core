using Xunit;

namespace CreativeCoders.Core.UnitTests
{
    public class ObjectExtensionsTests
    {
        [Fact]
        public void ToStringSafe_InstanceIsNull_ReturnsEmptyString()
        {
            var result = ((object) null).ToStringSafe();

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void ToStringSafe_InstanceIsNullAndDefaultIsGiven_ReturnsDefaultValue()
        {
            const string text = "This is a test";

            var result = ((object) null).ToStringSafe(text);

            Assert.Equal(text, result);
        }

        [Fact]
        public void ToStringSafe_InstanceIsInteger_ReturnsIntegerValueAsString()
        {
            const int value = 1234;

            var result = value.ToStringSafe();

            Assert.Equal("1234", result);
        }

        [Fact]
        public void ToStringSafe_InstanceIsIntegerDefaultIsGiven_ReturnsIntegerValueAsString()
        {
            const int value = 1234;

            var result = value.ToStringSafe("Test");

            Assert.Equal("1234", result);
        }

        [Fact]
        public void As_WithDefaultValueInteger_ReturnsInteger()
        {
            object obj = 123;

            var intValue = obj.As(1);
            
            Assert.IsType<int>(intValue);
            Assert.Equal(123, intValue);
        }
        
        [Fact]
        public void As_Integer_ReturnsInteger()
        {
            object obj = 123;

            var intValue = obj.As<int>();
            
            Assert.IsType<int>(intValue);
            Assert.Equal(123, intValue);
        }
        
        [Fact]
        public void As_WithDefaultValueObject_ReturnsDefaultValue()
        {
            var obj = new object();

            var intValue = obj.As(1);
            
            Assert.IsType<int>(intValue);
            Assert.Equal(1, intValue);
        }
        
        [Fact]
        public void As_Object_ReturnsDefaultValue()
        {
            var obj = new object();

            var intValue = obj.As<int>();
            
            Assert.IsType<int>(intValue);
            Assert.Equal(0, intValue);
        }
    }
}