using System.Linq;
using System.Text;
using CreativeCoders.Core;
using CreativeCoders.Core.Visitors;

namespace CreativeCoders.Scripting.CSharp.SourceCodeGenerator;

public class ClassSyntaxTree
{
    public ClassSyntaxTree()
    {
        RootNode = new ClassSyntaxTreeNode();
    }

    public string Emit(string sourceCode)
    {
        Ensure.IsNotNull(sourceCode, nameof(sourceCode));

        var sb = new StringBuilder();

        var visitor = new SyntaxSourceCodeEmitVisitor(sb, sourceCode);
        new ListVisitor<SyntaxSourceCodeEmitVisitor>(visitor).Visit(
            RootNode.SubNodes.Select(subNode => subNode.AsVisitable));

        return sb.ToString();
    }

    public ClassSyntaxTreeNode RootNode { get; }
}
