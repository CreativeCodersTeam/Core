using System.Net.Http;
using System.Reflection;

namespace CreativeCoders.Net.WebApi.Specification.Parameters;

public class ParameterCompletionOptionDefinition : ParameterDefinitionBase<HttpCompletionOption>
{
    public ParameterCompletionOptionDefinition(ParameterInfo parameterInfo) : base(parameterInfo,
        value => (HttpCompletionOption) value) { }
}
