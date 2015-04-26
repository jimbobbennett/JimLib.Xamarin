using JimBobBennett.JimLib.Xamarin.Navigation;
using TestApp.ViewModels;

namespace TestApp.Views
{
    public partial class ActivityIndicatorTestView
    {
        public ActivityIndicatorTestView()
        {
            InitializeComponent();
        }

        public ActivityIndicatorTestView(ActivityIndicatorTestViewModel viewModel, INavigationStackManager navigationStackManager)
            : base(viewModel, navigationStackManager)
        {
            InitializeComponent();
        }
    }
}
