using AllOverIt.Extensions;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Converters
{
  // Great for converting an enum value to individual words, for use with EnumPicker
  public class EnumToStringConverter<TEnum> : IValueConverter
    where TEnum : Enum
  {
    public bool SplitWords { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var enumValue = $"{value}";

      if (!SplitWords)
      {
        return enumValue;
      }

      // split words at each upper case letter
      var words = Regex.Split($"{value}", @"(?<!^)(?=[A-Z])");

      return string.Join(" ", words);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var enumValue = ((string)value).Replace(" ", string.Empty);

      return enumValue.As<TEnum>();
    }
  }
}