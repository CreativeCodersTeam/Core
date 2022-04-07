using CreativeCoders.Core.Comparing;

namespace CreativeCoders.Core.UnitTests.Comparing.TestData;

public class ComparableStringInterfaceObject :
    ComparableObject<ComparableStringInterfaceObject, IComparableStringInterfaceObject>,
    IComparableStringInterfaceObject
{
    public string TextValue { get; set; }

    public override string ToString() => TextValue;
}
