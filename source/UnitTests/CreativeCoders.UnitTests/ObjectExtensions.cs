using CreativeCoders.Core;
using FluentAssertions;
using JetBrains.Annotations;

namespace CreativeCoders.UnitTests;

[PublicAPI]
public static class ObjectExtensions
{
    public static bool IsEquivalentTo(this object instance1, object instance2)
    {
        Ensure.NotNull(instance1);
        Ensure.NotNull(instance2);

        try
        {
            instance1.Should().BeEquivalentTo(instance2);

            return true;
        }
        catch (FluentAssertions.Execution.AssertionFailedException)
        {
            return false;
        }
    }
}
