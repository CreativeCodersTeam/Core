using System.Collections.ObjectModel;
using System.Linq;
using Xunit;

namespace CreativeCoders.Core.UnitTests
{
    public class ObservableCollectionSynchronizerTests
    {
        [Fact]
        public void ElementInitiallyAdded_CollectionContainsElement()
        {
            var integerList = new ObservableCollection<int> {1, 3};
            var stringList = new ObservableCollection<string>();

            var _ =
                new ObservableCollectionSynchronizer<int, string>(integerList, stringList, x => x.ToString(),
                    x => x.ToString());
            
            Assert.Equal(2, stringList.Count);
            Assert.Equal("1", stringList.First());
            Assert.Equal("3", stringList.Last());
        }
        
        [Fact]
        public void ElementAdded_CollectionContainsElement()
        {
            var integerList = new ObservableCollection<int>();
            var stringList = new ObservableCollection<string>();

            var _ =
                new ObservableCollectionSynchronizer<int, string>(integerList, stringList, x => x.ToString(),
                    x => x.ToString());
            
            integerList.Add(1);
            integerList.Add(3);
            
            Assert.Equal(2, stringList.Count);
            Assert.Equal("1", stringList.First());
            Assert.Equal("3", stringList.Last());
        }
        
        [Fact]
        public void ElementInserted_CollectionContainsElement()
        {
            var integerList = new ObservableCollection<int>();
            var stringList = new ObservableCollection<string>();

            var _ =
                new ObservableCollectionSynchronizer<int, string>(integerList, stringList, x => x.ToString(),
                    x => x.ToString());
            
            integerList.Add(1);
            integerList.Add(3);
            integerList.Insert(1, 2);
            
            Assert.Equal(3, stringList.Count);
            Assert.Equal("1", stringList[0]);
            Assert.Equal("2", stringList[1]);
            Assert.Equal("3", stringList[2]);
        }
        
        [Fact]
        public void ElementRemoved_CollectionNotContainsElement()
        {
            var integerList = new ObservableCollection<int>();
            var stringList = new ObservableCollection<string>();

            var _ =
                new ObservableCollectionSynchronizer<int, string>(integerList, stringList, x => x.ToString(),
                    x => x.ToString());
            
            integerList.Add(1);
            integerList.Add(2);
            integerList.Add(3);
            integerList.Add(4);

            integerList.Remove(2);
            
            Assert.Equal(3, stringList.Count);
            Assert.Equal("1", stringList[0]);
            Assert.Equal("3", stringList[1]);
            Assert.Equal("4", stringList[2]);
        }
        
        [Fact]
        public void ElementRemovedAt_CollectionNotContainsElement()
        {
            var integerList = new ObservableCollection<int>();
            var stringList = new ObservableCollection<string>();

            var _ =
                new ObservableCollectionSynchronizer<int, string>(integerList, stringList, x => x.ToString(),
                    x => x.ToString());
            
            integerList.Add(1);
            integerList.Add(2);
            integerList.Add(3);
            integerList.Add(4);

            integerList.RemoveAt(2);
            
            Assert.Equal(3, stringList.Count);
            Assert.Equal("1", stringList[0]);
            Assert.Equal("2", stringList[1]);
            Assert.Equal("4", stringList[2]);
        }
        
        [Fact]
        public void ElementsCleared_CollectionIsEmpty()
        {
            var integerList = new ObservableCollection<int>();
            var stringList = new ObservableCollection<string>();

            var _ =
                new ObservableCollectionSynchronizer<int, string>(integerList, stringList, x => x.ToString(),
                    x => x.ToString());
            
            integerList.Add(1);
            integerList.Add(2);
            integerList.Add(3);
            integerList.Add(4);

            integerList.Clear();
            
            Assert.Empty(stringList);
        }
        
        [Fact]
        public void ElementReplaced_CollectionContainsNewElement()
        {
            var integerList = new ObservableCollection<int>();
            var stringList = new ObservableCollection<string>();

            var _ =
                new ObservableCollectionSynchronizer<int, string>(integerList, stringList, x => x.ToString(),
                    x => x.ToString());
            
            integerList.Add(1);
            integerList.Add(2);
            integerList.Add(3);
            integerList.Add(4);

            integerList[2] = 5;
            
            Assert.Equal(4, stringList.Count);
            Assert.Equal("1", stringList[0]);
            Assert.Equal("2", stringList[1]);
            Assert.Equal("5", stringList[2]);
            Assert.Equal("4", stringList[3]);
        }
        
        [Fact]
        public void ElementMoved_CollectionContainsElementAtRightPosition()
        {
            var integerList = new ObservableCollection<int>();
            var stringList = new ObservableCollection<string>();

            var _ =
                new ObservableCollectionSynchronizer<int, string>(integerList, stringList, x => x.ToString(),
                    x => x.ToString());
            
            integerList.Add(1);
            integerList.Add(2);
            integerList.Add(3);
            integerList.Add(4);

            integerList.Move(1, 3);
            
            Assert.Equal(4, stringList.Count);
            Assert.Equal("1", stringList[0]);
            Assert.Equal("3", stringList[1]);
            Assert.Equal("4", stringList[2]);
            Assert.Equal("2", stringList[3]);
        }
        
        [Fact]
        public void Dispose_CollectionDoesntUpdateAnymore()
        {
            var integerList = new ObservableCollection<int>();
            var stringList = new ObservableCollection<string>();

            var synchronizer =
                new ObservableCollectionSynchronizer<int, string>(integerList, stringList, x => x.ToString(),
                    x => x.ToString());
            
            integerList.Add(1);
            integerList.Add(3);
            
            synchronizer.Dispose();
            
            integerList.Add(6);
            integerList.Add(7);
            
            Assert.Equal(2, stringList.Count);
            Assert.Equal("1", stringList.First());
            Assert.Equal("3", stringList.Last());
        }
    }
}