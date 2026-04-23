using System;

namespace CreativeCoders.Core.Visitors;

/// <summary>
/// Represents a visitable object that delegates the accept operation to a user-supplied action,
/// allowing inline visitor handling without creating a dedicated visitable class.
/// </summary>
/// <typeparam name="TVisitor">The type of visitor this action accepts.</typeparam>
public class VisitableAction<TVisitor> : IVisitable where TVisitor : class
{
    private readonly Action<TVisitor> _acceptAction;

    /// <summary>
    /// Initializes a new instance of the <see cref="VisitableAction{TVisitor}"/> class.
    /// </summary>
    /// <param name="acceptAction">The action to invoke when a visitor is accepted.</param>
    public VisitableAction(Action<TVisitor> acceptAction)
    {
        _acceptAction = acceptAction;
    }

    /// <inheritdoc/>
    public void Accept(object visitor)
    {
        if (visitor is not TVisitor theVisitor)
        {
            return;
        }

        _acceptAction?.Invoke(theVisitor);
    }
}
