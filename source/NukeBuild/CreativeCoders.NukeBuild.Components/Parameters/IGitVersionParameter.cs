using Nuke.Common;
using Nuke.Common.Tools.GitVersion;

namespace CreativeCoders.NukeBuild.Components.Parameters;

public interface IGitVersionParameter : INukeBuild
{
    [GitVersion]
    GitVersion? GitVersion => TryGetValue(() => GitVersion);
}
