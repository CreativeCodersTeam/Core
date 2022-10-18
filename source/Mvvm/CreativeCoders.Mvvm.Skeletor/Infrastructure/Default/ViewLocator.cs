using System;
using System.Windows;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Skeletor.Infrastructure.Default;

[PublicAPI]
public class ViewLocator : IViewLocator
{
    private readonly IViewModelToViewMappings _viewModelToViewMappings;

    private readonly IServiceProvider _serviceProvider;

    public ViewLocator(IViewModelToViewMappings viewModelToViewMappings, IServiceProvider serviceProvider)
    {
        _viewModelToViewMappings = viewModelToViewMappings;
        _serviceProvider = serviceProvider;
    }

    public object CreateViewForViewModel(object viewModel)
    {
        return CreateViewForViewModel(viewModel, null);
    }

    public object CreateViewForViewModel(object viewModel, object context)
    {
        var viewType = GetViewTypeForViewModel(viewModel, context);
        var view = _serviceProvider.GetService(viewType) as DependencyObject;
        return view;
    }

    public Type GetViewType(object viewModel)
    {
        return GetViewTypeForViewModel(viewModel, null);
    }

    public Type GetViewType(object viewModel, object context)
    {
        return GetViewTypeForViewModel(viewModel, context);
    }

    private Type GetViewTypeForViewModel(object viewModel, object context)
    {
        var viewType = _viewModelToViewMappings.FindViewType(viewModel.GetType());
        if (viewType != null)
        {
            return viewType;
        }

        var viewModelTypeName = viewModel.GetType().AssemblyQualifiedName;
        var viewTypeName = TransformViewModelTypeName(viewModelTypeName, context);
        return Type.GetType(viewTypeName, true);
    }

    private static string TransformViewModelTypeName(string viewModelTypeName, object context)
    {
        var viewTypeName = viewModelTypeName.Replace("Model", string.Empty);
        if (context != null) { }

        return viewTypeName;
    }
}
