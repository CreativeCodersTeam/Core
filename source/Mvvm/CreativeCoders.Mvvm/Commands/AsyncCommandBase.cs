using System.Threading.Tasks;

namespace CreativeCoders.Mvvm.Commands
{
    public abstract class AsyncCommandBase : CommandBase, IAsyncCommand
    {
        public abstract Task ExecuteAsync(object parameter);
    }
    
    public abstract class AsyncCommandBase<T> : CommandBase, IAsyncCommand<T>
    {
        public abstract Task ExecuteAsync(T parameter);
    }
}