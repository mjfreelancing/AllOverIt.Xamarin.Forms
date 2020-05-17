using System;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Controls
{
  public static class ControlHelpers
  {
    public static void SetProperty<TControl, TPropertyType>(BindableObject bindable, object newValue, Action<TControl, TPropertyType> propertyAssigner)
    {
      if (!(bindable is TControl control) || !(newValue is TPropertyType propertyValue))
      {
        return;
      }

      propertyAssigner.Invoke(control, propertyValue);
    }

    public static TElement GetNonPublicFieldFromControl<TElement>(object control, string name)
    {
      var fieldInfo = control.GetType()
        .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
        .Single(item => item.Name == name);

      return (TElement)fieldInfo.GetValue(control);
    }
  }
}