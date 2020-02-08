namespace CreativeCoders.Net.WebApi.Specification
{
    public class QueryDefinition
    {
        public QueryDefinition(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; }

        public string Value { get; }

        public bool UrlEncode { get; set; }
    }
}