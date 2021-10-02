using System.Collections;
using CreativeCoders.Core.Collections;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Collections
{
    public class CollectionUtilsTests
    {
        [Fact]
        public void Count_FilledArray_ReturnsArrayLength()
        {
            var items = new[] { 1, 2, 3 };

            // Act
            var count = CollectionUtils<ICollection>.Count(items);

            // Assert
            count
                .Should()
                .Be(items.Length);
        }
    }
}
