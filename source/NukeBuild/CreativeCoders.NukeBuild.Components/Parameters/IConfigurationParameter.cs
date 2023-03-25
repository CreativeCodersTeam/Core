using Nuke.Common;
using Nuke.Common.IO;

namespace CreativeCoders.NukeBuild.Components.Parameters;

public interface IConfigurationParameter : INukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    Configuration Configuration => TryGetValue(() => Configuration) ??
                                   (IsLocalBuild ? Configuration.Debug : Configuration.Release);
}
