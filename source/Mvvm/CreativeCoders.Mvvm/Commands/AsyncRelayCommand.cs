using System;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.Core.Error;
using CreativeCoders.Core.Threading;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Commands;

[PublicAPI]
public class AsyncRelayCommand : AsyncCommandBase
{
    private readonly Func<object, Task> _execute;

    private readonly Func<object, bool> _canExecute;

    private readonly IErrorHandler _errorHandler;

    public AsyncRelayCommand(Func<object, Task> execute) :
        this(execute, _ => true, NullErrorHandler.Instance) { }

    public AsyncRelayCommand(Func<object, Task> execute, IErrorHandler errorHandler) : this(execute,
        _ => true, errorHandler) { }

    public AsyncRelayCommand(Func<object, Task> execute, Func<object, bool> canExecute) : this(execute,
        canExecute, NullErrorHandler.Instance) { }

    public AsyncRelayCommand(Func<object, Task> execute, Func<object, bool> canExecute,
        IErrorHandler errorHandler)
    {
        Ensure.IsNotNull(execute, nameof(execute));
        Ensure.IsNotNull(canExecute, nameof(canExecute));
        Ensure.IsNotNull(errorHandler, nameof(errorHandler));

        _execute = execute;
        _canExecute = canExecute;
        _errorHandler = errorHandler;
    }

    public override bool CanExecute(object parameter)
    {
        return _canExecute(parameter);
    }

    public override void Execute(object parameter)
    {
        ExecuteAsync(parameter).FireAndForgetAsync(_errorHandler);
    }

    public override async Task ExecuteAsync(object parameter)
    {
        if (!CanExecute(parameter))
        {
            return;
        }

        await _execute(parameter);

        RaiseCanExecuteChanged();
    }
}
