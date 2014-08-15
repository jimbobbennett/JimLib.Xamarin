using System;
using System.Drawing;
using JimBobBennett.JimLib.Xamarin.Images;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.ios.Images
{
    public class ImageHelper : IImageHelper
    {
        public ImageSource GetImageSource(string base64)
        {
            var data = new NSData(base64, NSDataBase64DecodingOptions.None);
            var image = UIImage.LoadFromData(data);
            return GetImageSourceFromUIImage(image);
        }

        internal static ImageSource GetImageSourceFromUIImage(UIImage uiImage)
        {
            return uiImage == null ? null : ImageSource.FromStream(() => uiImage.AsPNG().AsStream());
        }
        
        // resize the image to be contained within a maximum width and height, keeping aspect ratio
        internal static UIImage MaxResizeImage(UIImage sourceImage, float maxWidth, float maxHeight)
        {
            if (sourceImage == null) return null;

            var sourceSize = sourceImage.Size;
            var maxResizeFactor = Math.Max(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
            if (maxResizeFactor > 1) return sourceImage;
            var width = maxResizeFactor * sourceSize.Width;
            var height = maxResizeFactor * sourceSize.Height;
            UIGraphics.BeginImageContext(new SizeF(width, height));
            sourceImage.Draw(new RectangleF(0, 0, width, height));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return resultImage;
        }
    }
}