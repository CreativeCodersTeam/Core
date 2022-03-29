using System.Reactive;
using System.Threading.Tasks;
using CreativeCoders.Messaging.DefaultMessageQueue;
using Xunit;

namespace CreativeCoders.Messaging.UnitTests;

public class MessageQueueTests
{
    [Fact]
    public async Task EnqueueAsync_MessageText_CorrectMessageDequeued()
    {
        const string expectedMessage = "TestMessage";

        var queue = new MessageQueue<string>();

        await queue.EnqueueAsync(expectedMessage);

        Assert.Equal(expectedMessage, await queue.DequeueAsync());
    }

    [Fact]
    public async Task EnqueueAsync_QueueCancelled_ThrowsException()
    {
        const string message = "TestMessage";

        var queue = MessageQueue<string>.Create(1);

        await queue.DisposeAsync();

        await Assert.ThrowsAsync<MessageEnqueueFailedException>(async () =>
            await queue.EnqueueAsync(message));
    }

    [Fact]
    public async Task DequeueAsync_MessageText_CorrectMessageDequeued()
    {
        const string expectedMessage = "TestMessage";

        var queue = new MessageQueue<string>();

        await queue.EnqueueAsync(expectedMessage);

        Assert.Equal(expectedMessage, await queue.DequeueAsync());
    }

    [Fact]
    public async Task TryEnqueueAsync_QueueCancelled_ReturnsFalse()
    {
        const string message = "TestMessage";

        var queue = MessageQueue<string>.Create(1);

        await queue.DisposeAsync();

        Assert.False(await queue.TryEnqueueAsync(message));
    }

    [Fact]
    public void TryEnqueue_QueueCancelled_ReturnsFalse()
    {
        const string message = "TestMessage";

        var queue = MessageQueue<string>.Create(1);

        queue.Dispose();

        Assert.False(queue.TryEnqueue(message));
    }

    [Fact]
    public void Dispose_WithCompletion_TryEnqueueReturnsFalse()
    {
        const string message = "TestMessage";

        var queue = MessageQueue<string>.Create(1);

        queue.CompleteOnDispose = true;
        queue.Dispose();

        Assert.False(queue.TryEnqueue(message));
    }

    [Fact]
    public void Enqueue_QueueCancelled_ExceptionIsThrown()
    {
        const string message = "TestMessage";

        var queue = MessageQueue<string>.Create(1);

        queue.Dispose();

        Assert.Throws<MessageEnqueueFailedException>(() => queue.Enqueue(message));
    }

    [Fact]
    public async Task TryEnqueueAsync_QueueAcceptsMessage_ReturnsTrue()
    {
        const string expectedMessage = "TestMessage";

        var queue = MessageQueue<string>.Create(1);

        await queue.TryEnqueueAsync(expectedMessage);

        Assert.Equal(expectedMessage, await queue.DequeueAsync());
    }

    [Fact]
    public void Enqueue_QueueAcceptsMessage_ReturnsTrue()
    {
        const string expectedMessage = "TestMessage";

        var queue = MessageQueue<string>.Create(1);

        queue.Enqueue(expectedMessage);

        Assert.Equal(expectedMessage, queue.Dequeue());
    }

    [Fact]
    public void TryEnqueue_QueueAcceptsMessage_ReturnsTrue()
    {
        const string expectedMessage = "TestMessage";

        var queue = MessageQueue<string>.Create(1);

        queue.TryEnqueue(expectedMessage);

        Assert.Equal(expectedMessage, queue.Dequeue());
    }

    [Fact]
    public void TryDequeue_NoMessageInQueue_ReturnsFalse()
    {
        var queue = new MessageQueue<string>();

        Assert.False(queue.TryDequeue(out _));
    }

    [Fact]
    public void TryDequeue_MessageInQueue_ReturnsTrueAndValue()
    {
        const string expectedMessage = "TestMessage";

        var queue = new MessageQueue<string>();

        queue.Enqueue(expectedMessage);

        var isEnqueued = queue.TryDequeue(out var msg);
        Assert.True(isEnqueued);
        Assert.Equal(expectedMessage, msg);
    }

    [Fact]
    public async Task AsObservable_MessageEnqueued_MessageReceived()
    {
        const string expectedMessage = "TestMessage";

        var messageCount = 0;
        var messageReceived = string.Empty;

        var queue = new MessageQueue<string>();

        queue
            .AsObservable()
            .Subscribe(Observer.Create<string>(msg =>
            {
                messageCount++;
                messageReceived = msg;
            }));

        await queue.EnqueueAsync(expectedMessage);

        queue.CompleteOnDispose = true;
        await queue.DisposeAsync();

        // don't know why, but without this delay this test fails if code coverage is run on all the tests in this class
        await Task.Delay(1000);

        Assert.Equal(1, messageCount);
        Assert.Equal(expectedMessage, messageReceived);
    }

    [Fact]
    public async Task AsObservable_MessageEnqueuedAfterDispose_MessageNotReceived()
    {
        const string expectedMessage = "TestMessage";

        var messageCount = 0;
        var messageReceived = string.Empty;

        var queue = new MessageQueue<string>();

        queue
            .AsObservable()
            .Subscribe(Observer.Create<string>(msg =>
            {
                messageCount++;
                messageReceived = msg;
            }));

        await queue.DisposeAsync();

        queue.AsObserver().OnNext(expectedMessage);

        Assert.Equal(0, messageCount);
        Assert.Equal(string.Empty, messageReceived);
    }

    [Fact]
    public void AsObserver_OnNextWithMessage_MessageCanBeDequeued()
    {
        const string expectedMessage = "TestMessage";
        const string expectedMessage2 = "TestMessage2";

        var queue = new MessageQueue<string>();

        queue.AsObserver().OnNext(expectedMessage);
        queue.AsObserver().OnNext(expectedMessage2);

        Assert.Equal(expectedMessage, queue.Dequeue());
        Assert.Equal(expectedMessage2, queue.Dequeue());
    }
}
