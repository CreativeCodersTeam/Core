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
    }
}