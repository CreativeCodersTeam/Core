using CreativeCoders.Net.WebApi.Specification.Parameters;

namespace CreativeCoders.Net.WebApi.Specification;

public class ApiMethodArgumentInfo
{
    public ParameterHeaderDefinition HeaderDefinition { get; set; }

    public ParameterQueryDefinition QueryDefinition { get; set; }

    public ParameterPathDefinition PathDefinition { get; set; }
}
