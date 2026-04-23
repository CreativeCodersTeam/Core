using JetBrains.Annotations;

namespace CreativeCoders.Core.Visitors;

/// <summary>
/// Defines a strongly-typed visitable object in the Visitor design pattern that accepts
/// a specific <see cref="IVisitor{TVisitor, TVisitableObject}"/> type.
/// </summary>
/// <typeparam name="TVisitor">The type of the visitor that can visit this object.</typeparam>
/// <typeparam name="TVisitableObject">The type of the visitable object.</typeparam>
[PublicAPI]
public interface IVisitable<in TVisitor, TVisitableObject>
    where TVisitor : IVisitor<TVisitor, TVisitableObject>
    where TVisitableObject : IVisitable<TVisitor, TVisitableObject>
{
    /// <summary>
    /// Accepts a strongly-typed visitor, allowing it to perform an operation on this object.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    void Accept(TVisitor visitor);
}
