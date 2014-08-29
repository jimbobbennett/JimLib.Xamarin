using System;
using System.Globalization;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.ValueConverters
{
    public class InvertBooleanValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool && !(bool) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
