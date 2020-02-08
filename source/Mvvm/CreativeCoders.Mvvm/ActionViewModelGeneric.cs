using System;
using System.Windows.Input;
using CreativeCoders.Mvvm.Commands;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm
{
    [PublicAPI]
    public class ActionViewModel<TCommandParam> : ActionViewModel
    {
        public ActionViewModel(Action<TCommandParam> executeAction) : this(executeAction, _ => true) {}

        public ActionViewModel(Action<TCommandParam> executeAction, Predicate<TCommandParam> canExecuteFunc) : base(new RelayCommand<TCommandParam>(executeAction, canExecuteFunc))
        {
        }

        public ActionViewModel(ICommand command) : base(command)
        {
        }

        public void Execute(TCommandParam parameter)
        {
            Command?.Execute(parameter);
        }
    }
}
