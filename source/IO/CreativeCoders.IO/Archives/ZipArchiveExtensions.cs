using System.IO.Compression;

namespace CreativeCoders.IO.Archives;

public static class ZipArchiveExtensions
{
    public static IEnumerable<ZipArchiveEntry> CreateEntriesFromDirectory(this ZipArchive zipArchive,
        string path, string entryPathName, bool recursive)
    {
        var entries = new List<ZipArchiveEntry>();

        foreach (var file in Directory.EnumerateFiles(path))
        {
            var entryName = Path.Combine(entryPathName, Path.GetFileName(file));

            entries.Add(zipArchive.CreateEntryFromFile(file, entryName));
        }

        if (!recursive)
        {
            return entries;
        }

        foreach (var directory in Directory.EnumerateDirectories(path))
        {
            var entryName = Path.Combine(entryPathName, Path.GetFileName(directory));

            entries.AddRange(zipArchive.CreateEntriesFromDirectory(directory, entryName, true));
        }

        return entries;
    }
}
