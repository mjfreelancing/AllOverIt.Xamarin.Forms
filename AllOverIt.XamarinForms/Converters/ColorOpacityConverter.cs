using AllOverIt.XamarinForms.Helpers;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Converters
{
  public class ColorOpacityConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var color = GetColor(value);
      var opacity = GetOpacity(parameter);

      return ColorHelper.FromHexWithOpacity(color.ToHex(), opacity);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }

    private static Color GetColor(object value)
    {
      switch (value)
      {
        case Color asColor:
          return asColor;

        case string asString:
          return Color.FromHex(asString);

        default:
          throw new NotSupportedException();
      }
    }

    private static double GetOpacity(object parameter)
    {
      switch (parameter)
      {
        case null:
          return 1.0d;

        case double asDouble:
          return asDouble;

        case string asString:
          return double.Parse(asString);

        default:
          throw new NotSupportedException();
      }
    }
  }
}