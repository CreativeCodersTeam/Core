using System.Windows;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Skeletor.Infrastructure.Default;

[PublicAPI]
public class ViewModelBinder : IViewModelBinder
{
    public void Bind(object view, object viewModel)
    {
        if (view is FrameworkElement frameworkElement)
        {
            frameworkElement.DataContext = viewModel;
        }
    }

    public void BindWindow(Window window, object viewModel)
    {
        if (viewModel is not IWindowViewModel windowViewModel)
        {
            return;
        }

        window.Loaded += (_, _) => windowViewModel.Loaded();
        window.Closing += (_, args) => windowViewModel.Closing(args);
        window.Closed += (_, _) => windowViewModel.Closed();
    }
}
