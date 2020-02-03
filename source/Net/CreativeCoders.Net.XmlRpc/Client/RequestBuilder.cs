using System.Collections.Generic;
using System.Linq;
using System.Text;
using CreativeCoders.Net.XmlRpc.Model;
using CreativeCoders.Net.XmlRpc.Model.Values.Converters;

namespace CreativeCoders.Net.XmlRpc.Client
{
    public class RequestBuilder : IRequestBuilder
    {
        private readonly IDataToXmlRpcValueConverter _dataToXmlRpcValueConverter;

        public RequestBuilder(IDataToXmlRpcValueConverter dataToXmlRpcValueConverter)
        {
            _dataToXmlRpcValueConverter = dataToXmlRpcValueConverter;
        }

        public XmlRpcRequest Build(string methodName, params object[] parameters)
        {
            var methodCall = new XmlRpcMethodCall(methodName, CreateXmlRpcValues(parameters).ToArray());

            var xmlRpcRequest = new XmlRpcRequest(new []{methodCall}, false);

            return xmlRpcRequest;
        }

        private IEnumerable<XmlRpcValue> CreateXmlRpcValues(IEnumerable<object> parameters)
        {
            return parameters.Select(parameter => _dataToXmlRpcValueConverter.Convert(parameter));
        }

        public Encoding XmlEncoding
        {
            get => _dataToXmlRpcValueConverter.XmlEncoding;
            set => _dataToXmlRpcValueConverter.XmlEncoding = value;
        }
    }
}