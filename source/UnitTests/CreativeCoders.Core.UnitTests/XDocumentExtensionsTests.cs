using System.Xml.Linq;
using Xunit;

namespace CreativeCoders.Core.UnitTests;

public class XDocumentExtensionsTests
{
    [Fact]
    public void FindElement_FindSubElement_ReturnsFoundNode()
    {
        var configNode = new XElement("Config", new XElement("Item"));

        var rootNode =
            new XElement("Test",
                configNode);
        var xmlDoc = new XDocument(rootNode);

        var foundNode = xmlDoc.Root.FindElement("Config");

        Assert.Same(configNode, foundNode);
    }

    [Fact]
    public void FindElement_FindSubSubElement_ReturnsFoundNode()
    {
        var itemNode = new XElement("Item");

        var rootNode =
            new XElement("Test",
                new XElement("Config",
                    itemNode));
        var xmlDoc = new XDocument(rootNode);

        var foundNode = xmlDoc.Root.FindElement("Config", "Item");

        Assert.Same(itemNode, foundNode);
    }

    [Fact]
    public void FindElement_FindNoneExistingSubSubElement_ReturnsNoNode()
    {
        var itemNode = new XElement("Item");

        var rootNode =
            new XElement("Test",
                new XElement("Config",
                    itemNode));
        var xmlDoc = new XDocument(rootNode);

        var foundNode = xmlDoc.Root.FindElement("Config", "Items");

        Assert.Null(foundNode);
    }
}
