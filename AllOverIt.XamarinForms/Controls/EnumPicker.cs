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

      switch (newValue)
      {
        case null:
          picker.ItemsSource = null;
          break;

        case Type enumType when !enumType.IsEnum:
          throw new ArgumentException("The EnumType property must be an Enum type");

        case Type enumType:
        {
          var items = Enum.GetNames(enumType).ToList();

          if (picker.DisplayItemConverter != null)
          {
            items = items
              .Select(item => (string)picker.DisplayItemConverter.Convert(item, typeof(string), null, CultureInfo.InvariantCulture))
              .ToList();
          }

          picker.ItemsSource = items;
          break;
        }

        default:
          throw new ArgumentOutOfRangeException(nameof(newValue), $"Expected an Enum Type for the {nameof(EnumType)} property");
      }
    }
  }
}