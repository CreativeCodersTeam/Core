using Cake.Common.Build;
using Cake.Common.Diagnostics;
using Cake.Common.Tools.GitVersion;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;
using CreativeCoders.Core.IO;
using JetBrains.Annotations;

namespace CreativeCoders.CakeBuild;

[PublicAPI]
public class CakeBuildContext : FrostingContext, ICakeBuildContext, IBuildContextAccessor
{
    private readonly List<IFrostingTask> _executedTasks = [];

    public CakeBuildContext(ICakeContext context)
        : base(context)
    {
        RootDir = FindGitRootPath(context.Environment.WorkingDirectory) ??
                  context.Environment.WorkingDirectory.GetParent();

        var solutionsFilePath = context.Arguments.GetArgument("solution");
        SolutionFile = string.IsNullOrWhiteSpace(solutionsFilePath)
            ? FindRootSolution(context)
            : new FilePath(solutionsFilePath);
    }

    private DirectoryPath? FindGitRootPath(DirectoryPath? startPath)
    {
        while (true)
        {
            if (startPath == null)
            {
                return null;
            }

            if (FileSystem.Exist(startPath.Combine(".git")))
            {
                return startPath;
            }

            startPath = startPath.GetParent();
        }
    }

    private FilePath FindRootSolution(ICakeContext context)
    {
        var solutionsFiles =
            FileSys.Directory.EnumerateFiles(RootDir.FullPath, "*.sln", SearchOption.AllDirectories);

        var solutionsSlnxFiles =
            FileSys.Directory.EnumerateFiles(RootDir.FullPath, "*.slnx", SearchOption.AllDirectories);

        var solutions = solutionsFiles.Concat(solutionsSlnxFiles).ToArray();

        if (solutions.Length > 1)
        {
            context.Error("Multiple solution files found.");
        }

        var solution = solutions.FirstOrDefault();

        if (solution != null)
        {
            return solution;
        }

        context.Error("No solution file found.");

        throw new InvalidOperationException("No solution file found");
    }

    public FilePath SolutionFile { get; }

    public DirectoryPath RootDir { get; }

    public virtual DirectoryPath ArtifactsDir => RootDir.Combine(".artifacts");

    public virtual DirectoryPath TestOutputBasePath => RootDir.Combine(".tests");

    public virtual DirectoryPath TestResultsDir => TestOutputBasePath.Combine("results");

    public virtual DirectoryPath CodeCoverageDir => TestOutputBasePath.Combine("coverage");

    public virtual DirectoryPath CodeCoverageReportDir => TestOutputBasePath.Combine("coverage-report");

    public virtual string BuildConfiguration => this.BuildSystem().IsLocalBuild ? "Debug" : "Release";

    public GitVersion Version => this.GetGitVersionSafe();

    public IList<IFrostingTask> ExecutedTasks => _executedTasks;

    public bool PrintSetupSummary => true;

    public void AddExecutedTask(IFrostingTask task)
    {
        _executedTasks.Add(task);
    }

    public bool HasExecutedTask(Type taskType)
    {
        return _executedTasks.Any(x => x.GetType().IsAssignableTo(taskType));
    }

    public T GetRequiredSettings<T>()
        where T : class
    {
        return GetSettings<T>()
               ?? throw new InvalidOperationException($"No settings of type '{typeof(T).Name}' found");
    }

    public T? GetSettings<T>()
        where T : class
    {
        return this as T
               ?? FindSettingsProperty<T>()
               ?? null;
    }

    private T? FindSettingsProperty<T>()
        where T : class
    {
        var matchingProperties = GetType()
            .GetProperties()
            .Where(x => x.PropertyType == typeof(T) ||
                        x.PropertyType.GetInterfaces().Any(intfType =>
                            intfType == typeof(T)));

        return matchingProperties
            .Select(x => x.GetValue(this))
            .OfType<T>()
            .FirstOrDefault();
    }

    public ICakeBuildContext Context => this;
}
