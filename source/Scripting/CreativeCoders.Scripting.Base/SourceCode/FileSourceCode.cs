using CreativeCoders.Core.IO;
using JetBrains.Annotations;

namespace CreativeCoders.Scripting.Base.SourceCode;

[PublicAPI]
public class FileSourceCode : DelegateSourceCode
{
    public FileSourceCode(string fileName) : base(() => FileSys.File.ReadAllText(fileName))
    {
    }
}