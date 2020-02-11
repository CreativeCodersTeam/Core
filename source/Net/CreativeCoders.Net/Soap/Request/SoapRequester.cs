using System;
using System.IO;
using System.Net;
using CreativeCoders.Core;
using CreativeCoders.Net.WebRequests;

namespace CreativeCoders.Net.Soap.Request {
    internal class SoapRequester
    {
        private readonly SoapRequestInfo _soapRequestInfo;

        private readonly Func<SoapRequestInfo, IHttpWebRequest> _createHttpWebRequest;

        public SoapRequester(SoapRequestInfo soapRequestInfo, Func<SoapRequestInfo, IHttpWebRequest> createHttpWebRequest)
        {
            Ensure.IsNotNull(soapRequestInfo, nameof(soapRequestInfo));
            Ensure.IsNotNull(createHttpWebRequest, nameof(createHttpWebRequest));

            _soapRequestInfo = soapRequestInfo;
            _createHttpWebRequest = createHttpWebRequest;
        }

        public WebResponse GetResponse()
        {
            var httpWebRequest = _createHttpWebRequest(_soapRequestInfo);

            using (var writer = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                new RequestXmlWriter(writer, _soapRequestInfo).Write();
            }

            return httpWebRequest.GetResponse();
        }
    }
}