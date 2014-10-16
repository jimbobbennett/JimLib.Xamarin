using Autofac;
using JimBobBennett.JimLib.Xamarin;
using JimBobBennett.JimLib.Xamarin.Navigation;
using JimBobBennett.JimLib.Xamarin.Views;
using TestApp.ViewModels;
using TestApp.Views;
using Xamarin.Forms;

namespace TestApp
{
    public class App : AppBase
    {
        protected override void OnInitialize(ContainerBuilder builder)
        {
            builder.RegisterType<ActionSheetTestViewModel>();
            builder.RegisterType<MainPageViewModel>();
            builder.RegisterType<CaptureImagePageViewModel>();
            builder.RegisterType<ExtendedListViewPageViewModel>();
            builder.RegisterType<ViewImagePageViewModel>();

            builder.RegisterType<ActionSheetTestPage>().UsingConstructor(typeof(ActionSheetTestViewModel),
                typeof(INavigationStackManager));
            builder.RegisterType<MainPage>().UsingConstructor(typeof(MainPageViewModel),
                typeof(INavigationStackManager));
            builder.RegisterType<CaptureImagePage>().UsingConstructor(typeof(CaptureImagePageViewModel),
                typeof(INavigationStackManager));
            builder.RegisterType<ExtendedListViewPage>().UsingConstructor(typeof(ExtendedListViewPageViewModel),
                typeof(INavigationStackManager));
            builder.RegisterType<ViewImagePage>().UsingConstructor(typeof(ViewImagePageViewModel),
                typeof(INavigationStackManager));
        }

        protected override void InitializeViewFactory(IViewFactory viewFactory)
        {
            base.InitializeViewFactory(viewFactory);

            viewFactory.Register<ActionSheetTestPage, ActionSheetTestViewModel>();
            viewFactory.Register<CaptureImagePage, CaptureImagePageViewModel>();
            viewFactory.Register<ExtendedListViewPage, ExtendedListViewPageViewModel>();
            viewFactory.Register<ViewImagePage, ViewImagePageViewModel>();
        }

        public override Page GetMainPage()
        {
            return CreateMainPage<MainPage>(true);
        }
    }
}
