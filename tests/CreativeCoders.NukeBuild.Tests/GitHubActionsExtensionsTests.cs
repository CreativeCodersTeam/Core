using CreativeCoders.Core.SysEnvironment;
using CreativeCoders.NukeBuild.Components;
using FakeItEasy;
using AwesomeAssertions;
using Nuke.Common.CI.GitHubActions;

namespace CreativeCoders.NukeBuild.Tests;

[Collection("Env")]
public class GitHubActionsExtensionsTests
{
    [Theory]
    [InlineData("Windows", "Windows", true)]
    [InlineData("windows", "Windows", true)]
    [InlineData("Linux", "Windows", false)]
    [InlineData("macOS", "macos", true)]
    public void IsRunnerOs_Always_CallsGetEnvironmentVariableAndReturnsExpectedResult(
        string environmentVariableValue, string runnerOs, bool expectedResult)
    {
        // Arrange
        var gitHubActions = GitHubActions.Instance;

        var env = A.Fake<IEnvironment>();
        Env.SetEnvironmentImpl(env);

        A.CallTo(() => env.GetEnvironmentVariable("RUNNER_OS"))
            .Returns(environmentVariableValue);

        // Act
        var actual = gitHubActions.IsRunnerOs(runnerOs);

        // Assert
        A.CallTo(() => env.GetEnvironmentVariable("RUNNER_OS"))
            .MustHaveHappenedOnceExactly();

        actual
            .Should()
            .Be(expectedResult);
    }
}
