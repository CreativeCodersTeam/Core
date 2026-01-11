using Cake.Common.Tools.GitVersion;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;
using JetBrains.Annotations;

namespace CreativeCoders.CakeBuild;

[PublicAPI]
public interface ICakeBuildContext : ICakeContext
{
    FilePath SolutionFile { get; }

    DirectoryPath RootDir { get; }

    DirectoryPath ArtifactsDir { get; }

    DirectoryPath TestOutputBasePath { get; }

    DirectoryPath TestResultsDir { get; }

    DirectoryPath CodeCoverageDir { get; }

    DirectoryPath CodeCoverageReportDir { get; }

    string BuildConfiguration { get; }

    GitVersion Version { get; }

    bool PrintSetupSummary { get; }

    IList<IFrostingTask> ExecutedTasks { get; }

    void AddExecutedTask(IFrostingTask task);

    bool HasExecutedTask(Type taskType);

    T GetRequiredSettings<T>()
        where T : class;
}
