using System.Collections.Generic;
using System.Collections.Specialized;

namespace AllOverIt.XamarinForms.Collections
{
  /// <summary>
  /// Represents an observable collection that raises notifications when items are added, removed, or completely replaced.
  /// </summary>
  /// <typeparam name="TType">The type store in the collection.</typeparam>
  public interface IObservableList<in TType>
  {
    /// <summary>
    /// Appends new items into the collection and raises a single notification when the action is complete.
    /// </summary>
    /// <param name="collection">The source data to be appended to the current list. No action is performed if this collection is empty.</param>
    /// <param name="mode">This can be either <see cref="NotifyCollectionChangedAction.Add"/> or <see cref="NotifyCollectionChangedAction.Reset"/>.
    /// <para>
    /// When the mode is <see cref="NotifyCollectionChangedAction.Add"/> the notification provides the list of items added and the starting index.
    /// </para>
    /// <para>
    /// When the mode is <see cref="NotifyCollectionChangedAction.Reset"/> the notification simply indicates something dramatic has changed (no
    /// list of items or starting index).
    /// </para>
    /// </param>
    /// <exception cref="System.ArgumentNullException">The collection argument cannot be null.</exception>
    /// <exception cref="System.ArgumentException">An invalid mode was provided.</exception>
    /// <exception cref="System.InvalidOperationException">An attempt was made to modify the collection during a
    /// <see cref="System.Collections.ObjectModel.ObservableCollection{T}.CollectionChanged"/> event.</exception>
    void AddRange(IEnumerable<TType> collection, NotifyCollectionChangedAction mode = NotifyCollectionChangedAction.Add);

    /// <summary>
    /// Removes items from the collection, if found, and subsequently raises a single notification when the action is complete.
    /// </summary>
    /// <param name="collection">The source data containing items to be removed from the current list. Items are compared using
    /// <see cref="Comparer{T}.Default"/>. No action is performed if this collection is empty.</param>
    /// <param name="mode">This can be either <see cref="NotifyCollectionChangedAction.Remove"/> or <see cref="NotifyCollectionChangedAction.Reset"/>.
    /// <para>
    /// When the mode is <see cref="NotifyCollectionChangedAction.Remove"/> the notification provides the list of items removed. A starting index
    /// is not provided because the sequence of deletions may not be sequential.
    /// </para>
    /// <para>
    /// When the mode is <see cref="NotifyCollectionChangedAction.Reset"/> the notification simply indicates something dramatic has changed (no
    /// list of items or starting index).
    /// </para>
    /// </param>
    /// <exception cref="System.ArgumentNullException">The collection argument cannot be null.</exception>
    /// <exception cref="System.ArgumentException">An invalid mode was provided.</exception>
    /// <exception cref="System.InvalidOperationException">An attempt was made to modify the collection during a
    /// <see cref="System.Collections.ObjectModel.ObservableCollection{T}.CollectionChanged"/> event.</exception>
    void RemoveRange(IEnumerable<TType> collection, NotifyCollectionChangedAction mode = NotifyCollectionChangedAction.Remove);

    /// <summary>
    /// Clears the current list and appends a single item.
    /// </summary>
    /// <param name="item">The item to be added to the list after clearing it.</param>
    /// <exception cref="System.ArgumentNullException">The collection argument cannot be null.</exception>
    /// <exception cref="System.InvalidOperationException">An attempt was made to modify the collection during a
    /// <see cref="System.Collections.ObjectModel.ObservableCollection{T}.CollectionChanged"/> event.</exception>
    /// <remarks>The <see cref="System.Collections.ObjectModel.ObservableCollection{T}.CollectionChanged"/> event is raised with an Action mode
    /// of <see cref="NotifyCollectionChangedAction.Reset"/>.</remarks>
    void Replace(TType item);

    /// <summary>
    /// Clears the current list and replaces it with a copy of the provided collection.
    /// </summary>
    /// <param name="collection">The collection of items to be added to the list after clearing it. No action is performed if the
    /// original list was empty and this collection is empty.</param>
    /// <exception cref="System.ArgumentNullException">The collection argument cannot be null.</exception>
    /// <exception cref="System.InvalidOperationException">An attempt was made to modify the collection during a
    /// <see cref="System.Collections.ObjectModel.ObservableCollection{T}.CollectionChanged"/> event.</exception>
    /// <remarks>The <see cref="System.Collections.ObjectModel.ObservableCollection{T}.CollectionChanged"/> event is raised with an Action mode
    /// of <see cref="NotifyCollectionChangedAction.Reset"/>.</remarks>
    void ReplaceRange(IEnumerable<TType> collection);
  }
}