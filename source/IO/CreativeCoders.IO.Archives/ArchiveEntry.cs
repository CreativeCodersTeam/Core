using CreativeCoders.Core;

namespace CreativeCoders.IO.Archives;

public class ArchiveEntry(string fullName)
{
    public string FileName { get; } = Path.GetFileName(fullName);

    public string FullName { get; } = Ensure.IsNotNullOrEmpty(fullName);

    public string DirectoryName { get; } = Path.GetDirectoryName(fullName) ?? string.Empty;
}
