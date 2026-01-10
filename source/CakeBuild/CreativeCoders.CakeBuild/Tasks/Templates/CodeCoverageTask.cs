using Cake.Common.Diagnostics;
using Cake.Common.Tools.ReportGenerator;
using Cake.Core.IO;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;

namespace CreativeCoders.CakeBuild.Tasks.Templates;

public class CodeCoverageTask<T> : FrostingTaskBase<T>
    where T : BuildContext
{
    protected override Task RunAsyncCore(T context)
    {
        var codeCoverageSettings = context.GetRequiredSettings<ICodeCoverageTaskSettings>();

        var reportGeneratorSettings = new ReportGeneratorSettings
        {
            ReportTypes = codeCoverageSettings.ReportTypes.ToList()
        };

        context.ReportGenerator(new GlobPattern(codeCoverageSettings.ReportGlobPattern),
            context.CodeCoverageReportDir, reportGeneratorSettings);

        return Task.CompletedTask;
    }
}
