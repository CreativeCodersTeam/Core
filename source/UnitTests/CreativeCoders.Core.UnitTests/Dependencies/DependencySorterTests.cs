using System.Linq;
using CreativeCoders.Core.Dependencies;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Dependencies
{
    public class DependencySorterTests
    {
        [Fact]
        public void Sort_CorrectCollection_ReturnCorrectSortedElements()
        {
            var collection = DependencyTestData.CreateTestData();

            var sorter = new DependencySorter<string>(collection);

            var result = sorter.Sort().ToArray();

            var expected =
                new[]
                {
                    DependencyTestData.Cpu,
                    DependencyTestData.Kernel,
                    DependencyTestData.VgaDriver,
                    DependencyTestData.Shell,
                    DependencyTestData.Application
                };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Sort_CollectionWithCircularReference_ThrowsException()
        {
            var collection = DependencyTestData.CreateTestDataWithCircularReference();

            var sorter = new DependencySorter<string>(collection);

            var exception = Assert.Throws<CircularReferenceException>(() => sorter.Sort());

            Assert.Contains(exception.PossibleCircularReferences, x => (string) x == DependencyTestData.Application);
            Assert.Contains(exception.PossibleCircularReferences, x => (string)x == DependencyTestData.Shell);
        }
    }
}