using AllOverIt.Helpers;
using AllOverIt.XamarinForms.Exceptions;
using AllOverIt.XamarinForms.Mvvm;
using AllOverIt.XamarinForms.Tasks;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AllOverIt.XamarinForms.Commands
{
  // based on https://github.com/johnthiriet/AsyncVoid

  /// <summary>
  /// Implements an asynchronous command (<see cref="IAsyncCommand"/>) that wraps a <see cref="Func{Task}"/>.
  /// </summary>
  public class AsyncCommand : IAsyncCommand
  {
    private readonly WeakEventManager _weakEventManager = new WeakEventManager();

    private bool _isExecuting;
    private readonly Func<Task> _execute;
    private readonly Func<bool> _canExecute;
    private readonly Action<Exception> _exceptionHandler;
    private readonly bool _continueOnCapturedContext;

    /// <summary>
    /// This event is raised when there is a request to execute the command.
    /// </summary>
    /// <remarks>The event is raised even if <see cref="CanExecute"/> returns false as there may be a change that has enabled / disabled the command.</remarks>
    public event EventHandler CanExecuteChanged
    {
      add => _weakEventManager.AddEventHandler(value);
      remove => _weakEventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Initializes the <see cref="AsyncCommand"/> with the <see cref="Func{Task}"/> to be executed.
    /// </summary>
    /// <param name="execute">The <see cref="Func{Task}"/> to be executed.</param>
    public AsyncCommand(Func<Task> execute)
      : this(execute, () => true, true)
    {
    }

    /// <summary>
    /// Initializes the <see cref="AsyncCommand"/> with the <see cref="Func{Task}"/> to be executed.
    /// </summary>
    /// <param name="execute">The <see cref="Func{Task}"/> to be executed.</param>
    /// <param name="continueOnCapturedContext">Indicates if the awaited task should be continued on the calling context.</param>
    public AsyncCommand(Func<Task> execute, bool continueOnCapturedContext)
      : this(execute, () => true, continueOnCapturedContext)
    {
    }

    /// <summary>
    /// Initializes the <see cref="AsyncCommand"/> with the <see cref="Func{Task}"/> to be executed.
    /// </summary>
    /// <param name="execute">The <see cref="Func{Task}"/> to be executed.</param>
    /// <param name="canExecute">A Func that is called before each command execution to indicate if the command should be executed.</param>
    /// <param name="continueOnCapturedContext">Indicates if the awaited task should be continued on the calling context.</param>
    /// <param name="exceptionHandler">An implementation if <see cref="IExceptionHandler"/> that will be invoked if an exception is raised when the command is executed.</param>
    public AsyncCommand(Func<Task> execute, Func<bool> canExecute, bool continueOnCapturedContext, IExceptionHandler exceptionHandler)
      : this(execute, canExecute, continueOnCapturedContext)
    {
      _ = exceptionHandler.WhenNotNull(nameof(exceptionHandler));

      _exceptionHandler = exceptionHandler.Handle;
    }

    /// <summary>
    /// Initializes the <see cref="AsyncCommand"/> with the <see cref="Func{Task}"/> to be executed.
    /// </summary>
    /// <param name="execute">The <see cref="Func{Task}"/> to be executed.</param>
    /// <param name="canExecute">A Func that is called before each command execution to indicate if the command should be executed.</param>
    /// <param name="continueOnCapturedContext">Indicates if the awaited task should be continued on the calling context.</param>
    /// <param name="exceptionHandler">An Action that will be invoked if an exception is raised when the command is executed.</param>
    public AsyncCommand(Func<Task> execute, Func<bool> canExecute, bool continueOnCapturedContext, Action<Exception> exceptionHandler)
      : this(execute, canExecute, continueOnCapturedContext)
    {
      _exceptionHandler = exceptionHandler.WhenNotNull(nameof(exceptionHandler));
    }

    private AsyncCommand(Func<Task> execute, Func<bool> canExecute, bool continueOnCapturedContext)
    {
      _execute = execute.WhenNotNull(nameof(execute));
      _canExecute = canExecute.WhenNotNull(nameof(canExecute));
      _continueOnCapturedContext = continueOnCapturedContext;
    }
    
    /// <summary>
    /// Determines if the command can be executed.
    /// </summary>
    /// <returns>True if the command can be executed, otherwise false. If this method is called while the command is currently being executed then false will be returned.</returns>
    public bool CanExecute() => !_isExecuting && _canExecute.Invoke();  // Not locking since commands are typically called from the main thread.

    /// <summary>
    /// Asynchronously executes the command.
    /// </summary>
    /// <returns>An awaitable task for the command.</returns>
    public async Task ExecuteAsync()
    {
      if (CanExecute())
      {
        try
        {
          _isExecuting = true;
          await _execute.Invoke().ConfigureAwait(_continueOnCapturedContext);
        }
        catch (Exception ex) when (_exceptionHandler != null)
        {
          _exceptionHandler.Invoke(ex);
        }
        finally
        {
          _isExecuting = false;
        }
      }

      // raised to indicate there may be a change that has enabled / disabled the command
      RaiseCanExecuteChanged();
    }

    bool ICommand.CanExecute(object parameter) => CanExecute();

    void ICommand.Execute(object parameter) => ExecuteAsync().SafeFireAndForget(_exceptionHandler, _continueOnCapturedContext);

    private void RaiseCanExecuteChanged() => _weakEventManager.HandleEvent(this, EventArgs.Empty, nameof(CanExecuteChanged));
  }

  /// <summary>
  /// Implements an asynchronous command (<see cref="IAsyncCommand{TType}"/>) that wraps a <see cref="Func{TType, Task}"/>.
  /// </summary>
  public class AsyncCommand<TType> : IAsyncCommand<TType>
  {
    private readonly WeakEventManager _weakEventManager = new WeakEventManager();

    private bool _isExecuting;
    private readonly Func<TType, Task> _execute;
    private readonly Func<TType, bool> _canExecute;
    private readonly Action<Exception> _exceptionHandler;
    private readonly bool _continueOnCapturedContext;

    /// <summary>
    /// This event is raised when there is a request to execute the command.
    /// </summary>
    /// <remarks>The event is raised even if <see cref="CanExecute"/> returns false as there may be a change that has enabled / disabled the command.</remarks>
    public event EventHandler CanExecuteChanged
    {
      add => _weakEventManager.AddEventHandler(value);
      remove => _weakEventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Initializes the <see cref="AsyncCommand{TType}"/> with the <see cref="Func{Task}"/> to be executed.
    /// </summary>
    /// <param name="execute">The <see cref="Func{Task}"/>to be executed.</param>
    public AsyncCommand(Func<TType, Task> execute)
      : this(execute, _ => true, true)
    {
    }

    /// <summary>
    /// Initializes the <see cref="AsyncCommand{TType}"/> with the <see cref="Func{TType, Task}"/> to be executed.
    /// </summary>
    /// <param name="execute">The <see cref="Func{TType, Task}"/> to be executed.</param>
    /// <param name="continueOnCapturedContext">Indicates if the awaited task should be continued on the calling context.</param>
    public AsyncCommand(Func<TType, Task> execute, bool continueOnCapturedContext)
      : this(execute, _ => true, continueOnCapturedContext)
    {
    }

    /// <summary>
    /// Initializes the <see cref="AsyncCommand{TType}"/> with the <see cref="Func{TType, Task}"/> to be executed.
    /// </summary>
    /// <param name="execute">The <see cref="Func{TType, Task}"/> to be executed.</param>
    /// <param name="canExecute">A Func that is called before each command execution to indicate if the command should be executed.</param>
    /// <param name="continueOnCapturedContext">Indicates if the awaited task should be continued on the calling context.</param>
    /// <param name="exceptionHandler">An implementation if <see cref="IExceptionHandler"/> that will be invoked if an exception is raised when the command is executed.</param>
    public AsyncCommand(Func<TType, Task> execute, Func<TType, bool> canExecute, bool continueOnCapturedContext, IExceptionHandler exceptionHandler)
      : this(execute, canExecute, continueOnCapturedContext)
    {
      _ = exceptionHandler.WhenNotNull(nameof(exceptionHandler));

      _exceptionHandler = exceptionHandler.Handle;
    }

    /// <summary>
    /// Initializes the <see cref="AsyncCommand{Task}"/> with the <see cref="Func{TType, Task}"/> to be executed.
    /// </summary>
    /// <param name="execute">The <see cref="Func{TType, Task}"/> to be executed.</param>
    /// <param name="canExecute">A Func that is called before each command execution to indicate if the command should be executed.</param>
    /// <param name="continueOnCapturedContext">Indicates if the awaited task should be continued on the calling context.</param>
    /// <param name="exceptionHandler">An Action that will be invoked if an exception is raised when the command is executed.</param>
    public AsyncCommand(Func<TType, Task> execute, Func<TType, bool> canExecute, bool continueOnCapturedContext, Action<Exception> exceptionHandler)
      : this(execute, canExecute, continueOnCapturedContext)
    {
      _exceptionHandler = exceptionHandler.WhenNotNull(nameof(exceptionHandler));
    }

    private AsyncCommand(Func<TType, Task> execute, Func<TType, bool> canExecute, bool continueOnCapturedContext)
    {
      _execute = execute.WhenNotNull(nameof(execute));
      _canExecute = canExecute.WhenNotNull(nameof(canExecute));
      _continueOnCapturedContext = continueOnCapturedContext;
    }

    /// <summary>
    /// Determines if the command can be executed with the provided parameter.
    /// </summary>
    /// <returns>True if the command can be executed, otherwise false. If this method is called while the command is currently being executed then false will be returned.</returns>
    public bool CanExecute(TType parameter) => !_isExecuting && _canExecute.Invoke(parameter);  // Not locking since commands are typically called from the main thread.

    /// <summary>
    /// Asynchronously executes the command with the provided parameter.
    /// </summary>
    /// <returns>An awaitable task for the command.</returns>
    public async Task ExecuteAsync(TType parameter)
    {
      if (CanExecute(parameter))
      {
        try
        {
          _isExecuting = true;
          await _execute(parameter).ConfigureAwait(_continueOnCapturedContext);
        }
        catch (Exception ex) when (_exceptionHandler != null)
        {
          _exceptionHandler.Invoke(ex);
        }
        finally
        {
          _isExecuting = false;
        }
      }

      // raised to indicate there may be a change that has enabled / disabled the command
      RaiseCanExecuteChanged();
    }

    bool ICommand.CanExecute(object parameter) => CanExecute((TType)parameter);

    void ICommand.Execute(object parameter) => ExecuteAsync((TType)parameter).SafeFireAndForget(_exceptionHandler, _continueOnCapturedContext);

    private void RaiseCanExecuteChanged() => _weakEventManager.HandleEvent(this, EventArgs.Empty, nameof(CanExecuteChanged));
  }
}