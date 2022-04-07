using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CreativeCoders.Net.XmlRpc.Model;

namespace CreativeCoders.Net.XmlRpc.Writer;

public class RequestModelWriter : ModelWriterBase<XmlRpcRequest>
{
    public RequestModelWriter(IValueWriters writers) : base(writers) { }

    protected override XDocument CreateXml(XmlRpcRequest xmlRpcRequest, Encoding encoding)
    {
        return xmlRpcRequest.Methods.Count() == 1
            ? new XDocument(
                new XDeclaration("1.0", encoding.BodyName.ToUpper(), null),
                CreateMethodCall(xmlRpcRequest.Methods.First()))
            : CreateMultiMethodCall(xmlRpcRequest.Methods);
    }

    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    private static XDocument CreateMultiMethodCall(IEnumerable<XmlRpcMethodCall> methods)
    {
        throw new NotImplementedException();
    }

    private XElement CreateMethodCall(XmlRpcMethodCall method)
    {
        if (!method.Parameters.Any())
        {
            return new XElement(XmlRpcTags.MethodCall, new XElement(XmlRpcTags.MethodName, method.Name));
        }

        var parameters = CreateParameters(method.Parameters);
        return new XElement(XmlRpcTags.MethodCall,
            new XElement(XmlRpcTags.MethodName, method.Name),
            new XElement(XmlRpcTags.Params, parameters));
    }

    private IEnumerable<XElement> CreateParameters(IEnumerable<XmlRpcValue> methodParameters)
    {
        foreach (var methodParameter in methodParameters)
        {
            var writer = Writers.GetWriter(methodParameter.GetType());

            var paramNode = new XElement(XmlRpcTags.Param);
            writer?.WriteTo(paramNode, methodParameter);

            yield return paramNode;
        }
    }
}
