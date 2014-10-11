using System;
using JimBobBennett.JimLib.Xamarin.Navigation;
using JimBobBennett.JimLib.Xamarin.Views;
using TestApp.ViewModels;

namespace TestApp.Views
{
    public partial class MainPage : BaseContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public MainPage(MainPageViewModel viewModel, INavigationStackManager navigationStackManager)
            : base(viewModel, navigationStackManager)
        {
            InitializeComponent();
        }

        public async void ActionSheetClicked(object sender, EventArgs e)
        {
            await NavigationStackManager.PushAsync<ActionSheetTestPage>();
        }

        public async void ImageCaptureClicked(object sender, EventArgs e)
        {
            await NavigationStackManager.PushAsync<CaptureImagePage>();
        }
    }
}
