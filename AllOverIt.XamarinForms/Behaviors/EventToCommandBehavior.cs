using AllOverIt.XamarinForms.Behaviors.Base;
using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Behaviors
{
  // Based on https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/behaviors/creating
  // Based on https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/behaviors/reusable/event-to-command-behavior
  public class EventToCommandBehavior : BehaviorBase<View>
  {
    private Delegate _eventHandler;

    public static readonly BindableProperty EventNameProperty = BindableProperty.Create(nameof(EventName), typeof(string), typeof(EventToCommandBehavior), propertyChanged: OnEventNameChanged);
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(EventToCommandBehavior));
    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(EventToCommandBehavior));
    public static readonly BindableProperty EventsArgsConverterProperty = BindableProperty.Create(nameof(EventsArgsConverter), typeof(IValueConverter), typeof(EventToCommandBehavior));
    public static readonly BindableProperty EventArgsConverterParameterProperty = BindableProperty.Create(nameof(EventArgsConverterParameter), typeof(object), typeof(EventToCommandBehavior));

    public string EventName
    {
      get => (string)GetValue(EventNameProperty);
      set => SetValue(EventNameProperty, value);
    }

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

    public IValueConverter EventsArgsConverter
    {
      get => (IValueConverter)GetValue(EventsArgsConverterProperty);
      set => SetValue(EventsArgsConverterProperty, value);
    }

    public object EventArgsConverterParameter
    {
      get => GetValue(EventArgsConverterParameterProperty);
      set => SetValue(EventArgsConverterParameterProperty, value);
    }

    protected override void OnAttachedTo(View bindable)
    {
      base.OnAttachedTo(bindable);

      RegisterEvent(EventName);
    }

    protected override void OnDetachingFrom(View bindable)
    {
      UnregisterEvent(EventName);

      base.OnDetachingFrom(bindable);
    }

    private void RegisterEvent(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        return;
      }

      var eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);

      if (eventInfo == null)
      {
        throw new ArgumentException($"Cannot register the '{EventName}' event");
      }

      var methodInfo = typeof(EventToCommandBehavior).GetTypeInfo().GetDeclaredMethod(nameof(OnEvent));

      _eventHandler = methodInfo.CreateDelegate(eventInfo.EventHandlerType, this);
      eventInfo.AddEventHandler(AssociatedObject, _eventHandler);
    }

    private void UnregisterEvent(string name)
    {
      if (string.IsNullOrWhiteSpace(name) || _eventHandler == null)
      {
        return;
      }

      var eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);

      if (eventInfo == null)
      {
        throw new ArgumentException($"Cannot unregister the '{EventName}' event");
      }

      eventInfo.RemoveEventHandler(AssociatedObject, _eventHandler);
      _eventHandler = null;
    }

    private void OnEvent(object sender, object eventArgs)
    {
      if (Command == null)
      {
        return;
      }

      var resolvedParameter = CommandParameter;

      if (eventArgs != null && eventArgs != EventArgs.Empty)
      {
        resolvedParameter = EventsArgsConverter != null 
          ? EventsArgsConverter.Convert(eventArgs, typeof(object), EventArgsConverterParameter, null) 
          : eventArgs;
      }

      if (Command.CanExecute(resolvedParameter))
      {
        Command.Execute(resolvedParameter);
      }
    }

    private static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var behavior = bindable as EventToCommandBehavior;

      if (behavior?.AssociatedObject == null)
      {
        return;
      }

      var oldEventName = (string)oldValue;
      behavior.UnregisterEvent(oldEventName);

      var newEventName = (string)newValue;
      behavior.RegisterEvent(newEventName);
    }
  }
}