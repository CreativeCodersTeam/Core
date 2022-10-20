using JetBrains.Annotations;

namespace CreativeCoders.Core.UnitTests.Comparing.TestData;

[PublicAPI]
public interface IComparableTextPropertyInterfaceObject
{
    string TextValue { get; set; }

    string ToString();
}
