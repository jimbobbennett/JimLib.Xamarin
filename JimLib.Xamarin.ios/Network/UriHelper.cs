using JimBobBennett.JimLib.Xamarin.Network;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.ios.Network
{
    public class UriHelper : IUriHelper
    {
        private readonly UIApplication _app;

        public UriHelper(UIApplication app)
        {
            _app = app;
        }

        public void OpenSchemeUri(System.Uri schemeUri, System.Uri fallbackUri)
        {
            if (schemeUri != null && _app.CanOpenUrl(NSUrl.FromString(schemeUri.AbsoluteUri)))
                Device.OpenUri(schemeUri);
            else
                Device.OpenUri(fallbackUri);
        }
    }
}