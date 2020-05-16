using AllOverIt.XamarinForms.Exceptions;
using AllOverIt.XamarinForms.Mvvm;
using AllOverIt.XamarinForms.Tasks;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AllOverIt.XamarinForms.Commands
{
  // based on https://github.com/johnthiriet/AsyncVoid

  public class AsyncCommand : IAsyncCommand
  {
    private readonly WeakEventManager _weakEventManager = new WeakEventManager();

    private bool _isExecuting;
    private readonly Func<Task> _execute;
    private readonly Func<bool> _canExecute;
    private readonly Action<Exception> _exceptionHandler;
    private readonly bool _continueOnCapturedContext;

    public event EventHandler CanExecuteChanged
    {
      add => _weakEventManager.AddEventHandler(value);
      remove => _weakEventManager.RemoveEventHandler(value);
    }

    public AsyncCommand(Func<Task> execute)
      : this(execute, () => true, true)
    {
    }

    public AsyncCommand(Func<Task> execute, bool continueOnCapturedContext)
      : this(execute, () => true, continueOnCapturedContext)
    {
    }

    public AsyncCommand(Func<Task> execute, Func<bool> canExecute, bool continueOnCapturedContext, IExceptionHandler exceptionHandler)
      : this(execute, canExecute, continueOnCapturedContext)
    {
      _ = exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler));

      _exceptionHandler = exceptionHandler.Handle;
    }

    public AsyncCommand(Func<Task> execute, Func<bool> canExecute, bool continueOnCapturedContext, Action<Exception> exceptionHandler)
      : this(execute, canExecute, continueOnCapturedContext)
    {
      _exceptionHandler = exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler));
    }

    private AsyncCommand(Func<Task> execute, Func<bool> canExecute, bool continueOnCapturedContext)
    {
      _execute = execute ?? throw new ArgumentNullException(nameof(execute));
      _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
      _continueOnCapturedContext = continueOnCapturedContext;
    }

    // Prevents re-entry. Not locking since commands are typically called from the main thread.
    public bool CanExecute() => !_isExecuting && _canExecute.Invoke();

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

  public class AsyncCommand<TType> : IAsyncCommand<TType>
  {
    private readonly WeakEventManager _weakEventManager = new WeakEventManager();

    private bool _isExecuting;
    private readonly Func<TType, Task> _execute;
    private readonly Func<TType, bool> _canExecute;
    private readonly Action<Exception> _exceptionHandler;
    private readonly bool _continueOnCapturedContext;

    public event EventHandler CanExecuteChanged
    {
      add => _weakEventManager.AddEventHandler(value);
      remove => _weakEventManager.RemoveEventHandler(value);
    }

    public AsyncCommand(Func<TType, Task> execute)
      : this(execute, _ => true, true)
    {
    }

    public AsyncCommand(Func<TType, Task> execute, bool continueOnCapturedContext)
      : this(execute, _ => true, continueOnCapturedContext)
    {
    }

    public AsyncCommand(Func<TType, Task> execute, Func<TType, bool> canExecute, bool continueOnCapturedContext, IExceptionHandler exceptionHandler)
      : this(execute, canExecute, continueOnCapturedContext)
    {
      _ = exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler));

      _exceptionHandler = exceptionHandler.Handle;
    }

    public AsyncCommand(Func<TType, Task> execute, Func<TType, bool> canExecute, bool continueOnCapturedContext, Action<Exception> exceptionHandler)
      : this(execute, canExecute, continueOnCapturedContext)
    {
      _exceptionHandler = exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler));
    }

    private AsyncCommand(Func<TType, Task> execute, Func<TType, bool> canExecute, bool continueOnCapturedContext)
    {
      _execute = execute ?? throw new ArgumentNullException(nameof(execute));
      _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
      _continueOnCapturedContext = continueOnCapturedContext;
    }

    // Prevents re-entry. Not locking since commands are typically called from the main thread.
    public bool CanExecute(TType parameter) => !_isExecuting && _canExecute.Invoke(parameter);

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