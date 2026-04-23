using System;
using System.Threading.Tasks;

#nullable enable
namespace CreativeCoders.Core.Tasking;

/// <summary>
/// Provides extension methods for converting synchronous actions into completed <see cref="Task"/> and <see cref="ValueTask"/> instances.
/// </summary>
public static class TaskEx
{
    /// <summary>
    /// Executes the provided action and returns a completed task.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <returns>A <see cref="Task"/> representing the completed operation.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
    public static Task AsCompletedTask(this Action action)
    {
        Ensure.NotNull(action).Invoke();

        return Task.CompletedTask;
    }

    /// <summary>
    /// Executes the provided action and returns a completed value task.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <returns>A <see cref="ValueTask"/> representing the completed operation.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
    public static ValueTask AsCompletedValueTask(this Action action)
    {
        Ensure.NotNull(action).Invoke();

        return ValueTask.CompletedTask;
    }
}
