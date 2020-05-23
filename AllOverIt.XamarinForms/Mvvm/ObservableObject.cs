using AllOverIt.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace AllOverIt.XamarinForms.Mvvm
{
  /// <summary>
  /// An abstract observable object implementing <see cref="INotifyPropertyChanged"/>.
  /// </summary>
  public abstract class ObservableObject : INotifyPropertyChanged
  {
    /// <summary>
    /// The event raised when a property changes state.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Instructs a property change notification for a specified property.
    /// </summary>
    /// <param name="propertyName">The name of the property being notified as changed.</param>
    public virtual void RaisePropertyChanged([CallerMemberName] string propertyName = "")
    {
      // null conditional operators are thread safe
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Instructs a property change notification for a specified property expressed as an expression.
    /// </summary>
    /// <typeparam name="TType">The property type.</typeparam>
    /// <param name="expression">The expression describing the property that has changed.</param>
    public virtual void RaisePropertyChanged<TType>(Expression<Func<TType>> expression)
    {
      var memberExpression = expression.UnwrapMemberExpression()
                             ?? throw new ArgumentException($"{nameof(expression)} is expected to be a LambdaExpression containing a MemberExpression");

      var propertyName = memberExpression.Member.Name;

      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Updates a property's backing store and raises a change notification.
    /// </summary>
    /// <typeparam name="TType">The type of the property.</typeparam>
    /// <param name="backingStore">The backing store maintaining the property's value.</param>
    /// <param name="value">The value to set. A change notification will only occur if the new value is different to the current value.</param>
    /// <param name="action">An optional <see cref="Action"/> to invoke after the change notification has occured.</param>
    /// <param name="propertyName">The name of the property.</param>
    protected void SetProperty<TType>(ref TType backingStore, TType value, Action action = null, [CallerMemberName] string propertyName = "")
    {
      if (EqualityComparer<TType>.Default.Equals(backingStore, value))
      {
        return;
      }

      backingStore = value;

      RaisePropertyChanged(propertyName);

      action?.Invoke();
    }
  }
}