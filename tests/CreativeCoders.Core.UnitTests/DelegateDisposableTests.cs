using AwesomeAssertions;
using Xunit;

namespace CreativeCoders.Core.UnitTests;

public class DelegateDisposableTests
{
    [Fact]
    public void Ctor_NoDisposeCalled_DisposeActionNotCalled()
    {
        var actionCalled = false;

        _ = new DelegateDisposable(() => actionCalled = true, true);

        actionCalled
            .Should()
            .BeFalse();
    }

    [Fact]
    public void Dispose_Once_DisposeActionIsCalled()
    {
        var actionCalled = false;

        var d = new DelegateDisposable(() => actionCalled = true, true);

        d.Dispose();

        actionCalled
            .Should()
            .BeTrue();
    }

    [Fact]
    public void Dispose_Twice_DisposeActionIsCalledTwice()
    {
        var actionCalledCounter = 0;

        var d = new DelegateDisposable(() => actionCalledCounter++, false);

        d.Dispose();
        d.Dispose();

        actionCalledCounter
            .Should()
            .Be(2);
    }

    [Fact]
    public void Dispose_TwiceWithOnlyDisposeOnceSet_DisposeActionIsCalledOnce()
    {
        var actionCalledCounter = 0;

        var d = new DelegateDisposable(() => actionCalledCounter++, true);

        d.Dispose();
        d.Dispose();

        actionCalledCounter
            .Should()
            .Be(1);
    }
}
