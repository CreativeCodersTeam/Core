using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using CreativeCoders.Core.Collections;
using CreativeCoders.Net.WebApi.Specification;

namespace CreativeCoders.Net.WebApi.Execution;

internal class RequestDataCreator
{
    private readonly object _target;

    private readonly ApiStructure _apiStructure;

    private readonly ApiMethodInfo _apiMethod;

    private readonly object[] _arguments;

    private readonly ApiData _apiData;

    public RequestDataCreator(object target, ApiStructure apiStructure, ApiMethodInfo apiMethod,
        object[] arguments, ApiData apiData)
    {
        _target = target;
        _apiStructure = apiStructure;
        _apiMethod = apiMethod;
        _arguments = arguments;
        _apiData = apiData;
    }

    public RequestData Create()
    {
        return new RequestData
        {
            Headers = CreateHeaders(),
            RequestUri = BuildRequestUri(),
            RequestMethod = _apiMethod.RequestMethod,
            RequestReturnType = _apiMethod.ReturnType,
            DataObjectType = GetDataObjectType(),
            DefaultDataFormatter = _apiData.DefaultDataFormatter,
            GetBodyValue = GetBodyValueFunction(),
            CompletionOption = GetCompletionOption(),
            CancellationToken = GetCancellationToken()
        };
    }

    private CancellationToken GetCancellationToken()
    {
        return
            _apiMethod.CancellationToken?.GetValue(_arguments)
            ?? CancellationToken.None;
    }

    private HttpCompletionOption GetCompletionOption()
    {
        return
            _apiMethod.CompletionOption?.GetValue(_arguments)
            ?? GetDefaultCompletionOption(_apiMethod.ReturnType);
    }

    private static HttpCompletionOption GetDefaultCompletionOption(ApiMethodReturnType apiMethodReturnType)
    {
        return apiMethodReturnType switch
        {
            ApiMethodReturnType.Void => HttpCompletionOption.ResponseHeadersRead,
            ApiMethodReturnType.HttpResponseMessage => HttpCompletionOption.ResponseHeadersRead,
            ApiMethodReturnType.String => HttpCompletionOption.ResponseContentRead,
            ApiMethodReturnType.Stream => HttpCompletionOption.ResponseHeadersRead,
            ApiMethodReturnType.DataObject => HttpCompletionOption.ResponseContentRead,
            ApiMethodReturnType.Response => HttpCompletionOption.ResponseHeadersRead,
            _ => throw new ArgumentOutOfRangeException(nameof(apiMethodReturnType), apiMethodReturnType, null)
        };
    }

    private Func<object> GetBodyValueFunction()
    {
        var body = _apiMethod.Body;

        if (body == null)
        {
            return () => null;
        }

        return () => body.GetValue(_arguments);
    }

    private Type GetDataObjectType()
    {
        switch (_apiMethod.ReturnType)
        {
            case ApiMethodReturnType.DataObject:
                return _apiMethod.Method.ReturnType.GetGenericArguments().First();
            case ApiMethodReturnType.Response:
                var responseType = _apiMethod.Method.ReturnType.GetGenericArguments().First();
                return responseType.GetGenericArguments().First();
            case ApiMethodReturnType.Void:
            case ApiMethodReturnType.HttpResponseMessage:
            case ApiMethodReturnType.String:
            case ApiMethodReturnType.Stream:
                return null;
            default:
                return null;
        }
    }

    private Uri BuildRequestUri()
    {
        var queryParams = CreateQueryStrings();

        var requestUri = new Uri(new Uri(_apiData.BaseUri), GetMethodPathUri()).ToString();

        var queryStrings = string.Join("&", queryParams.Select(q => q.Key + "=" + q.Value));

        var uriBuilder = new UriBuilder(requestUri);

        if (string.IsNullOrWhiteSpace(queryStrings))
        {
            return uriBuilder.Uri;
        }

        if (uriBuilder.Query.Length > 1)
        {
            uriBuilder.Query += "&" + queryStrings;
        }
        else
        {
            uriBuilder.Query = queryStrings;
        }

        return uriBuilder.Uri;
    }

    private string GetMethodPathUri()
    {
        var pathVars = CreatePathVars();
        var requestUri = _apiMethod.MethodUri;

        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var pathVariable in pathVars)
        {
            requestUri = requestUri.Replace($"{{{pathVariable.Name}}}", pathVariable.Value);
        }

        return requestUri;
    }

    private IEnumerable<QueryDefinition> CreateQueryStrings()
    {
        var queries = _apiStructure.PropertyInfos
            .SelectMany(p => p.QueryParameterDefinitions)
            .Select(q => new QueryDefinition(q.Name, q.GetValue(_target)))
            .ToList();

        queries.AddRange(
            _apiMethod.ArgumentInfos
                .Select(a => a.QueryDefinition)
                .WhereNotNull()
                .Select(q => new QueryDefinition(q.Name, q.GetValue(_arguments))));

        return queries;
    }

    private IEnumerable<RequestHeader> CreateHeaders()
    {
        var headers = _apiStructure.HeaderDefinitions.ToList();
        headers.AddRange(
            _apiStructure.PropertyInfos
                .SelectMany(p => p.HeaderDefinitions)
                .Select(h => new RequestHeader(h.Name, h.GetValue(_target))));

        headers.AddRange(_apiMethod.HeaderDefinitions);

        headers.AddRange(
            _apiMethod.ArgumentInfos
                .Select(a => a.HeaderDefinition)
                .WhereNotNull()
                .Select(h => new RequestHeader(h.Name, h.GetValue(_arguments))));

        return headers;
    }

    private IEnumerable<PathVariable> CreatePathVars()
    {
        var pathVars = _apiStructure.PropertyInfos
            .Select(p => p.PathDefinition)
            .WhereNotNull()
            .Select(p => new PathVariable(p.Name, p.GetValue(_target)))
            .ToList();

        pathVars.AddRange(_apiMethod.ArgumentInfos
            .Select(a => a.PathDefinition)
            .WhereNotNull()
            .Select(p => new PathVariable(p.Name, p.GetValue(_arguments))));

        return pathVars;
    }
}
