using System.Threading.Tasks;
using CreativeCoders.Messaging.DefaultMediator;
using Xunit;

namespace CreativeCoders.Messaging.UnitTests;

public class MediatorTests
{
    [Fact]
    public void RegisterHandler_NoSend_HandlerNotCalled()
    {
        var handlerCalled = false;

        var mediator = new Mediator();

        mediator.RegisterHandler<string>(this, _ => handlerCalled = true);

        Assert.False(handlerCalled);
    }

    [Fact]
    public void RegisterAsyncHandler_NoSend_HandlerNotCalled()
    {
        var handlerCalled = false;

        var mediator = new Mediator();

        mediator.RegisterAsyncHandler<string>(this,
            _ =>
            {
                handlerCalled = true;
                return Task.CompletedTask;
            });

        Assert.False(handlerCalled);
    }

    [Fact]
    public async Task SendAsync_Text_HandlerCalled()
    {
        var handlerCalled = false;

        var mediator = new Mediator();

        mediator.RegisterHandler<string>(this, _ => handlerCalled = true);

        await mediator.SendAsync("test");

        Assert.True(handlerCalled);
    }

    [Fact]
    public async Task SendAsync_Text_AsyncHandlerCalled()
    {
        var handlerCalled = false;

        var mediator = new Mediator();

        mediator.RegisterAsyncHandler<string>(this,
            _ =>
            {
                handlerCalled = true;
                return Task.CompletedTask;
            });

        await mediator.SendAsync("test");

        Assert.True(handlerCalled);
    }

    [Fact]
    public async Task SendAsync_Text_HandlerCalledWithText()
    {
        var messageValue = string.Empty;

        var mediator = new Mediator();

        mediator.RegisterHandler<string>(this, msg => messageValue = msg);

        await mediator.SendAsync("test");

        Assert.Equal("test", messageValue);
    }

    [Fact]
    public async Task SendAsync_Text_AsyncHandlerCalledWithText()
    {
        var messageValue = string.Empty;

        var mediator = new Mediator();

        mediator.RegisterAsyncHandler<string>(this,
            msg =>
            {
                messageValue = msg;
                return Task.CompletedTask;
            });

        await mediator.SendAsync("test");

        Assert.Equal("test", messageValue);
    }

    [Fact]
    public async Task UnregisterHandler_AfterUnregisterHandlerForTarget_HandlerNotCalled()
    {
        var handlerCalled = false;

        var mediator = new Mediator();

        mediator.RegisterHandler<string>(this, _ => handlerCalled = true);

        mediator.UnregisterHandler(this);

        await mediator.SendAsync("test");

        Assert.False(handlerCalled);
    }

    [Fact]
    public async Task UnregisterHandler_AfterUnregisterHandlerForTarget_AsyncHandlerNotCalled()
    {
        var handlerCalled = false;

        var mediator = new Mediator();

        mediator.RegisterAsyncHandler<string>(this,
            _ =>
            {
                handlerCalled = true;
                return Task.CompletedTask;
            });

        mediator.UnregisterHandler(this);

        await mediator.SendAsync("test");

        Assert.False(handlerCalled);
    }

    [Fact]
    public async Task UnregisterHandler_AfterUnregisterHandlerForMessage_HandlerNotCalled()
    {
        var handlerCalled = false;

        var mediator = new Mediator();

        mediator.RegisterHandler<string>(this, _ => handlerCalled = true);

        mediator.UnregisterHandler<string>(this);

        await mediator.SendAsync("test");

        Assert.False(handlerCalled);
    }

    [Fact]
    public async Task UnregisterHandler_AfterUnregisterHandlerForMessage_AsyncHandlerNotCalled()
    {
        var handlerCalled = false;

        var mediator = new Mediator();

        mediator.RegisterAsyncHandler<string>(this,
            _ =>
            {
                handlerCalled = true;
                return Task.CompletedTask;
            });

        mediator.UnregisterHandler<string>(this);

        await mediator.SendAsync("test");

        Assert.False(handlerCalled);
    }

    [Fact]
    public async Task DisposeRegistration_SendAsync_HandlerNotCalled()
    {
        var handlerCalled = false;

        var mediator = new Mediator();

        var registration = mediator.RegisterHandler<string>(this, _ => handlerCalled = true);

        registration.Dispose();

        await mediator.SendAsync("test");

        Assert.False(handlerCalled);
    }

    [Fact]
    public async Task DisposeRegistration_SendAsync_AsyncHandlerNotCalled()
    {
        var handlerCalled = false;

        var mediator = new Mediator();

        var registration = mediator.RegisterAsyncHandler<string>(this,
            _ =>
            {
                handlerCalled = true;
                return Task.CompletedTask;
            });

        registration.Dispose();

        await mediator.SendAsync("test");

        Assert.False(handlerCalled);
    }
}
