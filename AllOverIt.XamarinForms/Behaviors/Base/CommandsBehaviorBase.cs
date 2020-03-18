using System.Collections.Generic;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Behaviors.Base
{
  [ContentProperty("Commands")]
  public class CommandsBehaviorBase<TBindable> : AttachableBehavior<CommandsBehaviorBase<TBindable>, TBindable>
    where TBindable : VisualElement
  {
    public static readonly BindableProperty CommandsProperty = BindableProperty.Create(nameof(Commands), typeof(IList<EventCommand>), typeof(CommandsBehaviorBase<TBindable>));

    public IList<EventCommand> Commands => (IList<EventCommand>)GetValue(CommandsProperty);

    public CommandsBehaviorBase()
    {
      SetValue(CommandsProperty, new List<EventCommand>());
    }
  }
}