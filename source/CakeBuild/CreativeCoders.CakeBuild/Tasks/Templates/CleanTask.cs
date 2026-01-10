using Cake.Common.Diagnostics;
using Cake.Common.Tools.DotNet;
using Cake.Core.IO;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;

namespace CreativeCoders.CakeBuild.Tasks.Templates;

public class CleanTask<T> : FrostingTaskBase<T> where T : BuildContext
{
    protected override Task RunAsyncCore(T context)
    {
        var settings = context.GetRequiredSettings<ICleanTaskSettings>();

        DeleteDirectories(context, settings.DirectoriesToClean);

        context.DotNetClean(context.SolutionFile.FullPath);

        return Task.CompletedTask;
    }

    private static void DeleteDirectories(T context, IEnumerable<DirectoryPath> dirsForDelete)
    {
        foreach (var dir in dirsForDelete.Where(dir => context.FileSystem.GetDirectory(dir).Exists))
        {
            context.Information("Deleting directory '{0}'", dir);

            context.FileSystem.GetDirectory(dir).Delete(true);
        }
    }
}
