using System.ComponentModel;
using JimBobBennett.JimLib.Extensions;
using JimBobBennett.JimLib.Xamarin.Controls;
using JimBobBennett.JimLib.Xamarin.ios.Controls;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedTimePicker), typeof(ExtendedTimePickerRenderer))]

namespace JimBobBennett.JimLib.Xamarin.ios.Controls
{
    public class ExtendedTimePickerRenderer : TimePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
        {
            base.OnElementChanged(e);

            var element = (ExtendedTimePicker) Element;

			if (element != null)
			{
	            Set24Hours(element);
	            SetTextColor(element);
			}
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var element = (ExtendedTimePicker)Element;

            if (e.PropertyNameMatches(() => element.Show24Hours))
                Set24Hours(element);

            if (e.PropertyNameMatches(() => element.TextColor))
                SetTextColor(element);
        }

        private void SetTextColor(ExtendedTimePicker element)
        {
            if (element.TextColor != Color.Default)
                Control.TextColor = element.TextColor.ToUIColor();
        }

        private void Set24Hours(ExtendedTimePicker element)
        {
            if (element.Show24Hours)
            {
                var timePicker = (UIDatePicker) Control.InputView;
                timePicker.Locale = new NSLocale("no_nb");
            }
        }
    }
}
