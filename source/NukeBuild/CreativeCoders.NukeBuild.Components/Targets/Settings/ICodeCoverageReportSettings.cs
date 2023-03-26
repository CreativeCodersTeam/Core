using CreativeCoders.Core;
using CreativeCoders.Core.SysEnvironment;
using Nuke.Common;
using Nuke.Common.IO;

namespace CreativeCoders.NukeBuild.Components.Targets.Settings;

public interface ICodeCoverageReportSettings : INukeBuild, ITestSettings
{
    AbsolutePath TargetDirectory => this.TryAs<ITestSettings>(out var testSettings)
        ? testSettings.TestBaseDirectory / "coverage_report"
        : TemporaryDirectory / "coverage_report";

    string Framework => $"net{Env.Version.Major}.0";

    IEnumerable<string> Reports => new string[] {TestBaseDirectory / "coverage" / "**" / "*.xml"};
}
