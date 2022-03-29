using System.Collections.Generic;
using System.Xml.Linq;
using CreativeCoders.Net.XmlRpc.Model;

namespace CreativeCoders.Net.XmlRpc.Writer.Values;

public class StructValueWriter : ValueWriterBase<IDictionary<string, XmlRpcValue>>
{
    public StructValueWriter(IValueWriters writers) : base(XmlRpcTags.Struct, (value, element) => WriteToElement(value, element, writers))
    {
    }

    private static void WriteToElement(XmlRpcValue<IDictionary<string, XmlRpcValue>> value, XContainer element, IValueWriters writers)
    {
        foreach (var (key, xmlRpcValue) in value.Value)
        {
            var memberElement = new XElement(XmlRpcTags.Member);

            WriteMember(key, xmlRpcValue, writers, memberElement);

            element.Add(memberElement);
        }
    }

    private static void WriteMember(string name, XmlRpcValue value, IValueWriters writers, XElement element)
    {
        var writer = writers.GetWriter(value.GetType());

        var nameElement = new XElement(XmlRpcTags.Name);
        nameElement.Add(name);
        element.Add(nameElement);

        writer.WriteTo(element, value);
    }
}