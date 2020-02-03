using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CreativeCoders.Core.Collections;
using CreativeCoders.Core.Threading;
using CreativeCoders.UnitTests;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Collections
{
    public class ExtendedObservableCollectionTests
    {
        [Fact]
        [SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
        public void Ctor_WithoutParameters_EmptyCollection()
        {
            var collection = new ExtendedObservableCollection<int>();

            Assert.Empty(collection);
        }

        [Fact]
        public void Ctor_WithItems_ItemsAreSet()
        {
            var input = new[] {1, 3, 5, 7, 9};
            var collection = new ExtendedObservableCollection<int>(input);

            Assert.Equal(input.Length, collection.Count);

            input.ForEach((item, index) => Assert.Equal(item, collection[index]));
        }

        [Fact]
        public void Add_OneItem_ItemIsAddedAndRaisedCollectionChanged()
        {
            var collectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();

            var collection = new ExtendedObservableCollection<int>();
            collection.CollectionChanged += (sender, args) => collectionChangedEventArgsList.Add(args);

            collection.Add(123);

            Assert.Single(collection);
            Assert.Equal(123, collection[0]);

            Assert.Single(collectionChangedEventArgsList);
            Assert.Equal(NotifyCollectionChangedAction.Add, collectionChangedEventArgsList[0].Action);
            Assert.Single(collectionChangedEventArgsList[0].NewItems);
            Assert.Equal(123, collectionChangedEventArgsList[0].NewItems[0]);
        }

        [Fact]
        public void Add_TwoItems_ItemsAreAddedAndCollectionChangedRaised()
        {
            var collectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();

            var collection = new ExtendedObservableCollection<int>();
            collection.CollectionChanged += (sender, args) => collectionChangedEventArgsList.Add(args);

            collection.Add(123);
            collection.Add(456);

            Assert.Equal(2, collection.Count);
            Assert.Equal(123, collection[0]);
            Assert.Equal(456, collection[1]);

            Assert.Equal(2, collectionChangedEventArgsList.Count);
            Assert.Equal(NotifyCollectionChangedAction.Add, collectionChangedEventArgsList[0].Action);
            Assert.Equal(NotifyCollectionChangedAction.Add, collectionChangedEventArgsList[1].Action);
            Assert.Single(collectionChangedEventArgsList[0].NewItems);
            Assert.Single(collectionChangedEventArgsList[1].NewItems);
            Assert.Equal(123, collectionChangedEventArgsList[0].NewItems[0]);
            Assert.Equal(456, collectionChangedEventArgsList[1].NewItems[0]);
        }

        [Fact]
        public void AddRange_OneItem_ItemIsAddedAndCollectionChangedResetRaised()
        {
            var collectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();

            var collection = new ExtendedObservableCollection<int>();
            collection.CollectionChanged += (sender, args) => collectionChangedEventArgsList.Add(args);

            collection.AddRange(new[] {123});

            Assert.Single(collection);
            Assert.Equal(123, collection[0]);

            Assert.Single(collectionChangedEventArgsList);
            Assert.Equal(NotifyCollectionChangedAction.Reset, collectionChangedEventArgsList[0].Action);
        }

        [Fact]
        public void AddRange_TwoItems_ItemsAreAddedAndCollectionChangedResetRaised()
        {
            var collectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();

            var collection = new ExtendedObservableCollection<int>();
            collection.CollectionChanged += (sender, args) => collectionChangedEventArgsList.Add(args);

            collection.AddRange(new[] {123, 456});

            Assert.Equal(2, collection.Count);
            Assert.Equal(123, collection[0]);
            Assert.Equal(456, collection[1]);

            Assert.Single(collectionChangedEventArgsList);
            Assert.Equal(NotifyCollectionChangedAction.Reset, collectionChangedEventArgsList[0].Action);
        }

        [Fact]
        public void Clear_EmptyCollection_CollectionIsStillEmptyAndNoCollectionChangedRaised()
        {
            var collectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();

            var collection = new ExtendedObservableCollection<int>();
            collection.CollectionChanged += (sender, args) => collectionChangedEventArgsList.Add(args);

            collection.Clear();

            Assert.Empty(collection);
            Assert.Empty(collectionChangedEventArgsList);
        }

        [Fact]
        public void Clear_CollectionWithItems_CollectionIsEmptyAndCollectionChangedRaised()
        {
            var collectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();

            var collection = new ExtendedObservableCollection<int> {123, 456};
            collection.CollectionChanged += (sender, args) => collectionChangedEventArgsList.Add(args);

            Assert.Equal(2, collection.Count);

            collection.Clear();

            Assert.Empty(collection);

            Assert.Single(collectionChangedEventArgsList);
            Assert.Equal(NotifyCollectionChangedAction.Reset, collectionChangedEventArgsList[0].Action);
        }

        [Theory]
        [InlineData(123, true, 123, 456, 789)]
        [InlineData(12, false, 123, 456, 789)]
        [InlineData(123, true, 123, 123, 456, 789)]
        [InlineData(123, false)]
        [InlineData(123, true, 123)]
        public void Contains_ItemIsInCollection_ReturnsTrue(int item, bool isContained, params int[] items)
        {
            var collection = new ExtendedObservableCollection<int>(items);

            Assert.Equal(isContained, collection.Contains(item));
        }

        [Theory]
        [InlineData(123)]
        [InlineData(123, 456)]
        [InlineData(123, 456, 789)]
        public void CopyTo_InputItemsAtIndexZero_ResultingArrayEqual(params int[] items)
        {
            var collection = new ExtendedObservableCollection<int>(items);

            var buffer = new int[items.Length];

            collection.CopyTo(buffer, 0);

            Assert.Equal(items, collection);
            Assert.Equal(collection, buffer);
        }

        [Theory]
        [InlineData(1, 123)]
        [InlineData(2, 123)]
        [InlineData(3, 123)]
        [InlineData(1, 123, 456)]
        [InlineData(2, 123, 456)]
        [InlineData(3, 123, 456)]
        [InlineData(1, 123, 456, 789)]
        [InlineData(2, 123, 456, 789)]
        [InlineData(3, 123, 456, 789)]
        public void CopyTo_InputItemsAtSpecificIndex_ResultingArrayEqual(int index, params int[] items)
        {
            var collection = new ExtendedObservableCollection<int>(items);

            var buffer = new int[items.Length + index];

            collection.CopyTo(buffer, index);

            Assert.Equal(items, collection);
            Assert.Equal(collection, buffer.Skip(index));
        }

        [Theory]
        [InlineData(123, 0, 123, 456, 789)]
        [InlineData(12, -1, 123, 456, 789)]
        [InlineData(123, 1, 456, 123, 789)]
        [InlineData(789, 2, 123, 456, 789)]
        public void IndexOf_FindItemInItems_CorrectIndexFound(int item, int index, params int[] items)
        {
            var collection = new ExtendedObservableCollection<int>(items);

            var foundIndex = collection.IndexOf(item);

            Assert.Equal(index, foundIndex);
        }

        [Fact]
        public void Insert_OneItemAtFirstIndex_ItemIsAddedAndCollectionChangedRaised()
        {
            var collectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();

            var collection = new ExtendedObservableCollection<int> {123, 456, 789};
            collection.CollectionChanged += (sender, args) => collectionChangedEventArgsList.Add(args);

            collection.Insert(0, 0);

            Assert.Equal(4, collection.Count);
            Assert.Equal(new[] {0, 123, 456, 789}, collection);

            Assert.Single(collectionChangedEventArgsList);
            Assert.Equal(NotifyCollectionChangedAction.Add, collectionChangedEventArgsList[0].Action);
            Assert.Single(collectionChangedEventArgsList[0].NewItems);
            Assert.Equal(0, collectionChangedEventArgsList[0].NewItems[0]);
        }

        [Fact]
        public void Insert_OneItemAtLastIndex_ItemIsAddedAndCollectionChangedRaised()
        {
            var collectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();

            var collection = new ExtendedObservableCollection<int> {123, 456, 789};
            collection.CollectionChanged += (sender, args) => collectionChangedEventArgsList.Add(args);

            collection.Insert(3, 0);

            Assert.Equal(4, collection.Count);
            Assert.Equal(new[] {123, 456, 789, 0}, collection);

            Assert.Single(collectionChangedEventArgsList);
            Assert.Equal(NotifyCollectionChangedAction.Add, collectionChangedEventArgsList[0].Action);
            Assert.Single(collectionChangedEventArgsList[0].NewItems);
            Assert.Equal(0, collectionChangedEventArgsList[0].NewItems[0]);
        }

        [Fact]
        public void Insert_OneItemAtSecondIndex_ItemIsAddedAndCollectionChangedRaised()
        {
            var collectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();

            var collection = new ExtendedObservableCollection<int> {123, 456, 789};
            collection.CollectionChanged += (sender, args) => collectionChangedEventArgsList.Add(args);

            collection.Insert(1, 0);

            Assert.Equal(4, collection.Count);
            Assert.Equal(new[] {123, 0, 456, 789}, collection);

            Assert.Single(collectionChangedEventArgsList);
            Assert.Equal(NotifyCollectionChangedAction.Add, collectionChangedEventArgsList[0].Action);
            Assert.Single(collectionChangedEventArgsList[0].NewItems);
            Assert.Equal(0, collectionChangedEventArgsList[0].NewItems[0]);
        }

        [Fact]
        public void Move_OneItem_ItemIsMovedToNewIndex()
        {
            var collectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();

            var collection = new ExtendedObservableCollection<int> {123, 456, 789};
            collection.CollectionChanged += (sender, args) => collectionChangedEventArgsList.Add(args);
            
            collection.Move(1, 2);
            
            Assert.Equal(new[]{123, 789, 456}, collection);
            
            Assert.Single(collectionChangedEventArgsList);
            Assert.Equal(NotifyCollectionChangedAction.Move, collectionChangedEventArgsList[0].Action);
            Assert.Single(collectionChangedEventArgsList[0].NewItems);
            Assert.Equal(456, collectionChangedEventArgsList[0].NewItems[0]);
            Assert.Single(collectionChangedEventArgsList[0].OldItems);
            Assert.Equal(456, collectionChangedEventArgsList[0].OldItems[0]);
            Assert.Equal(1, collectionChangedEventArgsList[0].OldStartingIndex);
            Assert.Equal(2, collectionChangedEventArgsList[0].NewStartingIndex);
        }
        
        [Fact]
        public void Move_TwoItems_ItemsAreMovedToNewIndex()
        {
            var collectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();

            var collection = new ExtendedObservableCollection<int> {123, 456, 789, 135, 246};
            collection.CollectionChanged += (sender, args) => collectionChangedEventArgsList.Add(args);
            
            collection.Move(1, 2);
            collection.Move(4, 0);
            
            Assert.Equal(new[]{246, 123, 789, 456, 135}, collection);
            
            Assert.Equal(2, collectionChangedEventArgsList.Count);
            
            Assert.Equal(NotifyCollectionChangedAction.Move, collectionChangedEventArgsList[0].Action);
            Assert.Single(collectionChangedEventArgsList[0].NewItems);
            Assert.Equal(456, collectionChangedEventArgsList[0].NewItems[0]);
            Assert.Single(collectionChangedEventArgsList[0].OldItems);
            Assert.Equal(456, collectionChangedEventArgsList[0].OldItems[0]);
            Assert.Equal(1, collectionChangedEventArgsList[0].OldStartingIndex);
            Assert.Equal(2, collectionChangedEventArgsList[0].NewStartingIndex);
            
            Assert.Equal(NotifyCollectionChangedAction.Move, collectionChangedEventArgsList[1].Action);
            Assert.Single(collectionChangedEventArgsList[1].NewItems);
            Assert.Equal(246, collectionChangedEventArgsList[1].NewItems[0]);
            Assert.Single(collectionChangedEventArgsList[1].OldItems);
            Assert.Equal(246, collectionChangedEventArgsList[1].OldItems[0]);
            Assert.Equal(4, collectionChangedEventArgsList[1].OldStartingIndex);
            Assert.Equal(0, collectionChangedEventArgsList[1].NewStartingIndex);
        }

        [Fact]
        public void Remove_OneItem_ItemIsRemovedAndCollectionChangedRaised()
        {
            var collectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();

            var collection = new ExtendedObservableCollection<int> {123, 456};
            collection.CollectionChanged += (sender, args) => collectionChangedEventArgsList.Add(args);

            collection.Remove(123);

            Assert.Single(collection);
            Assert.Equal(456, collection[0]);

            Assert.Single(collectionChangedEventArgsList);
            Assert.Equal(NotifyCollectionChangedAction.Remove, collectionChangedEventArgsList[0].Action);
            Assert.Single(collectionChangedEventArgsList[0].OldItems);
            Assert.Equal(123, collectionChangedEventArgsList[0].OldItems[0]);
        }

        [Fact]
        public void Remove_TwoItems_ItemsAreRemovedAndCollectionChangedRaised()
        {
            var collectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();

            var collection = new ExtendedObservableCollection<int> {123, 456, 789};
            collection.CollectionChanged += (sender, args) => collectionChangedEventArgsList.Add(args);

            collection.Remove(123);
            collection.Remove(456);

            Assert.Single(collection);
            Assert.Equal(789, collection[0]);

            Assert.Equal(2, collectionChangedEventArgsList.Count);

            Assert.Equal(NotifyCollectionChangedAction.Remove, collectionChangedEventArgsList[0].Action);
            Assert.Single(collectionChangedEventArgsList[0].OldItems);
            Assert.Equal(123, collectionChangedEventArgsList[0].OldItems[0]);

            Assert.Equal(NotifyCollectionChangedAction.Remove, collectionChangedEventArgsList[1].Action);
            Assert.Single(collectionChangedEventArgsList[1].OldItems);
            Assert.Equal(456, collectionChangedEventArgsList[1].OldItems[0]);
        }

        [Fact]
        public void RemoveAt_OneItemAtSecondIndex_ItemIsRemovedAndCollectionChangedRaised()
        {
            var collectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();

            var collection = new ExtendedObservableCollection<int> {123, 456, 789};
            collection.CollectionChanged += (sender, args) => collectionChangedEventArgsList.Add(args);
            
            collection.RemoveAt(1);
            
            Assert.Equal(2, collection.Count);
            Assert.Equal(123, collection[0]);
            Assert.Equal(789, collection[1]);
            
            Assert.Single(collectionChangedEventArgsList);
            Assert.Equal(NotifyCollectionChangedAction.Remove, collectionChangedEventArgsList[0].Action);
            Assert.Single(collectionChangedEventArgsList[0].OldItems);
            Assert.Equal(456, collectionChangedEventArgsList[0].OldItems[0]);
        }
        
        [Fact]
        public void RemoveAt_TwoItems_ItemsAreRemovedAndCollectionChangedRaised()
        {
            var collectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();

            var collection = new ExtendedObservableCollection<int> {135, 123, 456, 789};
            collection.CollectionChanged += (sender, args) => collectionChangedEventArgsList.Add(args);
            
            collection.RemoveAt(3);
            collection.RemoveAt(1);
            
            Assert.Equal(2, collection.Count);
            Assert.Equal(135, collection[0]);
            Assert.Equal(456, collection[1]);
            
            Assert.Equal(2, collectionChangedEventArgsList.Count);

            Assert.Equal(NotifyCollectionChangedAction.Remove, collectionChangedEventArgsList[0].Action);
            Assert.Single(collectionChangedEventArgsList[0].OldItems);
            Assert.Equal(789, collectionChangedEventArgsList[0].OldItems[0]);

            Assert.Equal(NotifyCollectionChangedAction.Remove, collectionChangedEventArgsList[1].Action);
            Assert.Single(collectionChangedEventArgsList[1].OldItems);
            Assert.Equal(123, collectionChangedEventArgsList[1].OldItems[0]);
        }

        [Fact]
        public void Item_SetNewItem_ItemIsUpdatedAndCollectionChangedRaised()
        {
            var collectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();

            var collection = new ExtendedObservableCollection<int> {135, 123, 456, 789};
            collection.CollectionChanged += (sender, args) => collectionChangedEventArgsList.Add(args);

            collection[0] = 987;
            
            Assert.Equal(new[]{987, 123, 456, 789}, collection);
            
            Assert.Single(collectionChangedEventArgsList);
            Assert.Equal(NotifyCollectionChangedAction.Replace, collectionChangedEventArgsList[0].Action);
            Assert.Single(collectionChangedEventArgsList[0].OldItems);
            Assert.Equal(135, collectionChangedEventArgsList[0].OldItems[0]);
            Assert.Single(collectionChangedEventArgsList[0].NewItems);
            Assert.Equal(987, collectionChangedEventArgsList[0].NewItems[0]);
        }

        [Fact]
        public void BeginUpdate_AddToItemsAfterBeginUpdate_NoCollectionChangedRaised()
        {
            var collectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();

            var collection = new ExtendedObservableCollection<int> {135, 123, 456, 789};
            collection.CollectionChanged += (sender, args) => collectionChangedEventArgsList.Add(args);
            
            collection.BeginUpdate();
            collection.Add(42);
            
            Assert.Empty(collectionChangedEventArgsList);
        }
        
        [Fact]
        public void EndUpdate_EndUpdateAfterBeginUpdate_NoCollectionChangedRaised()
        {
            var collectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();

            var collection = new ExtendedObservableCollection<int> {135, 123, 456, 789};
            collection.CollectionChanged += (sender, args) => collectionChangedEventArgsList.Add(args);
            
            collection.BeginUpdate();
            collection.Add(42);
            
            Assert.Empty(collectionChangedEventArgsList);
            
            collection.EndUpdate();
            
            Assert.Single(collectionChangedEventArgsList);
            Assert.Equal(NotifyCollectionChangedAction.Reset, collectionChangedEventArgsList[0].Action);
        }

        [Fact]
        public void EndUpdate_NoUpdateMade_NoCollectionChangedRaised()
        {
            var collectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();

            var collection = new ExtendedObservableCollection<int>();
            collection.CollectionChanged += (sender, args) => collectionChangedEventArgsList.Add(args);
            
            collection.BeginUpdate();
            collection.EndUpdate();
            
            Assert.Empty(collection);
            Assert.Empty(collectionChangedEventArgsList);
        }
        
        [Fact]
        public void EndUpdate_OneEndUpdateAfterTwoBeginUpdate_NoCollectionChangedRaised()
        {
            var collectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();

            var collection = new ExtendedObservableCollection<int>();
            collection.CollectionChanged += (sender, args) => collectionChangedEventArgsList.Add(args);
            
            collection.BeginUpdate();
            collection.BeginUpdate();
            collection.Add(1234);
            collection.EndUpdate();
            
            Assert.Single(collection);
            Assert.Empty(collectionChangedEventArgsList);
        }
        
        [Fact]
        public void EndUpdate_NestedUpdates_CollectionChangedRaised()
        {
            var collectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();

            var collection = new ExtendedObservableCollection<int>();
            collection.CollectionChanged += (sender, args) => collectionChangedEventArgsList.Add(args);
            
            collection.BeginUpdate();
            
            collection.Add(1234);
            
            collection.BeginUpdate();
            
            collection.Add(5678);
            
            collection.EndUpdate();
            
            Assert.Empty(collectionChangedEventArgsList);
            
            collection.EndUpdate();
            
            Assert.Equal(2, collection.Count);
            Assert.Single(collectionChangedEventArgsList);
            Assert.Equal(NotifyCollectionChangedAction.Reset, collectionChangedEventArgsList[0].Action);
        }

        [Fact]
        public void Update_NoChangesMade_NoCollectionChangedRaised()
        {
            var collectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();

            var collection = new ExtendedObservableCollection<int>();
            collection.CollectionChanged += (sender, args) => collectionChangedEventArgsList.Add(args);

            using (collection.Update())
            {
            }
            
            Assert.Empty(collectionChangedEventArgsList);
        }
        
        [Fact]
        public void Update_ItemsAdded_CollectionChangedRaised()
        {
            var collectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();

            var collection = new ExtendedObservableCollection<int>();
            collection.CollectionChanged += (sender, args) => collectionChangedEventArgsList.Add(args);

            using (collection.Update())
            {
                collection.Add(123);
                collection.Add(456);
            }
            
            Assert.Equal(2, collection.Count);
            Assert.Single(collectionChangedEventArgsList);
            Assert.Equal(NotifyCollectionChangedAction.Reset, collectionChangedEventArgsList[0].Action);
        }

        [Fact]
        [SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
        public void ReadOnly_Get_IsFalse()
        {
            var collection = new ExtendedObservableCollection<int>();
            
            Assert.False(collection.IsReadOnly);
        }

        [Fact]
        public void Capacity_SetNewCapacity_NewCapacityIsSet()
        {
            var collection = new ExtendedObservableCollection<int> {Capacity = 100};
            
            Assert.Equal(100, collection.Capacity);

            collection.Capacity = 200;
            
            Assert.Equal(200, collection.Capacity);

            collection.Capacity = 50;
            
            Assert.Equal(50, collection.Capacity);
        }
        
        [Fact]
        public void Synchronization_TwoItemsAddedWithPostSyncMethod_ItemsAreAddedAndCollectionChangedRaised()
        {
            var collectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();

            var collection = new ExtendedObservableCollection<int>(new TestSynchronizationContext(), SynchronizationMethod.Post, new LockSlimLockingMechanism());
            collection.CollectionChanged += (sender, args) => collectionChangedEventArgsList.Add(args);

            collection.Add(123);
            collection.Add(456);

            Assert.Equal(2, collection.Count);
            Assert.Equal(123, collection[0]);
            Assert.Equal(456, collection[1]);

            Assert.Equal(2, collectionChangedEventArgsList.Count);
            Assert.Equal(NotifyCollectionChangedAction.Add, collectionChangedEventArgsList[0].Action);
            Assert.Equal(NotifyCollectionChangedAction.Add, collectionChangedEventArgsList[1].Action);
            Assert.Single(collectionChangedEventArgsList[0].NewItems);
            Assert.Single(collectionChangedEventArgsList[1].NewItems);
            Assert.Equal(123, collectionChangedEventArgsList[0].NewItems[0]);
            Assert.Equal(456, collectionChangedEventArgsList[1].NewItems[0]);
        }
        
        [Fact]
        public void Synchronization_TwoItemsAddedWithNoneSyncMethod_ItemsAreAddedAndCollectionChangedRaised()
        {
            var collectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();

            var collection = new ExtendedObservableCollection<int>(SynchronizationContext.Current, SynchronizationMethod.None, new LockSlimLockingMechanism());
            collection.CollectionChanged += (sender, args) => collectionChangedEventArgsList.Add(args);

            collection.Add(123);
            collection.Add(456);

            Assert.Equal(2, collection.Count);
            Assert.Equal(123, collection[0]);
            Assert.Equal(456, collection[1]);

            Assert.Equal(2, collectionChangedEventArgsList.Count);
            Assert.Equal(NotifyCollectionChangedAction.Add, collectionChangedEventArgsList[0].Action);
            Assert.Equal(NotifyCollectionChangedAction.Add, collectionChangedEventArgsList[1].Action);
            Assert.Single(collectionChangedEventArgsList[0].NewItems);
            Assert.Single(collectionChangedEventArgsList[1].NewItems);
            Assert.Equal(123, collectionChangedEventArgsList[0].NewItems[0]);
            Assert.Equal(456, collectionChangedEventArgsList[1].NewItems[0]);
        }

        [Fact]
        public void Reentrancy_AddNewItemOnCollectionChanged_ItemIsAdded()
        {
            var collection = new ExtendedObservableCollection<int>(SynchronizationContext.Current, SynchronizationMethod.None, new LockSlimLockingMechanism());
            collection.CollectionChanged += (sender, args) =>
            {
                if (collection.Count == 1)
                {
                    collection.Add(collection.Count + 1);
                }
            };
            
            collection.Add(123);
            
            Assert.Equal(2, collection.Count);
            Assert.Equal(123, collection[0]);
        }
        
        [Fact]
        public void Reentrancy_AddNewItemOnCollectionChangedWithTwoEventHandler_ExceptionIsThrown()
        {
            var collection = new ExtendedObservableCollection<int>(SynchronizationContext.Current, SynchronizationMethod.None, new LockSlimLockingMechanism());
            collection.CollectionChanged += (sender, args) =>
            {
                if (collection.Count == 1)
                {
                    Assert.Throws<InvalidOperationException>(() => collection.Add(collection.Count + 1));
                }
            };
            collection.CollectionChanged += (sender, args) =>
            {
                if (collection.Count == 1)
                {
                    Assert.Throws<InvalidOperationException>(() => collection.Add(collection.Count + 1));
                }
            };
            
            collection.Add(123);
        }
        
        [Fact]
        public async Task InvokeTestFromOtherThread()
        {
         var collection = new ExtendedObservableCollection<string>();

         var firstTask = Task.Run(() =>
         {
             for (var i = 0; i <= 99999; i++)
             {
                 collection.Add("test1");
             }
         });
         
         var secondTask = Task.Run(() =>
         {
             for (var i = 0; i <= 99999; i++)
             {
                 collection.Add("test2");
             }
         });

         await Task.WhenAll(firstTask, secondTask);

         Assert.Equal(200000, collection.Count);
         Assert.Equal(100000, collection.Count(x => x == "test1"));
         Assert.Equal(100000, collection.Count(x => x == "test2"));
        }
    }
}