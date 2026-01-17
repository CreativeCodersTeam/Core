using AwesomeAssertions;
using Cake.Core.IO;
using CreativeCoders.CakeBuild.Tasks.Templates;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.CakeBuild.Tests.Tasks.Templates;

[Collection("FileSys")]
public class RestoreTaskTests
{
    [Fact]
    public async Task RunAsync_ValidContext_RunsWithoutError()
    {
        // Arrange
        var cakeContext = CakeTestHelper.CreateCakeContext("dotnet-restore");
        CakeTestHelper.SetupFileSystem(cakeContext, "/repo", "/repo/test.sln");

        var context = new CakeBuildContext(cakeContext);
        var task = new RestoreTask<CakeBuildContext>();

        // Act
        var act = () => task.RunAsync(context);

        // Assert
        await act
            .Should()
            .NotThrowAsync();

        A.CallTo(() => cakeContext.ProcessRunner.Start("dotnet-restore",
                A<ProcessSettings>.That.Matches(x =>
                    x.Arguments.Select(arg => arg.Render()).Contains("restore") &&
                    x.Arguments.Select(arg => arg.Render()).Any(arg => arg.Contains("/repo")))))
            .MustHaveHappenedOnceExactly();
    }
}
