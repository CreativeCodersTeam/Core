namespace CreativeCoders.CodeCompilation.UnitTests.TestData;

public static class TestScripts
{
    public static string SimpleScript { get; } =
        @"using System;
              namespace CreativeCoders.CodeCompilation.UnitTests.TestData {
                public class SimpleScript : ISimpleScript { public int AddIntegers(int value0, int value1) => value0 + value1; } }";
}
