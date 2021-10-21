using System;
using System.Threading.Tasks;
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

            var main = A.Fake<IMain>();

            A.CallTo(() => main.ExecuteAsync()).Returns(Task.FromResult(expectedResult));

            var consoleApp = new DefaultConsoleApp(main);

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

            var main = A.Fake<IMain>();

            A.CallTo(() => main.ExecuteAsync()).Throws(_ => new ConsoleException(expectedResult));

            var consoleApp = new DefaultConsoleApp(main);

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
            var main = A.Fake<IMain>();

            A.CallTo(() => main.ExecuteAsync()).Throws(_ => new ArgumentException());

            var consoleApp = new DefaultConsoleApp(main);

            // Act
            var result = await consoleApp.RunAsync();

            // Assert
            result
                .Should()
                .Be(int.MinValue);
        }
    }
}
