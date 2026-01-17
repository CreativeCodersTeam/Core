using CreativeCoders.Core;
using CreativeCoders.Core.IO;

namespace CreativeCoders.IO.Archives;

public class ArchiveEntry(string fullName)
{
    public string FileName { get; } = FileSys.Path.GetFileName(fullName);

    public string FullName { get; } = Ensure.IsNotNullOrEmpty(fullName);

    public string DirectoryName { get; } = FileSys.Path.GetDirectoryName(fullName) ?? string.Empty;
}
