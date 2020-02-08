using System.Windows;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Skeletor.Infrastructure.Default
{
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
            if (!(viewModel is IWindowViewModel windowViewModel))
            {
                return;
            }

            window.Loaded += (sender, args) => windowViewModel.Loaded();
            window.Closing += (sender, args) => windowViewModel.Closing(args);
            window.Closed += (sender, args) => windowViewModel.Closed();
        }
    }
}
