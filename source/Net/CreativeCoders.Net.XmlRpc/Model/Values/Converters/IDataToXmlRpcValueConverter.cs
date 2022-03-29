using System.Text;

namespace CreativeCoders.Net.XmlRpc.Model.Values.Converters;

public interface IDataToXmlRpcValueConverter
{
    XmlRpcValue Convert(object data);

    Encoding XmlEncoding { get; set; }
}
