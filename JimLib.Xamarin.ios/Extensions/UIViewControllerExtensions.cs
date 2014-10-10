using System.Linq;
using MonoTouch.UIKit;

namespace JimBobBennett.JimLib.Xamarin.ios.Extensions
{
    public static class UIViewControllerExtensions
    {
        public static UIViewController GetTopViewController(this UIViewController rootViewController)
        {
            while (true)
            {
                if (rootViewController.PresentedViewController == null)
                    return rootViewController;

                var navigationController = rootViewController.PresentedViewController as UINavigationController;

                if (navigationController != null)
                {
                    var lastViewController = navigationController.ViewControllers.Last();
                    rootViewController = lastViewController;
                }
                else
                {
                    var presentedViewController = rootViewController.PresentedViewController;
                    rootViewController = presentedViewController;
                }
            }
        }
    }
}
