using System.Windows;

namespace CreativeCoders.Mvvm.Skeletor.Infrastructure;

public interface IViewModelBinder
{
    void Bind(object view, object viewModel);

    void BindWindow(Window window, object viewModel);
}