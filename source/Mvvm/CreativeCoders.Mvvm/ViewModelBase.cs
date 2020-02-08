using CreativeCoders.Core;
using CreativeCoders.Core.Messaging;

namespace CreativeCoders.Mvvm
{
    public class ViewModelBase : ObservableObject
    {
        private IMessenger _messenger;

        public ViewModelBase() {}

        public ViewModelBase(IMessenger messenger)
        {
            _messenger = messenger;
        }

        protected IMessenger Messenger
        {
            get => _messenger ?? CreativeCoders.Core.Messaging.Messenger.Default;
            set => _messenger = value;
        }
    }
}