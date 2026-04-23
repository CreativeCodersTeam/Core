namespace CreativeCoders.Core.Visitors;

/// <summary>
/// Provides configuration metadata for a visitor, controlling its behavior when
/// a visitable object does not support the visitor type.
/// </summary>
public interface IVisitorInfo
{
    /// <summary>
    /// Gets a value indicating whether an <see cref="AcceptForVisitorNotFoundException"/> is thrown
    /// when a visitable object has no matching accept method for this visitor.
    /// </summary>
    bool ThrowIfNoAcceptMethod { get; }
}
