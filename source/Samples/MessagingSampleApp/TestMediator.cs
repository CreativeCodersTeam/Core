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

            var registration1 = _mediator.RegisterAsyncHandler<string>(this, WriteLine);
            var registration2 = _mediator.RegisterAsyncHandler<string>(this, WriteLineDouble);

            await _mediator.SendAsync("Test Async");
            
            _mediator.Send("Test Sync");
            
            registration1.Dispose();
            
            _mediator.Send("After registration1.Dispose");
            
            registration2.Dispose();

            await _mediator.SendAsync("Should not be written");
        }

        private static async Task WriteLine(string text)
        {
            await Task.Delay(1000);
            Console.WriteLine(text);
        }

        private static async Task WriteLineDouble(string text)
        {
            await Task.Delay(1000);
            Console.WriteLine($"{text} + {text}");
        }
    }
}