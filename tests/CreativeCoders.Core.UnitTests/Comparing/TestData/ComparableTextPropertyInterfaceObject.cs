using CreativeCoders.Core.Comparing;

namespace CreativeCoders.Core.UnitTests.Comparing.TestData;

public class ComparableTextPropertyInterfaceObject :
    ComparableObject<ComparableTextPropertyInterfaceObject, IComparableTextPropertyInterfaceObject>,
    IComparableTextPropertyInterfaceObject
{
    static ComparableTextPropertyInterfaceObject()
    {
        InitComparableObject(x => x.TextValue);
    }

    public string TextValue { get; set; }

    public override string ToString() => TextValue;
}
