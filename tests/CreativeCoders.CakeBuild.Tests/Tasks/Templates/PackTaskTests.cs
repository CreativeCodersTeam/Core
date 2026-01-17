using AwesomeAssertions;
using CreativeCoders.CakeBuild.Tasks.Templates;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;
using FakeItEasy;
using Xunit;
using Cake.Core.IO;

namespace CreativeCoders.CakeBuild.Tests.Tasks.Templates;

public class PackTaskTests
{
    private class TestPackBuildContext(Cake.Core.ICakeContext context)
        : CakeBuildContext(context), IPackTaskSettings;

    [Fact]
    public async Task RunAsync_ValidContext_RunsWithoutError()
    {
        // Arrange
        var cakeContext = CakeTestHelper.CreateCakeContext("dotnet-pack");
        CakeTestHelper.SetupFileSystem(cakeContext, "/repo", "/repo/test.sln");

        var context = new TestPackBuildContext(cakeContext);
        var task = new PackTask<TestPackBuildContext>();

        // Act
        var act = () => task.RunAsync(context);

        // Assert
        await act
            .Should()
            .NotThrowAsync();

        A.CallTo(() => cakeContext.ProcessRunner.Start("dotnet-pack",
                A<ProcessSettings>.That.Matches(x =>
                    x.Arguments.Select(arg => arg.Render()).FirstOrDefault() == "pack" &&
                    x.Arguments.Select(arg => arg.Render()).Contains("\"/repo/test.sln\""))))
            .MustHaveHappenedOnceExactly();
    }
}
