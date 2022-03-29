using System;
using System.Threading;
using CreativeCoders.Mvvm.Commands;
using CreativeCoders.UnitTests;
using Xunit;

namespace CreativeCoders.Mvvm.UnitTests.Commands;

public class SimpleRelayCommandTests
{
    private bool _canExecuteChangedRaised;

    [Fact]
    public void Execute_Wait_ActionIsExecuted()
    {
        SynchronizationContext.SetSynchronizationContext(new TestSynchronizationContext());
            
        var commandExecuted = false;
        var canExecuteChangedRaised = false;
            
        var command = new SimpleRelayCommand(() =>
        {
            Assert.False(commandExecuted);
            commandExecuted = true;
        });
        command.CanExecuteChanged += (_, _) =>
        {
            Assert.False(canExecuteChangedRaised);
            canExecuteChangedRaised = true;
        };

        command.Execute(null);
            
        Assert.True(commandExecuted);
        Assert.True(canExecuteChangedRaised);
    }
        
    [Fact]
    public void CanExecuteChanged_RemoveHandler_HandlerIsNotCalled()
    {
        SynchronizationContext.SetSynchronizationContext(new TestSynchronizationContext());
            
        var commandExecuted = false;
        _canExecuteChangedRaised = false;
            
        var command = new SimpleRelayCommand(() =>
        {
            // ReSharper disable once AccessToModifiedClosure
            Assert.False(commandExecuted);
            commandExecuted = true;
        });
        command.CanExecuteChanged += CommandOnCanExecuteChanged;

        command.Execute(null);
            
        Assert.True(commandExecuted);
        Assert.True(_canExecuteChangedRaised);

        _canExecuteChangedRaised = false;
        commandExecuted = false;

        command.CanExecuteChanged -= CommandOnCanExecuteChanged;
            
        command.Execute(null);
            
        Assert.True(commandExecuted);
        Assert.False(_canExecuteChangedRaised);
    }

    private void CommandOnCanExecuteChanged(object sender, EventArgs e)
    {
        Assert.False(_canExecuteChangedRaised);
        _canExecuteChangedRaised = true;
    }
        
    [Fact]
    public void Execute_CanExecuteIsFalse_ActionIsNotExecuted()
    {
        var commandExecuted = false;
            
        var command = new SimpleRelayCommand(() =>
        {
            Assert.False(commandExecuted);
            commandExecuted = true;
        }, () => false);

        command.Execute(null);
            
        Assert.False(commandExecuted);
    }
}