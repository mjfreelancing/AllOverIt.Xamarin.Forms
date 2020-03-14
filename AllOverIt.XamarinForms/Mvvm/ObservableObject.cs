using AllOverIt.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AllOverIt.XamarinForms.Mvvm
{
  public abstract class ObservableObject : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

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

    protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = "")
    {
      // null conditional operators are thread safe
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void RaisePropertyChanged<TType>(Expression<Func<TType>> expression)
    {
      if (!(expression is LambdaExpression lambdaExpression))
      {
        throw new ArgumentException($"{nameof(expression)} is expected to be a lambda expression.");
      }

      var propertyName = GetMemberInfo(lambdaExpression).Name;

      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

      throw new ArgumentException($"RaisePropertyChanged() expects a lambda expression.");
    }

    private static MemberInfo GetMemberInfo(LambdaExpression expression)
    {
      var operand = (MemberExpression)expression.Body.RemoveUnary();
     
      return operand.Member;
    }
  }
}