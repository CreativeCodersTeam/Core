using System;
using CreativeCoders.Core.Weak;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Weak;

public class WeakBaseTests
{
    [Fact]
    public void GetData_DataIsString_ReturnsTestString()
    {
        const string data = "TestData";
            
        var weakData = new WeakBase<string>(null, data, KeepOwnerAliveMode.NotKeepAlive);
            
        Assert.Equal(data, weakData.GetData());
    }
        
    [Fact]
    public void GetData_DataIsStringReleased_ReturnsTestString()
    {
        var weakAction = CreateWeakAction();
            
        GC.Collect();
        GC.WaitForPendingFinalizers();
            
        Assert.False(weakAction.IsAlive());
        Assert.Null(weakAction.GetData());
    }

    [Fact]
    public void GetData_NoOwnerDataIsStringReleased_ReturnsTestString()
    {
        var weakAction = CreateWeakActionWithoutOwner();
            
        GC.Collect();
        GC.WaitForPendingFinalizers();
            
        Assert.False(weakAction.IsAlive());
        Assert.Null(weakAction.GetData());
    }

    private static WeakBase<Action> CreateWeakAction()
    {
        return new WeakBaseCreator().CreateWeakBase();
    }
        
    private static WeakBase<Action> CreateWeakActionWithoutOwner()
    {
        return new WeakBaseCreator().CreateWeakBaseWithoutOwner();
    }
}