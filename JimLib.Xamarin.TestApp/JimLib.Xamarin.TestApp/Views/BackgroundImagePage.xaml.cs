using JimBobBennett.JimLib.Xamarin.Navigation;
using JimBobBennett.JimLib.Xamarin.Views;
using TestApp.ViewModels;

namespace TestApp.Views
{
    public partial class BackgroundImagePage : BaseContentPage
    {
        public BackgroundImagePage()
        {
            InitializeComponent();
        }

        public BackgroundImagePage(BackgroundImagePageViewModel viewModel, INavigationStackManager navigationStackManager)
            : base(viewModel, navigationStackManager)
        {
            InitializeComponent();
        }
    }
}
