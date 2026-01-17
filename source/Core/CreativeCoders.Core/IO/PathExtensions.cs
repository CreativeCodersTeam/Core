using System.IO.Abstractions;
using System.Linq;

namespace CreativeCoders.Core.IO;

public static class PathExtensions
{
    public static string ReplaceInvalidFileNameChars(this IPath path, string fileName, string replacement)
    {
        return path.GetInvalidFileNameChars()
            .Aggregate(fileName, (current, c) => current.Replace(c.ToString(), replacement));
    }

    public static bool IsSafe(this IPath path, string pathToCheck, string basePath)
    {
        return PathSafety.IsSafe(path, pathToCheck, basePath);
    }

    public static void EnsureSafe(this IPath path, string pathToCheck, string basePath)
    {
        PathSafety.EnsureSafe(path, pathToCheck, basePath);
    }
}
