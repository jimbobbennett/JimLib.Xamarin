using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using JimBobBennett.JimLib.Xamarin.Images;
using JimBobBennett.JimLib.Xamarin.Network;
using Xamarin.Forms;
using Image = System.Drawing.Image;
using Point = System.Drawing.Point;

namespace JimBobBennett.JimLib.Xamarin.Net45.Images
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
            var bytes = Convert.FromBase64String(base64);
            return ImageSource.FromStream(() => new MemoryStream(bytes));
        }

#pragma warning disable 1998
        public async Task<Tuple<string, ImageSource>> GetImageAsync(PhotoSource source, ImageOptions options = null)
#pragma warning restore 1998
        {
            return null;
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
                        var request = (HttpWebRequest) WebRequest.Create(url);

                        using (var response = (HttpWebResponse) request.GetResponse())
                        {
                            using (var stream = response.GetResponseStream())
                            {
                                if (stream != null)
                                    return ProcessImageStream(options, stream);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Failed to get image: " + ex.Message);
                    }

                    return Tuple.Create((string) null, (ImageSource) null);
                });
        }

        public async Task<Tuple<string, ImageSource>> GetImageAsync(string baseUrl, string resource = "/", 
            string username = null, string password = null, int timeout = 10000,
            Dictionary<string, string> headers = null, ImageOptions options = null, bool canCache = false)
        {
            Tuple<string, ImageSource> retVal;
            var uriBuilder = new UriBuilder(baseUrl) {Fragment = resource};
            if (canCache && _cachedImages.TryGetValue(uriBuilder.Uri.ToString(), out retVal))
                return retVal;

            var bytes = await _restConnection.MakeRawGetRequestAsync(baseUrl, resource, username, password, timeout,
                headers);

            if (bytes == null)
                return Tuple.Create((string)null, (ImageSource)null);

            using (var ms = new MemoryStream(bytes))
                return ProcessImageStream(options, ms);
        }

        private Tuple<string, ImageSource> ProcessImageStream(ImageOptions options, Stream stream)
        {
            var bitmap = Image.FromStream(stream);

            if (options != null)
            {
                if (options.HasSizeSet)
                    bitmap = MaxResizeImage(bitmap, options.MaxWidth, options.MaxHeight);

                if (options.Circle)
                    bitmap = ClipToCircle(bitmap);
            }

            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Jpeg);
                ms.Seek(0, SeekOrigin.Begin);

                var numBytesToRead = (int)ms.Length;
                var bytes = new byte[numBytesToRead];
                ms.Read(bytes, 0, numBytesToRead);

                return Tuple.Create(Convert.ToBase64String(bytes),
                    ImageSource.FromStream(() => new MemoryStream(bytes)));
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

            return new Bitmap(sourceImage, new System.Drawing.Size(width, height));
        }

        private static Bitmap ClipToCircle(Image original)
        {
            var copy = new Bitmap(original);

            using (var g = Graphics.FromImage(copy))
            {
                var center = new Point(original.Width / 2, original.Height / 2);
                var radius = Math.Min(center.X, center.Y);
                var r = new RectangleF(center.X - radius, center.Y - radius, radius*2, radius*2);
                var path = new GraphicsPath();
                path.AddEllipse(r);
                g.Clip = new Region(path);
                g.DrawImage(original, 0, 0);
                return copy;
            }
        }

        public PhotoSource AvailablePhotoSources { get { return PhotoSource.None; } }
    }
}
