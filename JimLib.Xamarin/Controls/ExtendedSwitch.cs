using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Controls
{
    public class ExtendedSwitch : Switch
    {
        public static readonly BindableProperty OnTintColorProperty =
            BindableProperty.Create<ExtendedSwitch, Color>(p => p.OnTintColor, Color.Default);

        public Color OnTintColor
        {
            get { return (Color) GetValue(OnTintColorProperty); }
            set {  SetValue(OnTintColorProperty, value);}
        }
    }
}
