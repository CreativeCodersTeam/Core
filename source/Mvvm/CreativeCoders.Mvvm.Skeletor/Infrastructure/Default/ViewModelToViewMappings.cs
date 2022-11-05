using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CreativeCoders.Core.Threading;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Skeletor.Infrastructure.Default;

[PublicAPI]
public class ViewModelToViewMappings : IViewModelToViewMappings
{
    private readonly IList<ViewModelToViewMapping> _mappings;

    public ViewModelToViewMappings()
    {
        _mappings = new ConcurrentList<ViewModelToViewMapping>();
    }

    public void AddMapping<TViewModel, TView>()
        where TView : DependencyObject
    {
        AddMapping(typeof(TViewModel), typeof(TView));
    }

    public void AddMapping<TViewModel, TView>(string name)
        where TView : DependencyObject
    {
        var existingMapping = FindMapping(typeof(TViewModel), name);
        if (existingMapping != null)
        {
            _mappings.Remove(existingMapping);
        }

        var mapping = new ViewModelToViewMapping(typeof(TViewModel), typeof(TView), name);
        _mappings.Add(mapping);
    }

    public void AddMapping(Type viewModelType, Type viewType)
    {
        var existingMapping = FindMapping(viewModelType);
        if (existingMapping != null)
        {
            _mappings.Remove(existingMapping);
        }

        var mapping = new ViewModelToViewMapping(viewModelType, viewType, null);
        _mappings.Add(mapping);
    }

    public Type FindViewType(Type viewModelType)
    {
        var mapping = FindMapping(viewModelType);
        return mapping?.ViewType;
    }

    public Type FindViewType(Type viewModelType, string name)
    {
        var mapping = FindMapping(viewModelType, name);
        return mapping?.ViewType;
    }

    private ViewModelToViewMapping FindMapping(Type viewModelType)
    {
        var mapping =
            _mappings.FirstOrDefault(m => m.ViewModelType == viewModelType && string.IsNullOrEmpty(m.Name));
        return mapping;
    }

    private ViewModelToViewMapping FindMapping(Type viewModelType, string name)
    {
        var mapping =
            _mappings.FirstOrDefault(m => m.ViewModelType == viewModelType && m.Name == name);
        return mapping;
    }
}
