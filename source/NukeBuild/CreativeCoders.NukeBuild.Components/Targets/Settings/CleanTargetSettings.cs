using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

namespace CreativeCoders.NukeBuild.Components.Targets.Settings;

[Serializable]
public class CleanTargetSettings : DotNetCleanSettings
{
    public CleanTargetSettings AddDirectoryForClean(AbsolutePath directory)
    {
        var settings = this.NewInstance();

        settings.DirectoriesToClean.Add(directory);

        return settings;
    }

    public IList<AbsolutePath> DirectoriesToClean { get; set; } = new List<AbsolutePath>();
}
