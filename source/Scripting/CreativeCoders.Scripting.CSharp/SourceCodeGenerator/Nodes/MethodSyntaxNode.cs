using CreativeCoders.Core.Visitors;

namespace CreativeCoders.Scripting.CSharp.SourceCodeGenerator.Nodes;

public class MethodSyntaxNode : ClassSyntaxTreeNode, IVisitable<SyntaxSourceCodeEmitVisitor, MethodSyntaxNode>
{
    public MethodSyntaxNode(string methodName, string sourceCode)
    {
        MethodName = methodName;
        SourceCode = sourceCode;
    }

    public void Accept(SyntaxSourceCodeEmitVisitor visitor)
    {
        visitor.Visit(this);
    }

    protected override IVisitable GetAsVisitable()
    {
        return new VisitableAction<SyntaxSourceCodeEmitVisitor>(Accept);
    }

    public string MethodName { get; }

    public string SourceCode { get; }
}