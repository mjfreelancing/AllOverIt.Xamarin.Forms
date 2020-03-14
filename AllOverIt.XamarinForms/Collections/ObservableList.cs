using AllOverIt.Extensions;
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

  // cannot nest within ObservableList because a static field should not be declared within a generic class
  internal static class ObservableListEventArgs
  {
    internal static readonly PropertyChangedEventArgs CountPropertyChanged = new PropertyChangedEventArgs("Count");
    internal static readonly PropertyChangedEventArgs IndexerPropertyChanged = new PropertyChangedEventArgs("Item[]");
  }

  public class ObservableList<TType> : ObservableCollection<TType>
  {
    public ObservableList()
    {
    }

    public ObservableList(IEnumerable<TType> collection)
      : base(collection)
    {
    }

    public void AddRange(IEnumerable<TType> collection, NotifyCollectionChangedAction mode = NotifyCollectionChangedAction.Add)
    {
      if (collection == null)
      {
        throw new ArgumentNullException(nameof(collection));
      }

      if (mode != NotifyCollectionChangedAction.Add && mode != NotifyCollectionChangedAction.Reset)
      {
        throw new ArgumentException($"{mode} is an invalid option for AddRange()");
      }

      CheckReentrancy();

      var newItems = collection.AsList();

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

    public void RemoveRange(IEnumerable<TType> collection, NotifyCollectionChangedAction mode = NotifyCollectionChangedAction.Remove)
    {
      if (collection == null)
      {
        throw new ArgumentNullException(nameof(collection));
      }

      if (mode != NotifyCollectionChangedAction.Remove && mode != NotifyCollectionChangedAction.Reset)
      {
        throw new ArgumentException($"{mode} is an invalid option for RemoveRange()");
      }

      CheckReentrancy();

      if (mode == NotifyCollectionChangedAction.Reset)
      {
        RemoveUsingNotificationReset(collection);
      }
      else
      {
        RemoveUsingNotificationRemove(collection);
      }
    }

    public void Replace(TType item) => ReplaceRange(new TType[] { item });

    public void ReplaceRange(IEnumerable<TType> collection)
    {
      if (collection == null)
      {
        throw new ArgumentNullException(nameof(collection));
      }

      CheckReentrancy();
      
      var originallyEmpty = Items.Count == 0;

      Items.Clear();

      AddItems(collection);

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
}