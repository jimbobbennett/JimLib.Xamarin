namespace JimBobBennett.JimLib.Xamarin.Network
{
    public interface IUriHelper
    {
        void OpenSchemeUri(System.Uri schemeUri, System.Uri fallbackUri = null);
    }
}
