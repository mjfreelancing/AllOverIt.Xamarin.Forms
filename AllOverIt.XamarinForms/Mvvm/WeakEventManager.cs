using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AllOverIt.XamarinForms.Mvvm
{
  // based on Xamarin.Forms WeakEventManager

  /// <summary>
  /// Weak event manager to subscribe and unsubscribe from events.
  /// </summary>
  public class WeakEventManager
  {
    private struct Subscription
    {
      public readonly WeakReference Subscriber;
      public readonly MethodInfo Handler;

      // a null subscriber indicates a static method
      public Subscription(WeakReference subscriber, MethodInfo handler)
      {
        Subscriber = subscriber;
        Handler = handler ?? throw new ArgumentNullException(nameof(handler));
      }
    }

    private readonly IDictionary<string, List<Subscription>> _eventHandlers = new Dictionary<string, List<Subscription>>();

    /// <summary>
    /// Add an event handler to the manager.
    /// </summary>
    /// <typeparam name="TEventArgs">Event handler of T</typeparam>
    /// <param name="handler">Handler of the event</param>
    /// <param name="eventName">Name to use in the dictionary. Should be unique.</param>
    public void AddEventHandler<TEventArgs>(EventHandler<TEventArgs> handler, [CallerMemberName] string eventName = "")
      where TEventArgs : EventArgs
    {
      if (handler == null)
      {
        throw new ArgumentNullException(nameof(handler));
      }

      if (string.IsNullOrEmpty(eventName))
      {
        throw new ArgumentNullException(nameof(eventName));
      }

      AddEventHandler(eventName, handler.Target, handler.GetMethodInfo());
    }

    /// <summary>
    /// Add an event handler to the manager.
    /// </summary>
    /// <param name="handler">Handler of the event</param>
    /// <param name="eventName">Name to use in the dictionary. Should be unique.</param>
    public void AddEventHandler(EventHandler handler, [CallerMemberName] string eventName = "")
    {
      if (handler == null)
      {
        throw new ArgumentNullException(nameof(handler));
      }

      if (string.IsNullOrEmpty(eventName))
      {
        throw new ArgumentNullException(nameof(eventName));
      }

      AddEventHandler(eventName, handler.Target, handler.GetMethodInfo());
    }

    /// <summary>
    /// Handle an event
    /// </summary>
    /// <param name="sender">Sender of the event</param>
    /// <param name="args">Arguments for the event</param>
    /// <param name="eventName">Name of the event.</param>
    public void HandleEvent(object sender, object args, string eventName)
    {
      if (!_eventHandlers.TryGetValue(eventName, out var target))
      {
        return;
      }

      var toRaise = new List<(object subscriber, MethodInfo handler)>();
      var toRemove = new List<Subscription>();

      foreach (var subscription in target)
      {
        if (subscription.Subscriber == null)
        {
          // the subscriber is static
          toRaise.Add((null, subscription.Handler));
        }
        else
        {
          var subscriber = subscription.Subscriber.Target;

          if (subscriber == null)
          { 
            // The subscriber has been garbage collected, so we can remove the subscription
            toRemove.Add(subscription);
          }
          else
          {
            toRaise.Add((subscriber, subscription.Handler));
          }
        }
      }

      foreach (var subscription in toRemove)
      {
        target.Remove(subscription);
      }

      foreach (var (subscriber, handler) in toRaise)
      {
        handler.Invoke(subscriber, new[] { sender, args });
      }
    }

    /// <summary>
    /// Remove an event handler.
    /// </summary>
    /// <typeparam name="TEventArgs">Type of the EventArgs</typeparam>
    /// <param name="handler">Handler to remove</param>
    /// <param name="eventName">Event name to remove</param>
    public void RemoveEventHandler<TEventArgs>(EventHandler<TEventArgs> handler, [CallerMemberName] string eventName = "")
      where TEventArgs : EventArgs
    {
      if (string.IsNullOrEmpty(eventName))
      {
        throw new ArgumentNullException(nameof(eventName));
      }

      if (handler == null)
      {
        throw new ArgumentNullException(nameof(handler));
      }

      RemoveEventHandler(eventName, handler.Target, handler.GetMethodInfo());
    }

    /// <summary>
    /// Remove an event handler.
    /// </summary>
    /// <param name="handler">Handler to remove</param>
    /// <param name="eventName">Event name to remove</param>
    public void RemoveEventHandler(EventHandler handler, [CallerMemberName] string eventName = "")
    {
      if (string.IsNullOrEmpty(eventName))
      {
        throw new ArgumentNullException(nameof(eventName));
      }

      if (handler == null)
      {
        throw new ArgumentNullException(nameof(handler));
      }

      RemoveEventHandler(eventName, handler.Target, handler.GetMethodInfo());
    }

    private void AddEventHandler(string eventName, object handlerTarget, MethodInfo methodInfo)
    {
      if (!_eventHandlers.TryGetValue(eventName, out var targets))
      {
        targets = new List<Subscription>();
        _eventHandlers.Add(eventName, targets);
      }

      // when handlerTarget is null, the event handler is a static method
      var subscriber = handlerTarget == null
        ? null 
        : new WeakReference(handlerTarget);

      targets.Add(new Subscription(subscriber, methodInfo));
    }

    private void RemoveEventHandler(string eventName, object handlerTarget, MemberInfo methodInfo)
    {
      if (!_eventHandlers.TryGetValue(eventName, out var subscriptions))
      {
        return;
      }

      for (var index = subscriptions.Count; index > 0; index--)
      {
        var current = subscriptions[index - 1];

        if (current.Subscriber?.Target != handlerTarget || current.Handler.Name != methodInfo.Name)
        {
          continue;
        }

        subscriptions.Remove(current);
        break;
      }
    }
  }
}