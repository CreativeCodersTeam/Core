using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Wpf;

[PublicAPI]
[ExcludeFromCodeCoverage]
public class WindowHelper : IWindowHelper
{
    public Window GetActiveWindow()
    {
        return Application.Current.Windows.Cast<Window>().SingleOrDefault(x => x.IsActive);
    }

    public Window FindOwnerWindow(object viewModel)
    {
        var owner = Application.Current.Windows.Cast<Window>().SingleOrDefault(x => x.DataContext == viewModel);

        if (owner == null)
        {
            foreach (var window in from Window window in Application.Current.Windows
                     from frameworkElement in FindAllChildren<FrameworkElement>(window)
                     where frameworkElement.DataContext == viewModel
                     select window)
            {
                return window;
            }
        }
        else
        {
            return owner;
        }

        return GetActiveWindow();
    }

    public IEnumerable<T> FindAllChildren<T>(DependencyObject container)
    {
        foreach (var child in LogicalTreeHelper.GetChildren(container))
        {
            if (child is T children)
            {
                yield return children;
            }

            // ReSharper disable once InvertIf
            if (child is DependencyObject dependencyObject)
            {
                foreach (var subChild in FindAllChildren<T>(dependencyObject))
                {
                    yield return subChild;
                }
            }
        }
    }
}