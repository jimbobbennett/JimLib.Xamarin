using System.Threading.Tasks;
using JimBobBennett.JimLib.Xamarin.Views;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Navigation
{
    public class UtilityViewNavigation : IUtilityViewNavigation
    {
        private readonly INavigationStackManager _navigationStackManager;

        public UtilityViewNavigation(INavigationStackManager navigationStackManager)
        {
            _navigationStackManager = navigationStackManager;
        }

        public async Task ShowImageViewer(ImageSource source, string title, string text)
        {
            await _navigationStackManager.PushModalAsync<ImageViewerPage>(v => v.SetImage(source, title, text));
        }
    }
}
