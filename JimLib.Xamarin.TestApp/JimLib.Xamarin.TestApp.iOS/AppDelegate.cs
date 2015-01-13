using JimBobBennett.JimLib.Xamarin;
using JimBobBennett.JimLib.Xamarin.ios;
using Foundation;

namespace TestApp.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : AppDelegateBase
    {
        protected override AppBase CreateApp()
        {
            return new App();
        }
    }
}
