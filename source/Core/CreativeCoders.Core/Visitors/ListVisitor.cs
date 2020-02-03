using System.Collections.Generic;

namespace CreativeCoders.Core.Visitors
{
    public class ListVisitor<TVisitor>
    {
        private readonly TVisitor _visitor;

        public ListVisitor(TVisitor visitor)
        {
            _visitor = visitor;
        }

        public void Visit(IEnumerable<IVisitable> visitables)
        {
            visitables.ForEach(visitable => visitable.Accept(_visitor));
        }
    }
}