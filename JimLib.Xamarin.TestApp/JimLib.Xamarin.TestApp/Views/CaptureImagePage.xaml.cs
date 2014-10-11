using JimBobBennett.JimLib.Xamarin.Navigation;
using JimBobBennett.JimLib.Xamarin.Views;
using TestApp.ViewModels;

namespace TestApp.Views
{
    public partial class CaptureImagePage : BaseContentPage
    {
        public CaptureImagePage()
        {
            InitializeComponent();
        }

        public CaptureImagePage(CaptureImageViewModel viewModel, INavigationStackManager navigationStackManager)
            : base(viewModel, navigationStackManager)
        {
            InitializeComponent();
        }
    }
}
