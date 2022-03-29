using System;

namespace CreativeCoders.Core.Visitors;

public class AcceptForVisitorNotFoundException : Exception
{
    public AcceptForVisitorNotFoundException(string visitorName) : base(
        $"Visitable object has no accept method for visitor '{visitorName}'") { }
}
