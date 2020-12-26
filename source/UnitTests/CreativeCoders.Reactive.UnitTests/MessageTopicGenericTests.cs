using System;
using System.Reactive.Concurrency;
using CreativeCoders.Reactive.Messaging;
using Xunit;

namespace CreativeCoders.Reactive.UnitTests
{
    public class MessageTopicGenericTests
    {
        [Fact]
        public void CtorTest()
        {
            var _ = new MessageTopic<object>();
        }

        [Fact]
        public void PublishTest()
        {
            var topic = new MessageTopic<object>();

            topic.Publish("Test");

            Assert.Throws<ArgumentNullException>(() => topic.Publish(null));
        }

        [Fact]
        public void RegisterTest()
        {
            var topic = new MessageTopic<object>();

            object msgPublished = null;

            topic.Register().Subscribe(o => msgPublished = o);

            var msg = new object();

            topic.Publish(msg);

            Assert.Equal(msg, msgPublished);
        }

        [Fact]
        public void PublishTestDifferentMessageTypesWithScheduler()
        {
            var topic = new MessageTopic<object>();

            object msgPublished = null;
            var strPublished = false;

            topic.Register(Scheduler.Immediate).Subscribe(o => msgPublished = o);
            topic.Register<string>(Scheduler.Immediate).Subscribe(_ => strPublished = true);

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
        public void PublishTestDifferentMessageTypes()
        {
            var topic = new MessageTopic<object>();

            object msgPublished = null;
            var strPublished = false;

            topic.Register().Subscribe(o => msgPublished = o);
            topic.Register<string>().Subscribe(_ => strPublished = true);

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
        public void PublishTestBeforeRegisterGeneric()
        {
            var topic = new MessageTopic<object>();

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

        [Fact]
        public void PublishTestBeforeRegister()
        {
            var topic = new MessageTopic<object>();

            object objPublished = null;

            topic.Register().Subscribe(o => objPublished = o);

            var obj = new object();

            topic.Publish(obj);

            object secondObjPublished = null;

            topic.Register().Subscribe(o => secondObjPublished = o);

            Assert.Equal(obj, objPublished);

            Assert.Null(secondObjPublished);

            var obj2 = new object();

            topic.Publish(obj2);

            Assert.Equal(obj2, objPublished);
            Assert.Equal(obj2, secondObjPublished);
        }
    }
}
