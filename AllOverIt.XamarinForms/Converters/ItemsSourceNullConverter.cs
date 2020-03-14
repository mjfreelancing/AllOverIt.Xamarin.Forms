using System;
using System.Collections;
using System.Globalization;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Converters
{
  public class ItemsSourceNullConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var items = (IEnumerable) value;

      return items == null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}