using System;
using JetBrains.Annotations;

namespace CreativeCoders.Net.XmlRpc.Definition
{
    [PublicAPI]
    [AttributeUsage(AttributeTargets.Property)]
    public class XmlRpcStructMemberAttribute : Attribute
    {
        public XmlRpcStructMemberAttribute() : this(string.Empty)
        {
        }

        public XmlRpcStructMemberAttribute(string name)
        {
            Name = name;
            DefaultValue = NoDefaultValue;
        }

        public IXmlRpcMemberValueConverter GetConverter()
        {
            if (Converter == null)
            {
                return null;
            }

            var converter = Activator.CreateInstance(Converter) as IXmlRpcMemberValueConverter;

            return converter;
        }

        public string Name { get; }

        public Type Converter { get; set; }

        public object DefaultValue { get; set; }

        public bool Required { get; set; }

        public Type DataType { get; set; }
        
        public static readonly object NoDefaultValue = new();
    }
}
