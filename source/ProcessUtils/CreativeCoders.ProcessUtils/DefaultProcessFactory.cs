using System.Diagnostics;
using CreativeCoders.Core;

namespace CreativeCoders.ProcessUtils;

public class DefaultProcessFactory : IProcessFactory
{
    public IProcess CreateProcess(string fileName, string[] arguments)
    {
        Ensure.IsNotNullOrWhitespace(fileName);

        var process = new Process();
        process.StartInfo.FileName = fileName;
        process.StartInfo.Arguments = string.Join(" ", arguments);

        return new DefaultProcess(process);
    }

    public IProcess CreateProcess()
    {
        var process = new Process();

        return new DefaultProcess(process);
    }

    public IProcess StartProcess(string fileName, string[] arguments)
    {
        return StartProcess(new ProcessStartInfo(fileName, arguments));
    }

    public IProcess StartProcess(ProcessStartInfo startInfo)
    {
        var process = Process.Start(startInfo);

        return process == null
            ? throw new InvalidOperationException("Failed to start process.")
            : new DefaultProcess(process);
    }
}
