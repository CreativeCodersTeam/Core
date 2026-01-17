using CreativeCoders.CakeBuild.GitHub;
using CreativeCoders.CakeBuild.Tasks.Templates;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;
using FakeItEasy;
using Octokit;
using Xunit;

namespace CreativeCoders.CakeBuild.Tests.Tasks.Templates;

public class CreateGitHubReleaseTaskTests
{
    private class TestGitHubReleaseBuildContext(Cake.Core.ICakeContext context)
        : CakeBuildContext(context), ICreateGitHubReleaseTaskSettings
    {
        public string ReleaseName => "Release 1.0.0";

        public string ReleaseVersion => "1.0.0";
    }

    [Fact]
    public async Task RunAsync_ValidContext_CallsGitHubClient()
    {
        // Arrange
        var cakeContext = CakeTestHelper.CreateCakeContext();
        CakeTestHelper.SetupFileSystem(cakeContext, "/repo", "/repo/test.sln");

        var gitHubClientFactory = A.Fake<IGitHubClientFactory>();
        var gitHubClient = A.Fake<IGitHubClient>();
        var repositoryClient = A.Fake<IRepositoriesClient>();
        var releasesClient = A.Fake<IReleasesClient>();

        A.CallTo(() => gitHubClientFactory.Create(A<string>._)).Returns(gitHubClient);
        A.CallTo(() => gitHubClient.Repository).Returns(repositoryClient);
        A.CallTo(() => repositoryClient.Release).Returns(releasesClient);

        // Mock GitHubActions environment variables
        A.CallTo(() => cakeContext.Environment.GetEnvironmentVariable("GITHUB_ACTIONS")).Returns("true");
        A.CallTo(() => cakeContext.Environment.GetEnvironmentVariable("GITHUB_REPOSITORY"))
            .Returns("owner/repo");

        var context = new TestGitHubReleaseBuildContext(cakeContext);
        var task = new CreateGitHubReleaseTask<TestGitHubReleaseBuildContext>(gitHubClientFactory);

        // Act
        await task.RunAsync(context);

        // Assert
        A.CallTo(() => gitHubClientFactory.Create(A<string>._)).MustHaveHappened();
    }
}
