namespace CreativeCoders.Net.Soap.Response
{
    public class SoapResponseInfo
    {
        public SoapResponseInfo()
        {
            PropertyMappings = new PropertyFieldMapping[0];
        }

        public string Name { get; set; }

        public string NameSpace { get; set; }

        public PropertyFieldMapping[] PropertyMappings { get; set; }
    }
}