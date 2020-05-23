using AllOverIt.XamarinForms.Exceptions;
using System;
using System.Threading.Tasks;

namespace AllOverIt.XamarinForms.Tasks
{
  public static class TaskHelper
  {
    private static Action<Exception> _defaultExceptionHandler;

    /// <summary>
    /// Sets a default exception handler that will be invoked if an exception occurs while calling one of the SafeFireAndForget() overloads.
    /// </summary>
    /// <param name="exceptionHandler">The default exception handler to be used.</param>
    public static void SetDefaultExceptionHandler(Action<Exception> exceptionHandler) => _defaultExceptionHandler = exceptionHandler;

    /// <summary>
    /// Awaits a task and invokes the default exception handler if an exception is raised.
    /// </summary>
    /// <param name="task">The task to be awaited.</param>
    /// <param name="continueOnCapturedContext">Indicates if execution should continue on the captured context.</param>
    /// <remarks>This method assumes the default exception handler has been configured. Without a handler, if this method is called from a
    /// void function and an exception occurs then the application is likely to abort with an unhandled exception.</remarks>
    public static void SafeFireAndForget(this Task task, bool continueOnCapturedContext = true)
      => DoSafeFireAndForget(task, continueOnCapturedContext, null);

    /// <summary>
    /// Awaits a task and invokes the provided exception handler if an exception is raised.
    /// </summary>
    /// <param name="task">The task to be awaited.</param>
    /// <param name="exceptionHandler">The exception handler to be invoked if an exception occurs while awaiting the task.</param>
    /// <param name="continueOnCapturedContext">Indicates if execution should continue on the captured context.</param>
    /// <remarks>If a default exception handler has been assigned then that will also be invoked in the event of an exception.</remarks>
    public static void SafeFireAndForget(this Task task, Action<Exception> exceptionHandler, bool continueOnCapturedContext = true)
      => DoSafeFireAndForget(task, continueOnCapturedContext, exceptionHandler);

    /// <summary>
    /// Awaits a task and invokes the provided exception handler if an exception is raised.
    /// </summary>
    /// <param name="task">The task to be awaited.</param>
    /// <param name="exceptionHandler">The exception handler to be invoked if an exception occurs while awaiting the task.</param>
    /// <param name="continueOnCapturedContext">Indicates if execution should continue on the captured context.</param>
    /// <remarks>If a default exception handler has been assigned then that will also be invoked in the event of an exception.</remarks>
    public static void SafeFireAndForget(this Task task, IExceptionHandler exceptionHandler, bool continueOnCapturedContext = true)
      => DoSafeFireAndForget(task, continueOnCapturedContext, exceptionHandler.Handle);

    private static async void DoSafeFireAndForget(Task task, bool continueOnCapturedContext, Action<Exception> exceptionHandler)
    {
      try
      {
        await task.ConfigureAwait(continueOnCapturedContext);
      }
      catch (Exception ex) when (_defaultExceptionHandler != null || exceptionHandler != null)
      {
        _defaultExceptionHandler?.Invoke(ex);
        exceptionHandler?.Invoke(ex);
      }
    }
  }
}