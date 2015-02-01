using CoreGraphics;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace JimBobBennett.JimLib.Xamarin.ios.Extensions
{
    public static class ImageSourceExtensions
    {
        public static IImageSourceHandler GetHandler(this ImageSource source)
        {
            IImageSourceHandler returnValue = null;
            if (source is UriImageSource)
                returnValue = new ImageLoaderSourceHandler();
            else if (source is FileImageSource)
                returnValue = new FileImageSourceHandler();
            else if (source is StreamImageSource)
                returnValue = new StreamImagesourceHandler();
            return returnValue;
        }

        public static async Task<UIImage> GetImageAsync(this ImageSource source)
        {
            var handler = source.GetHandler();
            using (var image = await handler.LoadImageAsync(source))
            {
                if (image == null) return null;

                UIGraphics.BeginImageContext(image.Size);
                image.Draw(new CGRect(0, 0, image.Size.Width, image.Size.Height));
                return UIGraphics.GetImageFromCurrentImageContext();
            }
        }
    }
}
