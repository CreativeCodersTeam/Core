using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.Net.WebApi.Definition;
using CreativeCoders.Net.WebApi.Specification;
using CreativeCoders.Net.WebApi.Specification.Properties;

namespace CreativeCoders.Net.WebApi.Building;

public class ApiPropertyAnalyzer
{
    private readonly PropertyInfo _propertyInfo;

    public ApiPropertyAnalyzer(PropertyInfo propertyInfo)
    {
        _propertyInfo = propertyInfo;
    }

    public ApiPropertyInfo GetPropertyInfo()
    {
        var headerDefinitions = GetHeaderDefinitions().ToArray();
        var queryDefinitions = GetQueryDefinitions().ToArray();
        var pathDefinition = GetPathDefinition();

        var apiPropertyInfo = new ApiPropertyInfo
        {
            HeaderDefinitions = headerDefinitions,
            QueryParameterDefinitions = queryDefinitions,
            PathDefinition = pathDefinition
        };

        return apiPropertyInfo;
    }

    private PropertyPathDefinition GetPathDefinition()
    {
        var pathAttribute = _propertyInfo.GetCustomAttribute<PathAttribute>();

        return pathAttribute == null
            ? null
            : new PropertyPathDefinition(_propertyInfo, pathAttribute.UrlEncode, pathAttribute.Name);
    }

    private IEnumerable<PropertyQueryDefinition> GetQueryDefinitions()
    {
        return
            _propertyInfo
                .GetCustomAttributes<QueryAttribute>()
                .Select(a => new PropertyQueryDefinition(a.Name, a.UrlEncode, _propertyInfo));
    }

    private IEnumerable<PropertyHeaderDefinition> GetHeaderDefinitions()
    {
        return
            _propertyInfo
                .GetCustomAttributes<HeaderAttribute>()
                .Select(a => new PropertyHeaderDefinition(a.Name, _propertyInfo, a.SerializationKind));
    }
}
