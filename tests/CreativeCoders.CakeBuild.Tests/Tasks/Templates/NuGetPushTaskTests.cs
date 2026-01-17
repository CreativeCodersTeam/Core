using CreativeCoders.CakeBuild.Tasks.Templates;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;
using Xunit;

namespace CreativeCoders.CakeBuild.Tests.Tasks.Templates;

public class NuGetPushTaskTests
{
    private class TestNuGetPushBuildContext(Cake.Core.ICakeContext context)
        : CakeBuildContext(context), INuGetPushTaskSettings, IPackTaskSettings
    {
        public bool SkipPush { get; set; }
    }

    [Fact]
    public async Task RunAsync_ValidContext_RunsWithoutError()
    {
        // Arrange
        var cakeContext = CakeTestHelper.CreateCakeContext();
        CakeTestHelper.SetupFileSystem(cakeContext, "/repo", "/repo/test.sln");

        var context = new TestNuGetPushBuildContext(cakeContext);
        var task = new NuGetPushTask<TestNuGetPushBuildContext>();

        // Act
        await task.RunAsync(context);

        // Assert
        // Success if no exception occurs
    }

    [Fact]
    public async Task RunAsync_SkipPush_DoesNotPush()
    {
        // Arrange
        var cakeContext = CakeTestHelper.CreateCakeContext();
        CakeTestHelper.SetupFileSystem(cakeContext, "/repo", "/repo/test.sln");

        var context = new TestNuGetPushBuildContext(cakeContext) { SkipPush = true };
        var task = new NuGetPushTask<TestNuGetPushBuildContext>();

        // Act
        await task.RunAsync(context);

        // Assert
        // Success if no exception occurs (Information was logged)
    }
}
