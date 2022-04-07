using System;
using System.Windows;
using CreativeCoders.Mvvm.Wpf;

namespace CreativeCoders.Mvvm.UnitTests.TestObjects;

public class TestMappingTemplateSelector : ItemTypeTemplateSelector
{
    public void AddMapping<T>(Func<DataTemplate> templateFunc)
    {
        AddTypeTemplateMapping<T>(templateFunc);
    }
}
