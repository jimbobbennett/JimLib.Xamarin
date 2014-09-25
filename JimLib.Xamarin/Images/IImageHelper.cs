using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Images
{
    public interface IImageHelper
    {
        ImageSource GetImageSource(string base64);
        Task<Tuple<string, ImageSource>> GetImageAsync(PhotoSource source, ImageOptions options = null);
        Task<Tuple<string, ImageSource>> GetImageAsync(string url, ImageOptions options = null, bool canCache = false);

        Task<Tuple<string, ImageSource>> GetImageAsync(string baseUrl, string resource = "/",
            string username = null, string password = null, int timeout = 10000,
            Dictionary<string, string> headers = null, ImageOptions options = null, bool canCache = false);

        PhotoSource AvailablePhotoSources { get; }
    }
}
