using AllOverIt.XamarinForms.Behaviors.Base;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Commands
{
  /// <summary>
  /// An abstract, bindable, class that represents a command handler for an event.
  /// </summary>
  public abstract class EventCommandBase : BindableObject, IEventCommand
  {
    public abstract bool Execute(object sender, object parameter);
  }
}