using System.Threading.Tasks;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Sharing
{
    public interface IShare
    {
        Task ShareAsync(string url, ImageSource image, string message);
        Task ShareAsync(ImageSource image, string message);
    }
}
