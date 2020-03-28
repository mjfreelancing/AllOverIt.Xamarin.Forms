using AllOverIt.XamarinForms.Logging;
using AllOverIt.XamarinForms.Mvvm;
using System;
using Xamarin.Forms;

namespace Behaviors.ViewModels
{
  public abstract class BehaviorViewModelBase : ViewModelBase
  {
    public BehaviorViewModelBase()
      : base(CreateLazyLogger())
    {
    }

    private static Lazy<ILogger> CreateLazyLogger()
    {
      return new Lazy<ILogger>(() => DependencyService.Resolve<ILogger>());
    }
  }
}