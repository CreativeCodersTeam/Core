using System.Collections.Generic;
using System.Windows;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Wpf
{
    [PublicAPI]
    public interface IWindowHelper
    {
        Window GetActiveWindow();

        Window FindOwnerWindow(object viewModel);

        IEnumerable<T> FindAllChildren<T>(DependencyObject container);
    }
}