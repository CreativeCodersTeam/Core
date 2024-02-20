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
    public static async Task RunAsync()
    {
        var queue = new MessageQueue<string>() as IMessageQueue<string>;

        Enumerable
            .Range(0, 10)
            .ForEach(index =>
            {
                new Thread(ExecuteAsync).Start();
                return;
                async void ExecuteAsync() => await ProcessQueueEntry(index, queue).ConfigureAwait(false);
            });

        await Task.Delay(2000).ConfigureAwait(false);

        await Enumerable
            .Range(0, 10)
            .ForEachAsync(async index =>
            {
                var message = $"Test{index}";

                Console.WriteLine($"Before message '{message}' write");

                await queue.EnqueueAsync(message).ConfigureAwait(false);

                Console.WriteLine($"After message '{message}' written");
            }).ConfigureAwait(false);
    }

    private static async Task ProcessQueueEntry(int workerIndex, IMessageQueue<string> queue)
    {
        Console.WriteLine($"Start processing message. Worker index = {workerIndex}");

        await Task.Delay(1000).ConfigureAwait(false);

        Console.WriteLine($"Dequeue message. Worker index = {workerIndex}");

        var data = await queue.DequeueAsync().ConfigureAwait(false);

        Console.WriteLine($"Index = {workerIndex}, ThreadId = {Environment.CurrentManagedThreadId}: {data}");
    }
}
