using System.Linq;
using System.Threading.Tasks;
using JimBobBennett.JimLib.Xamarin.ios.Extensions;
using JimBobBennett.JimLib.Xamarin.Sharing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.ios.Sharing
{
    public class ShareUrl : IShareUrl
    {
        public async Task ShareAsync(string url, ImageSource image, string message)
        {
            var handler = image.GetHandler();

            if (handler == null) return;

            var uiImage = await handler.LoadImageAsync(image);

            var items = new NSObject[]
            {
                new NSString(message),
                new NSString(url),
                uiImage
            };

            var controller = new UIActivityViewController(items, null);
            TopViewController.PresentViewController(controller, true, null);
        }

        private static UIViewController TopViewController
        {
            get { return GetTopViewController(UIApplication.SharedApplication.KeyWindow.RootViewController); }
        }

        private static UIViewController GetTopViewController(UIViewController rootViewController)
        {
            while (true)
            {
                if (rootViewController.PresentedViewController == null)
                    return rootViewController;

                var navigationController = rootViewController.PresentedViewController as UINavigationController;

                if (navigationController != null)
                {
                    var lastViewController = navigationController.ViewControllers.Last();
                    rootViewController = lastViewController;
                }
                else
                {
                    var presentedViewController = rootViewController.PresentedViewController;
                    rootViewController = presentedViewController;
                }
            }
        }
    }
}
