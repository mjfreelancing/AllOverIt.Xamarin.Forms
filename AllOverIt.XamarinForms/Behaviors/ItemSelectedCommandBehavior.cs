using AllOverIt.XamarinForms.Behaviors.Base;
using System.Windows.Input;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Behaviors
{
  public class ItemSelectedCommandBehavior : BehaviorBase<ListView>
  {
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ItemSelectedCommandBehavior));

    // refers to a converter for the event args
    public static readonly BindableProperty ConverterProperty = BindableProperty.Create(nameof(Converter), typeof(IValueConverter), typeof(InvokeEventCommand));
    public static readonly BindableProperty ConverterParameterProperty = BindableProperty.Create(nameof(ConverterParameter), typeof(object), typeof(InvokeEventCommand));


    public ICommand Command
    {
      get => (ICommand)GetValue(CommandProperty);
      set => SetValue(CommandProperty, value);
    }

    public IValueConverter Converter
    {
      get => (IValueConverter)GetValue(ConverterProperty);
      set => SetValue(ConverterProperty, value);
    }

    public object ConverterParameter
    {
      get => GetValue(ConverterParameterProperty);
      set => SetValue(ConverterParameterProperty, value);
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
      var resolvedArgs = Converter != null
        ? Converter.Convert(eventArgs, typeof(object), ConverterParameter, null)
        : eventArgs;

      if (Command.CanExecute(resolvedArgs))
      {
        Command.Execute(resolvedArgs);
      }
    }
  }
}