using CreativeCoders.Core;

namespace CreativeCoders.ProcessUtils.Execution;

public static class ProcessExecutorGenericExtensions
{
    public static T? Execute<T, TVars>(this IProcessExecutor<T> executor, TVars placeholderVars)
        where TVars : class
    {
        return executor.Execute(placeholderVars.ToDictionary());
    }

    public static Task<T?> ExecuteAsync<T, TVars>(this IProcessExecutor<T> executor, TVars placeholderVars)
        where TVars : class
    {
        return executor.ExecuteAsync(placeholderVars.ToDictionary());
    }

    public static ProcessExecutionResult<T?> ExecuteEx<T, TVars>(this IProcessExecutor<T> executor,
        TVars placeholderVars)
        where TVars : class
    {
        return executor.ExecuteEx(placeholderVars.ToDictionary());
    }

    public static Task<ProcessExecutionResult<T?>> ExecuteExAsync<T, TVars>(this IProcessExecutor<T> executor,
        TVars placeholderVars)
        where TVars : class
    {
        return executor.ExecuteExAsync(placeholderVars.ToDictionary());
    }
}
