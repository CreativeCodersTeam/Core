using CreativeCoders.Core.Messaging;

namespace CreativeCoders.Mvvm.UnitTests.TestObjects;

public class ViewModelBaseWithPublicMessenger : ViewModelBase
{
    public ViewModelBaseWithPublicMessenger() { }

    public ViewModelBaseWithPublicMessenger(IMessenger messenger) : base(messenger) { }

    public IMessenger ExposedMessenger
    {
        get => Messenger;
        set => Messenger = value;
    }
}
