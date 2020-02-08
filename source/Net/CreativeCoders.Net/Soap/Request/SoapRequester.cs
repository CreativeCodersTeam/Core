using System;
using System.IO;
using System.Net;
using CreativeCoders.Core;
using CreativeCoders.Net.WebRequests;

namespace CreativeCoders.Net.Soap.Request {
    internal class SoapRequester
    {
        private readonly SoapRequestInfo _soapRequestInfo;

        private readonly Func<SoapRequestInfo, IHttpWebRequest> _createHttpWebRequestFunc;

        public SoapRequester(SoapRequestInfo soapRequestInfo, Func<SoapRequestInfo, IHttpWebRequest> createHttpWebRequestFunc)
        {
            Ensure.IsNotNull(soapRequestInfo, nameof(soapRequestInfo));
            Ensure.IsNotNull(createHttpWebRequestFunc, nameof(createHttpWebRequestFunc));

            _soapRequestInfo = soapRequestInfo;
            _createHttpWebRequestFunc = createHttpWebRequestFunc;
        }

        public WebResponse GetResponse()
        {
            var httpWebRequest = _createHttpWebRequestFunc(_soapRequestInfo);

            using (var writer = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                new RequestXmlWriter(writer, _soapRequestInfo).Write();
            }

            return httpWebRequest.GetResponse();
        }
    }
}