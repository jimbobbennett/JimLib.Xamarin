using Autofac;
using Foundation;
using JimBobBennett.JimLib.Xamarin.Application;
using JimBobBennett.JimLib.Xamarin.Contacts;
using JimBobBennett.JimLib.Xamarin.Controls;
using JimBobBennett.JimLib.Xamarin.ios.Controls;
using JimBobBennett.JimLib.Xamarin.ios.Images;
using JimBobBennett.JimLib.Xamarin.ios.Network;
using JimBobBennett.JimLib.Xamarin.ios.Purchases;
using JimBobBennett.JimLib.Xamarin.ios.Sharing;
using JimBobBennett.JimLib.Xamarin.ios.SocialMedia;
using JimBobBennett.JimLib.Xamarin.Images;
using JimBobBennett.JimLib.Xamarin.Network;
using JimBobBennett.JimLib.Xamarin.Purchases;
using JimBobBennett.JimLib.Xamarin.Sharing;
using JimBobBennett.JimLib.Xamarin.SocialMedia;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using INavigation = JimBobBennett.JimLib.Xamarin.ios.Navigation.INavigation;

namespace JimBobBennett.JimLib.Xamarin.ios
{
    public abstract class AppDelegateBase : FormsApplicationDelegate
    {
        // class-level declarations
        private UIWindow _window;
        private IApplicationEvents _applicationEvents;

        protected AppBase AppBase { get; private set; }

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();

            _window = new UIWindow(UIScreen.MainScreen.Bounds);

            var navigation = new Navigation.Navigation();

            AppBase = CreateApp();

            LoadApplication(AppBase);

            AppBase.InitializeContainer(builder =>
            {
                builder.RegisterType<LocalServerDiscovery>().As<ILocalServerDiscovery>().SingleInstance();
                builder.RegisterType<Contacts.Contacts>().As<IContacts>().SingleInstance();
                builder.RegisterType<ImageHelper>().As<IImageHelper>().SingleInstance();
                builder.RegisterType<KeyboardHelper>().As<IKeyboardHelper>().SingleInstance();
                builder.RegisterType<SocialMediaConnections>().As<ISocialMediaConnections>().SingleInstance();
                builder.RegisterType<InAppPurchase>().As<IInAppPurchase>().SingleInstance();
                builder.RegisterType<Share>().As<IShare>().SingleInstance();
                builder.RegisterInstance(new UriHelper(app)).As<IUriHelper>().SingleInstance();
                builder.RegisterInstance(navigation).As<INavigation>().SingleInstance();

                OnInitializeContainer(builder);
            });

            _applicationEvents = AppBase.ComponentContext.Resolve<IApplicationEvents>();

            _window.RootViewController = AppBase.GetMainPage().CreateViewController();
            navigation.RootViewController = _window.RootViewController;

            _window.MakeKeyAndVisible();

            return true;
        }

        protected virtual void OnInitializeContainer(ContainerBuilder builder)
        {
        }

        protected abstract AppBase CreateApp();

        public override void WillTerminate(UIApplication application)
        {
            _applicationEvents.OnClosing();
        }
    }
}