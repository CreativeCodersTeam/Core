using Cake.Common.Tools.GitVersion;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;

namespace CreativeCoders.CakeBuild;

public interface IBuildContext : ICakeContext
{
    FilePath SolutionFile { get; }

    DirectoryPath RootDir { get; }

    DirectoryPath ArtifactsDir { get; }

    DirectoryPath TestOutputBasePath { get; }

    DirectoryPath CodeCoverageResultsDir { get; }

    string BuildConfiguration { get; }

    GitVersion Version { get; }

    IList<IFrostingTask> ExecutedTasks { get; }

    void AddExecutedTask(IFrostingTask task);

    bool HasExecutedTask(Type taskType);

    T GetSettings<T>()
        where T : class;
}
