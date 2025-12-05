using CreativeCoders.Core;

namespace CreativeCoders.ProcessUtils.Execution;

public class ProcessExecutor(ProcessExecutorInfo processExecutorInfo, IProcessFactory processFactory)
    : ProcessExecutorBase(processExecutorInfo, processFactory), IProcessExecutor
{
    public void Execute()
    {
        using var _ = ExecuteEx();
    }

    public void Execute(string[] args)
    {
        using var _ = ExecuteEx(args);
    }

    public void Execute(IDictionary<string, object?> placeholderVars)
    {
        using var _ = ExecuteEx(placeholderVars);
    }

    public async Task ExecuteAsync()
    {
        using var _ = await ExecuteExAsync().ConfigureAwait(false);
    }

    public async Task ExecuteAsync(string[] args)
    {
        using var _ = await ExecuteExAsync(args).ConfigureAwait(false);
    }

    public async Task ExecuteAsync(IDictionary<string, object?> placeholderVars)
    {
        using var _ = await ExecuteExAsync(placeholderVars).ConfigureAwait(false);
    }

    public IProcess ExecuteEx()
        => ExecuteEx(null, null);

    public IProcess ExecuteEx(string[] args)
        => ExecuteEx(args, null);

    public IProcess ExecuteEx(IDictionary<string, object?> placeholderVars)
        => ExecuteEx(null, placeholderVars);

    private IProcess ExecuteEx(string[]? args, IDictionary<string, object?>? placeholderVars)
    {
        var process = StartProcess(args, placeholderVars);

        process.WaitForExit();

        CheckThrowOnError(process, true);

        return process;
    }

    public Task<IProcess> ExecuteExAsync()
        => ExecuteExAsync(null, null);

    public Task<IProcess> ExecuteExAsync(string[] args)
        => ExecuteExAsync(args, null);

    public Task<IProcess> ExecuteExAsync(IDictionary<string, object?> placeholderVars)
        => ExecuteExAsync(null, placeholderVars);

    private async Task<IProcess> ExecuteExAsync(string[]? args, IDictionary<string, object?>? placeholderVars)
    {
        var process = StartProcess(args, placeholderVars);

        await process.WaitForExitAsync().ConfigureAwait(false);

        await CheckThrowOnErrorAsync(process, true).ConfigureAwait(false);

        return process;
    }

    public int ExecuteAndReturnExitCode()
    {
        using var process = ExecuteEx();

        return process.ExitCode;
    }

    public int ExecuteAndReturnExitCode(string[] args)
    {
        using var process = ExecuteEx(args);

        return process.ExitCode;
    }

    public int ExecuteAndReturnExitCode(IDictionary<string, object?> placeholderVars)
    {
        using var process = ExecuteEx(placeholderVars);

        return process.ExitCode;
    }

    public async Task<int> ExecuteAndReturnExitCodeAsync()
    {
        using var process = await ExecuteExAsync().ConfigureAwait(false);

        return process.ExitCode;
    }

    public async Task<int> ExecuteAndReturnExitCodeAsync(string[] args)
    {
        using var process = await ExecuteExAsync(args).ConfigureAwait(false);

        return process.ExitCode;
    }

    public async Task<int> ExecuteAndReturnExitCodeAsync(IDictionary<string, object?> placeholderVars)
    {
        using var process = await ExecuteExAsync(placeholderVars).ConfigureAwait(false);

        return process.ExitCode;
    }
}

public class ProcessExecutor<T>(ProcessExecutorInfo<T> processExecutorInfo, IProcessFactory processFactory)
    : ProcessExecutorBase(processExecutorInfo, processFactory), IProcessExecutor<T>
{
    private readonly IProcessOutputParser<T> _outputParser = Ensure.NotNull(processExecutorInfo.OutputParser);

    public T? Execute()
        => Execute(null, null);

    public T? Execute(string[] args)
        => Execute(args, null);

    public T? Execute(IDictionary<string, object?> placeholderVars)
        => Execute(null, placeholderVars);

    private T? Execute(string[]? args, IDictionary<string, object?>? placeholderVars)
    {
        using var result = ExecuteEx(args, placeholderVars);

        return result.Value;
    }

    public Task<T?> ExecuteAsync()
        => ExecuteAsync(null, null);

    public Task<T?> ExecuteAsync(string[] args)
        => ExecuteAsync(args, null);

    public Task<T?> ExecuteAsync(IDictionary<string, object?> placeholderVars)
        => ExecuteAsync(null, placeholderVars);

    private async Task<T?> ExecuteAsync(string[]? args, IDictionary<string, object?>? placeholderVars)
    {
        using var result = await ExecuteExAsync(args, placeholderVars).ConfigureAwait(false);

        return result.Value;
    }

    public ProcessExecutionResult<T?> ExecuteEx() => ExecuteEx(null, null);

    public ProcessExecutionResult<T?> ExecuteEx(string[] args)
        => ExecuteEx(args, null);

    public ProcessExecutionResult<T?> ExecuteEx(IDictionary<string, object?> placeholderVars)
        => ExecuteEx(null, placeholderVars);

    private ProcessExecutionResult<T?> ExecuteEx(string[]? args,
        IDictionary<string, object?>? placeholderVars)
    {
        var process = StartProcess(args, placeholderVars);

        var output = process.StandardOutput.ReadToEnd();

        process.WaitForExit();

        CheckThrowOnError(process, true);

        return new ProcessExecutionResult<T?>(process, _outputParser.ParseOutput(output));
    }

    public Task<ProcessExecutionResult<T?>> ExecuteExAsync()
        => ExecuteExAsync(null, null);

    public Task<ProcessExecutionResult<T?>> ExecuteExAsync(string[] args)
        => ExecuteExAsync(args, null);

    public Task<ProcessExecutionResult<T?>> ExecuteExAsync(IDictionary<string, object?> placeholderVars)
        => ExecuteExAsync(null, placeholderVars);

    private async Task<ProcessExecutionResult<T?>> ExecuteExAsync(string[]? args,
        IDictionary<string, object?>? placeholderVars)
    {
        var process = StartProcess(args, placeholderVars);

        var output = await process.StandardOutput.ReadToEndAsync().ConfigureAwait(false);

        await process.WaitForExitAsync().ConfigureAwait(false);

        await CheckThrowOnErrorAsync(process, true).ConfigureAwait(false);

        return new ProcessExecutionResult<T?>(process, _outputParser.ParseOutput(output));
    }
}
