using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using JimBobBennett.JimLib.Extensions;
using JimBobBennett.JimLib.Network;
using JimBobBennett.JimLib.Xamarin.ios.Extensions;
using JimBobBennett.JimLib.Xamarin.Images;
using UIKit;
using Xamarin.Forms;
using Xamarin.Media;

namespace JimBobBennett.JimLib.Xamarin.ios.Images
{
    public class ImageHelper : IImageHelper
    {
        private readonly Dictionary<string,  ImageSource> _cachedImages = new Dictionary<string, ImageSource>();
        private readonly IRestConnection _restConnection;

        public ImageHelper(IRestConnection restConnection)
        {
            _restConnection = restConnection;
        }

        public ImageSource GetImageSourceFromBase64(string base64)
        {
            if (base64.IsNullOrEmpty()) return null;
            return GetImageSourceFromUIImage(GetUIImageFromBase64(base64));
        }

        public async Task<ImageSource> GetImageAsync(PhotoSource source,
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

        public async Task<ImageSource> GetImageAsync(string url, ImageOptions options = null, bool canCache = false)
        {
            ImageSource retVal;
            if (canCache && _cachedImages.TryGetValue(url, out retVal))
                return retVal;

            return await Task.Run(() =>
                {
                    try
                    {
                        var image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(url)));
                        if (image == null)
                            return null;

                        retVal = ProcessImage(options, image);
                        _cachedImages[url] = retVal;

                        return retVal;
                    }
                    catch
                    {
                        return null;
                    }
                });
        }

        public async Task<ImageSource> GetImageAsync(string baseUrl, string resource = "/",
            string username = null, string password = null, int timeout = 10000,
            Dictionary<string, string> headers = null, ImageOptions options = null, bool canCache = false)
        {
            ImageSource retVal;
            var uriBuilder = new UriBuilder(baseUrl) { Fragment = resource };
            var key = uriBuilder.Uri.ToString();
            if (canCache && _cachedImages.TryGetValue(key, out retVal))
                return retVal;

            var bytes = await _restConnection.MakeRawGetRequestAsync(baseUrl, resource, username, password, 
                timeout, headers);

            if (bytes == null)
                return null;

            var image = GetUIImageFromBase64(Convert.ToBase64String(bytes));

            if (image == null)
                return null;

            retVal = ProcessImage(options, image);
            _cachedImages[key] = retVal;

            return retVal;
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

        private static async Task<ImageSource> GetImageFromCameraAsync(ImageOptions options = null)
        {
            var mediaPicker = new MediaPicker();
            if (mediaPicker.IsCameraAvailable)
            {
                try
                {
                    var style = UIApplication.SharedApplication.StatusBarStyle;
                    var file = await mediaPicker.TakePhotoAsync(new StoreCameraMediaOptions());
                    UIApplication.SharedApplication.StatusBarStyle = style;

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

        private static async Task<ImageSource> GetImageFromExistingAsync(ImageOptions options = null)
        {
            var mediaPicker = new MediaPicker();
            if (mediaPicker.PhotosSupported)
            {
                try
                {
                    var style = UIApplication.SharedApplication.StatusBarStyle;
                    var file = await mediaPicker.PickPhotoAsync();
                    UIApplication.SharedApplication.StatusBarStyle = style;

                    return ProcessImageFile(options, file);
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return null;
        }

        private static ImageSource ProcessImageFile(ImageOptions options, MediaFile file)
        {
            if (file == null)
                return null;

            var image = UIImage.FromFile(file.Path);

            return ProcessImage(options, image);
        }

        private static ImageSource ProcessImage(ImageOptions options, UIImage image)
        {
            if (options != null)
            {
                if (options.HasSizeSet)
                    image = MaxResizeImage(image, options.MaxWidth, options.MaxHeight);

                if (options.FixOrientation)
                    image = FixOrientation(image);
            }
            
            return GetImageSourceFromUIImage(image);
        }

        private static ImageSource ProcessImageSource(ImageOptions options, UIImage image)
        {
            if (options != null)
            {
                if (options.HasSizeSet)
                    image = MaxResizeImage(image, options.MaxWidth, options.MaxHeight);
                
                if (options.FixOrientation)
                    image = FixOrientation(image);
            }
            
            return GetImageSourceFromUIImage(image);
        }

        private static UIImage FixOrientation(UIImage image)
        {
            if (image.Orientation == UIImageOrientation.Up) return image;

            UIGraphics.BeginImageContextWithOptions(image.Size, false, image.CurrentScale);
            image.Draw(new CGRect(new CGPoint(0, 0), image.Size));
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
        internal static UIImage MaxResizeImage(UIImage sourceImage, nfloat maxWidth, nfloat maxHeight)
        {
            if (sourceImage == null || maxWidth <= 0 || maxHeight <= 0) return null;

            var sourceSize = sourceImage.Size;
			var maxResizeFactor = (nfloat)Math.Max(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
            if (maxResizeFactor > 1) return sourceImage;
            var width = maxResizeFactor * sourceSize.Width;
            var height = maxResizeFactor * sourceSize.Height;

            UIGraphics.BeginImageContext(new CGSize(width, height));

            sourceImage.Draw(new CGRect(0, 0, width, height));

            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return resultImage;
        }

        internal static UIImage AdjustOpacity(UIImage image, float opacity)
        {
            UIGraphics.BeginImageContextWithOptions(image.Size, false, 0.0f);

            var ctx = UIGraphics.GetCurrentContext();
            var area = new CGRect(0, 0, image.Size.Width, image.Size.Height);

            ctx.ScaleCTM(1, -1);
            ctx.TranslateCTM(0, -area.Height);
            ctx.SetBlendMode(CGBlendMode.Multiply);
            ctx.SetAlpha(opacity);
            ctx.DrawImage(area, image.CGImage);

            var newImage = UIGraphics.GetImageFromCurrentImageContext();

            UIGraphics.EndImageContext();

            return newImage;
        }
        
        public async Task<ImageSource> GetProcessedImageSourceAsync(ImageSource imageSource, ImageOptions options)
        {
            var uiImage = await imageSource.GetImageAsync();

            return ProcessImageSource(options, uiImage);
        }

        public async Task<string> GetBase64FromImageSource(ImageSource imageSource, ImageType imageType = ImageType.Jpeg)
        {
            var image = await imageSource.GetImageAsync();

            switch (imageType)
            {
                case ImageType.Jpeg:
                    return image.AsJPEG(0.75f).GetBase64EncodedString(NSDataBase64EncodingOptions.None);
                case ImageType.Png:
                    return image.AsPNG().GetBase64EncodedString(NSDataBase64EncodingOptions.None);
            }

            return null;
        }
    }
}