using System.Windows;
using System.Windows.Controls;
using CreativeCoders.Di;
using CreativeCoders.Mvvm.Skeletor.Infrastructure;
using CreativeCoders.Mvvm.Wpf;

namespace CreativeCoders.Mvvm.Skeletor;

public class ViewModelTemplateSelector : DataTemplateSelector
{
    private readonly IViewLocator _viewLocator;

    private readonly IDataTemplateGenerator _dataTemplateGenerator;

    public ViewModelTemplateSelector()
    {
        if (GuiDevHelper.IsInDesignMode)
        {
            return;
        }

        _viewLocator = ServiceLocator.GetInstance<IViewLocator>();
        _dataTemplateGenerator = ServiceLocator.GetInstance<IDataTemplateGenerator>();
    }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (item == null)
        {
            return null;
        }

        var viewType = _viewLocator.GetViewType(item);

        return viewType != null
            ? _dataTemplateGenerator.CreateDataTemplate(item.GetType(), viewType)
            : base.SelectTemplate(item, container);
    }
}
