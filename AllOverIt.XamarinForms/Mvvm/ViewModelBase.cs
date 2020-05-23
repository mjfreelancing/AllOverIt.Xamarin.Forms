using AllOverIt.Helpers;
using AllOverIt.XamarinForms.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AllOverIt.XamarinForms.Mvvm
{
  /// <summary>
  /// An abstract base class for view models.
  /// </summary>
  public abstract class ViewModelBase : ObservableObject
  {
    private bool _isBusy;
    private string _title;
    private IList<string> _deferredNotifications;
    private readonly Lazy<ILogger> _logger;

    protected ILogger Logger => _logger?.Value;

    /// <summary>
    /// Indicates if the view model is busy.
    /// </summary>
    public bool IsBusy
    {
      get => _isBusy;
      set => SetProperty(ref _isBusy, value);
    }

    /// <summary>
    /// A title for the view model.
    /// </summary>
    public string Title
    {
      get => _title ?? string.Empty;
      set => SetProperty(ref _title, value);
    }

    /// <summary>
    /// Default initializes the view model.
    /// </summary>
    /// <remarks><see cref="Logger"/> will be null when using this constructor.</remarks>
    protected ViewModelBase()
    {
    }

    /// <summary>
    /// Initializes the view model with an instance of an <see cref="ILogger"/>.
    /// </summary>
    /// <param name="logger"></param>
    protected ViewModelBase(ILogger logger)
      : this(new Lazy<ILogger>(() => logger))
    {
    }

    /// <summary>
    /// Initializes the view model with an instance of a <see cref="Lazy{ILogger}"/>.
    /// </summary>
    /// <param name="logger"></param>
    protected ViewModelBase(Lazy<ILogger> logger)
    {
      _logger = logger;
    }

    /// <summary>
    /// Raises property change notifications or tracks them for deferred notification.
    /// </summary>
    /// <param name="propertyName"></param>
    public override void RaisePropertyChanged([CallerMemberName] string propertyName = "")
    {
      // not checking for _propertiesNotified == null since this code allows for the event handler to be assigned just before
      // a deferred notification is performed (when the IDisposable from CreateDeferredPropertyChangeScope() is disposed of)

      if (_deferredNotifications != null)
      {
        if (_deferredNotifications.Contains(propertyName))
        {
          return;
        }

        _deferredNotifications.Add(propertyName);
      }
      else
      {
        base.RaisePropertyChanged(propertyName);
      }
    }

    /// <summary>
    /// Property change notifications will be tracked and later executed when the returned <see cref="IDisposable"/> is disposed.
    /// </summary>
    /// <returns></returns>
    public IDisposable CreateDeferredPropertyChangeScope()
    {
      void InitialiseNotification()
      {
        if (_deferredNotifications != null)
        {
          throw new InvalidOperationException("Deferred property notification cannot be nested");
        }

        _deferredNotifications = new List<string>();
      }

      void CleanupNotification()
      {
        foreach (var propertyName in _deferredNotifications)
        {
          base.RaisePropertyChanged(propertyName);
        }

        _deferredNotifications = null;
      }

      return new Raii(InitialiseNotification, CleanupNotification);
    }
  }
}
