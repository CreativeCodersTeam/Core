using System.Collections.Generic;
using CreativeCoders.Net.XmlRpc.Model;
using CreativeCoders.Net.XmlRpc.Model.Values;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Model.Values;

public class StructValueTests
{
    [Fact]
    public void Ctor_EmptyDictionary_ValueIsDictionary()
    {
        var dict = new Dictionary<string, XmlRpcValue>();

        var value = new StructValue(dict);

        Assert.Equal(dict, value.Value);
    }
}