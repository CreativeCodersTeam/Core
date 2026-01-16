using Cake.Frosting;
using CreativeCoders.CakeBuild.GitHub;
using CreativeCoders.CakeBuild.Tasks.Templates;

namespace CreativeCoders.CakeBuild.Tasks.Defaults;

[TaskName("CreateGitHubRelease")]
public class CreateGitHubReleaseTask(IGitHubClientFactory gitHubClientFactory)
    : CreateGitHubReleaseTask<CakeBuildContext>(gitHubClientFactory);
