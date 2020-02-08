using System;
using CreativeCoders.Net.XmlRpc.Model;
using CreativeCoders.Net.XmlRpc.Model.Values;

namespace CreativeCoders.Net.XmlRpc.Definition.MemberConverters
{
    public class BoolToIntegerValueMemberValueConverter : IXmlRpcMemberValueConverter
    {
        public object ConvertFromValue(XmlRpcValue xmlRpcValue)
        {
            if (xmlRpcValue is IntegerValue integerValue)
            {
                return integerValue.Value != 0;
            }

            throw new InvalidOperationException($"'{xmlRpcValue}' is no xml rpc integer value");
        }

        public XmlRpcValue ConvertFromObject(object value)
        {
            if (value is bool boolValue)
            {
                return new IntegerValue(boolValue ? 1 : 0);
            }

            throw new InvalidOperationException($"'{value}' is no bool value");
        }
    }
}