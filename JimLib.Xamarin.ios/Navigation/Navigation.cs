using System.Linq;
using MonoTouch.UIKit;

namespace JimBobBennett.JimLib.Xamarin.ios.Navigation
{
    public class Navigation : INavigation
    {
        public UIViewController RootViewController { get; set; }

        public UINavigationController NavigationController
        {
            get
            {
                var controller = GetNavigationController(RootViewController);
                return controller;
            }
        }

        private UINavigationController GetNavigationController(UIViewController viewController)
        {
            if (viewController is UINavigationController)
                return viewController as UINavigationController;

            var nc = viewController.ChildViewControllers.OfType<UINavigationController>().FirstOrDefault();

            if (nc != null)
                return nc;

            foreach (var childVc in viewController.ChildViewControllers)
            {
                nc = GetNavigationController(childVc);
                if (nc != null)
                    return nc;
            }

            return null;
        }
    }
}