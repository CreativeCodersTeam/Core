using Nuke.Common.IO;

namespace CreativeCoders.NukeBuild.Components.Targets.Settings;

public interface ICodeCoverageSettings : ITestTargetSettings
{
    bool CodeCoverageIsEnabled => true;

    AbsolutePath CoverageDirectory => TestBaseDirectory / "coverage";
}
