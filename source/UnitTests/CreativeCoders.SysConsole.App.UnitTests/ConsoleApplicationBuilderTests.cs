using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CreativeCoders.SysConsole.App.UnitTests.TestData;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace CreativeCoders.SysConsole.App.UnitTests
{
    public class ConsoleApplicationBuilderTests
    {
        [Fact]
        public void Build_NoVerbAndProgramMainGiven_ThrowsException()
        {
            var builder = new ConsoleApplicationBuilder(Array.Empty<string>());

            // Act
            Action act = () => builder.Build();

            // Assert
            act
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void Build_ProgramMainHasInvalidCtorArgs_ThrowsException()
        {
            var builder = new ConsoleApplicationBuilder(Array.Empty<string>());

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
            var builder = new ConsoleApplicationBuilder(Array.Empty<string>());

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
            var builder = new ConsoleApplicationBuilder(Array.Empty<string>());

            builder.UseProgramMain<TestProgramMain>();
            builder.UseStartup<TestStartup>();

            // Act
            var _ = builder.Build();

            // Assert
            TestStartup.ConfigureServicesIsCalled
                .Should()
                .BeTrue();
        }

        [Fact]
        public void Build_ProgramMainHasPrivateCtor_ThrowsException()
        {
            var builder = new ConsoleApplicationBuilder(Array.Empty<string>());

            builder.UseProgramMain<TestProgramMainWithPrivateCtor>();

            // Act
            Action act = () => builder.Build();

            // Assert
            act
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public async Task RunAsync_WithVerb_VerbIsExecuted()
        {
            var args = new[] { "test", "arg0" };

            var consoleApp = new ConsoleApplicationBuilder(args)
                .UseVerbs(x => x
                    .AddVerb<TestVerb>())
                .Build();

            // Act
            var result = await consoleApp.RunAsync();

            // Assert
            result
                .Should()
                .Be(TestVerb.ReturnCode);

            TestVerb.OptionsFirstArg
                .Should()
                .Be("arg0");
        }

        [Fact]
        public async Task RunAsync_WithVerbNotMatchingVerb_ReturnsMinInt()
        {
            var args = new[] { "test1", "arg0" };

            var consoleApp = new ConsoleApplicationBuilder(args)
                .UseVerbs(x => x
                    .AddVerb<TestVerb>())
                .Build();

            // Act
            var result = await consoleApp.RunAsync();

            // Assert
            result
                .Should()
                .Be(int.MinValue);
        }

        [Fact]
        public async Task RunAsync_WithVerbAndErrorHandlerNotMatchingVerb_ExecutesErrorHandler()
        {
            var args = new[] { "test1", "arg0" };

            var consoleApp = new ConsoleApplicationBuilder(args)
                .UseVerbs(x => x
                    .AddVerb<TestVerb>()
                    .AddErrors<TestVerbErrorHandler>())
                .Build();

            // Act
            var result = await consoleApp.RunAsync();

            // Assert
            result
                .Should()
                .Be(TestVerbErrorHandler.ReturnCode);
        }

        [Fact]
        public async Task RunAsync_WithVerbAndProgramMainNotMatchingVerb_ExecutesProgramMain()
        {
            var args = new[] { "test1", "arg0" };

            var consoleApp = new ConsoleApplicationBuilder(args)
                .UseProgramMain<TestErrorProgramMain>()
                .UseVerbs(x => x
                    .AddVerb<TestVerb>())
                .Build();

            // Act
            var result = await consoleApp.RunAsync();

            // Assert
            result
                .Should()
                .Be(TestErrorProgramMain.ReturnCode);
        }

        [Fact]
        public async Task UseConfiguration_ConfigurationValuesGiven_ReturnsReturnCodeFromConfiguration()
        {
            const int returnCode = 4567;

            var consoleApp = new ConsoleApplicationBuilder(Array.Empty<string>())
                .UseProgramMain<TestProgramMainWithConfiguration>()
                .UseConfiguration(x => x.AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"ReturnCode", returnCode.ToString()}
                }))
                .Build();

            // Act
            var result = await consoleApp.RunAsync().ConfigureAwait(false);

            // Assert
            result
                .Should()
                .Be(returnCode);
        }
    }
}
