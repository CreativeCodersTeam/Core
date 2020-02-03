using CreativeCoders.Core.Visitors;

namespace CreativeCoders.Scripting.Impl.SourceCodeGenerator.Nodes {
    public class PropertySyntaxNode : ClassSyntaxTreeNode, IVisitable<SyntaxSourceCodeEmitVisitor, PropertySyntaxNode>
    {
        public PropertySyntaxNode(string propertyName, string valueType, string propertyGetterSourceCode, string propertySetterSourceCode)
        {
            PropertyName = propertyName;
            ValueType = valueType;
            PropertyGetterSourceCode = propertyGetterSourceCode;
            PropertySetterSourceCode = propertySetterSourceCode;
        }

        public void Accept(SyntaxSourceCodeEmitVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override IVisitable GetAsVisitable()
        {
            return new VisitableAction<SyntaxSourceCodeEmitVisitor>(Accept);
        }

        public string PropertyName { get; }

        public string ValueType { get; }

        public string PropertyGetterSourceCode { get; }

        public string PropertySetterSourceCode { get; }
    }
}