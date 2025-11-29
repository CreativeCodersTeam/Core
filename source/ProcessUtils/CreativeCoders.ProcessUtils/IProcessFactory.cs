using System.Diagnostics;

namespace CreativeCoders.ProcessUtils;

public interface IProcessFactory
{
    IProcess CreateProcess(string fileName, string[] arguments);

    IProcess CreateProcess();
    
    IProcess StartProcess(string fileName, string[] arguments);
    
    IProcess StartProcess(ProcessStartInfo startInfo);
}