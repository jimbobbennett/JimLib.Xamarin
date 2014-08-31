using System.Collections.Generic;
using System.Threading.Tasks;
using JimBobBennett.JimLib.Xamarin.Contacts;

namespace JimBobBennett.JimLib.Xamarin.SocialMedia
{
    public interface ISocialMediaConnections
    {
        Task<IEnumerable<Account>> GetFacebookUserAsync();
        void ViewOnFacebook(Account facebookUser);

        Task<IEnumerable<Account>> GetTwitterHandleAsync();
        void ViewOnTwitter(Account twitterUser);

        void ChatOnWhatsApp(Account whatsAppUser, ContactOverview contact);
    }
}
