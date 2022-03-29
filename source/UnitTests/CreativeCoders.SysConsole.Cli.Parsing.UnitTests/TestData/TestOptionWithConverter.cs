using System;
using CreativeCoders.SysConsole.Cli.Parsing.Properties;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Parsing.UnitTests.TestData;

public class TestOptionWithConverter
{
    [UsedImplicitly]
    [OptionParameter('t', "text", Converter = typeof(TestTextConverter))]
    public string? Text { get; set; }
}

public class TestTextConverter : ICliValueConverter
{
    public object Convert(object? value, Type targetType, OptionBaseAttribute optionAttribute)
    {
        var text = value?.ToString();

        return text switch
        {
            "Hello" => "World",
            "World" => "Hello",
            _ => text?.ToUpper() ?? string.Empty
        };
    }
}