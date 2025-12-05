using CreativeCoders.Core;

namespace CreativeCoders.ProcessUtils.Execution;

public static class ProcessExecutorExtensions
{
    public static void Execute<TVars>(this IProcessExecutor executor,
        TVars placeholderVars)
        where TVars : class
    {
        executor.Execute(placeholderVars.ToDictionary());
    }

    public static Task ExecuteAsync<TVars>(this IProcessExecutor executor,
        TVars placeholderVars)
        where TVars : class
    {
        return executor.ExecuteAsync(placeholderVars.ToDictionary());
    }

    public static IProcess ExecuteEx<TVars>(this IProcessExecutor executor,
        TVars placeholderVars)
        where TVars : class
    {
        return executor.ExecuteEx(placeholderVars.ToDictionary());
    }

    public static Task<IProcess> ExecuteExAsync<TVars>(this IProcessExecutor executor,
        TVars placeholderVars)
        where TVars : class
    {
        return executor.ExecuteExAsync(placeholderVars.ToDictionary());
    }

    public static int ExecuteAndReturnExitCode<TVars>(this IProcessExecutor executor,
        TVars placeholderVars)
        where TVars : class
    {
        return executor.ExecuteAndReturnExitCode(placeholderVars.ToDictionary());
    }

    public static Task<int> ExecuteAndReturnExitCodeAsync<TVars>(this IProcessExecutor executor,
        TVars placeholderVars)
        where TVars : class
    {
        return executor.ExecuteAndReturnExitCodeAsync(placeholderVars.ToDictionary());
    }
}
