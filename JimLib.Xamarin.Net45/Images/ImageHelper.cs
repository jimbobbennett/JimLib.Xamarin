using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using JimBobBennett.JimLib.Network;
using JimBobBennett.JimLib.Xamarin.Images;
using Xamarin.Forms;
using Image = System.Drawing.Image;
using Size = System.Drawing.Size;

namespace JimBobBennett.JimLib.Xamarin.Net45.Images
{
    public class ImageHelper : IImageHelper
    {
        private readonly Dictionary<string, ImageSource> _cachedImages = new Dictionary<string, ImageSource>();
 
        private readonly IRestConnection _restConnection;

        public ImageHelper(IRestConnection restConnection)
        {
            _restConnection = restConnection;
        }

        public ImageSource GetImageSourceFromBase64(string base64)
        {
            var bytes = Convert.FromBase64String(base64);
            return ImageSource.FromStream(() => new MemoryStream(bytes));
        }

        public Task<string> GetBase64FromImageSource(ImageSource imageSource, ImageType imageType = ImageType.Jpeg)
        {
            return null;
        }

#pragma warning disable 1998
        public async Task<ImageSource> GetImageAsync(PhotoSource source, ImageOptions options = null)
#pragma warning restore 1998
        {
            return null;
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
                        var request = (HttpWebRequest) WebRequest.Create(url);

                        using (var response = (HttpWebResponse) request.GetResponse())
                        {
                            using (var stream = response.GetResponseStream())
                            {
                                if (stream != null)
                                    return ProcessImageStream(options, stream, url);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Failed to get image: " + ex.Message);
                    }

                    return null;
                });
        }

        public async Task<ImageSource> GetImageAsync(string baseUrl, string resource = "/", 
            string username = null, string password = null, int timeout = 10000,
            Dictionary<string, string> headers = null, ImageOptions options = null, bool canCache = false)
        {
            ImageSource retVal;
            var uriBuilder = new UriBuilder(baseUrl) {Fragment = resource};
            var url = uriBuilder.Uri.ToString();

            if (canCache && _cachedImages.TryGetValue(url, out retVal))
                return retVal;

            var bytes = await _restConnection.MakeRawGetRequestAsync(baseUrl, resource, username, password, timeout,
                headers);

            if (bytes == null)
                return null;

            using (var ms = new MemoryStream(bytes))
                return ProcessImageStream(options, ms, url);
        }

        private ImageSource ProcessImageStream(ImageOptions options, Stream stream, string url)
        {
            var bitmap = Image.FromStream(stream);

            if (options != null)
            {
                if (options.HasSizeSet)
                    bitmap = MaxResizeImage(bitmap, options.MaxWidth, options.MaxHeight);
            }

            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Jpeg);
                ms.Seek(0, SeekOrigin.Begin);

                var numBytesToRead = (int)ms.Length;
                var bytes = new byte[numBytesToRead];
                ms.Read(bytes, 0, numBytesToRead);

                var imageSource = ImageSource.FromStream(() => new MemoryStream(bytes));
                _cachedImages[url] = imageSource;

                return imageSource;
            }
        }

        private static Image MaxResizeImage(Image sourceImage, float maxWidth, float maxHeight)
        {
            if (sourceImage == null || maxWidth <= 0 || maxHeight <= 0) return null;

            var sourceSize = sourceImage.Size;
            var maxResizeFactor = Math.Max(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
            if (maxResizeFactor > 1) return sourceImage;
            var width = Convert.ToInt32(maxResizeFactor * sourceSize.Width);
            var height = Convert.ToInt32(maxResizeFactor * sourceSize.Height);

            return new Bitmap(sourceImage, new Size(width, height));
        }

        public PhotoSource AvailablePhotoSources { get { return PhotoSource.None; } }

#pragma warning disable 1998
        public async Task<ImageSource> GetProcessedImageSourceAsync(ImageSource imageSource, ImageOptions options)
#pragma warning restore 1998
        {
            throw new NotImplementedException();
        }
    }
}
