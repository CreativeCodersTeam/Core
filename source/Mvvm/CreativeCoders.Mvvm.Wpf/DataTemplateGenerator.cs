using System;
using System.Windows;
using System.Windows.Markup;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Wpf
{
    [PublicAPI]
    public class DataTemplateGenerator : IDataTemplateGenerator
    {
        public DataTemplate CreateDataTemplate(Type viewModelType, Type viewType)
        {
            var xaml =
                $"<DataTemplate DataType=\"{{x:Type vm:{viewModelType.Name}}}\"><v:{viewType.Name} /></DataTemplate>";

            var context = new ParserContext {XamlTypeMapper = new XamlTypeMapper(new string[0])};

            context.XamlTypeMapper.AddMappingProcessingInstruction("vm",
                viewModelType.Namespace ?? throw new InvalidOperationException(), viewModelType.Assembly.FullName);
            context.XamlTypeMapper.AddMappingProcessingInstruction("v",
                viewType.Namespace ?? throw new InvalidOperationException(), viewType.Assembly.FullName);

            context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
            context.XmlnsDictionary.Add("vm", "vm");
            context.XmlnsDictionary.Add("v", "v");

            var template = (DataTemplate) XamlReader.Parse(xaml, context);
            return template;
        }
    }
}