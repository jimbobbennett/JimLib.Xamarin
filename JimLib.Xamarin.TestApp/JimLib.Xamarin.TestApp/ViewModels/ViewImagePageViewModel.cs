using System.Windows.Input;
using JimBobBennett.JimLib.Commands;
using JimBobBennett.JimLib.Xamarin.Mvvm;
using JimBobBennett.JimLib.Xamarin.Navigation;
using Xamarin.Forms;

namespace TestApp.ViewModels
{
    public class ViewImagePageViewModel : ContentPageViewModelBase
    {
        public ViewImagePageViewModel(IUtilityViewNavigation utilityViewNavigation)
        {
            OpenImageCommand = new AsyncCommand(async () =>
            {
                await utilityViewNavigation.ShowImageViewer(ImageSource);
            });
        }

        public ICommand OpenImageCommand { get; private set; }

        public ImageSource ImageSource { get { return ImageSource.FromFile("Images/r2d2.jpg"); } }
    }
}
