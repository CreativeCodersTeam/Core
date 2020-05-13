using System.IO;

namespace CreativeCoders.CodeCompilation
{
    public interface ICompilationOutputData
    {
        Stream GetPeStream();
    }
}