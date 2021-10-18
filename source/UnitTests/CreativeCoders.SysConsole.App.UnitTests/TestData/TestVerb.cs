using System.Threading.Tasks;
using CreativeCoders.SysConsole.App.Verbs;

namespace CreativeCoders.SysConsole.App.UnitTests.TestData
{
    public class TestVerb : VerbBase<TestVerbOptions>
    {
        public const int ReturnCode = 123;

        public TestVerb(TestVerbOptions options) : base(options)
        {
        }

        public override Task<int> ExecuteAsync()
        {
            OptionsFirstArg = Options.FirstArg;

            return Task.FromResult(ReturnCode);
        }

        public static string? OptionsFirstArg { get; set; }
    }
}
