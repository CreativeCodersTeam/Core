using CreativeCoders.Core.Collections;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Visitors;

[PublicAPI]
public abstract class VisitorBase<TVisitor, TVisitableObject> : IVisitorInfo,
    IVisitor<TVisitor, TVisitableObject>
    where TVisitor : IVisitor<TVisitor, TVisitableObject>
    where TVisitableObject : IVisitable<TVisitor, TVisitableObject>
{
    protected VisitorBase(bool throwIfNoAcceptMethod)
    {
        ThrowIfNoAcceptMethod = throwIfNoAcceptMethod;
    }

    public abstract void Visit(TVisitableObject visitableObject);

    protected virtual void VisitSubItems(object visitableObject)
    {
        var visitableSubItems = visitableObject as IVisitableSubItems;
        visitableSubItems?.GetVisitableSubItems()?.ForEach(subItem => subItem.Accept(this));
    }

    public bool ThrowIfNoAcceptMethod { get; }
}
