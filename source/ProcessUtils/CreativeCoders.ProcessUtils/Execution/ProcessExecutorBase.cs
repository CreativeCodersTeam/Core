using System.Diagnostics;
using CreativeCoders.Core;

namespace CreativeCoders.ProcessUtils.Execution;

public abstract class ProcessExecutorBase(
    ProcessExecutorInfo processExecutorInfo,
    IProcessFactory processFactory)
{
    private readonly ProcessExecutorInfo _processExecutorInfo = Ensure.NotNull(processExecutorInfo);

    private readonly IProcessFactory _processFactory = Ensure.NotNull(processFactory);

    private ProcessStartInfo CreateProcessStartInfo()
    {
        var startupInfo = new ProcessStartInfo
        {
            FileName = _processExecutorInfo.FileName,
            Arguments = string.Join(" ", _processExecutorInfo.Arguments),
            RedirectStandardOutput = _processExecutorInfo.RedirectStandardOutput,
            RedirectStandardError = _processExecutorInfo.RedirectStandardError,
            RedirectStandardInput = _processExecutorInfo.RedirectStandardInput,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        return startupInfo;
    }

    protected IProcess StartProcess()
    {
        var startupInfo = CreateProcessStartInfo();

        var process = _processFactory.StartProcess(startupInfo);

        return process ?? throw new InvalidOperationException("Failed to start process.");
    }
}
