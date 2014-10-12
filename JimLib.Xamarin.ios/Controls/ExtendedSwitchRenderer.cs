using System.ComponentModel;
using JimBobBennett.JimLib.Extensions;
using JimBobBennett.JimLib.Xamarin.Controls;
using JimBobBennett.JimLib.Xamarin.ios.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedSwitch), typeof(ExtendedSwitchRenderer))]

namespace JimBobBennett.JimLib.Xamarin.ios.Controls
{
    public class ExtendedSwitchRenderer : SwitchRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Switch> e)
        {
            base.OnElementChanged(e);

            SetTint((ExtendedSwitch)Element);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var element = (ExtendedSwitch) Element;

            if (e.PropertyNameMatches(() => element.OnTintColor))
                SetTint(element);
        }

        private void SetTint(ExtendedSwitch element)
        {
            if (element.OnTintColor != Color.Default)
                Control.OnTintColor = element.OnTintColor.ToUIColor();
        }
    }
}
