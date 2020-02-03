using System;
using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Commands
{
    [PublicAPI]
    public class RelayCommand : CommandBase
    {
        private readonly Action<object> _execute;

        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute) : this(execute, _ => true) {}

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            Ensure.IsNotNull(execute, nameof(execute));
            Ensure.IsNotNull(canExecute, nameof(canExecute));

            _execute = execute;
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            if (!CanExecute(parameter))
            {
                return;
            }
            
            _execute(parameter);
            
            RaiseCanExecuteChanged();
        }        
    }
}