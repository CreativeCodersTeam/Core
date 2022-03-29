using System;
using CreativeCoders.Core;

namespace CreativeCoders.Mvvm.Commands;

public class RelayCommand<T> : CommandBase
{
    private readonly Action<T> _execute;

    private readonly Predicate<T> _canExecute;

    public RelayCommand(Action<T> execute) : this(execute, _ => true) { }

    public RelayCommand(Action<T> execute, Predicate<T> canExecute)
    {
        Ensure.IsNotNull(execute, nameof(execute));
        Ensure.IsNotNull(canExecute, nameof(canExecute));

        _execute = execute;
        _canExecute = canExecute;
    }

    public override bool CanExecute(object parameter)
    {
        return _canExecute(parameter.As<T>());
    }

    public override void Execute(object parameter)
    {
        if (!CanExecute(parameter))
        {
            return;
        }
            
        _execute(parameter.As<T>());
            
        RaiseCanExecuteChanged();
    }
}