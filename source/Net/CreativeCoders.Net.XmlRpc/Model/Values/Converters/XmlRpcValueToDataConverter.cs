using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.Core.Collections;
using CreativeCoders.Core.Reflection;
using CreativeCoders.Net.XmlRpc.Definition;
using CreativeCoders.Net.XmlRpc.Exceptions;

namespace CreativeCoders.Net.XmlRpc.Model.Values.Converters;

public class XmlRpcValueToDataConverter : IXmlRpcValueToDataConverter
{
    private readonly Dictionary<Type, Func<Type, XmlRpcValue, object>> _valueMappings;

    public XmlRpcValueToDataConverter()
    {
        _valueMappings = new Dictionary<Type, Func<Type, XmlRpcValue, object>>
        {
            {typeof(int), (_, xmlRpcValue) => xmlRpcValue.GetValue<int>()},
            {typeof(string), (_, xmlRpcValue) => xmlRpcValue.GetValue<string>()},
            {typeof(double), (_, xmlRpcValue) => xmlRpcValue.GetValue<double>()},
            {typeof(DateTime), (_, xmlRpcValue) => xmlRpcValue.GetValue<DateTime>()},
            {typeof(bool), (_, xmlRpcValue) => xmlRpcValue.GetValue<bool>()},
            {typeof(byte[]), (_, xmlRpcValue) => xmlRpcValue.GetValue<byte[]>()},
            {typeof(IDictionary), ConvertToDictionary},
            {typeof(IEnumerable), ConvertToEnumerable},
            {typeof(object), ConvertToObject}
        };
    }

    public object Convert(XmlRpcValue xmlRpcValue, Type targetType)
    {
        if (typeof(XmlRpcValue).IsAssignableFrom(targetType))
        {
            return xmlRpcValue;
        }

        var mapper = GetMapper(targetType);

        return mapper(targetType, xmlRpcValue);
    }

    public T Convert<T>(XmlRpcValue xmlRpcValue)
    {
        return (T) Convert(xmlRpcValue, typeof(T));
    }

    private Func<Type, XmlRpcValue, object> GetMapper(Type targetType)
    {
        var mapper = _valueMappings.FirstOrDefault(mapping => mapping.Key.IsAssignableFrom(targetType)).Value;

        if (mapper != null)
        {
            return mapper;
        }

        throw new InvalidOperationException($"Value of type '{targetType.Name}' cannot be converted");
    }

    private object ConvertToEnumerable(Type targetType, XmlRpcValue xmlRpcValue)
    {
        if (xmlRpcValue is not ArrayValue arrayValue)
        {
            throw new InvalidOperationException(
                $"Xml rpc value must be an array value. Current type '{xmlRpcValue.GetType().Name}'");
        }

        var elementType =
            targetType.IsArray
                ? targetType.GetElementType()
                : targetType.IsGenericType
                    ? targetType.GetGenericArguments().First()
                    : null;

        return CreateArray(arrayValue, elementType ?? typeof(object));
    }

    private object CreateArray(ArrayValue arrayValue, Type elementType)
    {
        var arrayType = elementType.MakeArrayType();
        var array = Activator.CreateInstance(arrayType, arrayValue.Value.Count());

        var values = arrayValue.Value.Select(v => Convert(v, elementType));

        if (array is not IList list)
        {
            throw new InvalidOperationException("Array type does not implement IList");
        }

        values.ForEach((obj, index) => list[index] = obj);

        return array;
    }

    private object ConvertToDictionary(Type targetType, XmlRpcValue xmlRpcValue)
    {
        if (xmlRpcValue is not StructValue structValue)
        {
            throw new InvalidOperationException(
                $"Xml rpc value must be an struct value. Current type '{xmlRpcValue.GetType().Name}'");
        }

        var elementType = targetType.GetGenericArguments().Skip(1).First();

        var convertValue = GetConvertValueFunction(elementType);

        var dictionary = this.ExecuteGenericMethod<object>(nameof(ToDictionary), new[] {elementType},
            structValue.Value as IDictionary, convertValue);

        return dictionary;
    }

    private Func<XmlRpcValue, Type, object> GetConvertValueFunction(Type elementType)
    {
        if (elementType == typeof(object))
        {
            return (valueToConvert, _) =>
                Convert(valueToConvert, valueToConvert.Data.GetType());
        }

        return Convert;
    }

    // ReSharper disable once MemberCanBeMadeStatic.Local
    private IDictionary<string, TElement> ToDictionary<TElement>(IDictionary sourceDictionary,
        Func<XmlRpcValue, Type, object> convertValue)
    {
        return sourceDictionary.ToDictionary<string, TElement>(
            o => (TElement) convertValue((XmlRpcValue) o, typeof(TElement)), true);
    }

    private object ConvertToObject(Type targetType, XmlRpcValue xmlRpcValue)
    {
        if (xmlRpcValue is StructValue structValue)
        {
            return CreateFromStruct(structValue, targetType);
        }

        if (targetType == typeof(object))
        {
            return xmlRpcValue.Data;
        }

        throw new InvalidOperationException(
            $"Xml rpc value must be an struct value or target type must be object. Current type '{xmlRpcValue.GetType().Name}'");
    }

    private object CreateFromStruct(StructValue structValue, Type targetType)
    {
        var instance = Activator.CreateInstance(targetType);

        var properties = instance?.GetType().GetProperties() ??
                         throw new InvalidOperationException("Object for xml rpc struct cannot be created");

        var memberProperties = (from property in properties
            let memberAttribute = property.GetCustomAttribute<XmlRpcStructMemberAttribute>()
            where memberAttribute != null
            select (property, memberAttribute)).ToArray();

        memberProperties.ForEach(v => SetProperty(instance, v.property, v.memberAttribute, structValue));

        return instance;
    }

    private void SetProperty(object obj, PropertyInfo property, XmlRpcStructMemberAttribute memberAttribute,
        StructValue structValue)
    {
        var memberName = string.IsNullOrEmpty(memberAttribute.Name)
            ? property.Name
            : memberAttribute.Name;

        if (!structValue.Value.TryGetValue(memberName, out var xmlRpcValue))
        {
            if (memberAttribute.Required)
            {
                throw new RequiredMemberMissingException(property, memberAttribute);
            }

            if (memberAttribute.DefaultValue != XmlRpcStructMemberAttribute.NoDefaultValue)
            {
                property.SetValue(obj, memberAttribute.DefaultValue);
            }

            return;
        }

        var memberConverter = memberAttribute.GetConverter();

        var memberValue = memberConverter == null
            ? Convert(xmlRpcValue, memberAttribute.DataType ?? property.PropertyType)
            : memberConverter.ConvertFromValue(xmlRpcValue);

        property.SetValue(obj, memberValue);
    }
}
