using System;
using System.Collections.Generic;
using System.ComponentModel;
using CreativeCoders.Core.Collections;

namespace CreativeCoders.Core.ObjectLinking;

/// <summary>
///     Manages a live link between two object instances, automatically synchronizing property values
///     when either object raises <see cref="INotifyPropertyChanged.PropertyChanged"/>. Disposing the
///     link disconnects the event handlers.
/// </summary>
public sealed class ObjectLink : IDisposable
{
    private readonly object _instance0;

    private readonly object _instance1;

    private readonly IEnumerable<PropertyLinkItem> _linkItems;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ObjectLink"/> class, connects property change
    ///     event handlers, and performs the initial property value synchronization.
    /// </summary>
    /// <param name="instance0">The first linked object instance.</param>
    /// <param name="instance1">The second linked object instance.</param>
    /// <param name="linkItems">The property link items defining the individual property links.</param>
    public ObjectLink(object instance0, object instance1, IEnumerable<PropertyLinkItem> linkItems)
    {
        _instance0 = instance0;
        _instance1 = instance1;
        _linkItems = linkItems;

        ConnectChangedEvents();

        InitLinks();
    }

    private void InitLinks()
    {
        _linkItems.ForEach(linkItem => linkItem.Init());
    }

    private void ConnectChangedEvents()
    {
        if (_instance0 is INotifyPropertyChanged notifyPropertyChanged0)
        {
            notifyPropertyChanged0.PropertyChanged += NotifyPropertyChangedOnPropertyChanged;
        }

        if (_instance1 is INotifyPropertyChanged notifyPropertyChanged1)
        {
            notifyPropertyChanged1.PropertyChanged += NotifyPropertyChangedOnPropertyChanged;
        }
    }

    private void DisconnectChangedEvents()
    {
        if (_instance0 is INotifyPropertyChanged notifyPropertyChanged0)
        {
            notifyPropertyChanged0.PropertyChanged -= NotifyPropertyChangedOnPropertyChanged;
        }

        if (_instance1 is INotifyPropertyChanged notifyPropertyChanged1)
        {
            notifyPropertyChanged1.PropertyChanged -= NotifyPropertyChangedOnPropertyChanged;
        }
    }

    private void NotifyPropertyChangedOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        _linkItems.ForEach(linkItem => linkItem.HandleChange(sender, e.PropertyName));
    }

    /// <summary>
    ///     Disconnects the property change event handlers from both linked object instances.
    /// </summary>
    public void Dispose()
    {
        DisconnectChangedEvents();
    }
}
