using System;
using System.Threading.Tasks;
using CreativeCoders.Messaging.Core;
using CreativeCoders.Messaging.DefaultMediator;

namespace MessagingSampleApp
{
    public class TestMediator
    {
        private IMediator _mediator;
        
        public async Task Run()
        {
            _mediator = new Mediator();

            var registration1 = _mediator.RegisterAsyncHandler<string>(this, msg => WriteLine("registration1", msg));
            var registration2 = _mediator.RegisterAsyncHandler<string>(this, msg => WriteLine("registration2", msg));
            var registration3 = _mediator.RegisterAsyncHandler<string>(this, msg => WriteLineDouble("registration3", msg));

            await _mediator.SendAsync("Test 1");
            
            await _mediator.SendAsync("Test 2");
            
            registration1.Dispose();
            
            await _mediator.SendAsync("After registration1.Dispose");
            
            registration2.Dispose();
            
            await _mediator.SendAsync("After registration2.Dispose");
            
            registration3.Dispose();

            await _mediator.SendAsync("Should not be written");
        }

        private static async Task WriteLine(string prefix, string text)
        {
            await Task.Delay(500);
            Console.WriteLine($"{prefix}: {text}");
        }

        private static async Task WriteLineDouble(string prefix, string text)
        {
            await Task.Delay(500);
            Console.WriteLine($"{prefix}: {text} {text}");
        }
    }
}