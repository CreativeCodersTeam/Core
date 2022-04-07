using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;

namespace CreativeCoders.Net.Soap.Request;

internal class RequestXmlWriter
{
    private readonly StreamWriter _streamWriter;

    private readonly SoapRequestInfo _soapRequestInfo;

    public RequestXmlWriter(StreamWriter streamWriter, SoapRequestInfo soapRequestInfo)
    {
        Ensure.IsNotNull(streamWriter, nameof(streamWriter));
        Ensure.IsNotNull(soapRequestInfo, nameof(soapRequestInfo));

        _streamWriter = streamWriter;
        _soapRequestInfo = soapRequestInfo;
    }

    public void Write()
    {
        var xmlDoc = CreateXmlDoc();

        xmlDoc.Save(_streamWriter);
    }

    private XDocument CreateXmlDoc()
    {
        XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
        XNamespace xsd = "http://www.w3.org/2001/XMLSchema";

        var xmlDoc = new XDocument(
            new XDeclaration("1.0", "utf-8", "no"),
            new XElement(SoapConsts.EnvelopeName,
                new XAttribute(XNamespace.Xmlns + "xsi", xsi),
                new XAttribute(XNamespace.Xmlns + "xsd", xsd),
                new XAttribute(XNamespace.Xmlns + "soap", SoapConsts.SoapNameSpace),
                new XAttribute(SoapConsts.EncodingStyleName, "http://schemas.xmlsoap.org/soap/encoding/"),
                new XElement(SoapConsts.BodyName, CreateBodyContentXml())
            ));

        return xmlDoc;
    }

    private XElement CreateBodyContentXml()
    {
        XNamespace ns = _soapRequestInfo.ServiceNameSpace;

        var xmlNode = new XElement(ns + _soapRequestInfo.ActionName,
            new XAttribute(XNamespace.Xmlns + "u", ns));

        CreateParametersXml().ForEach(xmlNode.Add);

        return xmlNode;
    }

    private IEnumerable<XElement> CreateParametersXml()
    {
        return from propertyFieldMapping in _soapRequestInfo.PropertyMappings
            let propValue = propertyFieldMapping.Property.GetValue(_soapRequestInfo.Action)
            select new XElement(propertyFieldMapping.FieldName,
                new XText(propValue?.ToString() ?? string.Empty));
    }
}
