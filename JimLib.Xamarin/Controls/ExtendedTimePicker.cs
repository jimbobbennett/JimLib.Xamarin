using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Controls
{
    public class ExtendedTimePicker : TimePicker
    {
        public static readonly BindableProperty Show24HoursProperty =
            BindableProperty.Create<ExtendedTimePicker, bool>(p => p.Show24Hours, false);

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create<ExtendedTimePicker, Color>(p => p.TextColor, Color.Default);

        public bool Show24Hours
        {
            get { return (bool)GetValue(Show24HoursProperty); }
            set { SetValue(Show24HoursProperty, value); }
        }

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }
    }
}
