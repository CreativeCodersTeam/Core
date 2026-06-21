using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Messaging.Core;

[PublicAPI]
public interface IMessageQueue<T> : IDisposable, IAsyncDisposable
    where T : notnull
{
    Task EnqueueAsync(T message);

    Task<bool> TryEnqueueAsync(T message);

    void Enqueue(T message);

    bool TryEnqueue(T message);

    Task<T> DequeueAsync();

    T Dequeue();

    bool TryDequeue([MaybeNullWhen(false)] out T message);

    IObservable<T> AsObservable();

    IObserver<T> AsObserver();

    bool CompleteOnDispose { get; set; }
}
