using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Images
{
    public interface IImageHelper
    {
        ImageSource GetImageSource(string base64);
    }
}
