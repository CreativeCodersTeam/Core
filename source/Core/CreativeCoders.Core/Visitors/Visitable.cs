using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Visitors;

/// <summary>
/// Provides an abstract base class for visitable objects in the Visitor design pattern,
/// supporting both strongly-typed and dynamically-registered visitor types.
/// </summary>
/// <typeparam name="TVisitor">The type of the primary visitor.</typeparam>
/// <typeparam name="TVisitableObject">The type of the concrete visitable object.</typeparam>
[PublicAPI]
public abstract class Visitable<TVisitor, TVisitableObject> : IVisitable,
    IVisitable<TVisitor, TVisitableObject>
    where TVisitor : class, IVisitor<TVisitor, TVisitableObject>
    where TVisitableObject : Visitable<TVisitor, TVisitableObject>
{
    private readonly Dictionary<Type, Action<object>> _visitors = new Dictionary<Type, Action<object>>();

    /// <summary>
    /// Registers an additional visitor type that this object can accept.
    /// </summary>
    /// <typeparam name="T">The type of the visitor to register.</typeparam>
    /// <param name="action">The action to invoke when a visitor of this type is accepted.</param>
    protected void AddVisitorType<T>(Action<T> action) where T : class
    {
        AddVisitorType(typeof(T), param => DoAccept(param, action));
    }

    private void AddVisitorType(Type visitorType, Action<object> acceptAction)
    {
        _visitors[visitorType] = acceptAction;
    }

    private static void DoAccept<T>(object param, Action<T> action) where T : class
    {
        if (param is T visitor)
        {
            action(visitor);
        }
    }

    /// <inheritdoc/>
    public void Accept(object visitor)
    {
        if (this is not TVisitableObject self)
        {
            return;
        }

        var theVisitor = visitor as TVisitor;

        switch (theVisitor)
        {
            case null when _visitors.TryGetValue(visitor.GetType(), out var acceptAction):
                acceptAction(visitor);
                return;
            case null when (visitor as IVisitorInfo)?.ThrowIfNoAcceptMethod == true:
                throw new AcceptForVisitorNotFoundException(visitor.GetType().FullName);
            case null:
                return;
            default:
                theVisitor.Visit(self);
                break;
        }
    }

    /// <inheritdoc/>
    public void Accept(TVisitor visitor)
    {
        if (this is not TVisitableObject self)
        {
            return;
        }

        visitor.Visit(self);
    }
}
