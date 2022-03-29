using System.Collections.Generic;

namespace CreativeCoders.Net.XmlRpc.Proxy.Specification;

public class ApiStructure
{
    public IEnumerable<ApiMethodInfo> MethodInfos { get; set; }
}
