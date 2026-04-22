using System.Collections.Generic;

namespace CreativeCoders.Core.Visitors;

/// <summary>
/// Defines a visitable object that exposes child items for recursive visitation in the Visitor pattern.
/// </summary>
public interface IVisitableSubItems
{
    /// <summary>
    /// Returns the child items that should be visited recursively.
    /// </summary>
    /// <returns>An enumerable of visitable child items.</returns>
    IEnumerable<IVisitable> GetVisitableSubItems();
}
