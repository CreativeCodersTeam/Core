using CreativeCoders.Scripting.CSharp.ClassTemplating;

namespace CreativeCoders.Scripting.UnitTests.CSharp;

public class TestScriptClassWithoutInterfaceTemplate : ScriptClassTemplate
{
    public TestScriptClassWithoutInterfaceTemplate(ITestApi testApi)
    {
        Usings.Add("System");
        Usings.Add("System.Linq");
        Usings.Add("System.Threading.Tasks");
            
        Members.AddRawContent("$$code$$");

        Injections.AddProperty("Api", () => testApi);

        Members.AddMethod("CallApi", "Api.DoSomething(\"Call\");");

        Members.AddProperty("TestText", "string", "return \"SomeText\";", "Api.DoSomething(value);");
    }
}