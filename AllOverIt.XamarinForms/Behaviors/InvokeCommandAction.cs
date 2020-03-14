using AllOverIt.XamarinForms.Behaviors.Base;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Behaviors
{
  public sealed class InvokeCommandAction : BehaviorAction
  {
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(InvokeCommandAction));
    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(InvokeCommandAction));
    public static readonly BindableProperty ConverterProperty = BindableProperty.Create(nameof(Converter), typeof(IValueConverter), typeof(InvokeCommandAction));
    public static readonly BindableProperty ConverterParameterProperty = BindableProperty.Create(nameof(ConverterParameter), typeof(object), typeof(InvokeCommandAction));

    public ICommand Command
    {
      get => (ICommand)GetValue(CommandProperty);
      set => SetValue(CommandProperty, value);
    }

    public object CommandParameter
    {
      get => GetValue(CommandParameterProperty);
      set => SetValue(CommandParameterProperty, value);
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

    public override Task<bool> Execute(object sender, object parameter)
    {
      if (Command == null)
      {
        return Task.FromResult(false);
      }

      var resolvedParameter = CommandParameter;

      if (resolvedParameter == null)
      {
        resolvedParameter = Converter != null 
          ? Converter.Convert(parameter, typeof(object), ConverterParameter, null) 
          : parameter;

        if (!Command.CanExecute(resolvedParameter))
        {
          return Task.FromResult(false);
        }
      }

      Command.Execute(resolvedParameter);

      return Task.FromResult(true);
    }
  }
}