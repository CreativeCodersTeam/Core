namespace CreativeCoders.Net.XmlRpc.Writer.Values;

public class IntegerValueWriter : ValueWriterBase<int>
{
    public IntegerValueWriter() : base(XmlRpcTags.I4, value => value.Value.ToString()) { }
}
