namespace CreativeCoders.Net.WebApi.Specification;

public class PathVariable
{
    public PathVariable(string name, string value)
    {
        Name = name;
        Value = value;
    }

    public string Name { get; }

    public string Value { get; }
}