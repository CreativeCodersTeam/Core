using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CreativeCoders.Net.XmlRpc.Model;

namespace CreativeCoders.Net.XmlRpc.Reader.Values
{
    public class ValueReaders : IValueReaders
    {
        private readonly List<ValueReaderBase> _readers;

        public ValueReaders(Encoding encoding)
        {
            _readers = new List<ValueReaderBase>
            {
                new StringValueReader(),
                new IntegerValueReader(),
                new BooleanValueReader(),
                new DoubleValueReader(),
                new DateTimeValueReader(),
                new Base64ValueReader(encoding),
                new StructValueReader(this),
                new ArrayValueReader(this)
            };
        }

        public IValueReader GetReader(string valueDataType)
        {
            return _readers.FirstOrDefault(r => r.HandlesDataType(valueDataType));
        }

        public XmlRpcValue ReadValue(XElement valueElement)
        {
            if (!valueElement.Elements().Any())
            {
                var stringReader = GetReader(XmlRpcTags.String);

                return stringReader.ReadValue(valueElement);
            }

            var dataNode = valueElement.Elements().First();

            var valueDataType = dataNode.Name.LocalName;

            var reader = GetReader(valueDataType);

            return reader.ReadValue(dataNode);
        }
    }
}