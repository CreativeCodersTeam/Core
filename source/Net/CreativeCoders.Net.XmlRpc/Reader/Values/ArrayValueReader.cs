using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using CreativeCoders.Net.XmlRpc.Model;
using CreativeCoders.Net.XmlRpc.Model.Values;

namespace CreativeCoders.Net.XmlRpc.Reader.Values;

public class ArrayValueReader : ValueReaderBase
{
    public ArrayValueReader(IValueReaders readers) :
        base(new []{XmlRpcTags.Array}, valueElement => CreateValue(valueElement, readers))
    {
    }

    private static XmlRpcValue CreateValue(XNode valueNode, IValueReaders readers)
    {
        var valueElements = valueNode.XPathSelectElements(XmlRpcTags.Data + "/" + XmlRpcTags.Value);

        var values = GetValues(valueElements, readers);

        return new ArrayValue(values.ToArray());
    }

    private static IEnumerable<XmlRpcValue> GetValues(IEnumerable<XElement> valueElements, IValueReaders readers)
    {
        return valueElements.Select(readers.ReadValue);
    }
}