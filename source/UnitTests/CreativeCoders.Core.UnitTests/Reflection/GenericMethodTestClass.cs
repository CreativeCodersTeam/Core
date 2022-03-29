using System.Linq;
using JetBrains.Annotations;

namespace CreativeCoders.Core.UnitTests.Reflection;

[PublicAPI]
public class GenericMethodTestClass
{
    public void DoSomething<T>(T data)
    {
        Data = data.ToString();
    }

    public T GetData<T>(T data)
    {
        return data;
    }

    public string GetDataText<T>(T data)
    {
        return data.ToString();
    }

    public string GetData<T1, T2>(T1 data1, T2 data2)
    {
        return data1.ToString() + data2;
    }

    public string GetData3<T1, T2, T3>(T1 data1, T2 data2, T3 data3)
    {
        return data1.ToString() + data2 + data3;
    }

    public string GetData4<T1, T2, T3, T4>(T1 data1, T2 data2, T3 data3, T4 data4)
    {
        return data1.ToString() + data2 + data3 + data4;
    }

    public void SetDataWithParam<T>(params string[] lines)
    {
        Data = typeof(T).Name + string.Join(string.Empty, lines);
    }

    public void SetDataWithParam1<T>(string s, params string[] lines)
    {
        Data = typeof(T).Name + s + string.Join(string.Empty, lines);
    }

    public void SetDataWithParam2<T>(string s1, string s2, params string[] lines)
    {
        Data = typeof(T).Name + s1 + s2 + string.Join(string.Empty, lines);
    }

    public void SetDataWithParam3<T>(string s1, params object[] objects)
    {
        Data = typeof(T).Name + s1 + string.Join(string.Empty, objects.Select(o => o.ToString()));
    }

    public string Data { get; set; }
}

public class TestDataClassBase
{
    public override string ToString() => "TestDataClassBase";
}

public class TestDataClassSpecial : TestDataClassBase
{
    public override string ToString() => "TestDataClassSpecial";
}
