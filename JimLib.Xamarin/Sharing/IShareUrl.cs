using System.Threading.Tasks;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Sharing
{
    public interface IShareUrl
    {
        Task ShareAsync(string url, ImageSource image, string message);
    }
}
