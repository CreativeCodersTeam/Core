using CreativeCoders.Core.IO;
using JetBrains.Annotations;

namespace CreativeCoders.CodeCompilation;

[PublicAPI]
public class FileCompilationOutputData : StreamCompilationOutputData
{
    public FileCompilationOutputData(string fileName) : base(FileSys.File.Create(fileName)) { }
}
