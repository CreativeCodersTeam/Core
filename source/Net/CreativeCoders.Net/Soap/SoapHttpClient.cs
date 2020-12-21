using System.Linq;
using System.Net;
using System.Reflection;
using CreativeCoders.Core;
using CreativeCoders.Net.Soap.Exceptions;
using CreativeCoders.Net.Soap.Request;
using CreativeCoders.Net.Soap.Response;
using CreativeCoders.Net.WebRequests;

namespace CreativeCoders.Net.Soap
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class SoapHttpClient : ISoapHttpClient
    {
        private readonly IWebRequestFactory _webRequestFactory;

        public SoapHttpClient(IWebRequestFactory webRequestFactory)
        {
            Ensure.IsNotNull(webRequestFactory, nameof(webRequestFactory));

            _webRequestFactory = webRequestFactory;
        }

        public TResponse Invoke<TRequest, TResponse>(TRequest actionRequest) where TResponse: class, new()
        {
            Ensure.IsNotNull(actionRequest, nameof(actionRequest));

            var soapRequestInfo = CreateRequestInfo(actionRequest);

            var soapActionRequester = new SoapRequester(soapRequestInfo, CreateHttpWebRequest);

            using (var httpResponse = soapActionRequester.GetResponse())
            {
                using (var responseStream = httpResponse.GetResponseStream())
                {
                    var responseInfo = CreateResponseInfo<TResponse>();
                    var responseData = new SoapResponder<TResponse>(responseStream, responseInfo).Eval();
                    return responseData;
                }
            }
        }

        private static SoapResponseInfo CreateResponseInfo<TResponse>()
        {
            var responseType = typeof(TResponse);

            if (!(responseType.GetCustomAttributes(typeof(SoapResponseAttribute), false).FirstOrDefault() is
                SoapResponseAttribute soapResponseAttribute))
            {
                throw new SoapResponseAttributeNotFoundException(responseType);
            }

            var responseInfo = new SoapResponseInfo
            {
                Name = soapResponseAttribute.Name,
                NameSpace = soapResponseAttribute.NameSpace
            };

            var propertyMappings = (from property in responseType.GetProperties()
                let attribute = property.GetCustomAttribute<SoapResponseFieldAttribute>(false)
                where attribute != null
                select new PropertyFieldMapping {FieldName = attribute.Name, Property = property}).ToArray();

            responseInfo.PropertyMappings = propertyMappings;
            
            return responseInfo;
        }

        private SoapRequestInfo CreateRequestInfo<TRequest>(TRequest actionRequest)
        {
            var requestType = typeof(TRequest);

            if (!(requestType.GetCustomAttributes(typeof(SoapRequestAttribute), false)
                .FirstOrDefault() is SoapRequestAttribute soapRequestAttribute))
            {
                throw new SoapRequestAttributeNotFoundException(requestType);
            }

            var requestInfo = new SoapRequestInfo
            {
                Url = Url,
                Credentials = Credentials,
                Action = actionRequest,
                ActionName = soapRequestAttribute.Name,
                ServiceNameSpace = soapRequestAttribute.NameSpace
            };

            var propertyMappings = (from property in requestType.GetProperties()
                let attribute = property.GetCustomAttribute<SoapRequestFieldAttribute>(false)
                where attribute != null
                select new PropertyFieldMapping {FieldName = attribute.Name, Property = property}).ToArray();            

            requestInfo.PropertyMappings = propertyMappings;

            return requestInfo;
        }

        private IHttpWebRequest CreateHttpWebRequest(SoapRequestInfo soapRequestInfo)
        {
            var httpWebRequest = SoapHttpWebRequestCreator.Create(soapRequestInfo, _webRequestFactory);
            OnConfigureHttpWebRequest(httpWebRequest);
            return httpWebRequest;
        }

        protected virtual void OnConfigureHttpWebRequest(IHttpWebRequest httpWebRequest)
        {
            if (AllowUntrustedCertificates)
            {
                httpWebRequest.ServerCertificateValidationCallback = (_, _, _, _) => true;
            }
        }

        public ICredentials Credentials { get; set; }

        public string Url { get; set; }

        public bool AllowUntrustedCertificates { get; set; }
    }
}
