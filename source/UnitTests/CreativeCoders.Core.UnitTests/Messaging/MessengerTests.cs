using System;
using System.Diagnostics.CodeAnalysis;
using CreativeCoders.Core.Messaging;
using CreativeCoders.Core.Messaging.Messages;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Messaging
{
    [SuppressMessage("ReSharper", "UnusedVariable")]
    public class MessengerTests
    {
        private bool _testCallbackCalledUnRegMultipleEx;

        private bool _testCallbackCalledUnRegMultipleEx2;

        private bool _testCallbackCalledSendString;

        private bool _testCallbackCalledUnReg;

        private bool _testCallbackCalledUnRegString;

        private bool _testCallbackCalledSendMultiple;

        private bool _testCallbackCalledSendMultiple2;

        private bool _testCallbackCalledUnRegMultiple;

        private bool _testCallbackCalledUnRegMultiple2;

        private bool _testCallbackCalledSend;

        [Fact]
        public void MessengerDefaultTest()
        {
            var messenger = Messenger.Default;

            Assert.NotNull(messenger);
        }

        [Fact]
        public void MessengerCreateInstanceTest()
        {
            var messenger = Messenger.CreateInstance();

            Assert.NotNull(messenger);
        }

        [Fact]
        public void MessengerRegisterTest()
        {
            Messenger.Default.Register<TestMessage>(this, TestCallback);
        }

        [Fact]
        public void MessengerSendStringTest()
        {
            _testCallbackCalledSendString = false;

            Messenger.Default.Register<string>(this, msg => _testCallbackCalledSendString = true);
            Messenger.Default.Send(new string("Hallo".ToCharArray()));

            Assert.True(_testCallbackCalledSendString);
        }

        [Fact]
        public void MessengerUnregisterTest()
        {
            _testCallbackCalledUnReg = false;

            Messenger.Default.Register<string>(this, msg => _testCallbackCalledUnReg = true);
            Messenger.Default.Send(new string("Hallo".ToCharArray()));

            Assert.True(_testCallbackCalledUnReg);

            _testCallbackCalledUnReg = false;

            Messenger.Default.Unregister(this);
            Messenger.Default.Send(new string("Hallo".ToCharArray()));

            Assert.False(_testCallbackCalledUnReg);
        }

        [Fact]
        public void MessengerUnregisterStringTest()
        {
            _testCallbackCalledUnRegString = false;

            Messenger.Default.Register<string>(this, msg => _testCallbackCalledUnRegString = true);
            Messenger.Default.Send(new string("Hallo".ToCharArray()));

            Assert.True(_testCallbackCalledUnRegString);

            _testCallbackCalledUnRegString = false;

            Messenger.Default.Unregister<string>(this);
            Messenger.Default.Send(new string("Hallo".ToCharArray()));

            Assert.False(_testCallbackCalledUnRegString);
        }

        [Fact]
        public void MessengerSendMultipleTest()
        {
            _testCallbackCalledSendMultiple = false;
            _testCallbackCalledSendMultiple2 = false;

            Messenger.Default.Register<string>(this, msg => _testCallbackCalledSendMultiple = true);
            Messenger.Default.Register<string>(this, msg => _testCallbackCalledSendMultiple2 = true);
            Messenger.Default.Send(new string("Hallo".ToCharArray()));

            Assert.True(_testCallbackCalledSendMultiple);
            Assert.True(_testCallbackCalledSendMultiple2);
        }

        [Fact]
        public void MessengerUnregisterMultipleTest()
        {
            _testCallbackCalledUnRegMultiple = false;
            _testCallbackCalledUnRegMultiple2 = false;

            Messenger.Default.Register<string>(this, msg => _testCallbackCalledUnRegMultiple = true);
            Messenger.Default.Register<string>(this, msg => _testCallbackCalledUnRegMultiple2 = true);
            Messenger.Default.Send(new string("Hallo".ToCharArray()));

            Assert.True(_testCallbackCalledUnRegMultiple);
            Assert.True(_testCallbackCalledUnRegMultiple2);

            Messenger.Default.Unregister(this);

            _testCallbackCalledUnRegMultiple = false;
            _testCallbackCalledUnRegMultiple2 = false;

            Messenger.Default.Send(new string("Hallo".ToCharArray()));

            Assert.False(_testCallbackCalledUnRegMultiple);
            Assert.False(_testCallbackCalledUnRegMultiple2);
        }

        [Fact]
        public void MessengerUnregisterMultipleExTest()
        {
            _testCallbackCalledUnRegMultipleEx = false;
            _testCallbackCalledUnRegMultipleEx2 = false;

            Messenger.Default.Register<int>(this, msg => _testCallbackCalledUnRegMultipleEx = true);
            Messenger.Default.Register<string>(this, msg => _testCallbackCalledUnRegMultipleEx2 = true);
            Messenger.Default.Send(new string("Hallo".ToCharArray()));
            Messenger.Default.Send(1234);

            Assert.True(_testCallbackCalledUnRegMultipleEx);
            Assert.True(_testCallbackCalledUnRegMultipleEx2);

            Messenger.Default.Unregister<int>(this);

            _testCallbackCalledUnRegMultipleEx = false;
            _testCallbackCalledUnRegMultipleEx2 = false;

            Messenger.Default.Send(new string("Hallo".ToCharArray()));
            Messenger.Default.Send(1234);

            Assert.False(_testCallbackCalledUnRegMultipleEx);
            Assert.True(_testCallbackCalledUnRegMultipleEx2);

            Messenger.Default.Unregister<string>(this);

            _testCallbackCalledUnRegMultipleEx = false;
            _testCallbackCalledUnRegMultipleEx2 = false;

            Messenger.Default.Send(new string("Hallo".ToCharArray()));

            Assert.False(_testCallbackCalledUnRegMultipleEx);
            Assert.False(_testCallbackCalledUnRegMultipleEx2);
        }

        [Fact]
        public void MessagesCtorTest()
        {
            var msgBase = new MessageBase();
            var dlgMsg = new DialogMessage("Test");
            var callbackMsg = new CallbackMessage(OnCallbackMessage);
            Assert.Throws<ArgumentNullException>(() => new CallbackMessage(null));
        }

        [Fact]
        public void MessageBaseTest()
        {
            var msgBase = new MessageBase();
            Messenger.Default.Register<MessageBase>(null, msg => msg.IsHandled = true);
            Messenger.Default.Send(msgBase);
            Assert.True(msgBase.IsHandled);
        }

        private string _dialogMessageText;

        [Fact]
        public void DialogMessageTest()
        {
            var dialogMessage = new DialogMessage("Test1234");
            Messenger.Default.Register<DialogMessage>(null, msg => _dialogMessageText = msg.MessageText);
            Messenger.Default.Send(dialogMessage);
            Assert.True(_dialogMessageText == "Test1234");
        }

        [Fact]
        public void CallbackMessageTest()
        {
            _testCallbackCalledSend = false;

            var callbackMessage = new CallbackMessage(OnCallbackMessage);
            Messenger.Default.Register<CallbackMessage>(null, msg => msg.Execute());
            Messenger.Default.Send(callbackMessage);
            Assert.True(_testCallbackCalledSend);
        }

        [Fact]
        public void MessengerSendTest()
        {
            _testCallbackCalledSend = false;

            Messenger.Default.Register<TestMessage>(this, TestCallback);
            Messenger.Default.Send(new TestMessage());

            Assert.True(_testCallbackCalledSend);
        }

        [Fact]
        public void MessengerSendLambdaTest()
        {
            _testCallbackCalledUnRegMultipleEx = false;

            Messenger.Default.Register<TestMessage>(this, msg => _testCallbackCalledUnRegMultipleEx = true);
            Messenger.Default.Send(new TestMessage());

            Assert.True(_testCallbackCalledUnRegMultipleEx);
        }

        [Fact]
        public void SendTestWithExceptionInAction()
        {
            Messenger.Default.Register<TestMessage>(this, msg => throw new InvalidOperationException());
            Messenger.Default.Send(new TestMessage());
        }

        [Fact]
        public void SendAfterGarbageCollectTest()
        {
            var obj = new TestReceiver();

            Messenger.Default.Register<TestMessage>(obj, obj.HandleMessage);
            Messenger.Default.Send(new TestMessage());

            Assert.True(obj.MsgHandled);

            obj.MsgHandled = false;
            var weakRef = new WeakReference(obj);
            // ReSharper disable once RedundantAssignment
            obj = null;

            CollectGarbage();

            Messenger.Default.Send(new TestMessage());

            //Xunit.Assert.False(weakRef.IsAlive);
        }

        private static void CollectGarbage()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.WaitForFullGCComplete();
            GC.Collect();
        }

        private class TestReceiver
        {
            public void HandleMessage(TestMessage msg)
            {
                MsgHandled = true;
            }

            public bool MsgHandled { get; set; }
        }

        [Fact]
        public void DisposeRegistrationTest()
        {
            _testCallbackCalledSend = false;
            var obj0 = new object();

            var callbackMessage = new CallbackMessage(OnCallbackMessage);
            var registration = Messenger.Default.Register<CallbackMessage>(this, ExecuteCallback);
            Messenger.Default.Send(callbackMessage);

            Assert.True(_testCallbackCalledSend);

            _testCallbackCalledSend = false;
            registration.Dispose();

            Messenger.Default.Send(callbackMessage);

            Assert.False(_testCallbackCalledSend);
        }

        [Fact]
        public void DoubleDisposeRegistrationTest()
        {
            _testCallbackCalledSend = false;
            var obj0 = new object();

            var callbackMessage = new CallbackMessage(OnCallbackMessage);
            var registration = Messenger.Default.Register<CallbackMessage>(this, ExecuteCallback);
            Messenger.Default.Send(callbackMessage);

            Assert.True(_testCallbackCalledSend);

            _testCallbackCalledSend = false;
            registration.Dispose();
            registration.Dispose();

            Messenger.Default.Send(callbackMessage);

            Assert.False(_testCallbackCalledSend);
        }

        // ReSharper disable once MemberCanBeMadeStatic.Local
        private void ExecuteCallback(CallbackMessage msg)
        {
            msg.Execute();
        }

        private void OnCallbackMessage()
        {
            _testCallbackCalledSend = true;
        }

        private void TestCallback(TestMessage testMessage)
        {
            _testCallbackCalledSend = true;            
        }
    }
}