using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CreativeCoders.Core.Collections;
using CreativeCoders.Net.XmlRpc.Definition;

namespace CreativeCoders.Net.XmlRpc.Model.Values.Converters;

public class DataToXmlRpcValueConverter : IDataToXmlRpcValueConverter
{
    private readonly Dictionary<Type, Func<object, XmlRpcValue>> _valueMappings;

    public DataToXmlRpcValueConverter()
    {
        _valueMappings = new Dictionary<Type, Func<object, XmlRpcValue>>
        {
            {typeof(int), data => new IntegerValue((int) data)},
            {typeof(string), data => new StringValue((string) data)},
            {typeof(double), data => new DoubleValue((double) data)},
            {typeof(DateTime), data => new DateTimeValue((DateTime) data)},
            {typeof(bool), data => new BooleanValue((bool) data)},
            {typeof(byte[]), data => new Base64Value((byte[]) data, XmlEncoding)},
            {
                typeof(IDictionary), data =>
                {
                    var dictionary = (IDictionary) data;

                    return new StructValue(
                        dictionary
                            .ToDictionary<string, object>(true)
                            .ToDictionary(x => x.Key, x => Convert(x.Value)));
                }
            },
            {
                typeof(IEnumerable), data =>
                {
                    var items = ((IEnumerable) data).Cast<object>();

                    return new ArrayValue(
                        items
                            .Select(Convert)
                    );
                }
            },
            {typeof(object), ConvertObjectToStructValue}
        };
    }

    public XmlRpcValue Convert(object data)
    {
        switch (data)
        {
            case null:
                return null;
            case XmlRpcValue xmlRpcValue:
                return xmlRpcValue;
            default:
            {
                var mapper = GetMapper(data.GetType());

                return mapper(data);
            }
        }
    }

    private Func<object, XmlRpcValue> GetMapper(Type dataType)
    {
        if (dataType == null)
        {
            return null;
        }

        var directMapping = _valueMappings.FirstOrDefault(mapping => mapping.Key.IsAssignableFrom(dataType))
            .Value;

        if (directMapping != null)
        {
            return directMapping;
        }

        throw new InvalidOperationException($"Value of type '{dataType.Name}' cannot be converted");
    }

    private StructValue ConvertObjectToStructValue(object data)
    {
        if (!data.GetType().IsClass)
        {
            throw new InvalidOperationException($"Value of type '{data.GetType().Name}' cannot be converted");
        }

        var structValue = new StructValue();

        var properties = data.GetType().GetProperties();

        var memberProperties = from property in properties
            let memberAttribute = property.GetCustomAttribute<XmlRpcStructMemberAttribute>()
            where memberAttribute != null
            select (property, memberAttribute);

        memberProperties.ForEach(v => SetMemberValue(data, v.property, v.memberAttribute, structValue));

        return structValue;
    }


    private void SetMemberValue(object obj, PropertyInfo property,
        XmlRpcStructMemberAttribute memberAttribute,
        StructValue xmlRpcStruct)
    {
        var memberName = string.IsNullOrEmpty(memberAttribute.Name)
            ? property.Name
            : memberAttribute.Name;

        var propertyValue = property.GetValue(obj);

        var memberConverter = memberAttribute.GetConverter();

        var value = memberConverter != null
            ? memberConverter.ConvertFromObject(propertyValue)
            : Convert(propertyValue);

        if (value == null)
        {
            return;
        }

        xmlRpcStruct.Value[memberName] = value;
    }

    public Encoding XmlEncoding { get; set; } = Encoding.UTF8;
}
