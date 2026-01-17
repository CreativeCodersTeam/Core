using AwesomeAssertions;
using Cake.Core.IO;
using CreativeCoders.CakeBuild.Tasks.Templates;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.CakeBuild.Tests.Tasks.Templates;

public class TestTaskTests
{
    private class TestTestBuildContext(Cake.Core.ICakeContext context)
        : CakeBuildContext(context), ITestTaskSettings
    {
        public IEnumerable<FilePath> TestProjects =>
            [new FilePath("/repo/tests/Project.Tests.csproj")];
    }

    [Fact]
    public async Task RunAsync_ValidContext_RunsWithoutError()
    {
        // Arrange
        var cakeContext = CakeTestHelper.CreateCakeContext("dotnet-test");
        CakeTestHelper.SetupFileSystem(cakeContext, "/repo", "/repo/test.sln");

        var context = new TestTestBuildContext(cakeContext);
        var task = new TestTask<TestTestBuildContext>();

        // Act
        var act = () => task.RunAsync(context);

        // Assert
        await act
            .Should()
            .NotThrowAsync();

        A.CallTo(() => cakeContext.ProcessRunner.Start("dotnet-test",
                A<ProcessSettings>.That.Matches(x =>
                    x.Arguments.Select(arg => arg.Render()).FirstOrDefault() == "test" &&
                    x.Arguments.Select(arg => arg.Render())
                        .Contains("\"/repo/tests/Project.Tests.csproj\""))))
            .MustHaveHappenedOnceExactly();
    }
}
