using JimBobBennett.JimLib.Xamarin.Controls;
using JimBobBennett.JimLib.Xamarin.ios.Navigation;

namespace JimBobBennett.JimLib.Xamarin.ios.Controls
{
    public class KeyboardHelper : IKeyboardHelper
    {
        private readonly INavigation _navigation;

        public KeyboardHelper(INavigation navigation)
        {
            _navigation = navigation;
        }

        public void DismissKeyboard()
        {
            _navigation.NavigationController.VisibleViewController.View.EndEditing(true);
        }
    }
}