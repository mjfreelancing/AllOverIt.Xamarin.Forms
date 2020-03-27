using AllOverIt.XamarinForms.Behaviors.Base;
using System;
using System.Reflection;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Behaviors
{
  // Based on https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/behaviors/creating
  // Based on https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/behaviors/reusable/event-to-command-behavior
  //
  // Except this version supports multiple event/command handlers and can be attached via a style

  [ContentProperty("Commands")]
  public class EventToCommandBehavior : CommandsBehaviorBase<VisualElement>
  {
    private Delegate? _eventHandler;

    public static readonly BindableProperty EventNameProperty = BindableProperty.Create(nameof(EventName), typeof(string), typeof(EventToCommandBehavior), propertyChanged: OnEventNameChanged);

    public string EventName
    {
      get => (string)GetValue(EventNameProperty);
      set => SetValue(EventNameProperty, value);
    }

    protected override void OnAttachedTo(VisualElement bindable)
    {
      base.OnAttachedTo(bindable);

      RegisterEvent(EventName);
    }

    protected override void OnDetachingFrom(VisualElement bindable)
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
        throw new ArgumentException($"Cannot un-register the '{EventName}' event");
      }

      eventInfo.RemoveEventHandler(AssociatedObject, _eventHandler);
      _eventHandler = null;
    }

    private void OnEvent(object sender, object eventArgs)
    {
      foreach (var command in Commands)
      {
        command.BindingContext = BindingContext;
        command.Execute(sender, eventArgs);
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