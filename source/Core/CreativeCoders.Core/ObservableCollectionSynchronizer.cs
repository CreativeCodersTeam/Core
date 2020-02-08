using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using JetBrains.Annotations;

namespace CreativeCoders.Core
{
    [PublicAPI]
    public class ObservableCollectionSynchronizer<TMasterElement, TSlaveElement> : IDisposable
    {
        private readonly ObservableCollection<TMasterElement> _masterCollection;
        
        private readonly ObservableCollection<TSlaveElement> _slaveCollection;
        
        private readonly Func<TMasterElement, TSlaveElement> _createSlaveFunc;
        
        private readonly Func<TMasterElement, TSlaveElement> _getSlaveForMaster;

        public ObservableCollectionSynchronizer(ObservableCollection<TMasterElement> masterCollection,
            ObservableCollection<TSlaveElement> slaveCollection, Func<TMasterElement, TSlaveElement> createSlaveFunc,
            Func<TMasterElement, TSlaveElement> getSlaveForMaster)
        {
            _masterCollection = masterCollection;
            _slaveCollection = slaveCollection;
            _createSlaveFunc = createSlaveFunc;
            _getSlaveForMaster = getSlaveForMaster;

            _masterCollection.CollectionChanged += MasterCollectionOnCollectionChanged;
            
            _slaveCollection.AddRange(_masterCollection.Select(_createSlaveFunc));
        }

        private void MasterCollectionOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var newStartingIndex = e.NewStartingIndex;
                    e.NewItems
                        .Cast<TMasterElement>()
                        .ForEach(x => AddElement(newStartingIndex++, x));
                    break;
                case NotifyCollectionChangedAction.Move:
                    _slaveCollection.Move(e.OldStartingIndex, e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveElements(e.OldStartingIndex, e.OldItems.Cast<TMasterElement>());
                    break;
                case NotifyCollectionChangedAction.Replace:
                    ReplaceElement(e);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    _slaveCollection.Clear();
                    _slaveCollection.AddRange(_masterCollection.Select(_createSlaveFunc));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ReplaceElement(NotifyCollectionChangedEventArgs eventArgs)
        {
            if ((eventArgs.NewStartingIndex > -1 || eventArgs.OldStartingIndex > -1) &&
                eventArgs.NewStartingIndex == eventArgs.OldStartingIndex)
            {
                _slaveCollection[eventArgs.NewStartingIndex] =
                    _createSlaveFunc(eventArgs.NewItems.Cast<TMasterElement>().First());
            }
        }

        private void AddElement(int newStartingIndex, TMasterElement masterElement)
        {
            if (newStartingIndex == -1)
            {
                _slaveCollection.Add(_createSlaveFunc(masterElement));
            }
            else
            {
                _slaveCollection.Insert(newStartingIndex, _createSlaveFunc(masterElement));
            }
        }

        private void RemoveElements(int oldStartingIndex, IEnumerable<TMasterElement> masterElements)
        {
            if (oldStartingIndex == -1)
            {
                masterElements.ForEach(x => _slaveCollection.Remove(_getSlaveForMaster(x)));
            }
            else
            {
                _slaveCollection.RemoveAt(oldStartingIndex);
            }
        }

        public void Dispose()
        {
            _masterCollection.CollectionChanged -= MasterCollectionOnCollectionChanged;
        }
    }
}