namespace CreativeCoders.Core.Visitors
{
    public interface IVisitor<TVisitor, in TVisitableObject> where TVisitor : IVisitor<TVisitor, TVisitableObject>
        where TVisitableObject : IVisitable<TVisitor, TVisitableObject>
    {
        void Visit(TVisitableObject visitableObject);
    }
}