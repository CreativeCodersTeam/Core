using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;

namespace CreativeCoders.NukeBuild.Components.Targets.Settings;

public interface ITestSettings : INukeBuild
{
    IEnumerable<Project> TestProjects { get; }

    AbsolutePath TestBaseDirectory => RootDirectory / ".tests";

    AbsolutePath TestResultsDirectory => TestBaseDirectory / "results";

    bool GenerateCodeCoverage => true;

    AbsolutePath CoverageDirectory => TestBaseDirectory / "coverage";
}
