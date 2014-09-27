using System;
using System.Globalization;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Controls
{
    public class ImageSourceConverter : TypeConverter
    {
        public override bool CanConvertFrom(Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(CultureInfo culture, object value)
        {
            if (value == null)
                return null;

            var str = value as string;
            if (str == null)
                throw new InvalidOperationException(string.Format("Conversion failed: \"{0}\" into {1}", new[] {value, typeof (ImageSource)}));
            
            Uri result;
            if (!Uri.TryCreate(str, UriKind.Absolute, out result) || result.Scheme == "file")
                return ImageSource.FromFile(str);
            
            return ImageSource.FromUri(result);
        }
    }
}