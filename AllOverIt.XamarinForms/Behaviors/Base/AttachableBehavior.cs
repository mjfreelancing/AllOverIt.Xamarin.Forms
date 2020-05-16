using System;
using System.Linq;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Behaviors.Base
{
  // only inherit from AttachableBehavior if the behavior can function as-is without specific configuration of its properties
  public class AttachableBehavior<TBehavior, TBindable> : BehaviorBase<TBindable>
    where TBehavior : Behavior, new()
    where TBindable : VisualElement
  {
    // based on: https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/behaviors/creating
    // provides the ability to add/remove a behavior via a style
    //
    // <Style x:Key="MyValidationStyleName" TargetType="Entry">
    // <Style.Setters>
    //   <Setter Property = "local:NumericValidationBehavior.AttachBehavior" Value="true" />
    // </Style.Setters>
    // </Style>

    public static readonly BindableProperty AttachBehaviorProperty = BindableProperty.CreateAttached("AttachBehavior", typeof(bool), typeof(TBehavior), false, propertyChanged: OnAttachBehaviorChanged);

    public static bool GetAttachBehavior(BindableObject bindable) => (bool)bindable.GetValue(AttachBehaviorProperty);

    public static void SetAttachBehavior(BindableObject bindable, bool value) => bindable.SetValue(AttachBehaviorProperty, value);

    private static void OnAttachBehaviorChanged(BindableObject bindable, object oldValue, object newValue)
    {
      if (!(bindable is TBindable visualElement))
      {
        throw new ArgumentException($"Cannot attach behavior to target type {bindable.GetType()}, expected {typeof(TBindable)}");
      }

      var attachBehavior = (bool)newValue;

      if (attachBehavior)
      {
        visualElement.Behaviors.Add(new TBehavior());
      }
      else
      {
        var toRemove = visualElement.Behaviors.FirstOrDefault(behavior => behavior is TBehavior);

        if (toRemove != null)
        {
          visualElement.Behaviors.Remove(toRemove);
        }
      }
    }
  }
}