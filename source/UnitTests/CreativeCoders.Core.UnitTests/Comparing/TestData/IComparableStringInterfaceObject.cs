using JetBrains.Annotations;

namespace CreativeCoders.Core.UnitTests.Comparing.TestData;

public interface IComparableStringInterfaceObject
{
    [UsedImplicitly]
    string TextValue { get; set; }

    string ToString();
}