using System.Linq;
using System.Xml.Linq;

namespace CreativeCoders.Core;

public static class XDocumentExtensions
{
    public static XElement FindElement(this XElement rootElement, params string[] subNodesPath)
    {
        var currentElement = rootElement;
        foreach (var nodeName in subNodesPath)
        {
            currentElement = currentElement.Elements(nodeName).FirstOrDefault();
            if (currentElement == null)
            {
                return null;
            }
        }

        return currentElement;
    }
}