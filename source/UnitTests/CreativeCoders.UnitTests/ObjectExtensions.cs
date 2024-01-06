using FluentAssertions;

namespace CreativeCoders.UnitTests;

public static class ObjectExtensions
{
    public static bool IsEquivalentTo(this object instance1, object instance2)
    {
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
