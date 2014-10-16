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

        public async Task ShowImageViewer(ImageSource source, string title = null, string text = null)
        {
            await ShowImageViewer(source, Color.Default, title, text);
        }

        public async Task ShowImageViewer(ImageSource source, Color backgroundColor, string title = null, string text = null)
        {
            await _navigationStackManager.PushModalAsync<ImageViewerPage>(v =>
                {
                    if (backgroundColor != Color.Default)
                        v.BackgroundColor = backgroundColor;
                    v.SetImage(source, title, text);
                });
        }
    }
}
