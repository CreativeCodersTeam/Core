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
            var weakAction = new WeakActionCreator().CreateWeakAction();
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
            
            Assert.False(weakAction.IsAlive());
            Assert.Null(weakAction.GetData());
        }
    }
    
    public class WeakActionCreator
    {
        public WeakBase<Action> CreateWeakAction()
        {
            var writer = new ConsoleWriter();
            
            var weakAction = new WeakBase<Action>(this, () => writer.Write("Test"), KeepOwnerAliveMode.NotKeepAlive);
            return weakAction;
        }
    }

    public class ConsoleWriter
    {
        ~ConsoleWriter()
        {
            Console.WriteLine("Finalized");
        }
        
        public void Write(string text)
        {
            Console.WriteLine(text);
        }
    }
}