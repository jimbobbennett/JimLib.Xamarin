using Autofac;
using JimBobBennett.JimLib.Xamarin.Contacts;
using JimBobBennett.JimLib.Xamarin.Controls;
using JimBobBennett.JimLib.Xamarin.Images;
using JimBobBennett.JimLib.Xamarin.ios.Controls;
using JimBobBennett.JimLib.Xamarin.ios.Images;
using JimBobBennett.JimLib.Xamarin.ios.Network;
using JimBobBennett.JimLib.Xamarin.ios.Purchases;
using JimBobBennett.JimLib.Xamarin.ios.SocialMedia;
using JimBobBennett.JimLib.Xamarin.Network;
using JimBobBennett.JimLib.Xamarin.Purchases;
using JimBobBennett.JimLib.Xamarin.SocialMedia;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.ios
{
    public abstract class AppDelegateBase : UIApplicationDelegate
    {
        // class-level declarations
        UIWindow _window;

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

            var appBase = CreateApp();

            appBase.InitializeContainer(builder =>
            {
                builder.RegisterType<RestConnection>().As<IRestConnection>();
                builder.RegisterType<LocalServerDiscovery>().As<ILocalServerDiscovery>();
                builder.RegisterType<Contacts.Contacts>().As<IContacts>();
                builder.RegisterType<ImageHelper>().As<IImageHelper>();
                builder.RegisterType<KeyboardHelper>().As<IKeyboardHelper>();
                builder.RegisterType<SocialMediaConnections>().As<ISocialMediaConnections>();
                builder.RegisterType<InAppPurchase>().As<IInAppPurchase>().SingleInstance();
                builder.RegisterInstance(new UriHelper(app)).As<IUriHelper>();
                builder.RegisterInstance(navigation).As<Navigation.INavigation>();
            });

            _window.RootViewController = appBase.GetMainPage().CreateViewController();
            navigation.RootViewController = _window.RootViewController;

            _window.MakeKeyAndVisible();

            return true;
        }

        protected abstract AppBase CreateApp();
    }
}