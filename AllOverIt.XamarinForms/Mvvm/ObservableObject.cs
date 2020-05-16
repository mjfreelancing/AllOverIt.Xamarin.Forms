using AllOverIt.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace AllOverIt.XamarinForms.Mvvm
{
  public abstract class ObservableObject : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    public virtual void RaisePropertyChanged([CallerMemberName] string propertyName = "")
    {
      // null conditional operators are thread safe
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public virtual void RaisePropertyChanged<TType>(Expression<Func<TType>> expression)
    {
      var memberExpression = expression.UnwrapMemberExpression()
                             ?? throw new ArgumentException($"{nameof(expression)} is expected to be a LambdaExpression containing a MemberExpression");

      var propertyName = memberExpression.Member.Name;

      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

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