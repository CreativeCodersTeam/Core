using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CreativeCoders.Core.Collections;
using CreativeCoders.Messaging.Core;
using CreativeCoders.Messaging.DefaultMessageQueue;

namespace MessagingSampleApp;

public static class TestMessageQueue
{
    public static async Task Run()
    {
        var queue = new MessageQueue<string>() as IMessageQueue<string>;
            
        Enumerable
            .Range(0, 10)
            .ForEach(index => new Thread(async () => await ProcessQueueEntry(index, queue)).Start());

        await Task.Delay(2000);
            
        await Enumerable
            .Range(0, 10)
            .ForEachAsync(async index =>
            {
                var message = $"Test{index}";
                    
                Console.WriteLine($"Before message '{message}' write");
                    
                await queue.EnqueueAsync(message);
                    
                Console.WriteLine($"After message '{message}' written");
            });
    }

    private static async Task ProcessQueueEntry(int workerIndex, IMessageQueue<string> queue)
    {
        Console.WriteLine($"Start processing message. Worker index = {workerIndex}");
            
        await Task.Delay(1000);
            
        Console.WriteLine($"Dequeue message. Worker index = {workerIndex}");
            
        var data = await queue.DequeueAsync();
            
        Console.WriteLine($"Index = {workerIndex}, ThreadId = {Thread.CurrentThread.ManagedThreadId}: {data}");
    }
}