using System;
using System.Collections.Generic;

using Xamarin.Forms;
using JimBobBennett.JimLib.Xamarin.Navigation;
using JimBobBennett.JimLib.Xamarin.Views;

namespace TestApp
{
    public partial class GridViewPage : BaseContentPage
    {
        public GridViewPage()
        {
            InitializeComponent();
        }

        public GridViewPage(GridViewPageViewModel viewModel, INavigationStackManager navigationStackManager)
            : base(viewModel, navigationStackManager)
        {
            InitializeComponent();
        }
    }
}

