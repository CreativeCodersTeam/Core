using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CreativeCoders.Core.ObjectLinking
{
    public class ObjectLink : IDisposable
    {
        private readonly object _instance0;
        
        private readonly object _instance1;
        
        private readonly IEnumerable<PropertyLinkItem> _linkItems;

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

        public void Dispose()
        {
            DisconnectChangedEvents();
        }
    }
}