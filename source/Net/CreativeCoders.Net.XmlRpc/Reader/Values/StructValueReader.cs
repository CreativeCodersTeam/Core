using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using CreativeCoders.Net.XmlRpc.Model;
using CreativeCoders.Net.XmlRpc.Model.Values;

namespace CreativeCoders.Net.XmlRpc.Reader.Values;

public class StructValueReader : ValueReaderBase
{
    public StructValueReader(IValueReaders readers) :
        base(new[] {XmlRpcTags.Struct}, valueElement => CreateValue(valueElement, readers)) { }

    private static XmlRpcValue CreateValue(XNode valueNode, IValueReaders readers)
    {
        var memberElements = valueNode.XPathSelectElements(XmlRpcTags.Member);

        var structDictionary = ReadMembers(memberElements, readers);

        return new StructValue(structDictionary);
    }

    private static IDictionary<string, XmlRpcValue> ReadMembers(IEnumerable<XElement> memberElements,
        IValueReaders readers)
    {
        return memberElements
            .ToDictionary(me => me.Element(XmlRpcTags.Name)?.Value,
                me => ReadMemberValue(me.Element(XmlRpcTags.Value), readers));
    }

    private static XmlRpcValue ReadMemberValue(XElement memberValueElement, IValueReaders readers)
    {
        return readers.ReadValue(memberValueElement);
    }
}
