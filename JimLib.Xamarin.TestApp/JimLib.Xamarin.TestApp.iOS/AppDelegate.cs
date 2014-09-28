using JimBobBennett.JimLib.Xamarin;
using JimBobBennett.JimLib.Xamarin.ios;

namespace JimLib.Xamarin.TestApp.iOS
{
    public partial class AppDelegate : AppDelegateBase
    {
        protected override AppBase CreateApp()
        {
            return new App();
        }
    }
}
