using JetBrains.Annotations;

namespace CreativeCoders.IO;

[PublicAPI]
public interface ITarArchiveCreator
{
    ITarArchiveCreator SetArchiveFileName(string archiveFileName);

    ITarArchiveCreator AddFromDirectory(string path, string searchPattern, bool recursive);

    void Create();

    void Create(bool gzipArchive);
}
