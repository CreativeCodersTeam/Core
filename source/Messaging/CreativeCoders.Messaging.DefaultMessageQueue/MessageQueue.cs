using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using CreativeCoders.Messaging.Core;

namespace CreativeCoders.Messaging.DefaultMessageQueue;

public class MessageQueue<T> : IMessageQueue<T>
{
    private readonly BufferBlock<T> _bufferBlock;

    private readonly CancellationTokenSource _cancellationTokenSource;

    public MessageQueue()
    {
        _cancellationTokenSource = new CancellationTokenSource();

        _bufferBlock = new BufferBlock<T>(
            new DataflowBlockOptions
            {
                CancellationToken = _cancellationTokenSource.Token
            });
    }

    private MessageQueue(int maxQueueLength)
    {
        _cancellationTokenSource = new CancellationTokenSource();

        _bufferBlock = new BufferBlock<T>(
            new DataflowBlockOptions
            {
                BoundedCapacity = maxQueueLength,
                CancellationToken = _cancellationTokenSource.Token
            });
    }

    public static MessageQueue<T> Create(int maxQueueLength)
    {
        return new(maxQueueLength);
    }

    public async Task EnqueueAsync(T message)
    {
        var messageEnqueued = await _bufferBlock.SendAsync(message);

        if (!messageEnqueued)
        {
            throw new MessageEnqueueFailedException();
        }
    }

    public Task<bool> TryEnqueueAsync(T message)
    {
        return _bufferBlock.SendAsync(message);
    }

    public void Enqueue(T message)
    {
        var messageEnqueued = _bufferBlock.Post(message);

        if (!messageEnqueued)
        {
            throw new MessageEnqueueFailedException();
        }
    }

    public bool TryEnqueue(T message)
    {
        return _bufferBlock.Post(message);
    }

    public Task<T> DequeueAsync()
    {
        return _bufferBlock.ReceiveAsync();
    }

    public T Dequeue()
    {
        return _bufferBlock.Receive();
    }

    public bool TryDequeue(out T message)
    {
        return _bufferBlock.TryReceive(out message);
    }

    public IObservable<T> AsObservable()
    {
        return _bufferBlock.AsObservable();
    }

    public IObserver<T> AsObserver()
    {
        return _bufferBlock.AsObserver();
    }

    public bool CompleteOnDispose { get; set; }

    public async ValueTask DisposeAsync()
    {
        if (CompleteOnDispose)
        {
            _bufferBlock.Complete();
            await _bufferBlock.Completion.ConfigureAwait(false);
        }
        else
        {
            _cancellationTokenSource.Cancel();
        }
    }

    public void Dispose()
    {
        if (CompleteOnDispose)
        {
            _bufferBlock.Complete();
            _bufferBlock.Completion.GetAwaiter().GetResult();
        }
        else
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
