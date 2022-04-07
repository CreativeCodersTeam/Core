using System.Xml.Linq;
using CreativeCoders.Net.XmlRpc.Model;

namespace CreativeCoders.Net.XmlRpc.Writer;

public interface IValueWriter
{
    void WriteTo(XElement paramNode, XmlRpcValue xmlRpcValue);
}
