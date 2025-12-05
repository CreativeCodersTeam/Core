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

    private ProcessStartInfo CreateProcessStartInfo(string[]? args,
        IDictionary<string, object?>? placeholderVars)
    {
        var startupInfo = new ProcessStartInfo
        {
            FileName = _processExecutorInfo.FileName,
            Arguments = string.Join(" ", BuildArguments(args, placeholderVars)),
            RedirectStandardOutput = _processExecutorInfo.RedirectStandardOutput,
            RedirectStandardError = _processExecutorInfo.RedirectStandardError,
            RedirectStandardInput = _processExecutorInfo.RedirectStandardInput,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        _processExecutorInfo.ConfigureStartInfo?.Invoke(startupInfo);

        return startupInfo;
    }

    private string[] BuildArguments(string[]? args, IDictionary<string, object?>? placeholderVars)
    {
        return placeholderVars != null
            ? ReplacePlaceholders(_processExecutorInfo.Arguments, placeholderVars)
            : args ?? _processExecutorInfo.Arguments;
    }

    private static string[] ReplacePlaceholders(string[] arguments,
        IDictionary<string, object?> placeholderVars)
    {
        return arguments
            .ReplacePlaceholders("{{", "}}", placeholderVars)
            .ToArray();
    }

    protected IProcess StartProcess(string[]? args = null,
        IDictionary<string, object?>? placeholderVars = null)
    {
        var startupInfo = CreateProcessStartInfo(args, placeholderVars);

        var process = _processFactory.StartProcess(startupInfo);

        return process ?? throw new InvalidOperationException("Failed to start process.");
    }

    protected void CheckThrowOnError(IProcess process, bool disposeProcessOnThrow)
    {
        if (!_processExecutorInfo.ThrowOnError || process.ExitCode == 0)
        {
            return;
        }

        if (disposeProcessOnThrow)
        {
            process.Dispose();
        }

        throw new ProcessExecutionFailedException(process.ExitCode, process.StandardError.ReadToEnd());
    }

    protected async Task CheckThrowOnErrorAsync(IProcess process, bool disposeProcessOnThrow)
    {
        if (!_processExecutorInfo.ThrowOnError || process.ExitCode == 0)
        {
            return;
        }

        if (disposeProcessOnThrow)
        {
            process.Dispose();
        }

        throw new ProcessExecutionFailedException(process.ExitCode,
            await process.StandardError.ReadToEndAsync().ConfigureAwait(false));
    }
}
