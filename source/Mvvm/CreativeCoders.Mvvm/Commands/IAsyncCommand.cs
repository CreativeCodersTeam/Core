using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Commands;

[PublicAPI]
public interface IAsyncCommand : ICommandEx
{
    Task ExecuteAsync(object parameter);
}

[PublicAPI]
public interface IAsyncCommand<in T> : ICommandEx
{
    Task ExecuteAsync(T parameter);
}
