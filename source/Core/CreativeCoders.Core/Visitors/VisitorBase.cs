using CreativeCoders.Core.Collections;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Visitors;

/// <summary>
/// Provides a base implementation for visitors in the Visitor design pattern, with support
/// for recursive sub-item visitation and configurable error behavior.
/// </summary>
/// <typeparam name="TVisitor">The type of the concrete visitor.</typeparam>
/// <typeparam name="TVisitableObject">The type of the visitable object.</typeparam>
[PublicAPI]
public abstract class VisitorBase<TVisitor, TVisitableObject> : IVisitorInfo,
    IVisitor<TVisitor, TVisitableObject>
    where TVisitor : IVisitor<TVisitor, TVisitableObject>
    where TVisitableObject : IVisitable<TVisitor, TVisitableObject>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VisitorBase{TVisitor, TVisitableObject}"/> class.
    /// </summary>
    /// <param name="throwIfNoAcceptMethod">
    /// <see langword="true"/> to throw an <see cref="AcceptForVisitorNotFoundException"/> when a visitable
    /// object does not have a matching accept method; otherwise, <see langword="false"/>.
    /// </param>
    protected VisitorBase(bool throwIfNoAcceptMethod)
    {
        ThrowIfNoAcceptMethod = throwIfNoAcceptMethod;
    }

    /// <inheritdoc/>
    public abstract void Visit(TVisitableObject visitableObject);

    /// <summary>
    /// Visits the child items of the specified object if it implements <see cref="IVisitableSubItems"/>.
    /// </summary>
    /// <param name="visitableObject">The object whose child items to visit.</param>
    protected virtual void VisitSubItems(object visitableObject)
    {
        var visitableSubItems = visitableObject as IVisitableSubItems;
        visitableSubItems?.GetVisitableSubItems()?.ForEach(subItem => subItem.Accept(this));
    }

    /// <inheritdoc/>
    public bool ThrowIfNoAcceptMethod { get; }
}
