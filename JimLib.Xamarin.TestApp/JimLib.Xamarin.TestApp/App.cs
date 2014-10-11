using Autofac;
using JimBobBennett.JimLib.Xamarin;
using JimBobBennett.JimLib.Xamarin.Navigation;
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
            builder.RegisterType<CaptureImageViewModel>();

            builder.RegisterType<ActionSheetTestPage>().UsingConstructor(typeof(ActionSheetTestViewModel),
                typeof(INavigationStackManager));
            builder.RegisterType<MainPage>().UsingConstructor(typeof(MainPageViewModel),
                typeof(INavigationStackManager));
            builder.RegisterType<CaptureImagePage>().UsingConstructor(typeof(CaptureImageViewModel),
                typeof(INavigationStackManager));
        }

        public override Page GetMainPage()
        {
            return CreateMainPage<MainPage>(true);
        }
    }
}
