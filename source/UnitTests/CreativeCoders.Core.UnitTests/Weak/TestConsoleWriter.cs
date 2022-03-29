using System;
using System.Diagnostics.CodeAnalysis;

namespace CreativeCoders.Core.UnitTests.Weak;

public class TestConsoleWriter
{
    [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
    public void Write(string text)
    {
        Console.WriteLine(text);
    }
}