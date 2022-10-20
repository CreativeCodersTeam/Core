using CreativeCoders.Core.Comparing;

namespace CreativeCoders.Core.UnitTests.Comparing.TestData;

public class ComparableTextPropertyObject : ComparableObject<ComparableTextPropertyObject>
{
    static ComparableTextPropertyObject()
    {
        InitComparableObject(x => x.TextValue);
    }

    public string TextValue { get; init; }

    public override string ToString() => TextValue;
}
