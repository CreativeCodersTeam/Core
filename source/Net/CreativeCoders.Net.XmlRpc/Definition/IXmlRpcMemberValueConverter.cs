using CreativeCoders.Net.XmlRpc.Model;

namespace CreativeCoders.Net.XmlRpc.Definition;

public interface IXmlRpcMemberValueConverter
{
    object ConvertFromValue(XmlRpcValue xmlRpcValue);

    XmlRpcValue ConvertFromObject(object value);
}
