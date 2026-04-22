namespace CreativeCoders.Core.Visitors;

/// <summary>
/// Defines the visitor in the Visitor design pattern, responsible for performing operations
/// on <see cref="IVisitable{TVisitor, TVisitableObject}"/> objects without modifying their classes.
/// </summary>
/// <typeparam name="TVisitor">The type of the concrete visitor.</typeparam>
/// <typeparam name="TVisitableObject">The type of the visitable object.</typeparam>
public interface IVisitor<TVisitor, in TVisitableObject> where TVisitor : IVisitor<TVisitor, TVisitableObject>
    where TVisitableObject : IVisitable<TVisitor, TVisitableObject>
{
    /// <summary>
    /// Visits the specified visitable object and performs an operation on it.
    /// </summary>
    /// <param name="visitableObject">The visitable object to visit.</param>
    void Visit(TVisitableObject visitableObject);
}
