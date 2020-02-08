using CreativeCoders.Core.Visitors;

namespace CreativeCoders.Scripting.Impl.SourceCodeGenerator.Nodes
{
    public class NameSpaceSyntaxNode : ClassSyntaxTreeNode, IVisitable<SyntaxSourceCodeEmitVisitor, NameSpaceSyntaxNode>
    {
        public NameSpaceSyntaxNode(string nameSpace)
        {
            NameSpace = nameSpace;
        }

        public void Accept(SyntaxSourceCodeEmitVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override IVisitable GetAsVisitable()
        {
            return new VisitableAction<SyntaxSourceCodeEmitVisitor>(Accept);
        }

        public string NameSpace { get; }
    }
}