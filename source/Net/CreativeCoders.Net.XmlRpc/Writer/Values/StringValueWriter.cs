namespace CreativeCoders.Net.XmlRpc.Writer.Values;

public class StringValueWriter : ValueWriterBase<string>
{
    public StringValueWriter() : base(XmlRpcTags.String, value => value.Value)
    {
    }
}