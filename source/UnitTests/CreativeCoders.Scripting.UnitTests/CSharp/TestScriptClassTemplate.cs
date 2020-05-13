using CreativeCoders.Scripting.CSharp.ClassTemplating;

namespace CreativeCoders.Scripting.UnitTests.CSharp
{
    public class TestScriptClassTemplate : ScriptClassTemplate
    {
        public TestScriptClassTemplate(ITestApi testApi)
        {
            Usings.Add("System");
            Usings.Add("System.Linq");
            Usings.Add("System.Threading.Tasks");
            
            ImplementsInterfaces.Add(nameof(ITextScript));
            
            Members.AddRawContent("$$code$$");

            Injections.AddProperty("Api", () => testApi);

            Members.AddMethod("CallApi", "Api.DoSomething(\"Call\");");

            Members.AddProperty("TestText", "string", "return \"SomeText\";", "Api.DoSomething(value);");

            Members.AddProperty("IntValue", "int", "return 12345;");
        }
    }
}