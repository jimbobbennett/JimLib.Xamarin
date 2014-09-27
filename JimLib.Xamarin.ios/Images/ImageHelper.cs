using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using JimBobBennett.JimLib.Extensions;
using JimBobBennett.JimLib.Xamarin.Images;
using JimBobBennett.JimLib.Xamarin.Network;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Media;

namespace JimBobBennett.JimLib.Xamarin.ios.Images
{
    public class ImageHelper : IImageHelper
    {
        private readonly Dictionary<string, Tuple<string, ImageSource>> _cachedImages = new Dictionary<string, Tuple<string, ImageSource>>();
        private readonly IRestConnection _restConnection;

        public ImageHelper(IRestConnection restConnection)
        {
            _restConnection = restConnection;
        }

        public ImageSource GetImageSource(string base64)
        {
            if (base64.IsNullOrEmpty()) return null;
            return GetImageSourceFromUIImage(GetUIImageFromBase64(base64));
        }

        public async Task<Tuple<string, ImageSource>> GetImageAsync(PhotoSource source,
            ImageOptions options = null)
        {
            switch (source)
            {
                case PhotoSource.Camera:
                    return await GetImageFromCameraAsync(options);
                default:
                    return await GetImageFromExistingAsync(options);
            }
        }

        public async Task<Tuple<string, ImageSource>> GetImageAsync(string url, ImageOptions options = null, bool canCache = false)
        {
            Tuple<string, ImageSource> retVal;
            if (canCache && _cachedImages.TryGetValue(url, out retVal))
                return retVal;

            return await Task.Run(() =>
                {
                    try
                    {
                        var image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(url)));
                        return image != null ? ProcessImage(options, image) : new Tuple<string, ImageSource>(null, null);
                    }
                    catch
                    {
                        return new Tuple<string, ImageSource>(null, null);
                    }
                });
        }

        public async Task<Tuple<string, ImageSource>> GetImageAsync(string baseUrl, string resource = "/",
            string username = null, string password = null, int timeout = 10000,
            Dictionary<string, string> headers = null, ImageOptions options = null, bool canCache = false)
        {
            Tuple<string, ImageSource> retVal;
            var uriBuilder = new UriBuilder(baseUrl) { Fragment = resource };
            if (canCache && _cachedImages.TryGetValue(uriBuilder.Uri.ToString(), out retVal))
                return retVal;

            var bytes = await _restConnection.MakeRawGetRequestAsync(baseUrl, resource, username, password, 
                timeout, headers);

            if (bytes == null)
                return Tuple.Create((string)null, (ImageSource)null);

            var image = GetUIImageFromBase64(Convert.ToBase64String(bytes)); 
            return image != null ? ProcessImage(options, image) : new Tuple<string, ImageSource>(null, null);
        }

        public PhotoSource AvailablePhotoSources
        {
            get
            {
                var mediaPicker = new MediaPicker();

                var retVal = PhotoSource.None;

                if (mediaPicker.IsCameraAvailable)
                    retVal |= PhotoSource.Camera;
                if (mediaPicker.PhotosSupported)
                    retVal |= PhotoSource.Existing;

                return retVal;
            }
        }

        private static async Task<Tuple<string, ImageSource>> GetImageFromCameraAsync(ImageOptions options = null)
        {
            var mediaPicker = new MediaPicker();
            if (mediaPicker.IsCameraAvailable)
            {
                try
                {
                    var file = await mediaPicker.TakePhotoAsync(new StoreCameraMediaOptions());

                    options = options ?? new ImageOptions();
                    options.FixOrientation = true;

                    return ProcessImageFile(options, file);
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return null;
        }

        private static async Task<Tuple<string, ImageSource>> GetImageFromExistingAsync(ImageOptions options = null)
        {
            var mediaPicker = new MediaPicker();
            if (mediaPicker.PhotosSupported)
            {
                try
                {
                    var file = await mediaPicker.PickPhotoAsync();
                    return ProcessImageFile(options, file);
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return null;
        }

        private static Tuple<string, ImageSource> ProcessImageFile(ImageOptions options, MediaFile file)
        {
            if (file == null)
                return null;

            var image = UIImage.FromFile(file.Path);

            return ProcessImage(options, image);
        }

        private static Tuple<string, ImageSource> ProcessImage(ImageOptions options, UIImage image)
        {
            if (options != null)
            {
                if (options.HasSizeSet)
                    image = MaxResizeImage(image, options.MaxWidth, options.MaxHeight);

                if (options.Circle)
                    image = CropToCircle(image);

                if (options.FixOrientation)
                    image = FixOrientation(image);
            }

            var data = image.AsJPEG(0.75f).GetBase64EncodedString(NSDataBase64EncodingOptions.None);

            return Tuple.Create(data, GetImageSourceFromUIImage(image));
        }

        private static UIImage FixOrientation(UIImage image)
        {
            if (image.Orientation == UIImageOrientation.Up) return image;

            UIGraphics.BeginImageContextWithOptions(image.Size, false, image.CurrentScale);
            image.Draw(new RectangleF(new PointF(0, 0), image.Size));
            var normalizedImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return normalizedImage;
        }

        private static UIImage GetUIImageFromBase64(string base64)
        {
            var data = new NSData(base64, NSDataBase64DecodingOptions.None);
            return UIImage.LoadFromData(data);
        }

        internal static ImageSource GetImageSourceFromUIImage(UIImage uiImage)
        {
            return uiImage == null ? null : ImageSource.FromStream(() => uiImage.AsJPEG(0.75f).AsStream());
        }
        
        // resize the image to be contained within a maximum width and height, keeping aspect ratio
        internal static UIImage MaxResizeImage(UIImage sourceImage, float maxWidth, float maxHeight)
        {
            if (sourceImage == null || maxWidth <= 0 || maxHeight <= 0) return null;

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

        internal static UIImage AdjustOpacity(UIImage image, float opacity)
        {
            UIGraphics.BeginImageContextWithOptions(image.Size, false, 0.0f);

            var ctx = UIGraphics.GetCurrentContext();
            var area = new RectangleF(0, 0, image.Size.Width, image.Size.Height);

            ctx.ScaleCTM(1, -1);
            ctx.TranslateCTM(0, -area.Height);
            ctx.SetBlendMode(CGBlendMode.Multiply);
            ctx.SetAlpha(opacity);
            ctx.DrawImage(area, image.CGImage);

            var newImage = UIGraphics.GetImageFromCurrentImageContext();

            UIGraphics.EndImageContext();

            return newImage;
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

            // add a black circle around the image
            context.SetLineWidth(1);
            context.SetStrokeColor(new CGColor(0, 0, 0));
            context.BeginPath();
            context.AddEllipseInRect(new RectangleF(1, 1, imageWidth-2, imageHeight-2));
            context.DrawPath(CGPathDrawingMode.Stroke);

            var newImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return newImage;
        }
    }
}