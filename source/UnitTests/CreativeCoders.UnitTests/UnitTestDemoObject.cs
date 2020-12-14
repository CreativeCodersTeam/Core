using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace CreativeCoders.UnitTests
{
    [ExcludeFromCodeCoverage]
    [PublicAPI]
    public class UnitTestDemoObject
    {
        public string Text { get; set; }

        public int IntValue { get; set; }

        public bool IsBool { get; set; }
    }
}