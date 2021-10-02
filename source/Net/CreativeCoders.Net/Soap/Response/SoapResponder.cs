using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;

namespace CreativeCoders.Net.Soap.Response
{
    internal class SoapResponder<TResponse> where TResponse: class, new()
    {
        private readonly Stream _responseStream;

        private readonly SoapResponseInfo _responseInfo;

        public SoapResponder(Stream responseStream, SoapResponseInfo responseInfo)
        {
            Ensure.IsNotNull(responseStream, nameof(responseStream));
            Ensure.IsNotNull(responseInfo, nameof(responseInfo));

            _responseStream = responseStream;
            _responseInfo = responseInfo;
        }

        public TResponse Eval()
        {
            var responseData = new TResponse();

            var xmlDoc = XDocument.Load(_responseStream);
            MapXmlResponseToData(xmlDoc, responseData);

            return responseData;
        }

        private void MapXmlResponseToData(XContainer xmlDoc, TResponse responseData)
        {
            var bodyNode = xmlDoc.Elements(SoapConsts.EnvelopeName).Elements(SoapConsts.BodyName).FirstOrDefault();

            var contentElement = bodyNode?.Elements(XName.Get(_responseInfo.Name, _responseInfo.NameSpace))
                .FirstOrDefault();

            if (contentElement != null)
            {
                _responseInfo.PropertyMappings.ForEach(mapping => MapProperty(responseData, mapping, contentElement));
            }
        }

        private static void MapProperty(TResponse responseData, PropertyFieldMapping mapping, XContainer contentElement)
        {
            var fieldNode = contentElement.Elements(XName.Get(mapping.FieldName)).FirstOrDefault();
            if (fieldNode is { FirstNode: null })
            {
                var propValue = mapping.Property.PropertyType == typeof(string) ? string.Empty : null;
                mapping.Property.SetValue(responseData, propValue);
                return;
            }
            
            // ReSharper disable once InvertIf
            if (fieldNode is { FirstNode: { NodeType: XmlNodeType.Text } })
            {
                var fieldValue = (fieldNode.FirstNode as XText)?.Value;
                var propValue = Convert.ChangeType(fieldValue, mapping.Property.PropertyType);
                mapping.Property.SetValue(responseData, propValue);
            }
        }
    }
}
