using JimBobBennett.JimLib.Xamarin.ios.Extensions;
using JimBobBennett.JimLib.Xamarin.ios.Views;
using JimBobBennett.JimLib.Xamarin.Views;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BaseContentPage), typeof(BaseContentPageRenderer))]

namespace JimBobBennett.JimLib.Xamarin.ios.Views
{
    public class BaseContentPageRenderer : PageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            ((BaseContentPage) Element).Appearing += (s, e1) => SetOrientation();

            SetOrientation();
        }

        public override void WillRotate(UIInterfaceOrientation toInterfaceOrientation, double duration)
        {
            base.WillRotate(toInterfaceOrientation, duration);

            ((BaseContentPage)Element).OrientationChanging();
        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);

            SetOrientation();
        }

        private void SetOrientation()
        {
            ((BaseContentPage) Element).OrientationChanged(InterfaceOrientation.GetOrientation());
        }
    }
}
