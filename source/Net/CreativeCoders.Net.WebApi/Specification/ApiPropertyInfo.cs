using System.Collections.Generic;
using CreativeCoders.Net.WebApi.Specification.Properties;

namespace CreativeCoders.Net.WebApi.Specification;

internal class ApiPropertyInfo
{
    public IEnumerable<PropertyHeaderDefinition> HeaderDefinitions { get; init; }

    public IEnumerable<PropertyQueryDefinition> QueryParameterDefinitions { get; init; }

    public PropertyPathDefinition PathDefinition { get; init; }
}
