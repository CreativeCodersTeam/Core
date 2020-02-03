using System;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.Core.Threading;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Commands
{
    [PublicAPI]
    public class AsyncSimpleRelayCommand : AsyncCommandBase
    {
        private readonly Func<Task> _execute;
        
        private readonly Func<bool> _canExecute;
        
        private readonly IErrorHandler _errorHandler;

        public AsyncSimpleRelayCommand(Func<Task> execute) : this(execute, () => true, NullErrorHandler.Instance)
        {
        }
        
        public AsyncSimpleRelayCommand(Func<Task> execute, IErrorHandler errorHandler) : this(execute, () => true, errorHandler)
        {
        }

        public AsyncSimpleRelayCommand(Func<Task> execute, Func<bool> canExecute) : this(execute, canExecute, NullErrorHandler.Instance)
        {
        }
        
        public AsyncSimpleRelayCommand(Func<Task> execute, Func<bool> canExecute, IErrorHandler errorHandler)
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
            return _canExecute();
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

            await _execute();
            
            RaiseCanExecuteChanged();
        }
    }
}