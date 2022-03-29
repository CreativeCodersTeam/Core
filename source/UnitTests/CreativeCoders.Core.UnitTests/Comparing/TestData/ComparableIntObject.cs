using CreativeCoders.Core.Comparing;

namespace CreativeCoders.Core.UnitTests.Comparing.TestData;

public class ComparableIntObject : ComparableObject<ComparableIntObject>
{
    static ComparableIntObject()
    {
        InitComparableObject(x => x.IntValue);
    }

    public int IntValue { get; set; }
}