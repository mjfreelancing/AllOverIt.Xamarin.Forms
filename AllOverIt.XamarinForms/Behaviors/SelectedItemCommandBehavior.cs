using AllOverIt.XamarinForms.Behaviors.Base;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Behaviors
{
  /// <summary>
  /// A behavior that executes a command when a ListView item is selected.
  /// </summary>
  public class SelectedItemCommandBehavior : BehaviorBase<ListView>
  {
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(SelectedItemCommandBehavior));

    /// <summary>
    /// The command to execute when a ListView item is selected.
    /// </summary>
    public ICommand Command
    {
      get => (ICommand)GetValue(CommandProperty);
      set => SetValue(CommandProperty, value);
    }

    // refers to a converter for the event args
    public static readonly BindableProperty ConverterProperty = BindableProperty.Create(nameof(Converter), typeof(IValueConverter), typeof(SelectedItemCommandBehavior));

    /// <summary>
    /// An optional converter to apply against the <see cref="SelectedItemChangedEventArgs"/> associated with the selected item.
    /// </summary>
    public IValueConverter Converter
    {
      get => (IValueConverter)GetValue(ConverterProperty);
      set => SetValue(ConverterProperty, value);
    }

    public static readonly BindableProperty ConverterParameterProperty = BindableProperty.Create(nameof(ConverterParameter), typeof(object), typeof(SelectedItemCommandBehavior));

    /// <summary>
    /// An optional parameter to pass to the <see cref="Converter"/>, when used.
    /// </summary>
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
      _ = Command ?? throw new InvalidOperationException("No Command has been provided");

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