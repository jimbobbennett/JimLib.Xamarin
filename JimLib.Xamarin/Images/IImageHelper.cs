using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Images
{
    public interface IImageHelper
    {
        ImageSource GetImageSource(string base64);
        Task<Tuple<string, ImageSource>> GetImageAsync(PhotoSource source, ImageOptions options = null);
        Task<Tuple<string, ImageSource>> GetImageAsync(string url, ImageOptions options = null);
        PhotoSource AvailablePhotoSources { get; }
    }
}
