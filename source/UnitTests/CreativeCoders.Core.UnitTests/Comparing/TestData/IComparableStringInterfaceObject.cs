using JetBrains.Annotations;

namespace CreativeCoders.Core.UnitTests.Comparing.TestData;

[PublicAPI]
public interface IComparableStringInterfaceObject
{
    string TextValue { get; set; }

    string ToString();
}
