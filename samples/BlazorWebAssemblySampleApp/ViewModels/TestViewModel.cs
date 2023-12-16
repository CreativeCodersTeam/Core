using System;
using CreativeCoders.Core;
using JetBrains.Annotations;

namespace BlazorWebAssemblySampleApp.ViewModels;

public class TestViewModel : ObservableObject
{
    private string _text;

    private int _intValue;

    public string Text
    {
        get => _text;
        set
        {
            Set(ref _text, value);
            Console.WriteLine("Text changed -> " + Text);
        }
    }

    [UsedImplicitly]
    public int IntValue
    {
        get => _intValue;
        set => Set(ref _intValue, value);
    }
}
