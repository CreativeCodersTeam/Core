using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.Net.WebApi.Definition;
using CreativeCoders.Net.WebApi.Specification;

namespace CreativeCoders.Net.WebApi.Building
{
    public class ApiAnalyzer<T>
        where T : class
    {
        private readonly Type _apiType;

        public ApiAnalyzer()
        {
            _apiType = typeof(T);
        }

        public ApiStructure Analyze()
        {
            var methodInfos = AnalyzeMethods().ToArray();
            var headerDefinitions = GetApiHeaders().ToArray();
            var propertyInfos = AnalyzeProperties().ToArray();

            var apiStructure = new ApiStructure
            {
                MethodInfos = methodInfos,
                HeaderDefinitions = headerDefinitions,
                PropertyInfos = propertyInfos
            };

            return apiStructure;
        }        

        private IEnumerable<ApiPropertyInfo> AnalyzeProperties()
        {
            return
                _apiType
                    .GetProperties()
                    .Select(p => new ApiPropertyAnalyzer(p).GetPropertyInfo());
        }

        private IEnumerable<ApiMethodInfo> AnalyzeMethods()
        {
            return
                from methodInfo in _apiType.GetMethods()
                let apiMethodAttribute = methodInfo.GetCustomAttribute<ApiMethodBaseAttribute>()
                where apiMethodAttribute != null
                select new ApiMethodAnalyzer(methodInfo, apiMethodAttribute).GetMethodInfo();
        }

        private IEnumerable<RequestHeader> GetApiHeaders()
        {
            return
                _apiType
                    .GetCustomAttributes<HeaderAttribute>()
                    .Select(a => new RequestHeader(a.Name, a.Value));
        }
    }
}