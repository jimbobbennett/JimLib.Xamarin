using System.ComponentModel;
using JimBobBennett.JimLib.Extensions;
using JimBobBennett.JimLib.Xamarin.Controls;
using JimBobBennett.JimLib.Xamarin.ios.Controls;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedImage), typeof(ExtendedImageRenderer))]

namespace JimBobBennett.JimLib.Xamarin.ios.Controls
{
    public class ExtendedImageRenderer : ImageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            SetTintColor(((ExtendedImage)Element).TintColor);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyNameMatches(() => ((ExtendedImage) Element).TintColor) ||
                e.PropertyNameMatches(() => Element.Source))
                SetTintColor(((ExtendedImage) Element).TintColor);
        }

        private void SetTintColor(Color tintColor)
        {
            if (tintColor == Color.Default)
            {
                Control.TintColor = null;
                Control.Image = Control.Image.ImageWithRenderingMode(UIImageRenderingMode.Automatic);
            }
            else
            {
                Control.Image = Control.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                Control.TintColor = tintColor.ToUIColor();
            }
        }
    }
}