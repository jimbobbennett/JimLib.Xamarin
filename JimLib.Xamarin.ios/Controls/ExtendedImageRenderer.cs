using System.ComponentModel;
using System.Linq;
using JimBobBennett.JimLib.Extensions;
using JimBobBennett.JimLib.Xamarin.Controls;
using JimBobBennett.JimLib.Xamarin.ios.Controls;
using MonoTouch.Foundation;
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

            ((ExtendedImage)Element).Clicked += (s, e1) => ShowActivitySheet();
        }

        private void ShowActivitySheet()
        {
            if (((ExtendedImage)Element).IsSharable)
            {
                var items = new NSObject[] { new NSString(((ExtendedImage)Element).ShareText), Control.Image };

                var controller = new UIActivityViewController(items, null);
                TopViewController.PresentViewController(controller, true, null);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyNameMatches(() => ((ExtendedImage)Element).TintColor) ||
                e.PropertyNameMatches(() => Element.Source))
                SetTintColor(((ExtendedImage)Element).TintColor);
        }

        private void SetTintColor(Color tintColor)
        {
            if (tintColor == Color.Default)
            {
                Control.TintColor = null;

                if (Control.Image != null)
                    Control.Image = Control.Image.ImageWithRenderingMode(UIImageRenderingMode.Automatic);
            }
            else
            {
                if (Control.Image != null)
                    Control.Image = Control.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);

                Control.TintColor = tintColor.ToUIColor();
            }
        }

        private UIViewController TopViewController
        {
            get { return GetTopViewController(UIApplication.SharedApplication.KeyWindow.RootViewController); }
        }

        private UIViewController GetTopViewController(UIViewController rootViewController)
        {
            if (rootViewController.PresentedViewController == null)
            {
                return rootViewController;
            }

            var navigationController = rootViewController.PresentedViewController as UINavigationController;

            if (navigationController != null)
            {
                var lastViewController = navigationController.ViewControllers.Last();
                return GetTopViewController(lastViewController);
            }

            var presentedViewController = rootViewController.PresentedViewController;
            return GetTopViewController(presentedViewController);
        }
    }
}