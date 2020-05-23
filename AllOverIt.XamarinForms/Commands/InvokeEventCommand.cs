using System.Windows.Input;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Commands
{
  /// <summary>
  /// A bindable class that represents a command handler for an event that is typically used in conjunction with <see cref="Behaviors.EventToCommandBehavior"/>.
  /// </summary>
  /// <example>
  /// <Entry.Behaviors>
  ///   <behaviors:EventToCommandBehavior EventName="EntryFocused">
  ///     <commands:InvokeEventCommand Command="{Binding EntryFocusedCommand}" />
  ///   </behaviors:EventToCommandBehavior>
  /// </Entry.Behaviors>
  /// </example>
  public sealed class InvokeEventCommand : EventCommandBase
  {
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(InvokeEventCommand));

    /// <summary>
    /// The command to be executed.
    /// </summary>
    public ICommand Command
    {
      get => (ICommand)GetValue(CommandProperty);
      set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(InvokeEventCommand));

    /// <summary>
    /// An optional parameter passed to the command when it is executed.
    /// </summary>
    /// <remarks>When used with <see cref="Behaviors.EventToCommandBehavior"/>, if the event raised has an event argument then this is passed
    /// to the command instead of this command parameter.</remarks>
    public object CommandParameter
    {
      get => GetValue(CommandParameterProperty);
      set => SetValue(CommandParameterProperty, value);
    }

    // refers to a converter for the Execute() parameter (such as event args via EventToCommandBehavior)
    public static readonly BindableProperty ConverterProperty = BindableProperty.Create(nameof(Converter), typeof(IValueConverter), typeof(InvokeEventCommand));

    /// <summary>
    /// An optional converter to apply to the command parameter.
    /// </summary>
    public IValueConverter Converter
    {
      get => (IValueConverter)GetValue(ConverterProperty);
      set => SetValue(ConverterProperty, value);
    }

    public static readonly BindableProperty ConverterParameterProperty = BindableProperty.Create(nameof(ConverterParameter), typeof(object), typeof(InvokeEventCommand));

    /// <summary>
    /// An optional parameter to pass to the <see cref="Converter"/>, when used.
    /// </summary>
    public object ConverterParameter
    {
      get => GetValue(ConverterParameterProperty);
      set => SetValue(ConverterParameterProperty, value);
    }

    /// <summary>
    /// The command handler executed when an associated event is raised.
    /// </summary>
    /// <param name="sender">The object instance on which the event is being raised.</param>
    /// <param name="parameter">The event argument associated with the event that is being raised.</param>
    /// <returns>True if the command executed, otherwise false.</returns>
    public override bool Execute(object sender, object parameter)
    {
      if (Command == null)
      {
        return false;
      }

      var resolvedParameter = CommandParameter;

      if (resolvedParameter == null)
      {
        resolvedParameter = Converter != null
          ? Converter.Convert(parameter, typeof(object), ConverterParameter, null)
          : parameter;
      }

      if (!Command.CanExecute(resolvedParameter))
      {
        return false;
      }

      Command.Execute(resolvedParameter);

      return true;
    }
  }
}