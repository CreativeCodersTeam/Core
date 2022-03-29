using System.Xml.Linq;
using CreativeCoders.Net.XmlRpc.Model;

namespace CreativeCoders.Net.XmlRpc.Reader;

public interface IValueReader
{
    XmlRpcValue ReadValue(XElement valueElement);
}