using System.Threading.Tasks;
using CreativeCoders.Messaging.DefaultMediator;
using Xunit;

namespace CreativeCoders.Messaging.UnitTests
{
    public class MediatorTests
    {
        [Fact]
        public void RegisterHandler_NoSend_HandlerNotCalled()
        {
            var handlerCalled = false;
            
            var mediator = new Mediator();

            mediator.RegisterHandler<string>(this, msg => handlerCalled = true);
            
            Assert.False(handlerCalled);
        }
        
        [Fact]
        public async Task RegisterHandler_Send_HandlerCalled()
        {
            var handlerCalled = false;
            
            var mediator = new Mediator();

            mediator.RegisterHandler<string>(this, msg => handlerCalled = true);

            await mediator.SendAsync("test");
            
            Assert.True(handlerCalled);
        }
        
        [Fact]
        public async Task RegisterHandler_Send_HandlerCalledWithCorrectMessage()
        {
            var messageValue = string.Empty;
            
            var mediator = new Mediator();

            mediator.RegisterHandler<string>(this, msg => messageValue = msg);

            await mediator.SendAsync("test");
            
            Assert.Equal("test", messageValue);
        }
    }
}