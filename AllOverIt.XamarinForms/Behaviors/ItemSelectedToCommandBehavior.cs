using AllOverIt.XamarinForms.Behaviors.Base;
using System.Windows.Input;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Behaviors
{
  public class ItemSelectedToCommandBehavior : BehaviorBase<ListView>
  {
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ItemSelectedToCommandBehavior));

    public ICommand Command
    {
      get => (ICommand)GetValue(CommandProperty);
      set => SetValue(CommandProperty, value);
    }

    protected override void OnAttachedTo(ListView bindable)
    {
      base.OnAttachedTo(bindable);

      bindable.ItemSelected += OnItemSelected;
    }

    protected override void OnDetachingFrom(ListView bindable)
    {
      base.OnDetachingFrom(bindable);

      bindable.ItemSelected -= OnItemSelected;
    }

    private void OnItemSelected(object sender, SelectedItemChangedEventArgs eventArgs)
    {
      Command.Execute(null);
    }
  }
}