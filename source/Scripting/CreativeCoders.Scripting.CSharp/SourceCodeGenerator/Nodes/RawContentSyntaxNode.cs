using CreativeCoders.Core.Visitors;

namespace CreativeCoders.Scripting.CSharp.SourceCodeGenerator.Nodes;

public class RawContentSyntaxNode : ClassSyntaxTreeNode,
    IVisitable<SyntaxSourceCodeEmitVisitor, RawContentSyntaxNode>
{
    public RawContentSyntaxNode(string rawContent)
    {
        RawContent = rawContent;
    }

    protected override IVisitable GetAsVisitable()
    {
        return new VisitableAction<SyntaxSourceCodeEmitVisitor>(Accept);
    }

    public void Accept(SyntaxSourceCodeEmitVisitor visitor)
    {
        visitor.Visit(this);
    }

    public string RawContent { get; }
}
