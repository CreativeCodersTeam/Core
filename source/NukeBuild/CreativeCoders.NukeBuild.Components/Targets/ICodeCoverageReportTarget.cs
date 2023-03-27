using CreativeCoders.NukeBuild.Components.Targets.Settings;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.Tools.ReportGenerator;

namespace CreativeCoders.NukeBuild.Components.Targets;

[PublicAPI]
public interface ICodeCoverageReportTarget : ITestTarget, ICodeCoverageReportSettings
{
    Target CodeCoverageReport => _ => _
        .TryBefore<IPackTarget>()
        .DependsOn<ITestTarget>()
        .Produces(TargetDirectory)
        .Executes(() =>
        {
            ReportGeneratorTasks.ReportGenerator(x => x
                .SetFramework(Framework)
                .SetReports(Reports)
                .SetTargetDirectory(TargetDirectory));
        });
}
