using System.Reflection;
using CreativeCoders.Core;

namespace CreativeCoders.Net.WebApi.Specification.Parameters
{
    public class ParameterHeaderDefinition : ParameterDefinitionBase<string>
    {
        public ParameterHeaderDefinition(ParameterInfo parameterInfo, string name) : base(parameterInfo,
            value => value.ToStringSafe())
        {
            Name = name;
        }

        public string Name { get; }
    }
}