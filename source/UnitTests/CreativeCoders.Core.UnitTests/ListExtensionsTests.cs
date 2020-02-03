using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CreativeCoders.Core.UnitTests
{
    public class ListExtensionsTests
    {
        [Fact]
        public void AddRange_ListAddEmptyList_ListSameAsBefore()
        {
            var data = new[] {1, 2, 3, 4};
            var list = new List<int>(data) as IList<int>;
            
            list.AddRange(new int[0]);
            
            Assert.True(list.SequenceEqual(data));
        }
        
        [Fact]
        public void AddRange_ListAddOneElement_ElementIsAdded()
        {
            var data = new[] {1, 2, 3, 4};
            var dataResult = new[] {1, 2, 3, 4, 5};
            
            var list = new List<int>(data) as IList<int>;

            list.AddRange(new[] {5});

            Assert.True(list.SequenceEqual(dataResult));
        }
        
        [Fact]
        public void AddRange_ListAddTwoElements_ElementsAreAdded()
        {
            var data = new[] {1, 2, 3, 4};
            var dataResult = new[] {1, 2, 3, 4, 5, 6};
            
            var list = new List<int>(data) as IList<int>;

            list.AddRange(new[] {5, 6});

            Assert.True(list.SequenceEqual(dataResult));
        }

        [Fact]
        public void SetItems_EmptyListSetEmpty_ListIsStillEmpty()
        {
            var list = new List<int>();
            
            list.SetItems(new int[0]);
            
            Assert.Empty(list);
        }
        
        [Fact]
        public void SetItems_FilledListSetEmpty_ListIsStillEmpty()
        {
            var list = new List<int>(new []{1, 2, 3, 4});
            
            list.SetItems(new int[0]);
            
            Assert.Empty(list);
        }
        
        [Fact]
        public void SetItems_EmptyListSetItems_ListHasOnlyNewItems()
        {
            var data = new[] {1, 2, 3, 4};
            
            var list = new List<int>();
            
            list.SetItems(data);
            
            Assert.True(list.SequenceEqual(data));
        }
        
        [Fact]
        public void SetItems_FilledListSetItems_ListHasOnlyNewItems()
        {
            var data = new[] {10, 20, 30, 40};
            
            var list = new List<int>(new []{1, 2, 3, 4});
            
            list.SetItems(data);
            
            Assert.True(list.SequenceEqual(data));
        }
    }
}