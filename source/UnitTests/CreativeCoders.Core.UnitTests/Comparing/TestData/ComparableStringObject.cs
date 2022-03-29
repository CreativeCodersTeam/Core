using CreativeCoders.Core.Comparing;

namespace CreativeCoders.Core.UnitTests.Comparing.TestData;

public class ComparableStringObject : ComparableObject<ComparableStringObject>
{
    public string TextValue { get; set; }

    public override string ToString() => TextValue;
}