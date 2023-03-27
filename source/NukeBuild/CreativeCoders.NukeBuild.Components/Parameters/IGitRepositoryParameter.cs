using Nuke.Common;
using Nuke.Common.Git;

namespace CreativeCoders.NukeBuild.Components.Parameters;

public interface IGitRepositoryParameter : INukeBuild
{
    [GitRepository]
    GitRepository GitRepository => TryGetValue(() => GitRepository) ??
                                   throw new MissingMemberException(
                                       nameof(IGitRepositoryParameter),
                                       nameof(GitRepository));
}
