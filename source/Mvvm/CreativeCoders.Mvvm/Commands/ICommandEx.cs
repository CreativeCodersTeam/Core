using System.Windows.Input;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Commands
{
    [PublicAPI]
    public interface ICommandEx : ICommand
    {
        void RaiseCanExecuteChanged();
    }
}
