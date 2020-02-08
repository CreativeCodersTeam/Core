using System;
using CreativeCoders.Core.Enums;
using CreativeCoders.Net.XmlRpc.Model;
using CreativeCoders.Net.XmlRpc.Model.Values;

namespace CreativeCoders.Net.XmlRpc.Definition.MemberConverters
{
    public class EnumMemberValueConverter<T> : IXmlRpcMemberValueConverter
        where T : Enum
    {
        public object ConvertFromValue(XmlRpcValue xmlRpcValue)
        {
            if (xmlRpcValue is IntegerValue intValue)
            {
                return EnumUtils.GetEnum<T>(intValue.Value);
            }

            return xmlRpcValue.Data;
        }

        public XmlRpcValue ConvertFromObject(object value)
        {
            return new IntegerValue(Convert.ToInt32(value));
        }
    }
}