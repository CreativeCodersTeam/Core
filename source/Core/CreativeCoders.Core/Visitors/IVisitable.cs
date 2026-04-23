namespace CreativeCoders.Core.Visitors;

/// <summary>
/// Defines a non-generic visitable object in the Visitor design pattern that accepts any visitor.
/// </summary>
public interface IVisitable
{
    /// <summary>
    /// Accepts a visitor, allowing it to perform an operation on this object.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    void Accept(object visitor);
}
