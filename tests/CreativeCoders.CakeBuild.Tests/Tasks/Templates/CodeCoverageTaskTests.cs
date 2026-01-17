using CreativeCoders.CakeBuild.Tasks.Templates;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;
using Xunit;

namespace CreativeCoders.CakeBuild.Tests.Tasks.Templates;

public class CodeCoverageTaskTests
{
    private class TestCodeCoverageBuildContext(Cake.Core.ICakeContext context) : CakeBuildContext(context), ICodeCoverageTaskSettings;

    [Fact]
    public async Task RunAsync_ValidContext_RunsWithoutError()
    {
        // Arrange
        var cakeContext = CakeTestHelper.CreateCakeContext();
        CakeTestHelper.SetupFileSystem(cakeContext, "/repo", "/repo/test.sln");

        var context = new TestCodeCoverageBuildContext(cakeContext);
        var task = new CodeCoverageTask<TestCodeCoverageBuildContext>();

        // Act
        await task.RunAsync(context);

        // Assert
        // Success if no exception occurs
    }
}
