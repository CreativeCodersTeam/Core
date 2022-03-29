using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Collections;

[PublicAPI]
public class ObservableCollectionSynchronizer<TMasterElement, TSlaveElement> : IDisposable
{
    private readonly ObservableCollection<TMasterElement> _masterCollection;

    private readonly ObservableCollection<TSlaveElement> _slaveCollection;

    private readonly Func<TMasterElement, TSlaveElement> _createSlave;

    private readonly Func<TMasterElement, TSlaveElement> _getSlaveForMaster;

    public ObservableCollectionSynchronizer(ObservableCollection<TMasterElement> masterCollection,
        ObservableCollection<TSlaveElement> slaveCollection, Func<TMasterElement, TSlaveElement> createSlave,
        Func<TMasterElement, TSlaveElement> getSlaveForMaster)
    {
        _masterCollection = masterCollection;
        _slaveCollection = slaveCollection;
        _createSlave = createSlave;
        _getSlaveForMaster = getSlaveForMaster;

        _masterCollection.CollectionChanged += MasterCollectionOnCollectionChanged;

        _slaveCollection.AddRange(_masterCollection.Select(_createSlave));
    }

    private void MasterCollectionOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                var newStartingIndex = e.NewStartingIndex;
                e.NewItems?
                    .Cast<TMasterElement>()
                    .ForEach(x => AddElement(newStartingIndex++, x));
                break;
            case NotifyCollectionChangedAction.Move:
                _slaveCollection.Move(e.OldStartingIndex, e.NewStartingIndex);
                break;
            case NotifyCollectionChangedAction.Remove:
                RemoveElements(e.OldStartingIndex,
                    e.OldItems?.Cast<TMasterElement>() ?? Array.Empty<TMasterElement>());
                break;
            case NotifyCollectionChangedAction.Replace:
                ReplaceElement(e);
                break;
            case NotifyCollectionChangedAction.Reset:
                _slaveCollection.Clear();
                _slaveCollection.AddRange(_masterCollection.Select(_createSlave));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(e) + "." + nameof(e.Action),
                    $"Unknown action '{e.Action}'");
        }
    }

    private void ReplaceElement(NotifyCollectionChangedEventArgs eventArgs)
    {
        if ((eventArgs.NewStartingIndex <= -1 && eventArgs.OldStartingIndex <= -1) ||
            eventArgs.NewStartingIndex != eventArgs.OldStartingIndex || eventArgs.NewItems == null)
        {
            return;
        }

        var masterElement = eventArgs.NewItems.Cast<TMasterElement>().First();

        _slaveCollection[eventArgs.NewStartingIndex] =
            _createSlave(masterElement);
    }

    private void AddElement(int newStartingIndex, TMasterElement masterElement)
    {
        if (newStartingIndex == -1)
        {
            _slaveCollection.Add(_createSlave(masterElement));
        }
        else
        {
            _slaveCollection.Insert(newStartingIndex, _createSlave(masterElement));
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
