using CreativeCoders.Core.Reflection;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Reflection
{
    public class ReflectionUtilsTests
    {
        [Fact]
        public void GetAllAssemblies_Call_ReturnsAssemblies()
        {
            var assemblies = ReflectionUtils.GetAllAssemblies();

            Assert.NotEmpty(assemblies);
        }

        [Fact]
        public void GetTypes_ForAllAssemblies_ReturnsTypes()
        {
            var types = ReflectionUtils.GetAllTypes();

            Assert.NotEmpty(types);
        }

        [Fact]
        public void GetTypes_ForAllNotDynamicAssemblies_ReturnsTypes()
        {
            var types = ReflectionUtils.GetAllTypes(a => !a.IsDynamic);

            Assert.NotEmpty(types);
        }
    }
}