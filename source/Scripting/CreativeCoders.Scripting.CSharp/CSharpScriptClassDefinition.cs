using System.Collections.Generic;

namespace CreativeCoders.Scripting.CSharp
{
    public class CSharpScriptClassDefinition
    {
        public CSharpScriptClassDefinition()
        {
            Usings = new List<string>();
        }
        
        public string NameSpace { get; set; }

        public string ClassName { get; set; }

        public string SourceCode { get; set; }

        public IList<string> Usings { get; }
    }
}