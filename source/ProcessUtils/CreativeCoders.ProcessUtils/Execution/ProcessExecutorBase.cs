using System.Diagnostics;
using CreativeCoders.Core;
using CreativeCoders.Core.Text;

namespace CreativeCoders.ProcessUtils.Execution;

public abstract class ProcessExecutorBase(
    ProcessExecutorInfo processExecutorInfo,
    IProcessFactory processFactory)
{
    private readonly ProcessExecutorInfo _processExecutorInfo = Ensure.NotNull(processExecutorInfo);

    private readonly IProcessFactory _processFactory = Ensure.NotNull(processFactory);

    private ProcessStartInfo CreateProcessStartInfo(string[]? args = null)
    {
        var startupInfo = new ProcessStartInfo
        {
            FileName = _processExecutorInfo.FileName,
            Arguments = string.Join(" ", BuildArguments(args)),
            RedirectStandardOutput = _processExecutorInfo.RedirectStandardOutput,
            RedirectStandardError = _processExecutorInfo.RedirectStandardError,
            RedirectStandardInput = _processExecutorInfo.RedirectStandardInput,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        return startupInfo;
    }

    private string[] BuildArguments(string[]? args = null)
    {
        return _processExecutorInfo.UsePlaceholderVars
            ? ReplacePlaceholders(_processExecutorInfo.Arguments, args)
            : args ?? _processExecutorInfo.Arguments;
    }

    private static string[] ReplacePlaceholders(string[] arguments, string[]? args)
    {
        if (args == null)
        {
            return arguments;
        }

        var replaceVars = args.ToDictionary(":", false);

        return arguments
            .ReplacePlaceholders("{{", "}}", replaceVars)
            .ToArray();
    }

    protected IProcess StartProcess()
    {
        var startupInfo = CreateProcessStartInfo();

        var process = _processFactory.StartProcess(startupInfo);

        return process ?? throw new InvalidOperationException("Failed to start process.");
    }
}
