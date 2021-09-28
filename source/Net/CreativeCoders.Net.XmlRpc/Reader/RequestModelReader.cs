using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using CreativeCoders.Core;
using CreativeCoders.Net.XmlRpc.Model;
using CreativeCoders.Net.XmlRpc.Model.Values;

namespace CreativeCoders.Net.XmlRpc.Reader
{
    public class RequestModelReader : ModelReaderBase
    {
        public RequestModelReader(IValueReaders valueReaders) : base(valueReaders)
        {
        }

        public async Task<XmlRpcRequest> ReadAsync(Stream inputStream)
        {
            Ensure.IsNotNull(inputStream, nameof(inputStream));

            var xmlDoc = await ReadXmlDocAsync(inputStream).ConfigureAwait(false);

            var methodCallNode = xmlDoc.XPathSelectElement(XmlRpcTags.MethodCall);
            if (methodCallNode == null)
            {
                return new XmlRpcRequest(Array.Empty<XmlRpcMethodCall>(), false);
            }

            var method = ReadMethod(methodCallNode);

            return method.Name.Equals(XmlRpcConstants.MultiCallMethodName)
                ? UnwrapMultiCall(method)
                : new XmlRpcRequest(new []{method}, false);
        }

        private static XmlRpcRequest UnwrapMultiCall(XmlRpcMethodCall methodCall)
        {
            var callsParameter = methodCall.Parameters.FirstOrDefault();

            if (!(callsParameter is ArrayValue arrayValue))
            {
                throw new InvalidOperationException();
            }

            var callStructs = arrayValue.Value.OfType<StructValue>();
            var methodCalls = callStructs.Select(UnwrapCall);

            return new XmlRpcRequest(methodCalls, true);
        }

        private static XmlRpcMethodCall UnwrapCall(StructValue callStruct)
        {
            var nameValue = callStruct.Value[XmlRpcTags.MethodName];
            var paramsValue = callStruct.Value[XmlRpcTags.Params] as ArrayValue;

            return new XmlRpcMethodCall(nameValue?.Data?.ToString(), paramsValue?.Value.ToArray());
        }

        private XmlRpcMethodCall ReadMethod(XNode methodCallNode)
        {
            var methodName = ReadNodeValue(methodCallNode, XmlRpcTags.MethodName);

            var parameterNodes = methodCallNode.XPathSelectElements(XmlRpcTags.Params + "/" +XmlRpcTags.Param);

            var values = parameterNodes
                .Select(ReadXmlRpcValue)
                .WhereNotNull();

            return new XmlRpcMethodCall(methodName, values.ToArray());
        }

        private static string ReadNodeValue(XNode xmlNode, string elementName)
        {
            var node = xmlNode.XPathSelectElement(elementName);
            return node?.Value;
        }
    }
}
