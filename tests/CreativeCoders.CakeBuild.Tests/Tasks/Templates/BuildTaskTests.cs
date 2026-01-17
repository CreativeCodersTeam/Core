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

        // Act
        var act = () => task.RunAsync(context);

        // Assert
        await act
            .Should()
            .NotThrowAsync();

        A.CallTo(() => cakeContext.ProcessRunner.Start("dotnet-build",
                A<ProcessSettings>.That.Matches(x =>
                    x.Arguments.Select(arg => arg.Render()).FirstOrDefault() == "build" &&
                    x.Arguments.Select(arg => arg.Render()).Skip(1).FirstOrDefault() ==
                    "\"/repo/test.sln\"")))
            .MustHaveHappenedOnceExactly();
    }
}
