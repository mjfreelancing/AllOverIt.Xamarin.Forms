using AllOverIt.XamarinForms.Behaviors.Base;
using System.Windows.Input;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Behaviors
{
  public sealed class InvokeEventCommand : EventCommand
  {
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(InvokeEventCommand));

    public ICommand Command
    {
      get => (ICommand)GetValue(CommandProperty);
      set => SetValue(CommandProperty, value);
    }

    // if the event raised has an event args then this is passed to the command instead of this command parameter
    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(InvokeEventCommand));

    public object CommandParameter
    {
      get => GetValue(CommandParameterProperty);
      set => SetValue(CommandParameterProperty, value);
    }


    // refers to a converter for the event args
    public static readonly BindableProperty ConverterProperty = BindableProperty.Create(nameof(Converter), typeof(IValueConverter), typeof(InvokeEventCommand));

    public IValueConverter Converter
    {
      get => (IValueConverter)GetValue(ConverterProperty);
      set => SetValue(ConverterProperty, value);
    }

    public static readonly BindableProperty ConverterParameterProperty = BindableProperty.Create(nameof(ConverterParameter), typeof(object), typeof(InvokeEventCommand));

    public object ConverterParameter
    {
      get => GetValue(ConverterParameterProperty);
      set => SetValue(ConverterParameterProperty, value);
    }

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

        if (!Command.CanExecute(resolvedParameter))
        {
          return false;
        }
      }

      if (Command.CanExecute(resolvedParameter))
      {
        Command.Execute(resolvedParameter);
      }

      return true;
    }
  }
}