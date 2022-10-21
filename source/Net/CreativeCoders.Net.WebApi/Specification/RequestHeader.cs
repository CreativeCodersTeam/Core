namespace CreativeCoders.Net.WebApi.Specification;

internal class RequestHeader
{
    public RequestHeader(string name, string value)
    {
        Name = name;
        Value = value;
    }

    public string Name { get; }

    public string Value { get; }
}
