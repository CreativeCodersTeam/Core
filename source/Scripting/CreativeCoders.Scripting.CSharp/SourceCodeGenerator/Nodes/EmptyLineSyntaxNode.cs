using CreativeCoders.Core.Visitors;

namespace CreativeCoders.Scripting.CSharp.SourceCodeGenerator.Nodes
{
    public class EmptyLineSyntaxNode : ClassSyntaxTreeNode, IVisitable<SyntaxSourceCodeEmitVisitor, EmptyLineSyntaxNode>
    {
        public void Accept(SyntaxSourceCodeEmitVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override IVisitable GetAsVisitable()
        {
            return new VisitableAction<SyntaxSourceCodeEmitVisitor>(Accept);
        }
    }
}