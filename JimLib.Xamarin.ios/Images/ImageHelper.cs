using System;
using System.Drawing;
using JimBobBennett.JimLib.Extensions;
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
            if (base64.IsNullOrEmpty()) return null;
            return GetImageSourceFromUIImage(GetUIImageFromBase64(base64));
        }

        internal static UIImage GetUIImageFromBase64(string base64)
        {
            var data = new NSData(base64, NSDataBase64DecodingOptions.None);
            return UIImage.LoadFromData(data);
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

        internal static UIImage CropToCircle(UIImage sourceImage)
        {
            if (sourceImage == null) return null;

            var imageWidth = sourceImage.Size.Width;
            var imageHeight = sourceImage.Size.Height;

            UIGraphics.BeginImageContextWithOptions(new SizeF(imageWidth, imageHeight), false, 1f);
            var context = UIGraphics.GetCurrentContext();
            
            //Calculate the centre of the circle
            var imageCentreX = imageWidth/2;
            var imageCentreY = imageHeight/2;

            // Create and CLIP to a CIRCULAR Path
            // (This could be replaced with any closed path if you want a different shaped clip)
            var radius = imageWidth/2;
            context.BeginPath();
            context.AddArc(imageCentreX, imageCentreY, radius, 0, Convert.ToSingle(2*Math.PI), false);
            context.ClosePath();
            context.Clip();
            
            // Draw the IMAGE
            sourceImage.Draw(new RectangleF(0, 0, imageWidth, imageHeight));

            var newImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return newImage;
        }
    }
}