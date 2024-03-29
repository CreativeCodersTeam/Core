﻿using System;
using CreativeCoders.Core.Weak;

namespace CreativeCoders.Core.Messaging;

internal class MessengerRegistration<TMessage> : IMessengerRegistration
{
    private MessengerImpl _messenger;

    private bool _disposed;

    private readonly WeakAction<TMessage> _weakAction;

    public MessengerRegistration(MessengerImpl messenger, object receiver, Action<TMessage> action,
        KeepOwnerAliveMode keepOwnerAliveMode)
    {
        _messenger = messenger;
        _weakAction = new WeakAction<TMessage>(receiver, action, keepOwnerAliveMode);
    }

    public void Execute<T>(T message)
    {
        try
        {
            _weakAction.Execute(message);
        }
        catch (Exception)
        {
            // TODO think about logging
        }
    }

    public object Target => _weakAction.GetTarget();

    public bool IsAlive => _weakAction.IsAlive();

    private void Dispose(bool disposing)
    {
        if (!disposing || _disposed)
        {
            return;
        }

        _disposed = true;
        _weakAction.Dispose();
        _messenger.RemoveRegistration(this);
        _messenger = null;
    }

    public void Dispose()
    {
        Dispose(true);
    }

    public void RemovedFromMessenger()
    {
        _messenger = null;
        _disposed = true;
    }
}
