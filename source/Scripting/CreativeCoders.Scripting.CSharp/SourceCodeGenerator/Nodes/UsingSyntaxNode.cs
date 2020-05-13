using CreativeCoders.Core.Visitors;

namespace CreativeCoders.Scripting.CSharp.SourceCodeGenerator.Nodes
{
    public class UsingSyntaxNode : ClassSyntaxTreeNode, IVisitable<SyntaxSourceCodeEmitVisitor, UsingSyntaxNode>
    {
        public UsingSyntaxNode(string usingNameSpace)
        {
            UsingNameSpace = usingNameSpace;            
        }

        public void Accept(SyntaxSourceCodeEmitVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override IVisitable GetAsVisitable()
        {
            return new VisitableAction<SyntaxSourceCodeEmitVisitor>(Accept);
        }

        public string UsingNameSpace { get; }
    }
}