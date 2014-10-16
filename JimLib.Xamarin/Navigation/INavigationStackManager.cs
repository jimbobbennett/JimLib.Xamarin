using System;
using System.Threading.Tasks;
using JimBobBennett.JimLib.Events;
using JimBobBennett.JimLib.Xamarin.Mvvm;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Navigation
{
    public interface INavigationStackManager
    {
        Task PushModalAsync(Page page);
        Task PushAsync(Page page);

        Task PushModalAsync<TPage>(Action<TPage> startupAction = null) where TPage : Page;
        Task PushAsync<TPage>(Action<TPage> startupAction = null) where TPage : Page;

        Task PushViewModelModalAsync<TViewModel>(Action<TViewModel> startupAction = null) 
            where TViewModel : class, IContentPageViewModelBase;
        Task PushViewModelAsync<TViewModel>(Action<TViewModel> startupAction = null) 
            where TViewModel : class, IContentPageViewModelBase;

        Task PopAsync();

        Task RollbackToRootAsync();
        void SetPages(Page rootPage, NavigationPage navigationPage);

        event EventHandler<EventArgs<Page>> PagePushed;
        event EventHandler<EventArgs<Page>> PagePopped;

        Page TopPage { get; }
    }
}
