using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using CreativeCoders.Net.WebApi.Execution.Requests;
using CreativeCoders.Net.WebApi.Specification;

namespace CreativeCoders.Net.WebApi.Execution
{
    public class ApiMethodExecutor
    {
        private readonly ApiStructure _apiStructure;

        private readonly IList<IApiRequestHandler> _requestHandlers;

        public ApiMethodExecutor(ApiStructure apiStructure, HttpClient httpClient)
        {
            _apiStructure = apiStructure;

            _requestHandlers = new List<IApiRequestHandler>
            {
                new VoidApiRequestHandler(httpClient),
                new DataObjectApiRequestHandler(httpClient),
                new StringApiRequestHandler(httpClient),
                new ResponseApiRequestHandler(httpClient),
                new StreamApiRequestHandler(httpClient),
                new HttpResponseMessageApiRequestHandler(httpClient)
            };
        }

        public object Invoke(object target, ApiMethodInfo apiMethod, object[] arguments, ApiData apiData)
        {
            var requestData = new RequestDataCreator(target, _apiStructure, apiMethod, arguments, apiData).Create();

            return ExecuteRequest(requestData);
        }

        private object ExecuteRequest(RequestData requestData)
        {
            var requestHandler =
                _requestHandlers.FirstOrDefault(h => h.MethodReturnType == requestData.RequestReturnType);

            if (requestHandler == null)
            {
                throw new ArgumentOutOfRangeException(nameof(requestData.RequestReturnType), $"No request handler for {nameof(requestData.RequestReturnType)} found");
            }

            return requestHandler.SendRequest(requestData);
        }
    }
}