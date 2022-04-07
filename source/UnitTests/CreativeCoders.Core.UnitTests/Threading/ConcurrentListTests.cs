using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Core.Threading;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Threading;

public class ConcurrentListTests
{
    [Fact]
    public void CtorTest()
    {
        var _ = new ConcurrentList<int>();
    }

    [Fact]
    public void CtorWithEnumerableTest()
    {
        var list = new ConcurrentList<int>(new[] {1, 2, 3, 4});

        Assert.Equal(4, list.Count);
        for (var i = 0; i < 4; i++)
        {
            Assert.Equal(i + 1, list[i]);
        }
    }

    [Fact]
    public void CtorWithLockMechanismTest()
    {
        var _ = new ConcurrentList<int>(new LockLockingMechanism());
    }

    [Fact]
    public void CtorWithEnumerableAndLockMechanismTest()
    {
        var _ = new ConcurrentList<int>(new[] {1, 2, 3, 4}, new LockLockingMechanism());
    }

    [Fact]
    public void CtorTestNullParams()
    {
        Assert.Throws<ArgumentNullException>(() => new ConcurrentList<int>(null as IEnumerable<int>));
        Assert.Throws<ArgumentNullException>(() => new ConcurrentList<int>(null as ILockingMechanism));
        Assert.Throws<ArgumentNullException>(() => new ConcurrentList<int>(new[] {1, 2, 3}, null));
        Assert.Throws<ArgumentNullException>(() => new ConcurrentList<int>(null, new LockLockingMechanism()));
        Assert.Throws<ArgumentNullException>(() => new ConcurrentList<int>(null, null));
    }

    [Fact]
    public void AddTest()
    {
        var list = new ConcurrentList<int> {12345};

        Assert.Single(list);
        Assert.Equal(12345, list[0]);
    }

    [Fact]
    public void ClearTest()
    {
        var list = new ConcurrentList<int>(new[] {1, 2, 3, 4});
        list.Clear();

        Assert.Empty(list);
    }

    [Fact]
    public void ContainsTest()
    {
        var list = new ConcurrentList<int>(new[] {1, 2, 3, 4});

        for (var i = 1; i < 5; i++)
        {
            Assert.Contains(i, list);
        }

        for (var i = 5; i < 10; i++)
        {
            Assert.DoesNotContain(i, list);
        }
    }

    [Fact]
    public void CopyToTest()
    {
        var list = new ConcurrentList<int>(new[] {1, 2, 3, 4});

        var intArray = new int[4];

        list.CopyTo(intArray, 0);

        for (var i = 0; i < 4; i++)
        {
            Assert.Equal(i + 1, intArray[i]);
        }
    }

    [Fact]
    public void RemoveTest()
    {
        var list = new ConcurrentList<int>(new[] {1, 2, 3, 4});

        list.Remove(4);

        Assert.Equal(3, list.Count);
        Assert.DoesNotContain(4, list);
        for (var i = 0; i < list.Count; i++)
        {
            Assert.Equal(i + 1, list[i]);
        }
    }

    [Fact]
    public void IndexOfTest()
    {
        var list = new ConcurrentList<int>(new[] {1, 2, 3, 4});

        for (var i = 0; i < list.Count; i++)
        {
            Assert.Equal(i, list.IndexOf(i + 1));
        }

        Assert.Equal(-1, list.IndexOf(101));
    }

    [Fact]
    public void InsertTest()
    {
        var list = new ConcurrentList<int>(new[] {1, 2, 3, 4});

        list.Insert(4, 5);

        list.Insert(0, 0);

        Assert.Equal(6, list.Count);

        for (var i = 0; i < list.Count; i++)
        {
            Assert.Equal(i, list[i]);
        }
    }

    [Fact]
    public void RemoveAtTest()
    {
        var list = new ConcurrentList<int>(new[] {1, 2, 3, 4});

        list.RemoveAt(3);
        list.RemoveAt(1);

        Assert.Equal(2, list.Count);
        Assert.Equal(1, list[0]);
        Assert.Equal(3, list[1]);
    }

    [Fact]
    public void EnumeratorTest()
    {
        var list = new ConcurrentList<int>(new[] {1, 2, 3, 4}) as IEnumerable;
        var secondList = list.Cast<int>().ToList();

        Assert.Equal(4, secondList.Count);

        for (var i = 0; i < secondList.Count; i++)
        {
            Assert.Equal(i + 1, secondList[i]);
        }
    }

    [Fact]
    public void EnumeratorGenericTest()
    {
        var list = new ConcurrentList<int>(new[] {1, 2, 3, 4}) as IEnumerable<int>;
        var secondList = list.ToList();

        Assert.Equal(4, secondList.Count);

        for (var i = 0; i < secondList.Count; i++)
        {
            Assert.Equal(i + 1, secondList[i]);
        }
    }

    [Fact]
    public void ThisItemTest()
    {
        var list = new ConcurrentList<int>(new[] {1, 2, 3, 4}) {[0] = 1234};

        Assert.Equal(1234, list[0]);
    }

    [Fact]
    public void IsReadOnlyTest()
    {
        Assert.False(new ConcurrentList<int>(new[] {1, 2, 3, 4}).IsReadOnly);
    }
}
