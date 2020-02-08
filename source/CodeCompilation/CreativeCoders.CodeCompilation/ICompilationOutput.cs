using System.IO;

namespace CreativeCoders.CodeCompilation
{
    public interface ICompilationOutput
    {
        Stream GetPeStream();
    }
}