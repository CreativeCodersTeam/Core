using System;

namespace CreativeCoders.Net.Soap.Request
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SoapRequestAttribute : Attribute
    {
        public SoapRequestAttribute(string name, string nameSpace)
        {
            Name = name;
            NameSpace = nameSpace;
        }

        public string Name { get; }

        public string NameSpace { get; }
    }
}