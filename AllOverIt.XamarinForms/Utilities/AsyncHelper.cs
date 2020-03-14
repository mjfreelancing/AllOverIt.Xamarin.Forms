using AllOverIt.Extensions;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Utilities
{
  public static class AsyncHelper
  {
    public static void InvokeSafeAsync(Func<Task> action)
    {
      action.Invoke().SafeFireAndForget();
    }

    public static void InvokeSafeAsync<TParam>(Func<TParam, Task> action, TParam param)
    {
      action.Invoke(param).SafeFireAndForget();
    }

    public static ICommand CreateAsyncCommand(Func<Task> action)
    {
      return new Command(() => InvokeSafeAsync(action));
    }

    public static ICommand CreateAsyncCommand(Func<Task> action, Func<bool> canExecute)
    {
      return new Command(() => InvokeSafeAsync(action), canExecute);
    }

    public static ICommand CreateAsyncCommand<TParam>(Func<TParam, Task> action)
    {
      return new Command<TParam>(param => InvokeSafeAsync(action, param));
    }

    public static ICommand CreateAsyncCommand<TParam>(Func<TParam, Task> action, Func<TParam, bool> canExecute)
    {
      return new Command<TParam>(param => InvokeSafeAsync(action, param), canExecute);
    }
  }
}