using System.Collections;
using System.Collections.Generic;

namespace CreativeCoders.Core.UnitTests.Validation;

public class TestEnumerableImpl : IEnumerable
{
    public IEnumerator GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    public readonly IList<string> Items = new List<string>();
}