using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using CreativeCoders.Core;
using CreativeCoders.Net.XmlRpc.Model;

namespace CreativeCoders.Net.XmlRpc.Reader
{
    public abstract class ModelReaderBase
    {
        private readonly IValueReaders _readers;

        protected ModelReaderBase(IValueReaders readers)
        {
            Ensure.IsNotNull(readers, nameof(readers));

            _readers = readers;
        }

        protected static Task<XDocument> ReadXmlDocAsync(Stream inputStream)
        {
            // todo make async in .net core 3.0
            var xmlReader = XmlReader.Create(inputStream, new XmlReaderSettings
            {
                Async = true,
                IgnoreComments = true
            });
            var xmlDoc = XDocument.Load(xmlReader);

            return Task.FromResult(xmlDoc);
        }

        protected XmlRpcValue ReadXmlRpcValue(XElement parameterNode)
        {
            var valueNode = parameterNode.XPathSelectElement(XmlRpcTags.Value);
            var isValueParameter = valueNode != null;

            return isValueParameter ? _readers.ReadValue(valueNode) : null;
        }
    }
}