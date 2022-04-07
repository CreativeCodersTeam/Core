using Xunit;

namespace CreativeCoders.Core.UnitTests;

public class DelegateDisposableTests
{
    [Fact]
    public void Ctor_NoDisposeCalled_DisposeActionNotCalled()
    {
        var actionCalled = false;

        var _ = new DelegateDisposable(() => actionCalled = true, true);

        Assert.False(actionCalled);
    }

    [Fact]
    public void Dispose_Once_DisposeActionIsCalled()
    {
        var actionCalled = false;

        var d = new DelegateDisposable(() => actionCalled = true, true);

        d.Dispose();

        Assert.True(actionCalled);
    }

    [Fact]
    public void Dispose_Twice_DisposeActionIsCalledTwice()
    {
        var actionCalledCounter = 0;

        var d = new DelegateDisposable(() => actionCalledCounter++, false);

        d.Dispose();
        d.Dispose();

        Assert.Equal(2, actionCalledCounter);
    }

    [Fact]
    public void Dispose_TwiceWithOnlyDisposeOnceSet_DisposeActionIsCalledOnce()
    {
        var actionCalledCounter = 0;

        var d = new DelegateDisposable(() => actionCalledCounter++, true);

        d.Dispose();
        d.Dispose();

        Assert.Equal(1, actionCalledCounter);
    }
}
