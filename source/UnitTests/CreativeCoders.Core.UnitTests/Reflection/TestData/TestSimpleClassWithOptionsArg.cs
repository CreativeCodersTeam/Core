#nullable enable
namespace CreativeCoders.Core.UnitTests.Reflection.TestData
{
    public class TestSimpleClassWithOptionsArg
    {
        public TestSimpleClassWithOptionsArg(ITestSimpleClassOptions options)
        {
            Options = options;
        }

        public ITestSimpleClassOptions Options { get; }
    }
}
