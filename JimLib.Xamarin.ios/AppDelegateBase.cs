using Autofac;
using JimBobBennett.JimLib.Xamarin.Application;
using JimBobBennett.JimLib.Xamarin.Contacts;
using JimBobBennett.JimLib.Xamarin.Controls;
using JimBobBennett.JimLib.Xamarin.Images;
using JimBobBennett.JimLib.Xamarin.ios.Application;
using JimBobBennett.JimLib.Xamarin.ios.Controls;
using JimBobBennett.JimLib.Xamarin.ios.Images;
using JimBobBennett.JimLib.Xamarin.ios.Network;
using JimBobBennett.JimLib.Xamarin.ios.Purchases;
using JimBobBennett.JimLib.Xamarin.ios.Sharing;
using JimBobBennett.JimLib.Xamarin.ios.SocialMedia;
using JimBobBennett.JimLib.Xamarin.Network;
using JimBobBennett.JimLib.Xamarin.Purchases;
using JimBobBennett.JimLib.Xamarin.Sharing;
using JimBobBennett.JimLib.Xamarin.SocialMedia;
using Foundation;
using UIKit;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.ios
{
    public abstract class AppDelegateBase : UIApplicationDelegate
    {
        // class-level declarations
        UIWindow _window;
        private ApplicationEvents _applicationEvents = new ApplicationEvents();

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

            AppBase.InitializeContainer(builder =>
            {
                builder.RegisterType<RestConnection>().As<IRestConnection>().SingleInstance();
                builder.RegisterType<LocalServerDiscovery>().As<ILocalServerDiscovery>().SingleInstance();
                builder.RegisterType<Contacts.Contacts>().As<IContacts>().SingleInstance();
                builder.RegisterType<ImageHelper>().As<IImageHelper>().SingleInstance();
                builder.RegisterType<KeyboardHelper>().As<IKeyboardHelper>().SingleInstance();
                builder.RegisterType<SocialMediaConnections>().As<ISocialMediaConnections>().SingleInstance();
                builder.RegisterType<InAppPurchase>().As<IInAppPurchase>().SingleInstance();
                builder.RegisterType<Share>().As<IShare>().SingleInstance();
                builder.RegisterInstance(new UriHelper(app)).As<IUriHelper>().SingleInstance();
                builder.RegisterInstance(navigation).As<Navigation.INavigation>().SingleInstance();
                builder.RegisterInstance(_applicationEvents).As<IApplicationEvents>().SingleInstance();

                OnInitializeContainer(builder);
            });

            _window.RootViewController = AppBase.GetMainPage().CreateViewController();
            navigation.RootViewController = _window.RootViewController;

            _window.MakeKeyAndVisible();

           _applicationEvents.OnStart();

            return true;
        }

        protected virtual void OnInitializeContainer(ContainerBuilder builder)
        {
        }

        protected abstract AppBase CreateApp();

        public override void WillEnterForeground(UIApplication application)
        {
            _applicationEvents.OnAppear();
        }

        public override void DidEnterBackground(UIApplication application)
        {
            _applicationEvents.OnDisappear();
        }

        public override void WillTerminate(UIApplication application)
        {
            _applicationEvents.OnClosing();
        }
    }
}