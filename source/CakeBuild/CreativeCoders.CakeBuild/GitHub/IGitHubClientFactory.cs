using Octokit;

namespace CreativeCoders.CakeBuild.GitHub;

public interface IGitHubClientFactory
{
    IGitHubClient Create(string gitHubToken);
}

public class GitHubClientFactory() : IGitHubClientFactory
{
    public IGitHubClient Create(string gitHubToken)
    {
        return new GitHubClient(new ProductHeaderValue("CreativeCoders.CakeBuild"))
        {
            Credentials = new Credentials(gitHubToken)
        };
    }
}
