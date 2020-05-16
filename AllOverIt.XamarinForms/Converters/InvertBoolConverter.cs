using System;
using System.Globalization;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Converters
{
  public class InvertBoolConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => !(bool)value;
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => !(bool)value;
  }
}