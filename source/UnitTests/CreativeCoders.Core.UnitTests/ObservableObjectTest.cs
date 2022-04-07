using System;
using System.ComponentModel;
using Xunit;

namespace CreativeCoders.Core.UnitTests;

public class ObservableObjectTest
{
    private string _propertyName;

    [Fact]
    public void OnPropertyChangedTest()
    {
        _propertyName = string.Empty;

        var obj = new TestClass();
        obj.PropertyChanged += ObjOnPropertyChanged;

        obj.IntValue = 10;

        Assert.True(_propertyName == "IntValue");
        Assert.True(obj.IntValue == 10);
    }

    [Fact]
    public void OnPropertyChangedMemberExpressionTest()
    {
        _propertyName = string.Empty;

        var obj = new TestClass();
        obj.PropertyChanged += ObjOnPropertyChanged;

        obj.FloatValue = 1.234;

        Assert.True(_propertyName == "FloatValue");
        Assert.True(Math.Abs(obj.FloatValue - 1.234) < 0.0001);
    }

    [Fact]
    public void OnSetNameTest()
    {
        _propertyName = string.Empty;

        var obj = new TestClass();
        obj.PropertyChanged += ObjOnPropertyChanged;

        obj.StrValue = "abcd";

        Assert.True(_propertyName == "StrValue");
        Assert.True(obj.StrValue == "abcd");
    }

    [Fact]
    public void OnSetNameSameValueTest()
    {
        _propertyName = string.Empty;

        var obj = new TestClass();
        obj.PropertyChanged += ObjOnPropertyChanged;

        obj.StrValue = "abcd";

        Assert.True(_propertyName == "StrValue");
        _propertyName = string.Empty;

        obj.StrValue = "abcd";
        Assert.True(_propertyName == string.Empty);
    }

    [Fact]
    public void OnSetMemberExpressionTest()
    {
        _propertyName = string.Empty;

        var obj = new TestClass();
        obj.PropertyChanged += ObjOnPropertyChanged;

        obj.BoolValue = true;

        Assert.True(_propertyName == "BoolValue");
        Assert.True(obj.BoolValue);
    }

    [Fact]
    public void Set_StrValue_NotifyPropertyChangingFired()
    {
        string valueOnChanging = null;
        var eventHandlerCalled = false;

        var obj = new TestClass();
        obj.PropertyChanging += (_, _) =>
        {
            valueOnChanging = obj.StrValue;
            eventHandlerCalled = true;
        };

        obj.StrValue = "Test";

        Assert.Null(valueOnChanging);
        Assert.Equal("Test", obj.StrValue);
        Assert.True(eventHandlerCalled);

        eventHandlerCalled = false;
        obj.StrValue = "1234";
        Assert.Equal("Test", valueOnChanging);
        Assert.Equal("1234", obj.StrValue);
        Assert.True(eventHandlerCalled);
    }

    private void ObjOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
    {
        _propertyName = propertyChangedEventArgs.PropertyName;
    }
}

internal class TestClass : ObservableObject
{
    private int _intValue;

    private string _strValue;

    private bool _boolValue;

    private double _floatValue;

    public int IntValue
    {
        get => _intValue;
        set
        {
            _intValue = value;
            OnPropertyChanged();
        }
    }

    public double FloatValue
    {
        get => _floatValue;
        set
        {
            _floatValue = value;
            OnPropertyChanged(() => FloatValue);
        }
    }

    public string StrValue
    {
        get => _strValue;
        set => Set(ref _strValue, value);
    }

    public bool BoolValue
    {
        get => _boolValue;
        set => Set(ref _boolValue, value, () => BoolValue);
    }
}
