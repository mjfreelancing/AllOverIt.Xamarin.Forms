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
      return value switch
      {
        Color asColor => asColor,
        string asString => Color.FromHex(asString),
        _ => throw new NotSupportedException()
      };
    }

    private static double GetOpacity(object parameter)
    {
      return parameter switch
      {
        null => 1.0d,
        double asDouble => asDouble,
        string asString => double.Parse(asString),
        _ => throw new NotSupportedException()
      };
    }
  }
}