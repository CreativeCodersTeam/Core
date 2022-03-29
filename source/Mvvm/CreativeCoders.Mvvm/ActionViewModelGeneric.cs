using System;
using System.Windows.Input;
using CreativeCoders.Mvvm.Commands;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm;

[PublicAPI]
public class ActionViewModel<TCommandParam> : ActionViewModel
{
    public ActionViewModel(Action<TCommandParam> execute) : this(execute, _ => true) { }

    public ActionViewModel(Action<TCommandParam> execute, Predicate<TCommandParam> canExecute) : base(
        new RelayCommand<TCommandParam>(execute, canExecute)) { }

    public ActionViewModel(ICommand command) : base(command) { }

    public void Execute(TCommandParam parameter)
    {
        Command?.Execute(parameter);
    }
}
