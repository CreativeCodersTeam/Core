using System.Linq;
using System.Text;
using CreativeCoders.Core;
using CreativeCoders.Core.Visitors;

namespace CreativeCoders.Scripting.Impl.SourceCodeGenerator
{
    public class ClassSyntaxTree
    {
        public ClassSyntaxTree()
        {
            RootNode = new ClassSyntaxTreeNode();
        }

        public string Emit(IScript script)
        {
            Ensure.IsNotNull(script, nameof(script));

            var sb = new StringBuilder();
            var visitor = new SyntaxSourceCodeEmitVisitor(sb, script);
            new ListVisitor<SyntaxSourceCodeEmitVisitor>(visitor).Visit(RootNode.SubNodes.Select(subNode => subNode.AsVisitable));
            return sb.ToString();
        }

        public ClassSyntaxTreeNode RootNode { get; }
    }
}