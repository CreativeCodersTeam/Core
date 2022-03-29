using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Wpf;

[PublicAPI]
public class ItemTypeTemplateSelector : DataTemplateSelector
{
    private readonly IDictionary<Type, Func<DataTemplate>> _typeTemplates;

    public ItemTypeTemplateSelector()
    {
        _typeTemplates = new Dictionary<Type, Func<DataTemplate>>();
    }

    protected void AddTypeTemplateMapping(Type type, Func<DataTemplate> getTemplate)
    {
        Ensure.That(!_typeTemplates.ContainsKey(type), nameof(type),
            "Mapping already contains mapping for type: " + type.Name);
        _typeTemplates.Add(type, getTemplate);
    }

    protected void AddTypeTemplateMapping<T>(Func<DataTemplate> getTemplate)
    {
        AddTypeTemplateMapping(typeof(T), getTemplate);
    }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        var typeTemplate =
            _typeTemplates.FirstOrDefault(templateItem => templateItem.Key.IsInstanceOfType(item));
        return typeTemplate.Value != null ? typeTemplate.Value() : base.SelectTemplate(item, container);
    }
}
