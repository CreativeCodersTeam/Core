using System;
using System.Threading.Tasks;
using CreativeCoders.SysConsole.Core.Abstractions;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.SysConsole.App.UnitTests
{
    public class DefaultConsoleApplicationTests
    {
        [Fact]
        public async Task RunAsync_MainReturnsValue_ValueIsReturned()
        {
            const int expectedResult = 1234;

            var sysConsole = A.Fake<ISysConsole>();

            var executor = A.Fake<IConsoleAppExecutor>();

            A.CallTo(() => executor.ExecuteAsync(A<string[]>.Ignored)).Returns(Task.FromResult(expectedResult));

            var consoleApp = new DefaultConsoleApp(executor, Array.Empty<string>(), sysConsole);

            // Act
            var result = await consoleApp.RunAsync();

            // Assert
            result
                .Should()
                .Be(expectedResult);
        }

        [Fact]
        public async Task RunAsync_MainThrowsConsoleException_ReturnCodeFromExceptionIsReturned()
        {
            const int expectedResult = -1234;

            var sysConsole = A.Fake<ISysConsole>();

            var executor = A.Fake<IConsoleAppExecutor>();

            A.CallTo(() => executor.ExecuteAsync(A<string[]>.Ignored)).Throws(_ => new ConsoleException(expectedResult));

            var consoleApp = new DefaultConsoleApp(executor, Array.Empty<string>(), sysConsole);

            // Act
            var result = await consoleApp.RunAsync();

            // Assert
            result
                .Should()
                .Be(expectedResult);
        }

        [Fact]
        public async Task RunAsync_MainThrowsArgumentException_ReturnCodeIsMinInt()
        {
            var sysConsole = A.Fake<ISysConsole>();

            var executor = A.Fake<IConsoleAppExecutor>();

            A.CallTo(() => executor.ExecuteAsync(A<string[]>.Ignored)).Throws(_ => new ArgumentException());

            var consoleApp = new DefaultConsoleApp(executor, Array.Empty<string>(), sysConsole);

            // Act
            var result = await consoleApp.RunAsync();

            // Assert
            result
                .Should()
                .Be(int.MinValue);
        }
    }
}
