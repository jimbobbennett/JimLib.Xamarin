using JimBobBennett.JimLib.Xamarin.Navigation;
using JimBobBennett.JimLib.Xamarin.Views;
using TestApp.ViewModels;

namespace TestApp.Views
{
    public partial class ActionSheetTestPage : BaseContentPage
    {
        public ActionSheetTestPage()
        {
            InitializeComponent();
        }

        public ActionSheetTestPage(ActionSheetTestViewModel viewModel, INavigationStackManager navigationStackManager)
            : base(viewModel, navigationStackManager)
        {
            InitializeComponent();
        }
    }
}
