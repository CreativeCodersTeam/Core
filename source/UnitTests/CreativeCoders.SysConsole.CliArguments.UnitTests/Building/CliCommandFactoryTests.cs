using System;
using CreativeCoders.SysConsole.CliArguments.Building;
using CreativeCoders.SysConsole.CliArguments.Commands;
using CreativeCoders.SysConsole.CliArguments.Exceptions;
using CreativeCoders.SysConsole.CliArguments.UnitTests.Commands;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CreativeCoders.SysConsole.CliArguments.UnitTests.Building
{
    public class CliCommandFactoryTests
    {
        [Fact]
        public void CreateCommand_WithCommandFailingCtor_ThrowsException()
        {
            var factory = new CliCommandFactory(new ServiceCollection().BuildServiceProvider());

            // Act
            Action act = () => factory.CreateCommand<CommandWithErrorCtor>();

            // Assert
            act
                .Should()
                .Throw<CliCommandCreationFailedException>();
        }

        [Fact]
        public void CreateCommand_OptionWithCommandFailingCtor_ThrowsException()
        {
            var factory = new CliCommandFactory(new ServiceCollection().BuildServiceProvider());

            // Act
            Action act = () => factory.CreateCommand<CommandWithErrorCtor, TestOptionForCommand>(_ => { });

            // Assert
            act
                .Should()
                .Throw<CliCommandCreationFailedException>();
        }
    }

    [UsedImplicitly]
    public class CommandWithErrorCtor : DelegateCliCommand<TestOptionForCommand>
    {
        public CommandWithErrorCtor()
        {
            throw new ArgumentException();
        }
    }
}
