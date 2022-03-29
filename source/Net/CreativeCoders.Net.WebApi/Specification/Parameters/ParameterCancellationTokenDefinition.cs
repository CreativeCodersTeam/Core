using System.Reflection;
using System.Threading;

namespace CreativeCoders.Net.WebApi.Specification.Parameters;

public class ParameterCancellationTokenDefinition : ParameterDefinitionBase<CancellationToken>
{
    public ParameterCancellationTokenDefinition(ParameterInfo parameterInfo) : base(parameterInfo,
        value => (CancellationToken) value)
    {
    }
}