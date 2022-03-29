using System.Linq;
using CreativeCoders.Core.Dependencies;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Dependencies;

public class DependencyTreeBuilderTests
{
    [Fact]
    public void Build_ForVgaDriver_ReturnsTree()
    {
        var collection = DependencyTestData.CreateTestData();

        var treeBuilder = new DependencyTreeBuilder<string>(collection);

        var vgaDriverNode = treeBuilder.Build(DependencyTestData.VgaDriver);

        Assert.Equal(DependencyTestData.VgaDriver, vgaDriverNode.Element);

        Assert.Single(vgaDriverNode.SubNodes);

        var kernelNode = vgaDriverNode.SubNodes.First();

        Assert.Equal(DependencyTestData.Kernel, kernelNode.Element);

        Assert.Single(kernelNode.SubNodes);

        var cpuNode = kernelNode.SubNodes.First();

        Assert.Equal(DependencyTestData.Cpu, cpuNode.Element);

        Assert.Empty(cpuNode.SubNodes);
    }

    [Fact]
    public void Build_ForBrowser_ReturnsTree()
    {
        var collection = DependencyTestData.CreateTestDataForTree();

        var treeBuilder = new DependencyTreeBuilder<string>(collection);

        var browserNode = treeBuilder.Build(DependencyTestData.Browser);

        Assert.Equal(DependencyTestData.Browser, browserNode.Element);

        Assert.Equal(2, browserNode.SubNodes.Count);

        var appNode = browserNode.SubNodes.Single(x => x.Element == DependencyTestData.Application);

        Assert.Equal(DependencyTestData.Application, appNode.Element);

        var shellNode = appNode.SubNodes.Single(x => x.Element == DependencyTestData.Shell);

        var vgaDriverNode = shellNode.SubNodes.Single(x => x.Element == DependencyTestData.VgaDriver);

        Assert.Single(vgaDriverNode.SubNodes);

        var kernelNode = vgaDriverNode.SubNodes.First();

        Assert.Equal(DependencyTestData.Kernel, kernelNode.Element);

        Assert.Empty(kernelNode.SubNodes);


        var networkDriverNode =
            browserNode.SubNodes.Single(x => x.Element == DependencyTestData.NetworkDriver);

        var kernelNode2 = networkDriverNode.SubNodes.Single(x => x.Element == DependencyTestData.Kernel);

        Assert.Empty(kernelNode2.SubNodes);
    }
}
