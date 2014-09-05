using System.Threading.Tasks;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Navigation
{
    public interface INavigationStackManager
    {
        Task PushModalAsync(Page page);
        Task PopModalAsync();

        Task PushAsync(Page page);
        Task PopAsync();

        Task RollbackToRootAsync();
        void SetPages(Page rootPage, NavigationPage navigationPage);
    }
}
