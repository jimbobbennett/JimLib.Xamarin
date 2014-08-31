using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JimBobBennett.JimLib.Extensions;
using JimBobBennett.JimLib.Xamarin.Contacts;
using JimBobBennett.JimLib.Xamarin.Network;
using JimBobBennett.JimLib.Xamarin.SocialMedia;
using MonoTouch.Accounts;
using MonoTouch.FacebookConnect;

namespace JimBobBennett.JimLib.Xamarin.ios.SocialMedia
{
    public class SocialMediaConnections : ISocialMediaConnections
    {
        private readonly IUriHelper _uriHelper;

        public SocialMediaConnections(IUriHelper uriHelper)
        {
            _uriHelper = uriHelper;
        }

        public async Task<IEnumerable<Account>> GetFacebookUserAsync()
        {
            var store = new ACAccountStore();
            var accountType = store.FindAccountType(ACAccountType.Facebook);

            var options = new AccountStoreOptions {FacebookAppId = FBSettings.DefaultAppID};

            var allowed = false;

            try
            {
                allowed = await store.RequestAccessAsync(accountType, options);
            }
            catch { }

            if (allowed)
            {
                var accounts = store.FindAccounts(accountType).ToList();

                if (accounts.Any())
                    return accounts.Select(CreateFacebookUser);
            }
            
            // try the old fashioned way
            var done = false;

            if (FBSession.ActiveSession.State != FBSessionState.CreatedTokenLoaded)
            {
                FBSessionStateHandler handler = (session, status, error) =>
                    {
                        done = true;
                    };

                if (!FBSession.OpenActiveSession(false))
                {
                    FBSession.OpenActiveSession(new[] {"public_profile"}, true, handler);
                    await this.WaitForAsync(() => done, int.MaxValue);

                    if (!done)
                        throw new Exception("Failed to connect to facebook");
                }
            }
            else
                FBSession.OpenActiveSession(false);

            if (!FBSession.ActiveSession.IsOpen) return new List<Account>();

            var me = await FBRequestConnection.GetMeAsync();
                
            if (me == null || me.Result == null)
                throw new Exception("Failed to connect to facebook");

            return CreateFacebokUser((FBGraphObject) me.Result).AsList();
        }

        private static Account CreateFacebokUser(FBGraphObject graphObject)
        {
            return new Account
            {
                Type = Account.Facebook, 
                Name = graphObject.ObjectForKey("name").ToString(), 
                UserId = graphObject.ObjectForKey("id").ToString()
            };
        }

        private static Account CreateFacebookUser(ACAccount arg)
        {
            return new Account
            {
                Type = Account.Facebook, 
                UserId = arg.Username, 
                Name = arg.UserFullName
            };
        }

        public void ViewOnFacebook(Account facebookUser)
        {
            if (facebookUser.Type != Account.Facebook) return;

            try
            {
                _uriHelper.OpenSchemeUri(new System.Uri("http://facebook.com/" + facebookUser.UserId));
            }
            catch
            {

            }
        }

        public async Task<IEnumerable<Account>> GetTwitterHandleAsync()
        {
            var store = new ACAccountStore();
            var accountType = store.FindAccountType(ACAccountType.Twitter);
            var allowed = await store.RequestAccessAsync(accountType, null);

            return !allowed ? null : store.FindAccounts(accountType).Select(CreateTwitterUser).ToList();
        }

        private static Account CreateTwitterUser(ACAccount a)
        {
            return new Account
            {
                UserId = a.AccountDescription,
                Name = a.UserFullName,
                Type = Account.Twitter
            };
        }

        public void ViewOnTwitter(Account twitterUser)
        {
            if (twitterUser.Type != Account.Twitter) return;

            try
            {
                var userId = twitterUser.UserId.Replace("@", "");
                var schemeUri = new System.Uri("twitter://user?screen_name=" + userId);
                var fallbackUri = new System.Uri("https://twitter.com/" + userId);

                _uriHelper.OpenSchemeUri(schemeUri, fallbackUri);
            }
            catch
            {

            }
        }

        public void ChatOnWhatsApp(Account whatsAppUser, ContactOverview contact)
        {
            if (whatsAppUser.Type != Account.WhatsApp) return;
            if (contact.AddressBookId.IsNullOrEmpty()) return;

            try
            {
                _uriHelper.OpenSchemeUri(new System.Uri("whatsapp://send?abid=" + contact.AddressBookId));
            }
            catch 
            {

            }
        }
    }
}