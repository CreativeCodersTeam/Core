using AwesomeAssertions;
using CreativeCoders.CakeBuild.Tasks.Templates;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;
using FakeItEasy;
using Xunit;
using Cake.Core.IO;

namespace CreativeCoders.CakeBuild.Tests.Tasks.Templates;

public class PublishTaskTests
{
    private class TestPublishBuildContext(Cake.Core.ICakeContext context)
        : CakeBuildContext(context), IPublishTaskSettings;

    [Fact]
    public async Task RunAsync_ValidContext_RunsWithoutError()
    {
        // Arrange
        var cakeContext = CakeTestHelper.CreateCakeContext("dotnet-publish");
        CakeTestHelper.SetupFileSystem(cakeContext, "/repo", "/repo/test.sln");

        var context = new TestPublishBuildContext(cakeContext);
        var task = new PublishTask<TestPublishBuildContext>();

        // Act
        var act = () => task.RunAsync(context);

        // Assert
        await act
            .Should()
            .NotThrowAsync();

        A.CallTo(() => cakeContext.ProcessRunner.Start("dotnet-publish",
                A<ProcessSettings>.That.Matches(x =>
                    x.Arguments.Select(arg => arg.Render()).FirstOrDefault() == "publish" &&
                    x.Arguments.Select(arg => arg.Render()).Contains("\"/repo/test.sln\""))))
            .MustHaveHappenedOnceExactly();
    }
}
