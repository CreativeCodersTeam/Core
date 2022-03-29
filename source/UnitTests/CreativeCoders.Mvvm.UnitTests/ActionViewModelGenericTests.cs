using System.Windows.Input;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Mvvm.UnitTests;

public class ActionViewModelGenericTests
{
    [Fact]
    public void Execute_WithAction_IsExecuted()
    {
        var text = string.Empty;

        var action = new ActionViewModel<string>(parameter => text += parameter);

        action.Execute("1234");

        Assert.Equal("1234", text);
    }

    [Fact]
    public void Execute_CanExecuteIsFalse_IsNotExecuted()
    {
        var text = string.Empty;

        var action = new ActionViewModel<string>(parameter => text += parameter, _ => false);

        action.Execute("1234");

        Assert.Equal(string.Empty, text);
    }

    [Fact]
    public void Execute_WithCommand_ExecuteIsCalled()
    {
        var command = A.Fake<ICommand>();

        var action = new ActionViewModel<string>(command);

        action.Execute("1234");

        A.CallTo(() => command.Execute("1234")).MustHaveHappenedOnceExactly();
    }
}
