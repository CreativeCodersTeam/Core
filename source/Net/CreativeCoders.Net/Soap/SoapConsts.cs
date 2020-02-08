using System.Xml.Linq;

namespace CreativeCoders.Net.Soap
{
    public static class SoapConsts
    {
        public static readonly XNamespace SoapNameSpace = "http://schemas.xmlsoap.org/soap/envelope/";

        public static readonly XName EnvelopeName = SoapNameSpace + "Envelope";

        public static readonly XName BodyName = SoapNameSpace + "Body";

        public static readonly XName EncodingStyleName = SoapNameSpace + "encodingStyle";
    }
}