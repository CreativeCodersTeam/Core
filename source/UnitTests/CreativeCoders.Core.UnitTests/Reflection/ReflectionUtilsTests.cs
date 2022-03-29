using System.Linq;
using CreativeCoders.Core.Reflection;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Reflection;

public class ReflectionUtilsTests
{
    [Fact]
    public void GetAllAssemblies_Call_ReturnsAssemblies()
    {
        var assemblies = ReflectionUtils.GetAllAssemblies();

        Assert.NotEmpty(assemblies);
    }

    [Fact]
    public void GetTypes_ForAllAssemblies_ReturnsTypes()
    {
        var types = ReflectionUtils.GetAllTypes();

        Assert.NotEmpty(types);
    }

    [Fact]
    public void GetTypes_ForAllNotDynamicAssemblies_ReturnsTypes()
    {
        var types = ReflectionUtils.GetAllTypes(a => !a.IsDynamic);

        Assert.NotEmpty(types);
    }

    [Fact]
    public void GetAllAssemblies_CallWithReflectionOnlyAssemblies_ReturnsAssemblies()
    {
        var assemblies = ReflectionUtils.GetAllAssemblies(true);

        Assert.NotEmpty(assemblies);
    }

    [Fact]
    public void GetAllAssemblies_CallWithReflectionOnlyAssemblies_EqualOrMoreAssembliesThanGetAllAssemblies()
    {
        var assembliesWithReflectionOnly = ReflectionUtils.GetAllAssemblies(true).ToArray();

        var assemblies = ReflectionUtils.GetAllAssemblies().ToArray();

        Assert.True(assembliesWithReflectionOnly.Length >= assemblies.Length);
    }

    [Fact]
    public void GetTypes_ForAllAssembliesWithReflectionOnlyAssemblies_ReturnsTypes()
    {
        var types = ReflectionUtils.GetAllTypes(true);

        Assert.NotEmpty(types);
    }

    [Fact]
    public void GetTypes_ForAllNotDynamicAssembliesWithReflectionOnlyAssemblies_ReturnsTypes()
    {
        var types = ReflectionUtils.GetAllTypes(a => !a.IsDynamic, true);

        Assert.NotEmpty(types);
    }
}