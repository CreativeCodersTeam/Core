using System.Reflection;
using CreativeCoders.Net.XmlRpc.Definition;
using JetBrains.Annotations;

namespace CreativeCoders.Net.XmlRpc.Exceptions;

[PublicAPI]
public class RequiredMemberMissingException : XmlRpcException
{
    public RequiredMemberMissingException(PropertyInfo property, XmlRpcStructMemberAttribute memberAttribute) :
        base($"The required struct member '{memberAttribute.Name ?? property.Name}' is missing")
    {
        Property = property;
        StructMember = memberAttribute;
    }
        
    public PropertyInfo Property { get; }
        
    public XmlRpcStructMemberAttribute StructMember { get; }
}