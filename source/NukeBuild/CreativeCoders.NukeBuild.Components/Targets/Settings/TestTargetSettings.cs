using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

namespace CreativeCoders.NukeBuild.Components.Targets.Settings;

public class TestTargetSettings : DotNetTestSettings
{
    public TestTargetSettings AddTestProjectFile(AbsolutePath testProjectFile)
    {
        var settings = this.NewInstance();

        settings.TestProjectFiles.Add(testProjectFile);

        return settings;
    }

    public IList<AbsolutePath> TestProjectFiles { get; set; } = new List<AbsolutePath>();
}
