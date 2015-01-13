using System.ComponentModel;
using JimBobBennett.JimLib.Xamarin.Controls;
using JimBobBennett.JimLib.Xamarin.ios.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedLabel), typeof(ExtendedLabelRenderer))]

namespace JimBobBennett.JimLib.Xamarin.ios.Controls
{
    public class ExtendedLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            var view = (ExtendedLabel)Element;

            SetTextResize(view);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var view = (ExtendedLabel)Element;

            if (string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == "AdjustFontSizeToFitWidth")
                SetTextResize(view);
        }

        private void SetTextResize(ExtendedLabel view)
        {
			if (view == null) return;

            Control.AdjustsFontSizeToFitWidth = view.AdjustFontSizeToFitWidth;

            if (view.AdjustFontSizeToFitWidth)
            {
                Control.LineBreakMode = UILineBreakMode.Clip;
                Control.Lines = 1;
            }
        }
    }
}