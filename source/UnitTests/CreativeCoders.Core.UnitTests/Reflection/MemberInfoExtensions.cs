using System.Linq;
using System.Reflection;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Reflection
{
    public class MemberInfoExtensions
    {
        [Fact]
        public void GetAttribute_ForClassWithDummyAttribute_ReturnAttribute()
        {
            var attribute = typeof(GenericClass<>).GetCustomAttribute<DummyTestAttribute>(false);

            Assert.Equal(12345, attribute?.Value);
        }

        [Fact]
        public void GetAttribute_ForClassWithDummyAttributeWithoutParameter_ReturnAttribute()
        {
            var attribute = typeof(GenericClass<>).GetCustomAttribute<DummyTestAttribute>();

            Assert.Equal(12345, attribute?.Value);
        }

        [Fact]
        public void GetAttributes_ForClassWithMultipleDummyAttribute_ReturnAttributes()
        {
            var attributes = typeof(AttributesTestClass).GetCustomAttributes<DummyTestAttribute>(false).ToArray();

            Assert.Equal(3, attributes.Length);
            Assert.Contains(attributes, a => a.Value == 1);
            Assert.Contains(attributes, a => a.Value == 3);
            Assert.Contains(attributes, a => a.Value == 5);
        }

        [Fact]
        public void GetAttributes_ForClassWithMultipleDummyAttributeWithoutParameter_ReturnAttributes()
        {
            var attributes = typeof(AttributesTestClass).GetCustomAttributes<DummyTestAttribute>().ToArray();

            Assert.Equal(3, attributes.Length);
            Assert.Contains(attributes, a => a.Value == 1);
            Assert.Contains(attributes, a => a.Value == 3);
            Assert.Contains(attributes, a => a.Value == 5);
        }
    }
}