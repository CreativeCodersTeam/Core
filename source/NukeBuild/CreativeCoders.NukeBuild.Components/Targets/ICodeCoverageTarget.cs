using System.IO.Compression;
using CreativeCoders.NukeBuild.Components.Targets.Settings;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.Tools.ReportGenerator;

namespace CreativeCoders.NukeBuild.Components.Targets;

[PublicAPI]
public interface ICodeCoverageTarget : ITestTarget, ICodeCoverageReportSettings
{
    Target CodeCoverage => d => d
        .TryBefore<IPackTarget>()
        .DependsOn<ITestTarget>()
        .Produces(TargetDirectory / "*.*")
        .Executes(() =>
        {
            ReportGeneratorTasks.ReportGenerator(x => x
                .SetFramework(Framework)
                .SetReports(Reports)
                .SetTargetDirectory(TargetDirectory));

            ZipFile.CreateFromDirectory(TargetDirectory, CoverageReportArchive);
        });
}
