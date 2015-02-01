using JimBobBennett.JimLib.Xamarin.Navigation;
using TestApp.ViewModels;

namespace TestApp.Views
{
    public partial class ViewImagePage
    {
        public ViewImagePage()
        {
            InitializeComponent();
        }

        public ViewImagePage(ViewImagePageViewModel viewModel, INavigationStackManager navigationStackManager)
            : base(viewModel, navigationStackManager)
        {
            InitializeComponent();
        }
    }
}
