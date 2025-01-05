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
}
