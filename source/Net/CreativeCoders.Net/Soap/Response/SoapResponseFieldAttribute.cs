using System;

namespace CreativeCoders.Net.Soap.Response
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SoapResponseFieldAttribute : Attribute
    {
        public SoapResponseFieldAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}