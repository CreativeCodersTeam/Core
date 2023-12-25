using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CreativeCoders.SysConsole.App.MainProgram;
using CreativeCoders.SysConsole.App.UnitTests.TestData;
using CreativeCoders.SysConsole.CliArguments;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CreativeCoders.SysConsole.App.UnitTests;

public class ConsoleApplicationBuilderTests
{
    [Fact]
    public void Build_NoCommandAndProgramMainGiven_ThrowsException()
    {
        var builder = new ConsoleAppBuilder(Array.Empty<string>());

        // Act
        Action act = () => builder.Build();

        // Assert
        act
            .Should()
            .Throw<InvalidOperationException>();
    }

    [Fact]
    public void Build_ProgramMainHasInvalidCtorArgs_ReturnsIntMinValue()
    {
        var builder = new ConsoleAppBuilder(Array.Empty<string>());

        builder.UseProgramMain<TestProgramMainWithInvalidCtorArgs>();

        // Act
        Action act = () => builder.Build();

        // Assert
        act
            .Should()
            .Throw<InvalidOperationException>();
    }

    [Fact]
    public async Task RunAsync_ProgramMainGiven_RunsProgramMain()
    {
        var builder = new ConsoleAppBuilder(Array.Empty<string>());

        builder.UseProgramMain<TestProgramMain>();

        var consoleApp = builder.Build();

        // Act
        var result = await consoleApp.RunAsync();

        // Assert
        result
            .Should()
            .Be(TestProgramMain.ReturnCode);
    }

    [Fact]
    public void Build_WithStartup_StartupCreatedAndCalled()
    {
        var builder = new ConsoleAppBuilder(Array.Empty<string>());

        builder.UseProgramMain<TestProgramMain>();
        builder.UseStartup<TestStartup>();

        // Act
        _ = builder.Build();

        // Assert
        TestStartup.ConfigureServicesIsCalled
            .Should()
            .BeTrue();
    }

    [Fact]
    public void Build_ProgramMainHasPrivateCtor_ThrowsException()
    {
        var builder = new ConsoleAppBuilder(Array.Empty<string>());

        builder.UseProgramMain<TestProgramMainWithPrivateCtor>();

        // Act
        Action act = () => builder.Build();

        // Assert
        act
            .Should()
            .Throw<InvalidOperationException>();
    }

    [Fact]
    public async Task RunAsync_WithCommand_CommandIsExecuted()
    {
        var args = new[] {"test", "arg0"};

        var consoleApp = new ConsoleAppBuilder(args)
            .UseCli(x => x
                .AddCommand<TestCommand>())
            .Build();

        // Act
        var result = await consoleApp.RunAsync();

        // Assert
        result
            .Should()
            .Be(TestCommand.ReturnCode);

        TestCommand.OptionsFirstArg
            .Should()
            .Be("arg0");
    }

    [Fact]
    public async Task RunAsync_WithCommandNotMatchingCommand_ReturnsDefaultErrorReturnCode()
    {
        const int expectedReturnCode = -1357;
        var args = new[] {"test1", "arg0"};

        var consoleApp = new ConsoleAppBuilder(args)
            .UseCli(x => x
                .SetDefaultErrorReturnCode(expectedReturnCode)
                .AddCommand<TestCommand>())
            .Build();

        // Act
        var result = await consoleApp.RunAsync();

        // Assert
        result
            .Should()
            .Be(expectedReturnCode);
    }

    [Fact]
    public void UseCli_ProgramMainAlreadySet_ThrowsException()
    {
        var args = new[] {"test1", "arg0"};

        Action act = () => new ConsoleAppBuilder(args)
            .UseProgramMain<TestErrorProgramMain>()
            .UseCli(_ => { });

        // Assert
        act
            .Should()
            .Throw<InvalidOperationException>();
    }

    [Fact]
    public void UseProgramMain_CliBuilderAlreadySet_ThrowsException()
    {
        var args = new[] {"test1", "arg0"};

        Action act = () => new ConsoleAppBuilder(args)
            .UseCli(_ => { })
            .UseProgramMain<TestErrorProgramMain>();

        // Assert
        act
            .Should()
            .Throw<InvalidOperationException>();
    }

    [Fact]
    public async Task UseConfiguration_ConfigurationValuesGiven_ReturnsReturnCodeFromConfiguration()
    {
        const int returnCode = 4567;

        var consoleApp = new ConsoleAppBuilder(Array.Empty<string>())
            .UseProgramMain<TestProgramMainWithConfiguration>()
            .UseConfiguration(x => x.AddInMemoryCollection(new Dictionary<string, string>
            {
                {"ReturnCode", returnCode.ToString()}
            }))
            .Build();

        // Act
        var result = await consoleApp.RunAsync();

        // Assert
        result
            .Should()
            .Be(returnCode);
    }

    [Fact]
    public async Task UseCli_CallDemoGroupsCommand_CommandIsExecuted()
    {
        var args = new[] {"demo", "test"};

        var consoleApp = new ConsoleAppBuilder(args)
            .UseCli(x =>
                x.AddCommandGroup(groupBuilder =>
                    groupBuilder.SetName("demo").AddCommand<DemoGroupTestCommand>()))
            .Build();

        // Act
        var result = await consoleApp.RunAsync();

        // Assert
        result
            .Should()
            .Be(DemoGroupTestCommand.ReturnCode);
    }

    [Fact]
    public async Task UseVerbObjects_NoObjectFound_MatchingPureVerbIsCalled()
    {
        var args = new[] {"test", "something"};

        var consoleApp = new ConsoleAppBuilder(args)
            .UseCli(x => x
                .AddCommandGroup(groupBuilder =>
                    groupBuilder.SetName("demo").AddCommand<DemoGroupTestCommand>())
                .AddCommand<TestFallbackCommand>())
            .Build();

        // Act
        var result = await consoleApp.RunAsync();

        // Assert
        result
            .Should()
            .Be(TestFallbackCommand.ReturnCode);
    }

    [Fact]
    public async Task ConfigureServices_()
    {
        const int expectedReturnCode = 123456;
        const string expectedTextValue = "TestText";

        var testService = A.Fake<ITestService>();

        A.CallTo(() => testService.ReturnCode).Returns(expectedReturnCode);
        A.CallTo(() => testService.TextValue).Returns(expectedTextValue);

        var consoleApp = new ConsoleAppBuilder(Array.Empty<string>())
            .ConfigureServices(x => x.AddTransient(_ => testService))
            .UseProgramMain<TestServiceProgramMain>()
            .Build();

        // Act
        var result = await consoleApp.RunAsync();

        // Assert
        result
            .Should()
            .Be(expectedReturnCode);

        TestServiceProgramMain.TextValue
            .Should()
            .Be(expectedTextValue);
    }
}
