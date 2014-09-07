using System.Threading.Tasks;

namespace JimBobBennett.JimLib.Xamarin.Views
{
    public interface IView
    {
        Task<string> GetOptionFromUserAsync(string title, string cancel, string destruction, 
            params string[] buttons);

        Task<bool> ShowAlertAsync(string title, string message, string cancel, string accept = null);
    }
}