using System.Threading.Tasks;
using AwesomeAssertions;
using Xunit;

namespace CreativeCoders.Core.UnitTests;

public class DelegateAsyncDisposableTests
{
    [Fact]
    public void Ctor_NoDisposeCalled_DisposeActionNotCalled()
    {
        var actionCalled = false;

        _ = new DelegateAsyncDisposable(() =>
        {
            actionCalled = true;

            return ValueTask.CompletedTask;
        }, true);

        actionCalled
            .Should()
            .BeFalse();
    }

    [Fact]
    public async Task Dispose_Once_DisposeActionIsCalled()
    {
        var actionCalled = false;

        var d = new DelegateAsyncDisposable(() =>
        {
            actionCalled = true;

            return ValueTask.CompletedTask;
        }, true);

        await d.DisposeAsync();

        actionCalled
            .Should()
            .BeTrue();
    }

    [Fact]
    public async Task Dispose_Twice_DisposeActionIsCalledTwice()
    {
        var actionCalledCounter = 0;

        var d = new DelegateAsyncDisposable(() =>
        {
            actionCalledCounter++;

            return ValueTask.CompletedTask;
        }, false);

        await d.DisposeAsync();
        await d.DisposeAsync();

        actionCalledCounter
            .Should()
            .Be(2);
    }

    [Fact]
    public async Task Dispose_TwiceWithOnlyDisposeOnceSet_DisposeActionIsCalledOnce()
    {
        var actionCalledCounter = 0;

        var d = new DelegateAsyncDisposable(() =>
        {
            actionCalledCounter++;

            return ValueTask.CompletedTask;
        }, true);

        await d.DisposeAsync();
        await d.DisposeAsync();

        actionCalledCounter
            .Should()
            .Be(1);
    }
}
