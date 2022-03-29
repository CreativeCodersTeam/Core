namespace CreativeCoders.Net.XmlRpc.Writer.Values;

public class BooleanValueWriter : ValueWriterBase<bool>
{
    public BooleanValueWriter() : base(XmlRpcTags.Boolean, value => value.Value ? "1" : "0") { }
}
