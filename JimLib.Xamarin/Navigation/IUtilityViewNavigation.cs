using System.Threading.Tasks;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Navigation
{
    public interface IUtilityViewNavigation
    {
        Task ShowImageViewer(ImageSource source, string title = null, string text = null);
    }
}