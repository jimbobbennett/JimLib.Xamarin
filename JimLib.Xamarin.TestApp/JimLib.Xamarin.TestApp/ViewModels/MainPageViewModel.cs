using System.Windows.Input;
using JimBobBennett.JimLib.Commands;
using JimBobBennett.JimLib.Xamarin.Mvvm;
using JimBobBennett.JimLib.Xamarin.Navigation;

namespace TestApp.ViewModels
{
    public class MainPageViewModel : ContentPageViewModelBase
    {
        public MainPageViewModel(INavigationStackManager navigationStackManager)
        {
            ShowActionSheetTestCommand = new AsyncCommand(async () => await navigationStackManager.PushViewModelAsync<ActionSheetTestViewModel>());
            ImageCaptureTestCommand = new AsyncCommand(async () => await navigationStackManager.PushViewModelAsync<CaptureImagePageViewModel>());
            ExtendedListViewTestCommand = new AsyncCommand(async () => await navigationStackManager.PushViewModelAsync<ExtendedListViewPageViewModel>());
            ViewImagePageTestCommand = new AsyncCommand(async () => await navigationStackManager.PushViewModelAsync<ViewImagePageViewModel>());
            BackgroundImagePageTestCommand = new AsyncCommand(async () => await navigationStackManager.PushViewModelAsync<BackgroundImagePageViewModel>());
            ActivityIndicatorPageTestCommand = new AsyncCommand(async () => await navigationStackManager.PushViewModelAsync<ActivityIndicatorTestViewModel>());
        }

        public ICommand ShowActionSheetTestCommand { get; private set; }
        public ICommand ImageCaptureTestCommand { get; private set; }
        public ICommand ExtendedListViewTestCommand { get; private set; }
        public ICommand ViewImagePageTestCommand { get; private set; }
        public ICommand BackgroundImagePageTestCommand { get; private set; }
        public ICommand ActivityIndicatorPageTestCommand { get; private set; }
    }
}
