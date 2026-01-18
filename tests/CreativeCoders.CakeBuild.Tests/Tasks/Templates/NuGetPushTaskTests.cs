using AwesomeAssertions;
using CreativeCoders.CakeBuild.Tasks.Templates;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;
using FakeItEasy;
using Xunit;
using Cake.Core.IO;

namespace CreativeCoders.CakeBuild.Tests.Tasks.Templates;

public class NuGetPushTaskTests
{
    private class TestNuGetPushBuildContext(Cake.Core.ICakeContext context)
        : CakeBuildContext(context), INuGetPushTaskSettings, IPackTaskSettings
    {
        public bool SkipPush { get; set; }

        public DirectoryPath PackOutputDirectory => new("/repo/dist");
    }

    [Fact]
    public async Task RunAsync_ValidContext_RunsWithoutError()
    {
        // Arrange
        var cakeContext = CakeTestHelper.CreateCakeContext("dotnet-nuget-push");
        CakeTestHelper.SetupFileSystem(cakeContext, "/repo", "/repo/test.sln");

        var context = new TestNuGetPushBuildContext(cakeContext);
        var task = new NuGetPushTask<TestNuGetPushBuildContext>();

        // Act
        var act = () => task.RunAsync(context);

        // Assert
        await act
            .Should()
            .NotThrowAsync();

        A.CallTo(() => cakeContext.ProcessRunner.Start("dotnet-nuget-push",
                A<ProcessSettings>.That.Matches(x =>
                    x.Arguments.Select(arg => arg.Render()).Any(arg => arg.Contains("nuget")) &&
                    x.Arguments.Select(arg => arg.Render()).Any(arg => arg.Contains("push")))))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task RunAsync_SkipPush_DoesNotPush()
    {
        // Arrange
        var cakeContext = CakeTestHelper.CreateCakeContext("dotnet-nuget-push");
        CakeTestHelper.SetupFileSystem(cakeContext, "/repo", "/repo/test.sln");

        var context = new TestNuGetPushBuildContext(cakeContext) { SkipPush = true };
        var task = new NuGetPushTask<TestNuGetPushBuildContext>();

        // Act
        await task.RunAsync(context);

        // Assert
        A.CallTo(() => cakeContext.ProcessRunner.Start(A<FilePath>._, A<ProcessSettings>._))
            .MustNotHaveHappened();
    }
}
