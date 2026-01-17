using Cake.Core.IO;
using CreativeCoders.CakeBuild.Tasks.Templates;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;
using Xunit;

namespace CreativeCoders.CakeBuild.Tests.Tasks.Templates;

public class TestTaskTests
{
    private class TestTestBuildContext(Cake.Core.ICakeContext context) : CakeBuildContext(context), ITestTaskSettings
    {
        public IEnumerable<FilePath> TestProjects => new[] { new FilePath("/repo/tests/Project.Tests.csproj") };
    }

    [Fact]
    public async Task RunAsync_ValidContext_RunsWithoutError()
    {
        // Arrange
        var cakeContext = CakeTestHelper.CreateCakeContext();
        CakeTestHelper.SetupFileSystem(cakeContext, "/repo", "/repo/test.sln");

        var context = new TestTestBuildContext(cakeContext);
        var task = new TestTask<TestTestBuildContext>();

        // Act
        await task.RunAsync(context);

        // Assert
        // Success if no exception occurs
    }
}
