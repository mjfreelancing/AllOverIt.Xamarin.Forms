using AllOverIt.Helpers;
using AllOverIt.XamarinForms.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AllOverIt.XamarinForms.Mvvm
{
  public abstract class ViewModelBase : ObservableObject
  {
    private bool _deferredNotification;
    private IList<string> _propertiesNotified;
    private bool _isBusy;
    private string _title;
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

    protected override void RaisePropertyChanged([CallerMemberName] string propertyName = "")
    {
      // not checking for PropertyChanged == null since this code allows for the event handler to be assigned just before
      // a deferred notification is performed (when the disposable from CreatePropertyChangeTrackingScope() is disposed of)

      if (_propertiesNotified != null)
      {
        if (_propertiesNotified.Contains(propertyName))
        {
          return;
        }

        _propertiesNotified.Add(propertyName);
      }

      if (!_deferredNotification)
      {
        base.RaisePropertyChanged(propertyName);
      }
    }

    protected IDisposable CreatePropertyChangeTrackingScope(bool deferredNotification)
    {
      void InitialiseNotification()
      {
        _deferredNotification = deferredNotification;
        _propertiesNotified = new List<string>();
      }

      void CleanupNotification()
      {
        if (_deferredNotification)
        {
          foreach (var propertyName in _propertiesNotified)
          {
            base.RaisePropertyChanged(propertyName);
          }
        }

        _deferredNotification = false;
        _propertiesNotified = null;
      }

      return new Raii(InitialiseNotification, CleanupNotification);
    }
  }
}
