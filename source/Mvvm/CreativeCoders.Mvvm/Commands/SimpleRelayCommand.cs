using System;
using CreativeCoders.Core;

namespace CreativeCoders.Mvvm.Commands;

public class SimpleRelayCommand : CommandBase
{
    private readonly Action _execute;

    private readonly Func<bool> _canExecute;

    public SimpleRelayCommand(Action execute) : this(execute, () => true) { }

    public SimpleRelayCommand(Action execute, Func<bool> canExecute)
    {
        Ensure.IsNotNull(execute, nameof(execute));
        Ensure.IsNotNull(canExecute, nameof(canExecute));

        _execute = execute;
        _canExecute = canExecute;
    }

    public override bool CanExecute(object parameter)
    {
        return _canExecute();
    }

    public override void Execute(object parameter)
    {
        if (!CanExecute(parameter))
        {
            return;
        }

        _execute();

        RaiseCanExecuteChanged();
    }
}
