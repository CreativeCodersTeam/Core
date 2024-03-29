﻿using CreativeCoders.Net.WebApi.Specification.Parameters;

namespace CreativeCoders.Net.WebApi.Specification;

internal class ApiMethodArgumentInfo
{
    public ParameterHeaderDefinition HeaderDefinition { get; init; }

    public ParameterQueryDefinition QueryDefinition { get; init; }

    public ParameterPathDefinition PathDefinition { get; init; }
}
