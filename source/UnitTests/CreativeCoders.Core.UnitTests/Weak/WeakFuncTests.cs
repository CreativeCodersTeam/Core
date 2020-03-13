using CreativeCoders.Core.Weak;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Weak
{
    public class WeakFuncTests
    {
        [Fact]
        public void Execute_CtorIntFunction_ExecuteReturnsInt()
        {
            const int value = 1234;
            
            var weakFunction = new WeakFunc<int>(() => value);
            
            Assert.Equal(value, weakFunction.Execute());
        }
        
        [Fact]
        public void Execute_CtorIntFunctionKeepAliveMode_ExecuteReturnsInt()
        {
            const int value = 1234;
            
            var weakFunction = new WeakFunc<int>(() => value, KeepOwnerAliveMode.KeepAlive);
            
            Assert.Equal(value, weakFunction.Execute());
        }
        
        [Fact]
        public void Execute_CtorTargetAndIntFunction_ExecuteReturnsInt()
        {
            const int value = 1234;
            
            var weakFunction = new WeakFunc<int>(this, () => value);
            
            Assert.Equal(value, weakFunction.Execute());
        }
        
        [Fact]
        public void Execute_CtorTargetAndIntFunctionAndKeepAliveMode_ExecuteReturnsInt()
        {
            const int value = 1234;
            
            var weakFunction = new WeakFunc<int>(this, () => value, KeepOwnerAliveMode.KeepAlive);
            
            Assert.Equal(value, weakFunction.Execute());
        }
        
        [Fact]
        public void ExecuteWithParameter_CtorIntFunction_ExecuteReturnsInt()
        {
            const int value = 1234;
            const int parameter = 2345;
            
            var weakFunction = new WeakFunc<int, int>(intValue => value + intValue);
            
            Assert.Equal(value + parameter, weakFunction.Execute(parameter));
        }
        
        [Fact]
        public void ExecuteWithParameter_CtorIntFunctionKeepAliveMode_ExecuteReturnsInt()
        {
            const int value = 1234;
            const int parameter = 2345;
            
            var weakFunction = new WeakFunc<int, int>(intValue => value + intValue, KeepOwnerAliveMode.KeepAlive);
            
            Assert.Equal(value + parameter, weakFunction.Execute(parameter));
        }
        
        [Fact]
        public void ExecuteWithParameter_CtorTargetAndIntFunction_ExecuteReturnsInt()
        {
            const int value = 1234;
            const int parameter = 2345;
            
            var weakFunction = new WeakFunc<int, int>(this, intValue => value + intValue);
            
            Assert.Equal(value + parameter, weakFunction.Execute(parameter));
        }
        
        [Fact]
        public void ExecuteWithParameter_CtorTargetAndIntFunctionAndKeepAliveMode_ExecuteReturnsInt()
        {
            const int value = 1234;
            const int parameter = 2345;
            
            var weakFunction = new WeakFunc<int, int>(this, intValue => value + intValue, KeepOwnerAliveMode.KeepAlive);
            
            Assert.Equal(value + parameter, weakFunction.Execute(parameter));
        }
    }
}