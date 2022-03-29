namespace CreativeCoders.Core.Visitors;

public interface IVisitable
{
    void Accept(object visitor);
}
