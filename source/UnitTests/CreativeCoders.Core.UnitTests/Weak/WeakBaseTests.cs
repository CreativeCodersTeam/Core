using System;
using CreativeCoders.Core.Weak;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Weak
{
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
            var weakAction = new WeakBaseCreator().CreateWeakBase();
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
            
            Assert.False(weakAction.IsAlive());
            Assert.Null(weakAction.GetData());
        }
    }
}