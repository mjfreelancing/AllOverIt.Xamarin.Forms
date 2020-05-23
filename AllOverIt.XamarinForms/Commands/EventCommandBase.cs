using AllOverIt.XamarinForms.Behaviors.Base;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Commands
{
  /// <summary>
  /// An abstract, bindable, class that represents a command handler for an event.
  /// </summary>
  public abstract class EventCommandBase : BindableObject, IEventCommand
  {
    /// <summary>
    /// The command handler executed when an associated event is raised.
    /// </summary>
    /// <param name="sender">The object instance on which the event is being raised.</param>
    /// <param name="parameter">The event argument associated with the event that is being raised.</param>
    /// <returns>True if the command executed, otherwise false.</returns>
    public abstract bool Execute(object sender, object parameter);
  }
}