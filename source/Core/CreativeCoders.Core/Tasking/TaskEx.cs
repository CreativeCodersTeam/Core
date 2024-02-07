using System;
using System.Threading.Tasks;

#nullable enable
namespace CreativeCoders.Core.Tasking;

public static class TaskEx
{
    /// <summary>
    ///     Executes the provided action and returns a completed task.
    /// </summary>
    /// <param name="action">The action to be executed. This must not be null.</param>
    /// <returns>The <see cref="System.Threading.Tasks.Task" /> that represents the completed async operation.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown when the provided action is null.</exception>
    public static Task AsCompletedTask(this Action action)
    {
        Ensure.NotNull(action).Invoke();

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Executes the provided action and returns a completed task.
    /// </summary>
    /// <param name="action">The action to be executed. This must not be null.</param>
    /// <returns>A <see cref="System.Threading.Tasks.ValueTask" /> that represents the completed async operation.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="action" /> is null.</exception>
    public static ValueTask AsCompletedValueTask(this Action action)
    {
        Ensure.NotNull(action).Invoke();

        return ValueTask.CompletedTask;
    }
}
