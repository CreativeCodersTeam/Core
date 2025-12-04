using System.Diagnostics;
using JetBrains.Annotations;

namespace CreativeCoders.ProcessUtils;

[PublicAPI]
public interface IProcessFactory
{
    IProcess CreateProcess(string fileName, string[] arguments);

    IProcess CreateProcess();

    IProcess StartProcess(string fileName, string[] arguments);

    IProcess StartProcess(ProcessStartInfo startInfo);
}
