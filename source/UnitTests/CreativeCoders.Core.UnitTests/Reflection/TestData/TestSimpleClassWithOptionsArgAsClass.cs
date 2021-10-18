#nullable enable
namespace CreativeCoders.Core.UnitTests.Reflection.TestData
{
    public class TestSimpleClassWithOptionsArgAsClass
    {
        public TestSimpleClassWithOptionsArgAsClass(TestSimpleClassOptions options)
        {
            Options = options;
        }

        public TestSimpleClassOptions Options { get; }
    }
}
