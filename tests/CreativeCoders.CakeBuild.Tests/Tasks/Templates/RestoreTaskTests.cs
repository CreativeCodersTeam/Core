using Cake.Core.IO;
using CreativeCoders.CakeBuild.Tasks.Templates;
using Xunit;

namespace CreativeCoders.CakeBuild.Tests.Tasks.Templates;

public class RestoreTaskTests
{
    [Fact]
    public async Task RunAsync_ValidContext_RunsWithoutError()
    {
        // Arrange
        var cakeContext = CakeTestHelper.CreateCakeContext();
        CakeTestHelper.SetupFileSystem(cakeContext, "/repo", "/repo/test.sln");

        var context = new CakeBuildContext(cakeContext);
        var task = new RestoreTask<CakeBuildContext>();

        // Act
        await task.RunAsync(context);

        // Assert
        // If no exception occurs, it's already a good sign for this simple task
    }
}
