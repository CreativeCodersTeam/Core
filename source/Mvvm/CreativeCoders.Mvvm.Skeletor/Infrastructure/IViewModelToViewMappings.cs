using System;
using System.Windows;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Skeletor.Infrastructure;

[PublicAPI]
public interface IViewModelToViewMappings
{
    void AddMapping<TViewModel, TView>()
        where TView : DependencyObject;

    void AddMapping<TViewModel, TView>(string name)
        where TView : DependencyObject;

    void AddMapping(Type viewModelType, Type viewType);

    Type FindViewType(Type viewModelType);

    Type FindViewType(Type viewModelType, string name);
}
