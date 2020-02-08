using System.Collections.Generic;

namespace CreativeCoders.Core.Visitors
{
    public interface IVisitableSubItems
    {
        IEnumerable<IVisitable> GetVisitableSubItems();
    }
}