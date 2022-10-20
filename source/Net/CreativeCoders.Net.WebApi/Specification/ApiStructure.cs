using System.Collections.Generic;

namespace CreativeCoders.Net.WebApi.Specification;

public class ApiStructure
{
    public IEnumerable<ApiMethodInfo> MethodInfos { get; init; }

    public IEnumerable<RequestHeader> HeaderDefinitions { get; init; }

    public IEnumerable<ApiPropertyInfo> PropertyInfos { get; init; }
}
