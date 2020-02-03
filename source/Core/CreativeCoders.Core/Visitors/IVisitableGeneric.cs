using JetBrains.Annotations;

namespace CreativeCoders.Core.Visitors
{
    [PublicAPI]
    public interface IVisitable<in TVisitor, TVisitableObject>
        where TVisitor: IVisitor<TVisitor, TVisitableObject>
        where TVisitableObject : IVisitable<TVisitor, TVisitableObject>
    {
        void Accept(TVisitor visitor);
    }
}