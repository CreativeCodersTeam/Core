using CreativeCoders.Core.Comparing;

namespace CreativeCoders.Core.UnitTests.Comparing.TestData
{
    public class ComparableIntInterfaceObject :
        ComparableObject<ComparableIntInterfaceObject, IComparableIntInterfaceObject>, IComparableIntInterfaceObject
    {
        static ComparableIntInterfaceObject()
        {
            InitComparableObject(x => x.IntValue);
        }

        public int IntValue { get; set; }
    }
}
