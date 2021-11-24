using JetBrains.Annotations;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.ReportGenerator;

namespace CreativeCoders.NukeBuild.BuildActions
{
    [PublicAPI]
    public class CreateCoverageReportAction : BuildActionBase<CreateCoverageReportAction>
    {
        private AbsolutePath _reportsPath;

        private AbsolutePath _targetPath;

        protected override void OnExecute()
        {
            ReportGeneratorTasks.ReportGenerator(
                x => x
                    .SetProcessToolPath(ToolPathResolver
                        .GetPackageExecutable("ReportGenerator", "ReportGenerator.exe", null, "net5.0"))
                    .SetReports(_reportsPath)
                    .SetTargetDirectory(_targetPath)
                    .SetReportTypes(ReportTypes.Cobertura)
            );
        }

        public CreateCoverageReportAction SetReports(AbsolutePath reportsPath)
        {
            _reportsPath = reportsPath;

            return this;
        }

        public CreateCoverageReportAction SetTargetPath(AbsolutePath targetPath)
        {
            _targetPath = targetPath;

            return this;
        }
    }
}
