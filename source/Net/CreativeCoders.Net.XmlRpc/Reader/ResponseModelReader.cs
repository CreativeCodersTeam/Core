using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using CreativeCoders.Core;
using CreativeCoders.Net.XmlRpc.Exceptions;
using CreativeCoders.Net.XmlRpc.Model;
using CreativeCoders.Net.XmlRpc.Model.Values;

namespace CreativeCoders.Net.XmlRpc.Reader
{
    public class ResponseModelReader : ModelReaderBase, IResponseModelReader
    {
        public ResponseModelReader(IValueReaders readers) : base(readers)
        {
        }

        public async Task<XmlRpcResponse> ReadAsync(Stream inputStream, bool isMultiCallResponse)
        {
            Ensure.IsNotNull(inputStream, nameof(inputStream));

            var xmlDoc = await ReadXmlDocAsync(inputStream).ConfigureAwait(false);

            var methodResponseElement = xmlDoc.XPathSelectElement(XmlRpcTags.MethodResponse);
            if (methodResponseElement == null)
            {
                return new XmlRpcResponse(Array.Empty<XmlRpcMethodResult>(), false);
            }

            var methodResult = ReadMethodResponse(methodResponseElement);

            return new XmlRpcResponse(new []{methodResult}, false);
        }

        private XmlRpcMethodResult ReadMethodResponse(XNode methodResponseNode)
        {
            var faultResult = ReadFault(methodResponseNode);
            if (faultResult != null)
            {
                return faultResult;
            }

            var parameterNodes = methodResponseNode.XPathSelectElements(XmlRpcTags.Params + "/" + XmlRpcTags.Param);

            var values = parameterNodes
                .Select(ReadXmlRpcValue)
                .WhereNotNull();

            return new XmlRpcMethodResult(values.ToArray());
        }

        private XmlRpcMethodResult ReadFault(XNode methodResponseNode)
        {
            var faultElement = methodResponseNode.XPathSelectElement(XmlRpcTags.Fault);

            if (faultElement == null)
            {
                return null;
            }

            if (!(ReadXmlRpcValue(faultElement) is StructValue faultStruct))
            {
                throw new ParserException("No fault struct found inside fault xml element");
            }

            var faultCodeValue = faultStruct.Value[XmlRpcConstants.FaultCode] as IntegerValue;
            var faultStringValue = faultStruct.Value[XmlRpcConstants.FaultString] as StringValue;

            return new XmlRpcMethodResult(faultCodeValue?.Value ?? 0, faultStringValue?.Value ?? string.Empty);
        }
    }
}
