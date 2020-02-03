using CreativeCoders.Core.IO;
using JetBrains.Annotations;

namespace CreativeCoders.CodeCompilation
{
    [PublicAPI]
    public class FileCompilationOutput : StreamCompilationOutput
    {
        public FileCompilationOutput(string fileName) : base(FileSys.File.Create(fileName)) { }
    }
}