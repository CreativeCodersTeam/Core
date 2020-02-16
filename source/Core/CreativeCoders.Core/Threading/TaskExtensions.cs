using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Threading
{
    [PublicAPI]
    public static class TaskExtensions
    {
        public static async void FireAndForgetAsync(this Task task, IErrorHandler errorHandler)
        {
            try
            {
                await task;
            }
            catch (Exception e)
            {
                errorHandler?.HandleException(e);
            }
        }

        public static async Task<T> ToTask<T>(this Task task)
        {
            await task;

            var resultProperty = task.GetType().GetProperty("Result")
                                 ?? throw new InvalidCastException($"{task.GetType().FullName} has no Result property");

            return (T) resultProperty.GetValue(task);
        }
    }
}