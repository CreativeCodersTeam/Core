using JetBrains.Annotations;

namespace CreativeCoders.Net.WebApi.Specification;

[PublicAPI]
public class QueryDefinition
{
    public QueryDefinition(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public string Key { get; }

    public string Value { get; }

    //todo implement
    public bool UrlEncode { get; set; }
}