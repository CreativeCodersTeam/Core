using System;
using System.Windows;

namespace CreativeCoders.Mvvm.Wpf
{
    public interface IDataTemplateGenerator
    {
        DataTemplate CreateDataTemplate(Type viewModelType, Type viewType);
    }
}