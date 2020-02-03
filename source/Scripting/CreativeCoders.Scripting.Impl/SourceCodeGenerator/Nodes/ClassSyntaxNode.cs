using System.Collections.Generic;
using CreativeCoders.Core.Visitors;

namespace CreativeCoders.Scripting.Impl.SourceCodeGenerator.Nodes
{
    public class ClassSyntaxNode : ClassSyntaxTreeNode, IVisitable<SyntaxSourceCodeEmitVisitor, ClassSyntaxNode>
    {
        public ClassSyntaxNode(string className, IEnumerable<string> inheritsFrom)
        {
            ClassName = className;
            InheritsFrom = inheritsFrom;
        }

        public void Accept(SyntaxSourceCodeEmitVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override IVisitable GetAsVisitable()
        {
            return new VisitableAction<SyntaxSourceCodeEmitVisitor>(Accept);
        }

        public string ClassName { get; }

        public IEnumerable<string> InheritsFrom { get; }
    }
}