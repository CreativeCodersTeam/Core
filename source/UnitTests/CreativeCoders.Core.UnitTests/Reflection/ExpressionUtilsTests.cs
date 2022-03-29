using CreativeCoders.Core.Reflection;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Reflection;

public class ExpressionUtilsTests
{
    [Fact]
    public void CreateCallAction_CallWithoutParameter_MethodIsExecuted()
    {
        var instance = new CreateCallTestClass();
        var method = instance.GetType().GetMethod(nameof(CreateCallTestClass.SetData));

        var action = ExpressionUtils.CreateCallAction(instance, method);

        action();

        Assert.Equal("SetDataTest", instance.Data);
    }

    [Fact]
    public void CreateCallAction_CallOneParameter_MethodIsExecuted()
    {
        const int value = 12345;

        var instance = new CreateCallTestClass();
        var method = instance.GetType().GetMethod(nameof(CreateCallTestClass.SetData1));

        var action = ExpressionUtils.CreateCallAction<int>(instance, method);

        action(value);

        Assert.Equal(value.ToString(), instance.Data);
    }

    [Fact]
    public void CreateCallAction_CallTwoParameter_MethodIsExecuted()
    {
        const int value1 = 12345;
        const int value2 = 23456;

        var instance = new CreateCallTestClass();
        var method = instance.GetType().GetMethod(nameof(CreateCallTestClass.SetData2));

        var action = ExpressionUtils.CreateCallAction<int, int>(instance, method);

        action(value1, value2);

        Assert.Equal((value1 + value2 * 10).ToString(), instance.Data);
    }

    [Fact]
    public void CreateCallAction_CallThreeParameter_MethodIsExecuted()
    {
        const int value1 = 12345;
        const int value2 = 23456;
        const int value3 = 34567;

        var instance = new CreateCallTestClass();
        var method = instance.GetType().GetMethod(nameof(CreateCallTestClass.SetData3));

        var action = ExpressionUtils.CreateCallAction<int, int, int>(instance, method);

        action(value1, value2, value3);

        Assert.Equal((value1 + value2 * 10 + value3 * 100).ToString(), instance.Data);
    }

    [Fact]
    public void CreateCallAction_CallFourParameter_MethodIsExecuted()
    {
        const int value1 = 12345;
        const int value2 = 23456;
        const int value3 = 34567;
        const int value4 = 45678;

        var instance = new CreateCallTestClass();
        var method = instance.GetType().GetMethod(nameof(CreateCallTestClass.SetData4));

        var action = ExpressionUtils.CreateCallAction<int, int, int, int>(instance, method);

        action(value1, value2, value3, value4);

        Assert.Equal((value1 + value2 * 10 + value3 * 100 + value4 * 1000).ToString(), instance.Data);
    }

    [Fact]
    public void CreateCallFunc_CallWithoutParameter_ReturnsCorrectData()
    {
        var instance = new CreateCallTestClass
        {
            Data = "TestData"
        };
        var method = instance.GetType().GetMethod(nameof(CreateCallTestClass.GetData));

        var func = ExpressionUtils.CreateCallFunc<string>(instance, method);

        var result = func();

        Assert.Equal(instance.Data, result);
    }

    [Fact]
    public void CreateCallFunc_CallOneParameter_ReturnsCorrectData()
    {
        const int value = 12345;

        var instance = new CreateCallTestClass();
        var method = instance.GetType().GetMethod(nameof(CreateCallTestClass.GetData1));

        var func = ExpressionUtils.CreateCallFunc<int, string>(instance, method);

        var result = func(value);

        Assert.Equal(value.ToString(), result);
    }

    [Fact]
    public void CreateCallFunc_CallTwoParameter_ReturnsCorrectData()
    {
        const int value1 = 12345;
        const int value2 = 23456;

        var instance = new CreateCallTestClass();
        var method = instance.GetType().GetMethod(nameof(CreateCallTestClass.GetData2));

        var func = ExpressionUtils.CreateCallFunc<int, int, string>(instance, method);

        var result = func(value1, value2);

        Assert.Equal((value1 + value2 * 10).ToString(), result);
    }

    [Fact]
    public void CreateCallFunc_CallThreeParameter_ReturnsCorrectData()
    {
        const int value1 = 12345;
        const int value2 = 23456;
        const int value3 = 34567;

        var instance = new CreateCallTestClass();
        var method = instance.GetType().GetMethod(nameof(CreateCallTestClass.GetData3));

        var func = ExpressionUtils.CreateCallFunc<int, int, int, string>(instance, method);

        var result = func(value1, value2, value3);

        Assert.Equal((value1 + value2 * 10 + value3 * 100).ToString(), result);
    }

    [Fact]
    public void CreateCallFunc_CallFourParameter_ReturnsCorrectData()
    {
        const int value1 = 12345;
        const int value2 = 23456;
        const int value3 = 34567;
        const int value4 = 45678;

        var instance = new CreateCallTestClass();
        var method = instance.GetType().GetMethod(nameof(CreateCallTestClass.GetData4));

        var func = ExpressionUtils.CreateCallFunc<int, int, int, int, string>(instance, method);

        var result = func(value1, value2, value3, value4);

        Assert.Equal((value1 + value2 * 10 + value3 * 100 + value4 * 1000).ToString(), result);
    }
}