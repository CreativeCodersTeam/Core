using JetBrains.Annotations;

#nullable enable
namespace CreativeCoders.Core.UnitTests.Reflection.TestData
{
    public interface ITestSimpleClassOptions
    {
        [UsedImplicitly]
        string? Value { get; }
    }
}
