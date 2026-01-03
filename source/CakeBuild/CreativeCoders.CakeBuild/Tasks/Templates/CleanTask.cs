using Cake.Common.Diagnostics;
using Cake.Common.Tools.DotNet;
using Cake.Core.IO;

namespace CreativeCoders.CakeBuild.Tasks.Templates;

public class CleanTask<T> : FrostingTaskBase<T> where T : BuildContext
{
    public override async Task RunAsync(T context)
    {
        context.DotNetClean(context.SolutionFile.FullPath);

        DeleteDirectories(context, context.RootDir.FullPath + "/**/bin");
        DeleteDirectories(context, context.RootDir.FullPath + "/**/obj");
    }

    private static void DeleteDirectories(T context, string globPattern)
    {
        var dirs = context.Globber.Match(globPattern).OfType<DirectoryPath>();

        foreach (var dir in dirs)
        {
            context.Information("Deleting directory '{0}'", dir);

            context.FileSystem.GetDirectory(dir).Delete(true);
        }
    }
}
