using JimBobBennett.JimLib.Xamarin.Views;
using UIKit;

namespace JimBobBennett.JimLib.Xamarin.ios.Extensions
{
    public static class UIInterfaceOrientationExtensions
    {
        public static Orientation GetOrientation(this UIInterfaceOrientation interfaceOrientation)
        {
            if (interfaceOrientation == UIInterfaceOrientation.LandscapeLeft || 
                interfaceOrientation == UIInterfaceOrientation.LandscapeRight)
                return Orientation.Landscape;
            
            return Orientation.Portrait;
        }
    }
}
