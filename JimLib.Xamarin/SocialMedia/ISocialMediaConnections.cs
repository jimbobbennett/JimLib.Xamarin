using System.Collections.Generic;
using System.Threading.Tasks;

namespace JimBobBennett.JimLib.Xamarin.SocialMedia
{
    public interface ISocialMediaConnections
    {
        Task<IEnumerable<FacebookUser>> GetFacebookUserAsync();
        Task SendFacebookFriendRequestAsync(FacebookUser facebookUser);

        Task<IEnumerable<TwitterUser>> GetTwitterHandleAsync();
        Task FollowOnTwitterAsync(TwitterUser handle);
    }
}
