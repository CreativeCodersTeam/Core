using CreativeCoders.Core.Comparing;

namespace CreativeCoders.Core.UnitTests.Comparing.TestData;

public class ComparableStringObject : ComparableObject<ComparableStringObject>
{
    public string TextValue { get; init; }

    public override string ToString() => TextValue;
}
