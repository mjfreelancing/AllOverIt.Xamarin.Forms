using System;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Controls
{
  public class EnumPicker : Picker
  {
    public static readonly BindableProperty EnumTypeProperty = BindableProperty.Create(nameof(EnumType), typeof(Type), typeof(EnumPicker), propertyChanged: OnEnumTypeChanged);

    public Type EnumType
    {
      get => (Type)GetValue(EnumTypeProperty);
      set => SetValue(EnumTypeProperty, value);
    }

    public static readonly BindableProperty DisplayItemConverterProperty = BindableProperty.Create(nameof(DisplayItemConverter), typeof(IValueConverter), typeof(EnumPicker));

    public IValueConverter DisplayItemConverter
    {
      get => (IValueConverter)GetValue(DisplayItemConverterProperty);
      set => SetValue(DisplayItemConverterProperty, value);
    }

    private static void OnEnumTypeChanged(BindableObject bindable, object oldValue, object newValue)
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
        var newType = (Type) newValue;

        if(!newType.IsEnum)
        {
          throw new ArgumentException("The EnumType property must be an enumeration type");
        }

        var items = Enum.GetNames(newType).ToList();

        if (picker.DisplayItemConverter != null)
        {
          items = items
            .Select(item => (string)picker.DisplayItemConverter.Convert(item, typeof(string), null, CultureInfo.InvariantCulture))
            .ToList();
        }

        picker.ItemsSource = items;
      }
    }
  }
}