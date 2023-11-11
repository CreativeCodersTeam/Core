using System;
using System.Reactive.Concurrency;
using CreativeCoders.Reactive.Messaging;
using Xunit;

namespace CreativeCoders.Reactive.UnitTests;

public class MessageTopicTests
{
    [Fact]
    public void CtorTest()
    {
        _ = new MessageTopic();
    }

    [Fact]
    public void PublishTest()
    {
        var topic = new MessageTopic();

        topic.Publish("Test");

        Assert.Throws<ArgumentNullException>(() => topic.Publish<object>(null));
    }

    [Fact]
    public void RegisterTest()
    {
        var topic = new MessageTopic();

        object msgPublished = null;

        topic
            .Register<object>()
            .Subscribe(o => msgPublished = o);

        var msg = new object();

        topic.Publish(msg);

        Assert.Equal(msg, msgPublished);
    }

    [Fact]
    public void RegisterTestScheduler()
    {
        var topic = new MessageTopic();

        object msgPublished = null;

        topic
            .Register<object>(Scheduler.Immediate)
            .Subscribe(o => msgPublished = o);

        var msg = new object();

        topic.Publish(msg);

        Assert.Equal(msg, msgPublished);
    }

    [Fact]
    public void PublishTestDifferentMessageTypes()
    {
        var topic = new MessageTopic();

        object msgPublished = null;
        var strPublished = false;

        topic.Register<object>()
            .Subscribe(o => msgPublished = o);
        topic.Register<string>()
            .Subscribe(_ => strPublished = true);

        var msg = new object();

        topic.Publish(msg);

        Assert.Equal(msg, msgPublished);
        Assert.False(strPublished);

        const string msgStr = "TestMsg";

        topic.Publish(msgStr);

        Assert.Equal(msgStr, msgPublished);
        Assert.True(strPublished);
    }

    [Fact]
    public void PublishTestBeforeRegister()
    {
        var topic = new MessageTopic();

        object objPublished = null;

        topic.Register<object>().Subscribe(o => objPublished = o);

        var obj = new object();

        topic.Publish(obj);

        object secondObjPublished = null;

        topic.Register<object>().Subscribe(o => secondObjPublished = o);

        Assert.Equal(obj, objPublished);

        Assert.Null(secondObjPublished);

        var obj2 = new object();

        topic.Publish(obj2);

        Assert.Equal(obj2, objPublished);
        Assert.Equal(obj2, secondObjPublished);
    }
}
