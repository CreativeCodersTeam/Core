using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Messaging.Core
{
    [PublicAPI]
    public interface IMessageQueue<T> : IDisposable, IAsyncDisposable
    {
        Task EnqueueAsync(T message);

        Task<bool> TryEnqueueAsync(T message);

        void Enqueue(T message);
        
        bool TryEnqueue(T message);

        Task<T> DequeueAsync();

        T Dequeue();
        
        bool TryDequeue(out T message);

        IObservable<T> AsObservable();

        IObserver<T> AsObserver();
        
        bool CompleteOnDispose { get; set; }
    }
}