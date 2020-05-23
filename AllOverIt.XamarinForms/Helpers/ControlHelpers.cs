using System;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Helpers
{
  public static class ControlHelpers
  {
    /// <summary>
    /// Simplifies the setting of a property value on a control in a type safe manner.
    /// </summary>
    /// <typeparam name="TControl">The control type.</typeparam>
    /// <typeparam name="TPropertyType">The property type.</typeparam>
    /// <param name="bindable">The bindable control. Must be of type <see cref="TControl"/>.</param>
    /// <param name="newValue">The property value to apply.</param>
    /// <param name="propertyAssigner">The assignment action invoked to set the property value and apply any other required operation.</param>
    public static void SetProperty<TControl, TPropertyType>(BindableObject bindable, object newValue, Action<TControl, TPropertyType> propertyAssigner)
    {
      if (!(bindable is TControl control) || !(newValue is TPropertyType propertyValue))
      {
        return;
      }

      propertyAssigner.Invoke(control, propertyValue);
    }
  }
}