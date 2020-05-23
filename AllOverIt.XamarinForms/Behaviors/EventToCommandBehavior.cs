using AllOverIt.XamarinForms.Behaviors.Base;
using AllOverIt.XamarinForms.Commands;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Behaviors
{
  // Based on https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/behaviors/creating
  // Based on https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/behaviors/reusable/event-to-command-behavior
  //
  // This version supports multiple event/command handlers

  /// <summary>
  /// Executes commands when a registered event is triggered.
  /// </summary>
  /// <remarks>When executed, the current BindingContext is assigned to the command handler.</remarks>
  [ContentProperty("Commands")]
  public class EventToCommandBehavior : BehaviorBase<VisualElement>
  {
    private Delegate _eventHandler;

    public static readonly BindableProperty EventCommandsProperty = BindableProperty.Create(nameof(Commands), typeof(IList<EventCommandBase>), typeof(EventToCommandBehavior));

    /// <summary>
    /// The list of commands to be execute when the event is fired.
    /// </summary>
    public IList<EventCommandBase> Commands => (IList<EventCommandBase>)GetValue(EventCommandsProperty);

    public static readonly BindableProperty EventNameProperty = BindableProperty.Create(nameof(EventName), typeof(string), typeof(EventToCommandBehavior), propertyChanged: OnEventNameChanged);

    /// <summary>
    /// The name of the event that, when raised, executes all <see cref="Commands"/>.
    /// </summary>
    public string EventName
    {
      get => (string)GetValue(EventNameProperty);
      set => SetValue(EventNameProperty, value);
    }

    public EventToCommandBehavior() => SetValue(EventCommandsProperty, new List<EventCommandBase>());

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

      // this should never fail since the event must have been registered before it can be unregistered
      _ = eventInfo ?? throw new InvalidOperationException($"Cannot un-register the '{EventName}' event");

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