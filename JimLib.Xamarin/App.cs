using System;
using Autofac;
using JimBobBennett.JimLib.Xamarin.Navigation;
using JimBobBennett.JimLib.Xamarin.Timers;
using JimBobBennett.JimLib.Xamarin.ViewModels;
using JimBobBennett.JimLib.Xamarin.Views;
using Refractored.Xam.Settings;
using Refractored.Xam.Settings.Abstractions;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin
{
    public abstract class AppBase
    {
        protected static IContainer Container { get; private set; }

        public IComponentContext ComponentContext { get { return Container.Resolve<IComponentContext>(); } }

        public void InitializeContainer(Action<ContainerBuilder> customRegistration)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(CrossSettings.Current).As<ISettings>();
            builder.RegisterType<NavigationStackManager>().As<INavigationStackManager>().SingleInstance();
            builder.RegisterType<UtilityViewNavigation>().As<IUtilityViewNavigation>().SingleInstance();
            builder.RegisterType<ImageViewerViewModel>();
            builder.RegisterType<Timer>().As<ITimer>();
            builder.RegisterType<ImageViewerPage>().UsingConstructor(typeof(ImageViewerViewModel), typeof(INavigationStackManager));
            
            OnInitialize(builder);
            
            customRegistration(builder);

            Container = builder.Build();
        }

        protected abstract void OnInitialize(ContainerBuilder builder);

        public abstract Page GetMainPage();

        protected Page CreateMainPage<T>(bool needNavigation = false, Color barBackgroundColor = default(Color),
            Color barTextColor = default(Color)) 
            where T : Page
        {
            var mainPage = Container.Resolve<T>();

            if (!needNavigation) return mainPage;

            var navigationPage = new NavigationPage(mainPage);

            if (barBackgroundColor != default(Color) && barBackgroundColor != Color.Default)
                navigationPage.BarBackgroundColor = barBackgroundColor;

            if (barTextColor != default(Color) && barTextColor != Color.Default)
                navigationPage.BarTextColor = barTextColor;

            Container.Resolve<INavigationStackManager>().SetPages(mainPage, navigationPage);

            return navigationPage;
        }
    }
}
