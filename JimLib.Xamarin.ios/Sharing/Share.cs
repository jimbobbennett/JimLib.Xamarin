using System.Collections.Generic;
using System.Threading.Tasks;
using JimBobBennett.JimLib.Extensions;
using JimBobBennett.JimLib.Xamarin.ios.Extensions;
using JimBobBennett.JimLib.Xamarin.Sharing;
using Foundation;
using UIKit;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.ios.Sharing
{
    public class Share : IShare
    {
        public async Task ShareAsync(string url, ImageSource image, string message)
        {
            await ShareImageAsyc(image, message, url);
        }

        public async Task ShareAsync(ImageSource image, string message)
        {
            await ShareImageAsyc(image, message);
        }

        private static async Task ShareImageAsyc(ImageSource image, string message, string url = null)
        {
            var handler = image.GetHandler();

            if (handler == null) return;

            var uiImage = await handler.LoadImageAsync(image);

            var items = new List<NSObject> { new NSString(message ?? string.Empty) };
            if (!url.IsNullOrEmpty())
                items.Add(new NSString(url));
            items.Add(uiImage);

            var controller = new UIActivityViewController(items.ToArray(), null);

            UIApplication.SharedApplication.KeyWindow.RootViewController.GetTopViewController()
                .PresentViewController(controller, true, null);
        }
    }
}
