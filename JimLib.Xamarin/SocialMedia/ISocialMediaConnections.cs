using System.Collections.Generic;
using System.Threading.Tasks;

namespace JimBobBennett.JimLib.Xamarin.SocialMedia
{
    public interface ISocialMediaConnections
    {
        Task<IEnumerable<Account>> GetFacebookUserAsync();
        Task SendFacebookFriendRequestAsync(Account facebookUser);

        Task<IEnumerable<Account>> GetTwitterHandleAsync();
        Task FollowOnTwitterAsync(Account handle);
    }
}
