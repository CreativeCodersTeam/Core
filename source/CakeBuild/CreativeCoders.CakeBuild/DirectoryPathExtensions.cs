using System.Diagnostics.CodeAnalysis;
using Cake.Core.IO;

namespace CreativeCoders.CakeBuild;

public static class DirectoryPathExtensions
{
    [SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery")]
    public static DirectoryPath Combine(this DirectoryPath path, params string[] segments)
    {
        foreach (var segment in segments)
        {
            path = path.Combine(segment);
        }

        return path;
    }
}
