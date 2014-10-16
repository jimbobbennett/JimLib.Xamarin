using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Images
{
    public interface IImageHelper
    {
        ImageSource GetImageSourceFromBase64(string base64);
        Task<string> GetBase64FromImageSource(ImageSource imageSource, ImageType imageType = ImageType.Jpeg);

        Task<ImageSource> GetImageAsync(PhotoSource source, ImageOptions options = null);
        Task<ImageSource> GetImageAsync(string url, ImageOptions options = null, bool canCache = false);
        
        Task<ImageSource> GetImageAsync(string baseUrl, string resource = "/",
            string username = null, string password = null, int timeout = 10000,
            Dictionary<string, string> headers = null, ImageOptions options = null, bool canCache = false);

        PhotoSource AvailablePhotoSources { get; }

        Task<ImageSource> GetProcessedImageSourceAsync(ImageSource imageSource, ImageOptions options);
    }
}
