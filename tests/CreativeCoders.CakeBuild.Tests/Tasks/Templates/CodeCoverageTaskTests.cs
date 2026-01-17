using AwesomeAssertions;
using Cake.Core.IO;
using CreativeCoders.CakeBuild.Tasks.Templates;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.CakeBuild.Tests.Tasks.Templates;

public class CodeCoverageTaskTests
{
    private class TestCodeCoverageBuildContext(Cake.Core.ICakeContext context)
        : CakeBuildContext(context), ICodeCoverageTaskSettings;

    [Fact]
    public async Task RunAsync_ValidContext_RunsWithoutError()
    {
        // Arrange
        var cakeContext = CakeTestHelper.CreateCakeContext("reportgenerator");
        CakeTestHelper.SetupFileSystem(cakeContext, "/repo", "/repo/test.sln");

        var context = new TestCodeCoverageBuildContext(cakeContext);
        var task = new CodeCoverageTask<TestCodeCoverageBuildContext>();

        // Act
        var act = () => task.RunAsync(context);

        // Assert
        await act
            .Should()
            .NotThrowAsync();

        A.CallTo(() => cakeContext.ProcessRunner.Start("reportgenerator", A<ProcessSettings>._))
            .MustHaveHappenedOnceExactly();
    }
}
