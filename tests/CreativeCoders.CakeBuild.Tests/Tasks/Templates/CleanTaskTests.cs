using Cake.Core.IO;
using CreativeCoders.CakeBuild.Tasks.Templates;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;
using Xunit;

namespace CreativeCoders.CakeBuild.Tests.Tasks.Templates;

public class CleanTaskTests
{
    private class TestCleanBuildContext(Cake.Core.ICakeContext context) : CakeBuildContext(context), ICleanTaskSettings;

    [Fact]
    public async Task RunAsync_ValidContext_RunsWithoutError()
    {
        // Arrange
        var cakeContext = CakeTestHelper.CreateCakeContext();
        CakeTestHelper.SetupFileSystem(cakeContext, "/repo", "/repo/test.sln");

        var context = new TestCleanBuildContext(cakeContext);
        var task = new CleanTask<TestCleanBuildContext>();

        // Act
        await task.RunAsync(context);

        // Assert
        // Success if no exception occurs
    }
}
