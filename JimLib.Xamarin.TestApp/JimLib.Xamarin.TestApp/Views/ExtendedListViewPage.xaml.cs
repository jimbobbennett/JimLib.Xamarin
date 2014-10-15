using JimBobBennett.JimLib.Xamarin.Navigation;
using JimBobBennett.JimLib.Xamarin.Views;
using TestApp.ViewModels;

namespace TestApp.Views
{
    public partial class ExtendedListViewPage : BaseContentPage
    {
        public ExtendedListViewPage()
        {
            InitializeComponent();
        }


        public ExtendedListViewPage(ExtendedListViewPageViewModel viewModel, INavigationStackManager navigationStackManager)
            : base(viewModel, navigationStackManager)
        {
            InitializeComponent();
        }
    }
}
