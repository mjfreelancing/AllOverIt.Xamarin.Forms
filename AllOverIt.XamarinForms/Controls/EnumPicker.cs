using AllOverIt.Helpers;
using System;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Controls
{
  public class EnumPicker : Picker
  {
    public static readonly BindableProperty EnumTypeProperty = BindableProperty.Create(nameof(EnumType), typeof(Type), typeof(EnumPicker),
      propertyChanged: EnumTypeChanged);

    public Type EnumType
    {
      get => (Type)GetValue(EnumTypeProperty);
      set => SetValue(EnumTypeProperty, value);
    }

    public static readonly BindableProperty DisplayItemConverterProperty = BindableProperty.Create(nameof(DisplayItemConverter), typeof(IValueConverter), typeof(EnumPicker),
      propertyChanged: DisplayItemConverterChanged);

    public IValueConverter DisplayItemConverter
    {
      get => (IValueConverter)GetValue(DisplayItemConverterProperty);
      set => SetValue(DisplayItemConverterProperty, value);
    }

    private static void EnumTypeChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var picker = (bindable as EnumPicker).WhenNotNull(nameof(bindable));

      switch (newValue)
      {
        case null:
          picker.ItemsSource = null;
          break;

        case Type enumType when enumType.IsEnum:
        {
          SetItemsSource(picker, enumType);
          break;
        }

        default:
          throw new ArgumentOutOfRangeException($"{nameof(EnumType)}", $"The {nameof(EnumType)} property must be an Enum type");
      }
    }

    private static void DisplayItemConverterChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var picker = (bindable as EnumPicker).WhenNotNull(nameof(bindable));

      if (picker.EnumType == null)
      {
        picker.ItemsSource = null;
      }
      else
      {
        SetItemsSource(picker, picker.EnumType);
      }
    }

    private static void SetItemsSource(EnumPicker picker, Type enumType)
    {
      var items = Enum.GetNames(enumType).ToList();

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