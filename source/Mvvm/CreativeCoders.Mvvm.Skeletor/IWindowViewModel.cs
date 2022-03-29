using System.ComponentModel;

namespace CreativeCoders.Mvvm.Skeletor;

public interface IWindowViewModel
{
    void Loaded();

    void Closing(CancelEventArgs cancelEventArgs);

    void Closed();
}