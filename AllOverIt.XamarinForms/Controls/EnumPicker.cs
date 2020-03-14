using System;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Controls
{
  public class EnumPicker : Picker
  {
    public static readonly BindableProperty EnumTypeProperty = BindableProperty.Create(nameof(EnumType), typeof(Type), typeof(EnumPicker), propertyChanged: HandleEnumTypeChanged);

    public Type EnumType
    {
      get => (Type)GetValue(EnumTypeProperty);
      set => SetValue(EnumTypeProperty, value);
    }

    private static void HandleEnumTypeChanged(BindableObject bindable, object oldValue, object newValue)
    {
      if (!(bindable is EnumPicker picker))
      {
        return;
      }

      if (newValue == null)
      {
        picker.ItemsSource = null;
      }
      else
      {
        
        if (!typeof(Type).IsEnum)
        {
          throw new ArgumentException("The EnumType property must be an enumeration type");
        }

        picker.ItemsSource = Enum.GetNames((Type)newValue);
      }
    }
  }
}