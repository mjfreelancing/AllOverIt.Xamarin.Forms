using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Behaviors.Base
{
  /// <summary>
  /// An abstract, bindable, class that represents a command handler for an event.
  /// </summary>
  /// <remarks>When executed, the current BindingContext is assigned to the command handler.</remarks>
  public abstract class EventCommand : BindableObject, IEventCommand
  {
    public abstract bool Execute(object sender, object parameter);
  }
}