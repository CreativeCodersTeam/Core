using System.Collections.Generic;

namespace CreativeCoders.Net.WebApi.Specification;

internal class ApiStructure
{
    public IEnumerable<ApiMethodInfo> MethodInfos { get; init; }

    public IEnumerable<RequestHeader> HeaderDefinitions { get; init; }

    public IEnumerable<ApiPropertyInfo> PropertyInfos { get; init; }
}
