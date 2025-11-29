using CreativeCoders.Core;

namespace CreativeCoders.ProcessUtils.Execution;

public class ProcessExecutor(ProcessExecutorInfo processExecutorInfo, IProcessFactory processFactory)
    : ProcessExecutorBase(processExecutorInfo, processFactory), IProcessExecutor
{
    public void Execute()
    {
        using var process = StartProcess();

        process.WaitForExit();
    }

    public async Task ExecuteAsync()
    {
        using var process = StartProcess();

        await process.WaitForExitAsync().ConfigureAwait(false);
    }
}

public class ProcessExecutor<T>(ProcessExecutorInfo<T> processExecutorInfo, IProcessFactory processFactory)
    : ProcessExecutorBase(processExecutorInfo, processFactory), IProcessExecutor<T>
{
    private readonly IProcessOutputParser<T> _outputParser = Ensure.NotNull(processExecutorInfo.OutputParser);

    public T? Execute()
    {
        using var process = StartProcess();

        var output = process.StandardOutput.ReadToEnd();

        process.WaitForExit();

        return _outputParser.ParseOutput(output);
    }

    public async Task<T?> ExecuteAsync()
    {
        using var process = StartProcess();

        var output = await process.StandardOutput.ReadToEndAsync().ConfigureAwait(false);

        await process.WaitForExitAsync().ConfigureAwait(false);

        return _outputParser.ParseOutput(output);
    }
}
