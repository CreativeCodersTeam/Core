using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;

#nullable enable

namespace CreativeCoders.Core.Collections;

/// <summary>
///     Keeps a replica <see cref="ObservableCollection{T}"/> synchronized with a source
///     <see cref="ObservableCollection{T}"/> by subscribing to
///     <see cref="INotifyCollectionChanged.CollectionChanged"/> and mirroring all changes.
/// </summary>
/// <typeparam name="TSourceElement">The type of elements in the source collection.</typeparam>
/// <typeparam name="TReplicaElement">The type of elements in the replica collection.</typeparam>
[PublicAPI]
public sealed class ObservableCollectionSynchronizer<TSourceElement, TReplicaElement> : IDisposable
{
    private readonly ObservableCollection<TSourceElement> _sourceCollection;

    private readonly ObservableCollection<TReplicaElement> _replicaCollection;

    private readonly Func<TSourceElement, TReplicaElement> _createReplicaItemForSourceItem;

    private readonly Func<TSourceElement, TReplicaElement> _getReplicaItemForSourceItem;

    /// <summary>
    ///     Initializes a new instance of the
    ///     <see cref="ObservableCollectionSynchronizer{TSourceElement, TReplicaElement}"/> class,
    ///     populates the replica with existing source elements, and begins listening for source changes.
    /// </summary>
    /// <param name="sourceCollection">The source collection whose changes are mirrored.</param>
    /// <param name="replicaCollection">The replica collection that is kept in sync.</param>
    /// <param name="createReplicaItemForSourceItem">
    ///     The factory that creates a new replica element for a given source element.
    /// </param>
    /// <param name="getReplicaItemForSourceItem">
    ///     The function that retrieves the existing replica element corresponding to a given source element,
    ///     used during remove operations.
    /// </param>
    public ObservableCollectionSynchronizer(ObservableCollection<TSourceElement> sourceCollection,
        ObservableCollection<TReplicaElement> replicaCollection, Func<TSourceElement,
            TReplicaElement> createReplicaItemForSourceItem,
        Func<TSourceElement, TReplicaElement> getReplicaItemForSourceItem)
    {
        _sourceCollection = sourceCollection;
        _replicaCollection = replicaCollection;
        _createReplicaItemForSourceItem = createReplicaItemForSourceItem;
        _getReplicaItemForSourceItem = getReplicaItemForSourceItem;

        _sourceCollection.CollectionChanged += SourceCollectionOnCollectionChanged;

        _replicaCollection.AddRange(_sourceCollection.Select(_createReplicaItemForSourceItem));
    }

    [SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly")]
    private void SourceCollectionOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                var newStartingIndex = e.NewStartingIndex;
                e.NewItems?
                    .Cast<TSourceElement>()
                    .ForEach(x => AddReplicaElement(newStartingIndex++, x));
                break;
            case NotifyCollectionChangedAction.Move:
                _replicaCollection.Move(e.OldStartingIndex, e.NewStartingIndex);
                break;
            case NotifyCollectionChangedAction.Remove:
                RemoveElements(e.OldStartingIndex,
                    e.OldItems?.Cast<TSourceElement>() ?? []);
                break;
            case NotifyCollectionChangedAction.Replace:
                ReplaceElement(e);
                break;
            case NotifyCollectionChangedAction.Reset:
                _replicaCollection.Clear();
                _replicaCollection.AddRange(_sourceCollection.Select(_createReplicaItemForSourceItem));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(e) + "." + nameof(e.Action),
                    $"Unknown action '{e.Action}'");
        }
    }

    private void ReplaceElement(NotifyCollectionChangedEventArgs eventArgs)
    {
        if (eventArgs is { NewStartingIndex: <= -1, OldStartingIndex: <= -1 } ||
            eventArgs.NewStartingIndex != eventArgs.OldStartingIndex || eventArgs.NewItems == null)
        {
            return;
        }

        var masterElement = eventArgs.NewItems.Cast<TSourceElement>().First();

        _replicaCollection[eventArgs.NewStartingIndex] =
            _createReplicaItemForSourceItem(masterElement);
    }

    private void AddReplicaElement(int newStartingIndex, TSourceElement masterElement)
    {
        if (newStartingIndex == -1)
        {
            _replicaCollection.Add(_createReplicaItemForSourceItem(masterElement));
        }
        else
        {
            _replicaCollection.Insert(newStartingIndex, _createReplicaItemForSourceItem(masterElement));
        }
    }

    private void RemoveElements(int oldStartingIndex, IEnumerable<TSourceElement> sourceElements)
    {
        if (oldStartingIndex == -1)
        {
            sourceElements.ForEach(x => _replicaCollection.Remove(_getReplicaItemForSourceItem(x)));
        }
        else
        {
            _replicaCollection.RemoveAt(oldStartingIndex);
        }
    }

    /// <summary>
    ///     Unsubscribes from the source collection's change notifications, stopping synchronization.
    /// </summary>
    public void Dispose()
    {
        _sourceCollection.CollectionChanged -= SourceCollectionOnCollectionChanged;
    }
}
