using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using CreativeCoders.Net.WebApi.Definition;
using CreativeCoders.Net.WebApi.Exceptions;
using CreativeCoders.Net.WebApi.Specification;
using CreativeCoders.Net.WebApi.Specification.Parameters;

namespace CreativeCoders.Net.WebApi.Building
{
    public class ApiCallArgumentsAnalyzer
    {
        private readonly ApiMethodInfo _apiMethodInfo;

        public ApiCallArgumentsAnalyzer(ApiMethodInfo apiMethodInfo)
        {
            _apiMethodInfo = apiMethodInfo;
        }

        public IEnumerable<ApiMethodArgumentInfo> Analyze(MethodInfo method)
        {
            return
                method
                    .GetParameters()
                    .Select(AnalyzeArgument);
        }

        private ApiMethodArgumentInfo AnalyzeArgument(ParameterInfo parameterInfo)
        {
            SetBodyDefinition(parameterInfo);
            SetCancellationTokenDefinition(parameterInfo);
            SetCompletionOption(parameterInfo);

            var attributes = parameterInfo.GetCustomAttributes().ToArray();

            var headerDefinition = GetHeaderDefinition(parameterInfo, attributes);
            var queryDefinition = GetQueryDefinition(parameterInfo, attributes);
            var canBePath = headerDefinition == null && queryDefinition == null;
            var pathDefinition = canBePath
                ? GetPathDefinition(parameterInfo)
                : null;

            var argumentInfo = new ApiMethodArgumentInfo
            {
                HeaderDefinition = headerDefinition,
                QueryDefinition = queryDefinition,
                PathDefinition = pathDefinition
            };

            return argumentInfo;
        }

        private void SetCompletionOption(ParameterInfo parameterInfo)
        {
            if (parameterInfo.ParameterType != typeof(HttpCompletionOption))
            {
                return;
            }

            if (_apiMethodInfo.CompletionOption != null)
            {
                throw new ParameterDefinitionException(_apiMethodInfo.Method, parameterInfo);
            }

            _apiMethodInfo.CompletionOption = new ParameterCompletionOptionDefinition(parameterInfo);
        }

        private void SetCancellationTokenDefinition(ParameterInfo parameterInfo)
        {
            if (parameterInfo.ParameterType != typeof(CancellationToken))
            {
                return;
            }

            if (_apiMethodInfo.CancellationToken != null)
            {
                throw new ParameterDefinitionException(_apiMethodInfo.Method, parameterInfo);
            }

            _apiMethodInfo.CancellationToken = new ParameterCancellationTokenDefinition(parameterInfo);
        }

        private void SetBodyDefinition(ParameterInfo parameterInfo)
        {
            if (typeof(HttpContent).IsAssignableFrom(parameterInfo.ParameterType))
            {
                SetBody(new ParameterBodyDefinition(parameterInfo, null), parameterInfo);
                return;
            }

            var bodyAttribute = parameterInfo.GetCustomAttribute<ViaBodyAttribute>();

            if (bodyAttribute == null)
            {
                return;
            }

            var bodyDefinition = new ParameterBodyDefinition(parameterInfo, bodyAttribute.DataFormatterType);

            SetBody(bodyDefinition, parameterInfo);
        }

        private void SetBody(ParameterBodyDefinition bodyDefinition, ParameterInfo parameterInfo)
        {
            if (_apiMethodInfo.Body != null && bodyDefinition != null)
            {
                throw new ParameterDefinitionException(_apiMethodInfo.Method, parameterInfo);
            }

            _apiMethodInfo.Body = bodyDefinition;
        }

        private static ParameterPathDefinition GetPathDefinition(ParameterInfo parameterInfo)
        {
            var pathAttribute = parameterInfo.GetCustomAttribute<PathAttribute>();

            return pathAttribute == null
                ? new ParameterPathDefinition(parameterInfo, parameterInfo.Name, false)
                : new ParameterPathDefinition(parameterInfo, pathAttribute.Name, pathAttribute.UrlEncode);
        }

        [SuppressMessage("ReSharper", "ParameterTypeCanBeEnumerable.Local")]
        private static ParameterQueryDefinition GetQueryDefinition(ParameterInfo parameterInfo,
            Attribute[] attributes)
        {
            var queryAttribute = GetSingleAttribute<QueryAttribute>(parameterInfo, attributes, "query",
                typeof(HeaderAttribute), typeof(PathAttribute));

            return queryAttribute == null
                ? null
                : new ParameterQueryDefinition(parameterInfo, queryAttribute.Name, queryAttribute.UrlEncode);
        }

        [SuppressMessage("ReSharper", "ParameterTypeCanBeEnumerable.Local")]
        private static ParameterHeaderDefinition GetHeaderDefinition(ParameterInfo parameterInfo, Attribute[] attributes)
        {
            var headerAttribute = GetSingleAttribute<HeaderAttribute>(parameterInfo, attributes, "header",
                typeof(QueryAttribute), typeof(PathAttribute));

            return headerAttribute == null
                ? null
                : new ParameterHeaderDefinition(parameterInfo, headerAttribute.Name);
        }

        [SuppressMessage("ReSharper", "ParameterTypeCanBeEnumerable.Local")]
        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private static T GetSingleAttribute<T>(ParameterInfo parameterInfo, Attribute[] attributes, string attributeName,
            params Type[] notAllowedTypes)
            where T : Attribute
        {
            var requestedAttributes = attributes.Where(a => a.GetType() == typeof(T)).Cast<T>().ToArray();

            if (requestedAttributes.Length == 1 && attributes.Any(a => notAllowedTypes.Contains(a.GetType())))
            {
                throw new IllegalParameterAttributeException(parameterInfo, "Not allowed types mixed.");
            }

            if (requestedAttributes.Length > 1)
            {
                throw new IllegalParameterAttributeException(parameterInfo,
                    $"Parameter '{parameterInfo.Name}' has multiple '{attributeName}' attributes. Allowed is a maximum of one '{attributeName}' attribute.");
            }

            return requestedAttributes.FirstOrDefault();
        }
    }
}