using System.Collections.Generic;
using System.Xml.Linq;
using CreativeCoders.Core.Collections;
using CreativeCoders.Net.XmlRpc.Model;

namespace CreativeCoders.Net.XmlRpc.Writer.Values
{
    public class ArrayValueWriter : ValueWriterBase<IEnumerable<XmlRpcValue>>
    {
        public ArrayValueWriter(IValueWriters writers) : base(XmlRpcTags.Array, (value, element) => WriteToElement(value, element, writers))
        {
        }

        private static void WriteToElement(XmlRpcValue<IEnumerable<XmlRpcValue>> value, XContainer element, IValueWriters writers)
        {
            var dataElement = new XElement(XmlRpcTags.Data);

            WriteToElements(value.Value, dataElement, writers);

            element.Add(dataElement);
        }

        private static void WriteToElements(IEnumerable<XmlRpcValue> array, XElement dataElement, IValueWriters writers)
        {
            array.ForEach(value => WriteElement(value, dataElement, writers));
        }

        private static void WriteElement(XmlRpcValue value, XElement dataElement, IValueWriters writers)
        {
            var writer = writers.GetWriter(value.GetType());

            writer?.WriteTo(dataElement, value);
        }
    }
}
