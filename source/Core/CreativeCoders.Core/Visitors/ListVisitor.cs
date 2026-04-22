using System.Collections.Generic;
using CreativeCoders.Core.Collections;

namespace CreativeCoders.Core.Visitors;

/// <summary>
/// Applies a visitor to each element in a collection of <see cref="IVisitable"/> objects.
/// </summary>
/// <typeparam name="TVisitor">The type of the visitor to apply.</typeparam>
public class ListVisitor<TVisitor>
{
    private readonly TVisitor _visitor;

    /// <summary>
    /// Initializes a new instance of the <see cref="ListVisitor{TVisitor}"/> class.
    /// </summary>
    /// <param name="visitor">The visitor to apply to each element.</param>
    public ListVisitor(TVisitor visitor)
    {
        _visitor = visitor;
    }

    /// <summary>
    /// Visits each element in the specified collection by passing the visitor to each element's
    /// <see cref="IVisitable.Accept"/> method.
    /// </summary>
    /// <param name="visitables">The collection of visitable objects to visit.</param>
    public void Visit(IEnumerable<IVisitable> visitables)
    {
        visitables.ForEach(visitable => visitable.Accept(_visitor));
    }
}
