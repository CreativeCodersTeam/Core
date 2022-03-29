using System;
using System.Diagnostics.CodeAnalysis;
using CreativeCoders.Core.Messaging;
using CreativeCoders.Core.Messaging.Messages;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Messaging;

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

        var messenger = Messenger.CreateInstance();
            
        messenger.Register<string>(this, _ => _testCallbackCalledSendString = true);
        messenger.Send(new string("Hallo".ToCharArray()));

        Assert.True(_testCallbackCalledSendString);
    }

    [Fact]
    public void MessengerUnregisterTest()
    {
        _testCallbackCalledUnReg = false;

        var messenger = Messenger.CreateInstance();
            
        messenger.Register<string>(this, _ => _testCallbackCalledUnReg = true);
        messenger.Send(new string("Hallo".ToCharArray()));

        Assert.True(_testCallbackCalledUnReg);

        _testCallbackCalledUnReg = false;

        messenger.Unregister(this);
        messenger.Send(new string("Hallo".ToCharArray()));

        Assert.False(_testCallbackCalledUnReg);
    }

    [Fact]
    public void MessengerUnregisterStringTest()
    {
        _testCallbackCalledUnRegString = false;

        var messenger = Messenger.CreateInstance();
            
        messenger.Register<string>(this, _ => _testCallbackCalledUnRegString = true);
        messenger.Send(new string("Hallo".ToCharArray()));

        Assert.True(_testCallbackCalledUnRegString);

        _testCallbackCalledUnRegString = false;

        messenger.Unregister<string>(this);
        messenger.Send(new string("Hallo".ToCharArray()));

        Assert.False(_testCallbackCalledUnRegString);
    }

    [Fact]
    public void MessengerSendMultipleTest()
    {
        _testCallbackCalledSendMultiple = false;
        _testCallbackCalledSendMultiple2 = false;
            
        var messenger = Messenger.CreateInstance();

        messenger.Register<string>(this, _ => _testCallbackCalledSendMultiple = true);
        messenger.Register<string>(this, _ => _testCallbackCalledSendMultiple2 = true);
        messenger.Send(new string("Hallo".ToCharArray()));

        Assert.True(_testCallbackCalledSendMultiple);
        Assert.True(_testCallbackCalledSendMultiple2);
    }

    [Fact]
    public void MessengerUnregisterMultipleTest()
    {
        _testCallbackCalledUnRegMultiple = false;
        _testCallbackCalledUnRegMultiple2 = false;

        var messenger = Messenger.CreateInstance();
            
        messenger.Register<string>(this, _ => _testCallbackCalledUnRegMultiple = true);
        messenger.Register<string>(this, _ => _testCallbackCalledUnRegMultiple2 = true);
        messenger.Send(new string("Hallo".ToCharArray()));

        Assert.True(_testCallbackCalledUnRegMultiple);
        Assert.True(_testCallbackCalledUnRegMultiple2);

        messenger.Unregister(this);

        _testCallbackCalledUnRegMultiple = false;
        _testCallbackCalledUnRegMultiple2 = false;

        messenger.Send(new string("Hallo".ToCharArray()));

        Assert.False(_testCallbackCalledUnRegMultiple);
        Assert.False(_testCallbackCalledUnRegMultiple2);
    }

    [Fact]
    public void MessengerUnregisterMultipleExTest()
    {
        _testCallbackCalledUnRegMultipleEx = false;
        _testCallbackCalledUnRegMultipleEx2 = false;

        var messenger = Messenger.CreateInstance();
            
        messenger.Register<int>(this, _ => _testCallbackCalledUnRegMultipleEx = true);
        messenger.Register<string>(this, _ => _testCallbackCalledUnRegMultipleEx2 = true);
        messenger.Send(new string("Hallo".ToCharArray()));
        messenger.Send(1234);

        Assert.True(_testCallbackCalledUnRegMultipleEx);
        Assert.True(_testCallbackCalledUnRegMultipleEx2);

        messenger.Unregister<int>(this);

        _testCallbackCalledUnRegMultipleEx = false;
        _testCallbackCalledUnRegMultipleEx2 = false;

        messenger.Send(new string("Hallo".ToCharArray()));
        messenger.Send(1234);

        Assert.False(_testCallbackCalledUnRegMultipleEx);
        Assert.True(_testCallbackCalledUnRegMultipleEx2);

        messenger.Unregister<string>(this);

        _testCallbackCalledUnRegMultipleEx = false;
        _testCallbackCalledUnRegMultipleEx2 = false;

        messenger.Send(new string("Hallo".ToCharArray()));

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
        var messenger = Messenger.CreateInstance();
            
        var msgBase = new MessageBase();
        messenger.Register<MessageBase>(null, msg => msg.IsHandled = true);
        messenger.Send(msgBase);
        Assert.True(msgBase.IsHandled);
    }

    private string _dialogMessageText;

    [Fact]
    public void DialogMessageTest()
    {
        var messenger = Messenger.CreateInstance();
            
        var dialogMessage = new DialogMessage("Test1234");
        messenger.Register<DialogMessage>(null, msg => _dialogMessageText = msg.MessageText);
        messenger.Send(dialogMessage);
        Assert.True(_dialogMessageText == "Test1234");
    }

    [Fact]
    public void CallbackMessageTest()
    {
        _testCallbackCalledSend = false;

        var messenger = Messenger.CreateInstance();
            
        var callbackMessage = new CallbackMessage(OnCallbackMessage);
        messenger.Register<CallbackMessage>(null, msg => msg.Execute());
        messenger.Send(callbackMessage);
        Assert.True(_testCallbackCalledSend);
    }

    [Fact]
    public void MessengerSendTest()
    {
        _testCallbackCalledSend = false;

        var messenger = Messenger.CreateInstance();
            
        messenger.Register<TestMessage>(this, TestCallback);
        messenger.Send(new TestMessage());

        Assert.True(_testCallbackCalledSend);
    }

    [Fact]
    public void MessengerSendLambdaTest()
    {
        _testCallbackCalledUnRegMultipleEx = false;

        var messenger = Messenger.CreateInstance();
            
        messenger.Register<TestMessage>(this, _ => _testCallbackCalledUnRegMultipleEx = true);
        messenger.Send(new TestMessage());

        Assert.True(_testCallbackCalledUnRegMultipleEx);
    }

    [Fact]
    public void SendTestWithExceptionInAction()
    {
        var messenger = Messenger.CreateInstance();
            
        messenger.Register<TestMessage>(this, _ => throw new InvalidOperationException());
        messenger.Send(new TestMessage());
    }

    [Fact]
    public void SendAfterGarbageCollectTest()
    {
        var messenger = Messenger.CreateInstance();
            
        var obj = new TestReceiver();

        messenger.Register<TestMessage>(obj, obj.HandleMessage);
        messenger.Send(new TestMessage());

        Assert.True(obj.MsgHandled);

        obj.MsgHandled = false;
        var weakRef = new WeakReference(obj);
        // ReSharper disable once RedundantAssignment
        obj = null;

        CollectGarbage();

        messenger.Send(new TestMessage());

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
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public void HandleMessage(TestMessage msg)
        {
            MsgHandled = true;
        }

        public bool MsgHandled { get; set; }
    }

    [Fact]
    public void DisposeRegistrationTest()
    {
        var messenger = Messenger.CreateInstance();
            
        _testCallbackCalledSend = false;
        var obj0 = new object();

        var callbackMessage = new CallbackMessage(OnCallbackMessage);
        var registration = messenger.Register<CallbackMessage>(this, ExecuteCallback);
        messenger.Send(callbackMessage);

        Assert.True(_testCallbackCalledSend);

        _testCallbackCalledSend = false;
        registration.Dispose();

        messenger.Send(callbackMessage);

        Assert.False(_testCallbackCalledSend);
    }

    [Fact]
    public void DoubleDisposeRegistrationTest()
    {
        var messenger = Messenger.CreateInstance();
            
        _testCallbackCalledSend = false;
        var obj0 = new object();

        var callbackMessage = new CallbackMessage(OnCallbackMessage);
        var registration = messenger.Register<CallbackMessage>(this, ExecuteCallback);
        messenger.Send(callbackMessage);

        Assert.True(_testCallbackCalledSend);

        _testCallbackCalledSend = false;
        registration.Dispose();
        registration.Dispose();

        messenger.Send(callbackMessage);

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