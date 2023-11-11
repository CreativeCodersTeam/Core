using CreativeCoders.Core.Messaging;
using CreativeCoders.Mvvm.UnitTests.TestObjects;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Mvvm.UnitTests;

public class ViewModelBaseTests
{
    [Fact]
    public void CtorTest()
    {
        _ = new ViewModelBase();
    }

    [Fact]
    public void CtorTestWithMessenger()
    {
        var messenger = A.Fake<IMessenger>();
        _ = new ViewModelBase(messenger);
    }

    [Fact]
    public void CtorTestWithMessengerExposed()
    {
        var messenger = A.Fake<IMessenger>();
        var viewModel = new ViewModelBaseWithPublicMessenger(messenger);

        Assert.Equal(messenger, viewModel.ExposedMessenger);
    }

    [Fact]
    public void TestWithMessengerExposedSet()
    {
        var messenger = A.Fake<IMessenger>();
        var viewModel = new ViewModelBaseWithPublicMessenger();

        Assert.Equal(Messenger.Default, viewModel.ExposedMessenger);

        viewModel.ExposedMessenger = messenger;

        Assert.Equal(messenger, viewModel.ExposedMessenger);
    }
}
