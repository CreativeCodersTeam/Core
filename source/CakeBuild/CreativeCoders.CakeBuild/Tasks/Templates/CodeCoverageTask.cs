using Cake.Common.Tools.ReportGenerator;
using Cake.Core.IO;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;

namespace CreativeCoders.CakeBuild.Tasks.Templates;

public class CodeCoverageTask<T> : FrostingTaskBase<T, ICodeCoverageTaskSettings>
    where T : CakeBuildContext
{
    protected override Task RunAsyncCore(T context, ICodeCoverageTaskSettings taskSettings)
    {
        var reportGeneratorSettings = new ReportGeneratorSettings
        {
            ReportTypes = taskSettings.ReportTypes.ToList()
        };

        context.ReportGenerator(new GlobPattern(taskSettings.ReportGlobPattern),
            context.CodeCoverageReportDir, reportGeneratorSettings);

        return Task.CompletedTask;
    }
}
