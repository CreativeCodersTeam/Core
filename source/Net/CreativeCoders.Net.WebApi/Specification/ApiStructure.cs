using System.Collections.Generic;

namespace CreativeCoders.Net.WebApi.Specification;

public class ApiStructure
{
    public IEnumerable<ApiMethodInfo> MethodInfos { get; set; }

    public IEnumerable<RequestHeader> HeaderDefinitions { get; set; }

    public IEnumerable<ApiPropertyInfo> PropertyInfos { get; set; }
}
