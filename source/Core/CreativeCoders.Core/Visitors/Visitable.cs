using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Visitors;

[PublicAPI]
public abstract class Visitable<TVisitor, TVisitableObject> : IVisitable,
    IVisitable<TVisitor, TVisitableObject>
    where TVisitor : class, IVisitor<TVisitor, TVisitableObject>
    where TVisitableObject : Visitable<TVisitor, TVisitableObject>
{
    private readonly IDictionary<Type, Action<object>> _visitors = new Dictionary<Type, Action<object>>();

    public void Accept(TVisitor visitor)
    {
        if (this is not TVisitableObject self)
        {
            return;
        }

        visitor.Visit(self);
    }

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
}
