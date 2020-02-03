namespace CreativeCoders.Core.Messaging
{
    public static class Messenger
    {
        public static IMessenger Default { get; } = new MessengerImpl();

        public static IMessenger CreateInstance()
        {
            return new MessengerImpl();
        }
    }
}