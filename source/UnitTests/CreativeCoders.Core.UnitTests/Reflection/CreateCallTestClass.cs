using System.Diagnostics.CodeAnalysis;

namespace CreativeCoders.Core.UnitTests.Reflection;

[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
public class CreateCallTestClass
{
    public string GetData() => Data;
        
    public string GetData1(int data)
    {
        return data.ToString();
    }

    public string GetData2(int data1, int data2)
    {
        return (data1 + data2 * 10).ToString();
    }

    public string GetData3(int data1, int data2, int data3)
    {
        return (data1 + data2 * 10 + data3 * 100).ToString();
    }

    public string GetData4(int data1, int data2, int data3, int data4)
    {
        return (data1 + data2 * 10 + data3 * 100 + data4 * 1000).ToString();
    }

    public void SetData()
    {
        Data = "SetDataTest";
    }

    public void SetData1(int data)
    {
        Data = data.ToString();
    }

    public void SetData2(int data1, int data2)
    {
        Data = (data1 + data2 * 10).ToString();
    }

    public void SetData3(int data1, int data2, int data3)
    {
        Data = (data1 + data2 * 10 + data3 * 100).ToString();
    }

    public void SetData4(int data1, int data2, int data3, int data4)
    {
        Data = (data1 + data2 * 10 + data3 * 100 + data4 * 1000).ToString();
    }

    public string Data { get; set; }
}