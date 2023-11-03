using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CreativeCoders.Core.Error;
using CreativeCoders.Mvvm.Commands;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Mvvm.UnitTests.Commands;

public class AsyncRelayCommandGenericTests
{
    private bool _canExecuteChangedRaised;

    [Fact]
    public async Task Execute_Wait_ActionIsExecuted()
    {
        var commandExecuted = false;
        var canExecuteChangedRaised = false;

        var command = new AsyncRelayCommand<string>(_ =>
        {
            Assert.False(commandExecuted);
            commandExecuted = true;
            return Task.CompletedTask;
        });
        command.CanExecuteChanged += (_, _) =>
        {
            Assert.False(canExecuteChangedRaised);
            canExecuteChangedRaised = true;
        };

        command.Execute(null);

        await Task.Delay(100);

        Assert.True(commandExecuted);
        Assert.True(canExecuteChangedRaised);
    }

    [Fact]
    public async Task CanExecuteChanged_RemoveHandler_HandlerIsNotCalled()
    {
        var commandExecuted = false;
        _canExecuteChangedRaised = false;

        var command = new AsyncRelayCommand<string>(_ =>
        {
            // ReSharper disable once AccessToModifiedClosure
            Assert.False(commandExecuted);
            commandExecuted = true;
            return Task.CompletedTask;
        });
        command.CanExecuteChanged += CommandOnCanExecuteChanged;

        command.Execute(null);

        await Task.Delay(100);

        Assert.True(commandExecuted);
        Assert.True(_canExecuteChangedRaised);

        _canExecuteChangedRaised = false;
        commandExecuted = false;

        command.CanExecuteChanged -= CommandOnCanExecuteChanged;

        command.Execute(null);

        await Task.Delay(100);

        Assert.False(_canExecuteChangedRaised);
    }

    private void CommandOnCanExecuteChanged(object sender, EventArgs e)
    {
        Assert.False(_canExecuteChangedRaised);
        _canExecuteChangedRaised = true;
    }

    [Fact]
    public async Task ExecuteAsync_Await_ActionIsExecuted()
    {
        var commandExecuted = false;

        var command = new AsyncRelayCommand<string>(_ =>
        {
            Assert.False(commandExecuted);
            commandExecuted = true;
            return Task.CompletedTask;
        });

        await command.ExecuteAsync(null);

        Assert.True(commandExecuted);
    }

    [Fact]
    public async Task ExecuteAsync_CanExecuteIsFalse_ActionIsNotExecuted()
    {
        var commandExecuted = false;

        var command = new AsyncRelayCommand<string>(_ =>
        {
            Assert.False(commandExecuted);
            commandExecuted = true;
            return Task.CompletedTask;
        }, _ => false);

        await command.ExecuteAsync(null);

        Assert.False(commandExecuted);
    }

    [Fact]
    [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
    public async Task ExecuteAsync_ExceptionInExecute_ErrorHandlerIsCalled()
    {
        Exception handledException = null;

        var errorHandler = A.Fake<IErrorHandler>();
        A
            .CallTo(() => errorHandler.HandleException(A<Exception>.Ignored))
            .Invokes(call => handledException = call.Arguments.Get<Exception>(0));

        var command = new AsyncRelayCommand<string>(parameter =>
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            return Task.CompletedTask;
        }, errorHandler);

        command.Execute(null);

        await Task.Delay(100);

        Assert.IsType<ArgumentNullException>(handledException);
    }
}
