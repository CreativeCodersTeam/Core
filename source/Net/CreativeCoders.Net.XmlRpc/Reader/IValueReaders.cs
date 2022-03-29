using System.Xml.Linq;
using CreativeCoders.Net.XmlRpc.Model;

namespace CreativeCoders.Net.XmlRpc.Reader;

public interface IValueReaders
{
    XmlRpcValue ReadValue(XElement valueElement);

    IValueReader GetReader(string valueDataType);
}