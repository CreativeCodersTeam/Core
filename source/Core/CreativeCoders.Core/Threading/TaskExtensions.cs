using System;
using System.Threading.Tasks;
using CreativeCoders.Core.Error;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Threading;

/// <summary>
///     Provides extension methods for <see cref="Task"/> to support fire-and-forget execution
///     and result type conversion.
/// </summary>
[PublicAPI]
public static class TaskExtensions
{
    /// <summary>
    ///     Executes the task without awaiting it and routes any exception to the specified error handler.
    /// </summary>
    /// <param name="task">The task to execute.</param>
    /// <param name="errorHandler">The error handler that receives any thrown exception.</param>
    public static void FireAndForgetAsync(this Task task, IErrorHandler errorHandler)
    {
        task.FireAndForgetAsync(e => errorHandler?.HandleException(e));
    }

    /// <summary>
    ///     Executes the task without awaiting it and routes any exception to the specified callback.
    /// </summary>
    /// <param name="task">The task to execute.</param>
    /// <param name="errorHandler">The callback that receives any thrown exception.</param>
    public static async void FireAndForgetAsync(this Task task, Action<Exception> errorHandler)
    {
        try
        {
            await task.ConfigureAwait(false);
        }
        catch (Exception e)
        {
            errorHandler?.Invoke(e);
        }
    }

    /// <summary>
    ///     Awaits the task and extracts its result as the specified type using reflection.
    /// </summary>
    /// <typeparam name="T">The type of the expected result.</typeparam>
    /// <param name="task">The task whose result to extract.</param>
    /// <returns>The result of the task cast to <typeparamref name="T"/>.</returns>
    /// <exception cref="InvalidCastException">The task does not have a Result property.</exception>
    public static async Task<T> ToTask<T>(this Task task)
    {
        await task.ConfigureAwait(false);

        var resultProperty = task.GetType().GetProperty("Result")
                             ?? throw new InvalidCastException(
                                 $"{task.GetType().FullName} has no Result property");

        return (T) resultProperty.GetValue(task);
    }
}
