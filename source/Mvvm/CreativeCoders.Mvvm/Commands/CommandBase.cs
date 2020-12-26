using System;
using System.Threading;
using System.Windows.Input;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Commands
{
    [PublicAPI]
    public abstract class CommandBase : ICommandEx
    {
        private EventHandler _canExecuteChanged;

        private readonly SynchronizationContext _synchronizationContext;

        public abstract bool CanExecute(object parameter);

        protected CommandBase()
        {
            _synchronizationContext = SynchronizationContext.Current;
        }

        public abstract void Execute(object parameter);

        public event EventHandler CanExecuteChanged
        {
            add
            {
                _canExecuteChanged += value;
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                // ReSharper disable once DelegateSubtraction
                _canExecuteChanged -= value;
                CommandManager.RequerySuggested -= value;
            }
        }

        public virtual void RaiseCanExecuteChanged()
        {
            var handler = _canExecuteChanged;
            if (handler == null)
            {
                return;
            }
            
            _synchronizationContext.Post(_ => handler.Invoke(this, EventArgs.Empty), null);
        }
    }
}
