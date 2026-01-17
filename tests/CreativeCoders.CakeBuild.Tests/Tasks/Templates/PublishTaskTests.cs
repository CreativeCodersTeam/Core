using CreativeCoders.CakeBuild.Tasks.Templates;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;
using Xunit;

namespace CreativeCoders.CakeBuild.Tests.Tasks.Templates;

public class PublishTaskTests
{
    private class TestPublishBuildContext(Cake.Core.ICakeContext context) : CakeBuildContext(context), IPublishTaskSettings;

    [Fact]
    public async Task RunAsync_ValidContext_RunsWithoutError()
    {
        // Arrange
        var cakeContext = CakeTestHelper.CreateCakeContext();
        CakeTestHelper.SetupFileSystem(cakeContext, "/repo", "/repo/test.sln");

        var context = new TestPublishBuildContext(cakeContext);
        var task = new PublishTask<TestPublishBuildContext>();

        // Act
        await task.RunAsync(context);

        // Assert
        // Success if no exception occurs
    }
}
