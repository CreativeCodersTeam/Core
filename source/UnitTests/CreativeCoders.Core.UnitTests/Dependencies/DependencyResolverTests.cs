using System.Linq;
using CreativeCoders.Core.Dependencies;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Dependencies
{
    public class DependencyResolverTests
    {
        [Theory]
        [InlineData(DependencyTestData.Application,
            DependencyTestData.Cpu, DependencyTestData.Kernel,
            DependencyTestData.VgaDriver, DependencyTestData.Shell)]
        [InlineData(DependencyTestData.VgaDriver,
            DependencyTestData.Cpu, DependencyTestData.Kernel)]
        [InlineData(DependencyTestData.Cpu)]
        public void Resolve_ForElement_ReturnsDependentElements(string element, params string[] expectedDependencies)
        {
            var collection = DependencyTestData.CreateTestData();

            var resolver = new DependencyResolver<string>(collection);

            var dependencies = resolver.Resolve(element).ToArray();

            Assert.Equal(expectedDependencies.Length, dependencies.Length);

            dependencies.ForEach(x => Assert.Contains(dependencies, item => item == x));
        }
    }
}