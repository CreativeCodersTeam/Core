using System.Collections.Generic;
using System.Threading.Tasks;
using CommandLine;

namespace CreativeCoders.SysConsole.App.Verbs
{
    public interface IVerbParserErrorHandler
    {
        Task<int> HandleErrorsAsync(IEnumerable<Error> errors);
    }
}