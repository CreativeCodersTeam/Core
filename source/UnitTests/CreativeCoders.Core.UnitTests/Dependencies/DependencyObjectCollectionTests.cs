using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CreativeCoders.Core.Dependencies;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Dependencies
{
    public class DependencyObjectCollectionTests
    {
        [Fact]
        public void AddElement_ElementIsNull()
        {
            var collection = DependencyTestData.CreateTestData();

            collection.AddElement(null);

            var expected = DependencyTestData.CreateTestData();

            Assert.Equal(expected.DependencyObjects.Count, collection.DependencyObjects.Count);

            for (var i = 0; i < expected.DependencyObjects.Count; i++)
            {
                Assert.True(AreEqual(expected.DependencyObjects.ToList()[i], collection.DependencyObjects.ToList()[i]));
            }
        }

        [Fact]
        public void CheckForCircularReferences_CollectionHasNoCircularReferences_ReturnsFalse()
        {
            var collection = DependencyTestData.CreateTestData();

            Assert.False(collection.CheckForCircularReferences());
        }

        [Fact]
        public void CheckForCircularReferences_CollectionHasCircularReferences_ReturnsTrue()
        {
            var collection = DependencyTestData.CreateTestDataWithCircularReference();

            Assert.True(collection.CheckForCircularReferences());
        }

        [Fact]
        public void RemoveRedundancies_CollectionHasNoRedundancies_CollectionIsLikeBefore()
        {
            var collection = DependencyTestData.CreateTestData();
            
            collection.RemoveRedundancies();

            var expected = DependencyTestData.CreateTestData();
            
            Assert.Equal(expected.DependencyObjects.Count, collection.DependencyObjects.Count);

            for (var i = 0; i < expected.DependencyObjects.Count; i++)
            {
                Assert.True(AreEqual(expected.DependencyObjects.ToList()[i], collection.DependencyObjects.ToList()[i]));
            }
        }

        [Fact]
        public void RemoveRedundancies_CollectionHasRedundancies_CollectionIsWithoutRedundancies()
        {
            var collection = DependencyTestData.CreateTestData();

            collection.AddDependency(DependencyTestData.Application, DependencyTestData.Shell);
            collection.AddDependency(DependencyTestData.Application, DependencyTestData.Cpu);

            collection.RemoveRedundancies();

            var expected = DependencyTestData.CreateTestData();

            Assert.Equal(expected.DependencyObjects.Count, collection.DependencyObjects.Count);

            for (var i = 0; i < expected.DependencyObjects.Count; i++)
            {
                Assert.True(AreEqual(expected.DependencyObjects.ToList()[i], collection.DependencyObjects.ToList()[i]));
            }
        }

        [SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery")]
        private static bool AreEqual(DependencyObject<string> expectedDependencyObject, DependencyObject<string> collectionDependencyObject)
        {
            if (expectedDependencyObject.Element != collectionDependencyObject.Element)
            {
                return false;
            }

            if (expectedDependencyObject.DependsOn.Count != collectionDependencyObject.DependsOn.Count)
            {
                return false;
            }

            for (var i = 0; i < expectedDependencyObject.DependsOn.Count; i++)
            {
                if (!AreEqual(expectedDependencyObject.DependsOn[i], collectionDependencyObject.DependsOn[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}