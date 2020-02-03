using CreativeCoders.Core.Reflection;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Reflection
{
    public class TypeExtensionsTests
    {
        [Fact]
        public void CreateGenericInstance_WithoutCtorParameters_ReturnInstanceWithCorrectTypeAndData()
        {
            var instance = typeof(GenericClass<>).CreateGenericInstance(typeof(int));

            Assert.IsType<GenericClass<int>>(instance);
            Assert.Equal(0, ((GenericClass<int>)instance).Data);
        }

        [Fact]
        public void CreateGenericInstance_WithoutCtorParameters_ReturnInstanceWithCorrectType()
        {
            var instance = typeof(GenericClass<>).CreateGenericInstance(typeof(int), 1234);

            Assert.IsType<GenericClass<int>>(instance);
            Assert.Equal(1234, ((GenericClass<int>)instance).Data);
        }

        [Fact]
        public void GetDefault_ForInt_ReturnsZero()
        {
            var value = typeof(int).GetDefault();

            Assert.IsType<int>(value);

            var intValue = (int) value;
            
            Assert.Equal(0, intValue);
        }
        
        [Fact]
        public void GetDefault_ForBool_ReturnsFalse()
        {
            var value = typeof(bool).GetDefault();

            Assert.IsType<bool>(value);

            var boolValue = (bool) value;
            
            Assert.False(boolValue);
        }
        
        [Fact]
        public void GetDefault_ForString_ReturnsNull()
        {
            var value = typeof(string).GetDefault();

            Assert.Null(value);
        }
    }
}