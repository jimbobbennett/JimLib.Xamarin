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

            UIApplication.SharedApplication.KeyWindow.RootViewController.GetTopViewController()
                .PresentViewController(controller, true, null);
        }
    }
}
