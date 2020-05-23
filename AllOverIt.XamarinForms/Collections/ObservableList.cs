using AllOverIt.Extensions;
using AllOverIt.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace AllOverIt.XamarinForms.Collections
{
  // ObservableList is based on ObservableCollection found at https://github.com/jamesmontemagno/mvvm-helpers
  // ObservableList is interface based and is not 100% compatible with ObservableCollection

  /// <summary>
  /// A strongly typed observable list that provides notifications when items are added, removed, or the list is refreshed.
  /// </summary>
  /// <typeparam name="TType">The type to be stored in the list.</typeparam>
  public class ObservableList<TType> : ObservableCollection<TType>
  {
    /// <summary>
    /// Default initializes an empty list.
    /// </summary>
    public ObservableList()
    {
    }

    /// <summary>
    /// Initializes the list with an existing collection of items.
    /// </summary>
    /// <param name="collection">The collection of items to initialize the list with.</param>
    public ObservableList(IEnumerable<TType> collection)
      : base(collection)
    {
    }

    /// <summary>
    /// Adds one or more items to the list and raises a notification if items are added.
    /// </summary>
    /// <param name="collection">The collection of items to add to the collection.</param>
    /// <param name="mode">The notification mode. Valid values include <see cref="NotifyCollectionChangedAction.Add"/> or
    /// <see cref="NotifyCollectionChangedAction.Reset"/>.</param>
    public void AddRange(IEnumerable<TType> collection, NotifyCollectionChangedAction mode = NotifyCollectionChangedAction.Add)
    {
      var newItems = collection.WhenNotNull(nameof(collection)).AsReadOnlyList();

      if (mode != NotifyCollectionChangedAction.Add && mode != NotifyCollectionChangedAction.Reset)
      {
        throw new ArgumentException($"{mode} is an invalid option for AddRange()");
      }

      CheckReentrancy();

      if (newItems.Count == 0)
      {
        return;
      }

      AddItems(newItems);

      if (mode == NotifyCollectionChangedAction.Reset)
      {
        RaiseChangeNotificationEvents(NotifyCollectionChangedAction.Reset);
      }
      else
      {
        var startingIndex = Count - newItems.Count;
        RaiseChangeNotificationEvents(NotifyCollectionChangedAction.Add, (IList)newItems, startingIndex);
      }
    }

    /// <summary>
    /// Removes one or more items from the list and raises a notification if there is a change.
    /// </summary>
    /// <param name="collection">The collection of items to remove from the collection.</param>
    /// <param name="mode">The notification mode. Valid values include <see cref="NotifyCollectionChangedAction.Remove"/> or
    /// <see cref="NotifyCollectionChangedAction.Reset"/>.</param>
    public void RemoveRange(IEnumerable<TType> collection, NotifyCollectionChangedAction mode = NotifyCollectionChangedAction.Remove)
    {
      var newItems = collection.WhenNotNull(nameof(collection)).AsReadOnlyList();

      if (mode != NotifyCollectionChangedAction.Remove && mode != NotifyCollectionChangedAction.Reset)
      {
        throw new ArgumentException($"{mode} is an invalid option for RemoveRange()");
      }

      CheckReentrancy();

      if (mode == NotifyCollectionChangedAction.Reset)
      {
        RemoveUsingNotificationReset(newItems);
      }
      else
      {
        RemoveUsingNotificationRemove(newItems);
      }
    }

    /// <summary>
    /// Replaces the current list of items with a single item.
    /// </summary>
    /// <param name="item">The item that replaces the current list of items.</param>
    /// <remarks>If the provided <see cref="item"/> replaces the current list contents then a <see cref="NotifyCollectionChangedAction.Reset"/>
    /// notification will be raised.</remarks>
    public void Replace(TType item) => ReplaceRange(new TType[] { item });

    public void ReplaceRange(IEnumerable<TType> collection)
    {
      var newItems = collection.WhenNotNull(nameof(collection)).AsReadOnlyList();

      CheckReentrancy();
      
      var originallyEmpty = Items.Count == 0;

      Items.Clear();

      AddItems(newItems);

      if (originallyEmpty && Items.Count == 0)
      {
        return;
      }

      RaiseChangeNotificationEvents(NotifyCollectionChangedAction.Reset);
    }

    private void RemoveUsingNotificationReset(IEnumerable<TType> collection)
    {
      // using this approach to avoid the (potential) creation of a new list 
      var originalCount = Items.Count;

      foreach (var item in collection)
      {
        Items.Remove(item);
      }

      if (originalCount != Items.Count)
      {
        RaiseChangeNotificationEvents(NotifyCollectionChangedAction.Reset);
      }
    }

    private void RemoveUsingNotificationRemove(IEnumerable<TType> collection)
    {
      // capture all items removed
      var changedItems = collection.Where(item => Items.Remove(item)).ToList();

      if (changedItems.Count != 0)
      {
        // no 'startingIndex' because the removed items may not have been sequential
        RaiseChangeNotificationEvents(NotifyCollectionChangedAction.Remove, changedItems);
      }
    }

    private void AddItems(IEnumerable<TType> collection)
    {
      foreach (var item in collection)
      {
        Items.Add(item);
      }
    }

    private void RaiseChangeNotificationEvents(NotifyCollectionChangedAction action, IList changedItems = null, int startingIndex = -1)
    {
      OnPropertyChanged(ObservableListEventArgs.CountPropertyChanged);
      OnPropertyChanged(ObservableListEventArgs.IndexerPropertyChanged);
      OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, changedItems, startingIndex));
    }
  }

  // cannot nest within ObservableList because a static field should not be declared within a generic class
  internal static class ObservableListEventArgs
  {
    internal static readonly PropertyChangedEventArgs CountPropertyChanged = new PropertyChangedEventArgs("Count");
    internal static readonly PropertyChangedEventArgs IndexerPropertyChanged = new PropertyChangedEventArgs("Item[]");
  }
}