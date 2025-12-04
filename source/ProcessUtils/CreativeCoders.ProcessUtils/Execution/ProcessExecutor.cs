using CreativeCoders.Core;

namespace CreativeCoders.ProcessUtils.Execution;

public class ProcessExecutor(ProcessExecutorInfo processExecutorInfo, IProcessFactory processFactory)
    : ProcessExecutorBase(processExecutorInfo, processFactory), IProcessExecutor
{
    public void Execute()
    {
        using var _ = ExecuteEx();
    }

    public async Task ExecuteAsync()
    {
        using var _ = await ExecuteExAsync().ConfigureAwait(false);
    }

    public IProcess ExecuteEx()
    {
        var process = StartProcess();

        process.WaitForExit();

        return process;
    }

    public async Task<IProcess> ExecuteExAsync()
    {
        var process = StartProcess();

        await process.WaitForExitAsync().ConfigureAwait(false);

        return process;
    }

    public int ExecuteAndReturnExitCode()
    {
        using var process = ExecuteEx();

        return process.ExitCode;
    }

    public async Task<int> ExecuteAndReturnExitCodeAsync()
    {
        using var process = await ExecuteExAsync().ConfigureAwait(false);

        return process.ExitCode;
    }
}

public class ProcessExecutor<T>(ProcessExecutorInfo<T> processExecutorInfo, IProcessFactory processFactory)
    : ProcessExecutorBase(processExecutorInfo, processFactory), IProcessExecutor<T>
{
    private readonly IProcessOutputParser<T> _outputParser = Ensure.NotNull(processExecutorInfo.OutputParser);

    public T? Execute()
    {
        using var result = ExecuteEx();

        return result.Value;
    }

    public T? Execute(string[] args)
    {
        throw new NotImplementedException();
    }

    public async Task<T?> ExecuteAsync()
    {
        using var result = await ExecuteExAsync().ConfigureAwait(false);

        return result.Value;
    }

    public Task<T?> ExecuteAsync(string[] args)
    {
        throw new NotImplementedException();
    }

    public ProcessExecutionResult<T?> ExecuteEx()
    {
        var process = StartProcess();

        var output = process.StandardOutput.ReadToEnd();

        process.WaitForExit();

        return new ProcessExecutionResult<T?>(process, _outputParser.ParseOutput(output));
    }

    public ProcessExecutionResult<T?> ExecuteEx(string[] args)
    {
        throw new NotImplementedException();
    }

    public async Task<ProcessExecutionResult<T?>> ExecuteExAsync()
    {
        var process = StartProcess();

        var output = await process.StandardOutput.ReadToEndAsync().ConfigureAwait(false);

        await process.WaitForExitAsync().ConfigureAwait(false);

        return new ProcessExecutionResult<T?>(process, _outputParser.ParseOutput(output));
    }

    public Task<ProcessExecutionResult<T?>> ExecuteExAsync(string[] args)
    {
        throw new NotImplementedException();
    }
}
