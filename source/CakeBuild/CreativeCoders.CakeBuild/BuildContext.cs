using Cake.Common.Tools.GitVersion;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;

namespace CreativeCoders.CakeBuild;

public class BuildContext : FrostingContext
{
    private readonly List<IFrostingTask> _executedTasks = [];

    public FilePath SolutionFile { get; set; }

    public GitVersion Version { get; set; }

    public DirectoryPath RootDir { get; set; }

    public IEnumerable<IFrostingTask> ExecutedTasks => _executedTasks;

    public BuildContext(ICakeContext context)
        : base(context)
    {
        RootDir = FindGitRootPath(context.Environment.WorkingDirectory) ??
                  context.Environment.WorkingDirectory.GetParent();
        SolutionFile = context.Arguments.GetArgument("solution");
    }

    public void AddExecutedTask(IFrostingTask task)
    {
        _executedTasks.Add(task);
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
}
