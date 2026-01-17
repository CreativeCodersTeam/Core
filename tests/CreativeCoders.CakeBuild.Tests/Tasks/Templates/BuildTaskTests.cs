using AwesomeAssertions;
using Cake.Core.IO;
using CreativeCoders.CakeBuild.Tasks.Templates;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.CakeBuild.Tests.Tasks.Templates;

public class BuildTaskTests
{
    [Fact]
    public async Task RunAsync_ValidContext_RunsWithoutError()
    {
        // Arrange
        var cakeContext = CakeTestHelper.CreateCakeContext("dotnet-build");
        CakeTestHelper.SetupFileSystem(cakeContext, "/repo", "/repo/test.sln");

        var context = new CakeBuildContext(cakeContext);
        var task = new BuildTask<CakeBuildContext>();

        A.CallTo(() => cakeContext.ProcessRunner.Start("dotnet-build", A<ProcessSettings>._))
            .Invokes(call => Console.WriteLine(call.Arguments));

        // Act
        var act = () => task.RunAsync(context);

        // Assert
        await act
            .Should()
            .NotThrowAsync();

        A.CallTo(() => cakeContext.ProcessRunner.Start("dotnet-build",
                A<ProcessSettings>.That.Matches(x =>
                    x.Arguments.Select(x => x.Render()).FirstOrDefault() == "build" &&
                    x.Arguments.Select(x => x.Render()).Skip(1).FirstOrDefault() == "\"/repo/test.sln\"")))
            .MustHaveHappenedOnceExactly();
    }
}
