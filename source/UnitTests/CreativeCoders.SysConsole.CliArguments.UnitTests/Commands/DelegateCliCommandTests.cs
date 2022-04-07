using System;
using System.Threading.Tasks;
using CreativeCoders.SysConsole.CliArguments.Commands;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.SysConsole.CliArguments.UnitTests.Commands;

public class DelegateCliCommandTests
{
    [Fact]
    public async Task ExecuteAsync_NoExecFuncGiven_ThrowsException()
    {
        var command = new DelegateCliCommand<TestOptionForCommand>();

        // Act
        Func<Task> act = async () => await command.ExecuteAsync(new TestOptionForCommand());

        // Assert
        await act
            .Should()
            .ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public async Task ExecuteAsync_OptionTypeNotMatching_ThrowsException()
    {
        var command = new DelegateCliCommand<TestOptionForCommand>();

        // Act
        Func<Task> act = async () => await command.ExecuteAsync(2);

        // Assert
        await act
            .Should()
            .ThrowAsync<InvalidCastException>();
    }
}
