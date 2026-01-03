using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cake.Common.Diagnostics;
using Cake.Common.Tools.GitVersion;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;
using CreativeCoders.Core.IO;
using JetBrains.Annotations;

namespace CreativeCoders.CakeBuild;

[PublicAPI]
public class BuildContext : FrostingContext
{
    private readonly List<IFrostingTask> _executedTasks = [];

    public BuildContext(ICakeContext context)
        : base(context)
    {
        RootDir = FindGitRootPath(context.Environment.WorkingDirectory) ??
                  context.Environment.WorkingDirectory.GetParent();

        var solutionsFilePath = context.Arguments.GetArgument("solution");
        SolutionFile = string.IsNullOrWhiteSpace(solutionsFilePath)
            ? FindRootSolution(context)
            : new FilePath(solutionsFilePath);

        Version = context.GetGitVersionSafe();
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

        if (solution == null)
        {
            context.Error("No solution file found.");

            throw new InvalidOperationException("No solution file found");
        }

        return solution;
    }

    public FilePath SolutionFile { get; set; }

    public GitVersion Version { get; set; }

    public DirectoryPath RootDir { get; set; }

    public DirectoryPath ArtifactsDir => RootDir.Combine(".artifacts");

    public string BuildConfiguration { get; set; } = "Release";

    public IList<IFrostingTask> ExecutedTasks => _executedTasks;

    public void AddExecutedTask(IFrostingTask task)
    {
        _executedTasks.Add(task);
    }

    public bool HasExecutedTask(Type taskType)
    {
        return _executedTasks.Any(x => x.GetType().IsAssignableTo(taskType));
    }
}
