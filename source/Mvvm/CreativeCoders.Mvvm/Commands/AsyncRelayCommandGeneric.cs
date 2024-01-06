﻿using System;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.Core.Error;
using CreativeCoders.Core.Threading;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Commands;

[PublicAPI]
public class AsyncRelayCommand<T> : AsyncCommandBase<T>
{
    private readonly Func<T, Task> _execute;

    private readonly Func<T, bool> _canExecute;

    private readonly IErrorHandler _errorHandler;

    public AsyncRelayCommand(Func<T, Task> execute) : this(execute, _ => true, NullErrorHandler.Instance) { }

    public AsyncRelayCommand(Func<T, Task> execute, IErrorHandler errorHandler) : this(execute, _ => true,
        errorHandler) { }

    public AsyncRelayCommand(Func<T, Task> execute, Func<T, bool> canExecute) : this(execute, canExecute,
        NullErrorHandler.Instance) { }

    public AsyncRelayCommand(Func<T, Task> execute, Func<T, bool> canExecute, IErrorHandler errorHandler)
    {
        Ensure.IsNotNull(execute);
        Ensure.IsNotNull(canExecute);
        Ensure.IsNotNull(errorHandler);

        _execute = execute;
        _canExecute = canExecute;
        _errorHandler = errorHandler;
    }

    public override async Task ExecuteAsync(T parameter)
    {
        if (!CanExecute(parameter))
        {
            return;
        }

        await _execute(parameter).ConfigureAwait(true);

        RaiseCanExecuteChanged();
    }

    public override bool CanExecute(object parameter)
    {
        return _canExecute(parameter.As<T>());
    }

    public override void Execute(object parameter)
    {
        ExecuteAsync(parameter.As<T>()).FireAndForgetAsync(_errorHandler);
    }
}
