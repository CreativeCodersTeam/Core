using CreativeCoders.CakeBuild.Tasks.Templates;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;
using Xunit;

namespace CreativeCoders.CakeBuild.Tests.Tasks.Templates;

public class PackTaskTests
{
    private class TestPackBuildContext(Cake.Core.ICakeContext context) : CakeBuildContext(context), IPackTaskSettings;

    [Fact]
    public async Task RunAsync_ValidContext_RunsWithoutError()
    {
        // Arrange
        var cakeContext = CakeTestHelper.CreateCakeContext();
        CakeTestHelper.SetupFileSystem(cakeContext, "/repo", "/repo/test.sln");

        var context = new TestPackBuildContext(cakeContext);
        var task = new PackTask<TestPackBuildContext>();

        // Act
        await task.RunAsync(context);

        // Assert
        // Success if no exception occurs
    }
}
