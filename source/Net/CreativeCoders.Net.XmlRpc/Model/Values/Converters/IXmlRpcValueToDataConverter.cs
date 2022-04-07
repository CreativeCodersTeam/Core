using System;
using JetBrains.Annotations;

namespace CreativeCoders.Net.XmlRpc.Model.Values.Converters;

[PublicAPI]
public interface IXmlRpcValueToDataConverter
{
    object Convert(XmlRpcValue xmlRpcValue, Type targetType);

    T Convert<T>(XmlRpcValue xmlRpcValue);
}
