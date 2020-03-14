using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Behaviors.Base
{
  public class BindableObjectCollection<TBindable> : BindableObject, IEnumerable<TBindable>, INotifyCollectionChanged
    where TBindable : BindableObject
  {
    private readonly IList<TBindable> _items = new List<TBindable>();

    public event NotifyCollectionChangedEventHandler CollectionChanged;
    public int Count => _items.Count;

    public int IndexOf(TBindable item)
    {
      return _items.IndexOf(item);
    }

    public void Insert(int index, TBindable item)
    {
      _items.Insert(index, item);

      CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
    }

    public void RemoveAt(int index)
    {
      var oldItem = _items[index];
      _items.RemoveAt(index);

      CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItem, index));
    }

    public TBindable this[int index]
    {
      get => _items[index];
      set
      {
        var oldItem = _items[index];
        _items[index] = value;

        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, oldItem));
      }
    }

    public void Add(TBindable item)
    {
      _items.Add(item);

      CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, Count - 1));
    }

    public void Clear()
    {
      _items.Clear();

      CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public bool Contains(TBindable item)
    {
      return _items.Contains(item);
    }

    public void CopyTo(TBindable[] array, int arrayIndex)
    {
      _items.CopyTo(array, arrayIndex);
    }

    public bool Remove(TBindable item)
    {
      var oldIndex = IndexOf(item);

      if (!_items.Remove(item))
      {
        return false;
      }

      CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, oldIndex));

      return true;
    }

    public IEnumerator<TBindable> GetEnumerator()
    {
      return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}