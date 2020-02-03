using System;
using System.Reflection;
using CreativeCoders.Core;

namespace CreativeCoders.Net.WebApi.Specification.Parameters
{
    public class ParameterPathDefinition : ParameterDefinitionBase<string>
    {
        public ParameterPathDefinition(ParameterInfo parameterInfo, string name, bool urlEncode) : base(parameterInfo,
            value => GetPathValue(value, urlEncode))
        {
            Name = name;
        }

        private static string GetPathValue(object value, bool urlEncode)
        {
            return urlEncode
                ? Uri.EscapeDataString(value.ToStringSafe())
                : value.ToStringSafe();
        }

        public string Name { get; }
    }
}