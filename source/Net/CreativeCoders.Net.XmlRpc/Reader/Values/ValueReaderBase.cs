using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CreativeCoders.Net.XmlRpc.Exceptions;
using CreativeCoders.Net.XmlRpc.Model;

namespace CreativeCoders.Net.XmlRpc.Reader.Values;

public abstract class ValueReaderBase : IValueReader
{
    private readonly IEnumerable<string> _dataTypes;

    private readonly Func<string, XmlRpcValue> _createValue;

    private readonly Func<XElement, XmlRpcValue> _createValueFromXml;

    protected ValueReaderBase(IEnumerable<string> dataTypes, Func<string, XmlRpcValue> createValue)
    {
        _dataTypes = dataTypes;
        _createValue = createValue;
    }

    protected ValueReaderBase(IEnumerable<string> dataTypes, Func<XElement, XmlRpcValue> createValueFromXml)
    {
        _dataTypes = dataTypes;
        _createValueFromXml = createValueFromXml;
    }

    public XmlRpcValue ReadValue(XElement valueElement)
    {
        var dataType = valueElement.Name.LocalName;
        if (!HandlesDataType(dataType))
        {
            throw new ParserException($"Value has wrong data type '{dataType}'");
        }

        if (_createValueFromXml != null)
        {
            return _createValueFromXml(valueElement);
        }

        var value = valueElement.Value;
        return _createValue(value);
    }

    public bool HandlesDataType(string dataType)
    {
        return _dataTypes.Any(dt => dt.Equals(dataType, StringComparison.CurrentCultureIgnoreCase));
    }
}