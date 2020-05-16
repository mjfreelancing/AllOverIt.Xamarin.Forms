using AllOverIt.Helpers;
using AllOverIt.XamarinForms.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AllOverIt.XamarinForms.Mvvm
{
  public abstract class ViewModelBase : ObservableObject
  {
    private bool _isBusy;
    private string _title;
    private IList<string> _deferredNotifications;
    private readonly Lazy<ILogger> _logger;

    protected ILogger Logger => _logger?.Value;

    public bool IsBusy
    {
      get => _isBusy;
      set => SetProperty(ref _isBusy, value);
    }

    public string Title
    {
      get => _title ?? string.Empty;
      set => SetProperty(ref _title, value);
    }

    protected ViewModelBase()
    {
    }

    protected ViewModelBase(Lazy<ILogger> logger)
    {
      _logger = logger;
    }

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
