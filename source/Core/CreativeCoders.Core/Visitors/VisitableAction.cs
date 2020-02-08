using System;

namespace CreativeCoders.Core.Visitors
{
    public class VisitableAction<TVisitor> : IVisitable where TVisitor : class 
    {
        private readonly Action<TVisitor> _acceptAction;

        public VisitableAction(Action<TVisitor> acceptAction)
        {
            _acceptAction = acceptAction;            
        }

        public void Accept(object visitor)
        {
            if (!(visitor is TVisitor theVisitor))
            {
                return;
            }
            _acceptAction?.Invoke(theVisitor);
        }
    }
}