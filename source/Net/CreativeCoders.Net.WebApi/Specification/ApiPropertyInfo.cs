using System.Collections.Generic;
using CreativeCoders.Net.WebApi.Specification.Properties;

namespace CreativeCoders.Net.WebApi.Specification;

public class ApiPropertyInfo
{
    public IEnumerable<PropertyHeaderDefinition> HeaderDefinitions { get; set; }

    public IEnumerable<PropertyQueryDefinition> QueryParameterDefinitions { get; set; }

    public PropertyPathDefinition PathDefinition { get; set; }
}