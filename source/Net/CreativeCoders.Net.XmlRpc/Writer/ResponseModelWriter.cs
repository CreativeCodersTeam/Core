using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CreativeCoders.Net.XmlRpc.Model;
using CreativeCoders.Net.XmlRpc.Model.Values;

namespace CreativeCoders.Net.XmlRpc.Writer
{
    public class ResponseModelWriter : ModelWriterBase<XmlRpcResponse>, IResponseModelWriter
    {
        public ResponseModelWriter(IValueWriters writers) : base(writers)
        {
        }

        protected override XDocument CreateXml(XmlRpcResponse xmlRpcRequest, Encoding encoding)
        {
            return xmlRpcRequest.IsMultiCall
                ? CreateMultiCallResponse(xmlRpcRequest, encoding)
                : new XDocument(
                    new XDeclaration("1.0", encoding.BodyName.ToUpper(), null),
                    CreateResponse(xmlRpcRequest.Results.First()));
        }

        private XElement CreateResponse(XmlRpcMethodResult result)
        {
            var paramElement = new XElement(XmlRpcTags.Param);
            var writer = Writers.GetWriter(result.Values.FirstOrDefault()?.GetType());
            writer?.WriteTo(paramElement, result.Values.FirstOrDefault());
            var paramsElement = new XElement(XmlRpcTags.Params, paramElement);

            var methodResponseElement = new XElement(XmlRpcTags.MethodResponse, paramsElement);

            return methodResponseElement;
        }

        private XDocument CreateMultiCallResponse(XmlRpcResponse xmlRpcResponse, Encoding encoding)
        {
            return new XDocument(new XDeclaration("1.0", encoding.BodyName.ToUpper(), null), CreateMultiCallResultValues(xmlRpcResponse.Results));
        }

        private XElement CreateMultiCallResultValues(IEnumerable<XmlRpcMethodResult> results)
        {
            var paramElement = new XElement(XmlRpcTags.Param);

            var paramsElement = new XElement(XmlRpcTags.Params, paramElement);

            var values = results.Select(x => new ArrayValue(new[] {x.Values.First()})).AsEnumerable();

            var arrayValue = new ArrayValue(values);

            var writer = Writers.GetWriter(arrayValue.GetType());

            writer.WriteTo(paramElement, arrayValue);

            var methodResponseElement = new XElement(XmlRpcTags.MethodResponse, paramsElement);

            return methodResponseElement;
        }
    }
}